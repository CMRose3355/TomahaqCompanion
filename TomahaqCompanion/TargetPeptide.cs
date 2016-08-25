using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;
using CSMSL.IO.Thermo;
using ZedGraph;

namespace TomahaqCompanion
{
    public class TargetPeptide
    {
        public string PeptideString { get; set; }
        public int Charge { get; set; }

        public double MassShift { get; set; }

        public double MaxIntensity { get; set; }
        public double MaxRetentionTime { get; set; }

        public double TriggerMZ { get; set; }
        public double TargetMZ { get; set; }

        public Peptide Trigger { get; set; }
        public Peptide Target { get; set; }

        public SortedList<double, double> TriggerMS1xic {get; set;}
        public SortedList<double, double> TargetMS1xic { get; set; }

        public SortedList<double, double> TriggerCompositeSpectrum { get; set; }

        public SortedDictionary<int, double> MS1toTriggerInt { get; set; }

        public SortedDictionary<double,MS2Event> TriggerMS2sByInt { get; set; }
        public SortedDictionary<double, MS2Event> TargetMS2sByTriggerInt { get; set; }

        public List<MS2Event> TriggerMS2s { get; set; }
        public List<MS2Event> TargetMS2s { get; set; }
        public List<MS3Event> TargetMS3s { get; set; }

        public MS2Event AvgTriggerMS2 { get; set; }

        public List<Fragment> TriggerFrags { get; set; }
        public List<Fragment> TargetFrags { get; set; }

        public Dictionary<int, Dictionary<string, double>> TargetSPSFrags { get; set; }

        public List<double> TriggerIons { get; set; }
        public List<double> TargetSPSIons { get; set; }

        private Dictionary<int, Modification> DynModDict { get; set; }

        public List<ScanEventLine> ScanEventLines { get; set; }

        public PointPairList TriggerMS1XicPoints { get; set; }
        public PointPairList TargetMS1XicPoints { get; set; }
        public PointPairList TriggerMS2Points { get; set; }
        public PointPairList TargetMS2Points { get; set; }
        public PointPairList TargetMS3Points { get; set; }
        public PointPairList TriggerCompositeMS2Points { get; set; }

        public TargetPeptide(string peptideString, int charge, Dictionary<Modification, string> staticMods, Dictionary<Modification, char> dynMods, List<double> targetSPSIons, List<double> triggerFragIons)
        {
            //Save the original peptide string
            PeptideString = peptideString;
            Charge = charge;
            TargetSPSIons = targetSPSIons;
            TriggerIons = triggerFragIons;

            //Build the dynamic modification dictionary and return the stripped string
            string strippedPepString = BuildDynamicModDict(peptideString, dynMods);

            //Create the target and the trigger peptides
            Trigger = new Peptide(strippedPepString);
            Target = new Peptide(strippedPepString);

            //Add the modifications to the peptide
            Trigger = AddModifications(Trigger, staticMods, "Trigger");
            Target = AddModifications(Target, staticMods, "Target");

            TriggerMZ = Trigger.ToMz(Charge);
            TargetMZ = Target.ToMz(Charge);

            //Calculte the mass shift for the peptide
            MassShift = Math.Abs(Trigger.MonoisotopicMass - Target.MonoisotopicMass) / Charge;

            //Populate the fragments
            TriggerFrags = PopulateFragments(Trigger, "Trigger");
            TargetFrags = PopulateFragments(Target, "Target");

            TriggerMS1xic = new SortedList<double, double>();
            TargetMS1xic = new SortedList<double, double>();

            TriggerMS2s = new List<MS2Event>();
            TargetMS2s = new List<MS2Event>();
            TargetMS3s = new List<MS3Event>();

            ScanEventLines = new List<ScanEventLine>();

            TriggerMS1XicPoints = new PointPairList();
            TargetMS1XicPoints = new PointPairList();
            TriggerMS2Points = new PointPairList();
            TargetMS2Points = new PointPairList();
            TargetMS3Points = new PointPairList();
            TriggerCompositeMS2Points = new PointPairList();

            MS1toTriggerInt = new SortedDictionary<int, double>();

            TriggerMS2sByInt = new SortedDictionary<double, MS2Event>();

            TargetMS2sByTriggerInt = new SortedDictionary<double, MS2Event>();

            TriggerCompositeSpectrum = new SortedList<double, double>();

            TargetSPSFrags = new Dictionary<int, Dictionary<string,double>>();
            for(int i = 1; i<10; i++)
            {
                TargetSPSFrags.Add(i, new Dictionary<string, double>());
            }
        }

        public void AddMS1XICPoint(ThermoSpectrum spectrum, double rt, int scanNumber)
        {
            MzRange triggerRange = new MzRange(TriggerMZ, new Tolerance(ToleranceUnit.PPM, 15));
            MzRange targetRange = new MzRange(TargetMZ, new Tolerance(ToleranceUnit.PPM, 15));

            ThermoMzPeak triggerPeak = GetTallestPeak(triggerRange, spectrum);
            ThermoMzPeak targetPeak = GetTallestPeak(targetRange, spectrum);

            if(triggerPeak != null)
            {
                TriggerMS1xic.Add(rt, triggerPeak.Intensity);
                MS1toTriggerInt.Add(scanNumber, triggerPeak.Intensity);
            }

            if(targetPeak != null)
            {
                TargetMS1xic.Add(rt, targetPeak.Intensity);
            }
        }

        public void AddTriggerData(int ms1ScanNumber, int scanNumber, ThermoSpectrum spectrum, double rt, double it)
        {
            List<ThermoMzPeak> peaks = null;
            if(spectrum.TryGetPeaks(0,2000, out peaks))
            {
                double triggerMS1Int = MS1toTriggerInt[ms1ScanNumber];
                MS2Event ms2Event = new MS2Event(scanNumber, rt, peaks, it, triggerMS1Int);
                TriggerMS2s.Add(ms2Event);
                TriggerMS2sByInt.Add(triggerMS1Int, ms2Event);
            }
        }

        public void AddTargetData(int ms1ScanNumber, int scanNumber, ThermoSpectrum spectrum, double rt, double it)
        {
            List<ThermoMzPeak> peaks = null;
            if (spectrum.TryGetPeaks(0, 2000, out peaks))
            {
                double triggerInt = MS1toTriggerInt[ms1ScanNumber];
                MS2Event ms2Event = new MS2Event(scanNumber, rt, peaks, it, triggerInt);
                TargetMS2s.Add(ms2Event);
                TargetMS2sByTriggerInt.Add(triggerInt, ms2Event);
            }
        }

        public void AddTargetData(int ms1ScanNumber, int scanNumber, ThermoSpectrum spectrum, double rt, double it, int ms3scanNumber, ThermoSpectrum ms3spectrum, double ms3rt, double ms3it, List<double> spsIons, Dictionary<string, double> quantChannelDict)
        {
            //See if there are any peaks within the MS2 event
            List<ThermoMzPeak> peaks = null;
            if (spectrum.TryGetPeaks(0, 2000, out peaks))
            {
                double triggerInt = MS1toTriggerInt[ms1ScanNumber];
                MS2Event ms2Event = new MS2Event(scanNumber, rt, peaks, it, triggerInt);
                TargetMS2s.Add(ms2Event);
                TargetMS2sByTriggerInt.Add(triggerInt, ms2Event);

                //If we have an MS3 then we will add the MS3 event to the MS2 event
                peaks = null;
                if (ms3spectrum.TryGetPeaks(0, 2000, out peaks))
                { 
                    MS3Event ms3Event = new MS3Event(ms3scanNumber, ms3rt, ms3spectrum, peaks, ms3it, quantChannelDict, spsIons);
                    ms2Event.AddMS3Event(ms3Event);
                    TargetMS3s.Add(ms3Event);
                }
            }
        }

        public void PopulateTargetSPSIons(int numSPSIons)
        {
            if (TargetSPSIons.Count > 0)
            {
                return;
            }
            else
            {
                double precExclusionMin = TargetMZ - 70;
                double precExclusionMax = TargetMZ + 10;

                //Determine the maximum charge
                int maxCharge = Charge - 1;
                if (maxCharge > 2) { maxCharge = 2; }

                SortedList<double, double> mzsToAdd = new SortedList<double, double>();
                //Add in the fragment ions
                for (int i = 1; i <= maxCharge; i++)
                {
                    foreach(Fragment frag in TargetFrags)
                    {
                        double fragMZ = frag.ToMz(i);
                        double distanceFromPrec = fragMZ - TargetMZ;

                        if(fragMZ>400 && fragMZ<2000 && frag.Number >= 2 && (fragMZ > precExclusionMax || fragMZ < precExclusionMin))
                        {
                            double outDoub = 0;
                            if (!mzsToAdd.TryGetValue(distanceFromPrec, out outDoub))
                            {
                                mzsToAdd.Add(distanceFromPrec, fragMZ);
                                TargetSPSFrags[i].Add(frag.ToString(), 100);
                            }
                        }
                    }
                }

                if(mzsToAdd.Count < numSPSIons)
                {
                    numSPSIons = mzsToAdd.Count;
                }

                for(int i = 0;i<numSPSIons;i++)
                {
                    TargetSPSIons.Add(mzsToAdd.ElementAt(i).Value);
                }
            }
        }

        public void PopulateTriggerIons()
        {
            if (TriggerIons.Count > 0)
            {
                return;
            }
            else
            {
                //Determine the maximum charge
                int maxCharge = Charge - 1;
                if (maxCharge > 2) { maxCharge = 2; }

                //Add in the fragment ions
                for (int i = 1; i <= maxCharge; i++)
                {
                    foreach (Fragment frag in TriggerFrags)
                    {
                        TriggerIons.Add(frag.ToMz(i));
                    }
                }
            }
        }

        private string BuildDynamicModDict(string peptideString, Dictionary<Modification, char> dynMods)
        {
            //This will be the dictionary that will tell us where to place the modifications
            DynModDict = new Dictionary<int, Modification>();

            //Kepp track of the total modifications
            int totalMods = 0;

            //Cycle through the string to see if there is a dynamic modification
            for (int i = 0; i < peptideString.Length; i++)
            {
                //If there is a dynamic modification then save its location to a dictionary
                if (dynMods.Values.Contains(peptideString.ElementAt(i)))
                {
                    //Increment and save the index
                    totalMods++;
                    int addIndexToMod = i - totalMods + 1;

                    //Get the actual modification -- this is a bit clunky, but there shouldn't be too many modifications to cycle through
                    Modification addMod = null;
                    foreach(KeyValuePair<Modification, char> kvp in dynMods)
                    {
                        //Grab the modification you will be using
                        if(kvp.Value == peptideString.ElementAt(i))
                        {
                            addMod = kvp.Key;
                        }
                    }

                    //Add the modification with the correct index
                    DynModDict.Add(addIndexToMod, addMod);
                }
            }

            //Save the stripped peptide which will be used to make the peptides
            string strippedPeptideString = peptideString;

            //Remove each of the dynamic modification chars from the string so that we can make a peptide
            foreach (char mod in dynMods.Values)
            {
                strippedPeptideString = strippedPeptideString.Replace(mod.ToString(), "");
            }

            //Return the stripped peptide sequence. 
            return strippedPeptideString;
        }

        private Peptide AddModifications(Peptide peptide, Dictionary<Modification, string> staticMods, string type)
        {
            //Cycle through the static mods and only add the static mod to either a trigger or target peptide.
            foreach(KeyValuePair<Modification, string> kvp in staticMods)
            {
                if(kvp.Value == type)
                {
                    peptide.SetModification(kvp.Key);
                }
            }

            //Cycle through the dynamic mods and add them to both of the peptides. 
            foreach(KeyValuePair<int, Modification> kvp in DynModDict)
            {
                peptide.SetModification(kvp.Value, kvp.Key);
            }

            return peptide;
        }

        private List<Fragment> PopulateFragments(Peptide peptide, string type)
        {
            List<Fragment> retFrags = new List<Fragment>();
            List<Fragment> candidateFrags = new List<Fragment>();
            List<Fragment> frags = peptide.Fragment(FragmentTypes.b | FragmentTypes.y).ToList();

            foreach (Fragment frag in frags)
            {
                if(frag.Number > 1)
                {
                    candidateFrags.Add(frag);
                }
            }

            //Ensure Lysine residues for any Y ions of target peptides
            if (type == "Target")
            {
                foreach(Fragment frag in candidateFrags)
                {
                    if(frag.GetSequence().Contains("K") || frag.Type == FragmentTypes.b)
                    {
                        retFrags.Add(frag);
                    }
                }
            }
            else
            {
                retFrags = candidateFrags;
            }

            return retFrags;
        }

        public void PopulateAnalysisData()
        {
            //Populate the XIC data for the Trigger peptides
            TriggerMS1XicPoints = new PointPairList();
            foreach(KeyValuePair<double, double> kvp in TriggerMS1xic)
            {
                TriggerMS1XicPoints.Add(kvp.Key, kvp.Value);
            }

            //Populate the XIC data for the Target peptide
            TargetMS1XicPoints = new PointPairList();
            foreach (KeyValuePair<double, double> kvp in TargetMS1xic)
            {
                TargetMS1XicPoints.Add(kvp.Key, kvp.Value);
            }

            //Look for the maximum trigger intensity
            double maxTime = 0;
            double maxIntensity = 0;
            foreach (MS2Event scan in TargetMS2s)
            {
                if (scan.MS1Intensity > maxIntensity)
                {
                    maxTime = scan.RetentionTime;
                    maxIntensity = scan.MS1Intensity;
                }
            }

            //Set the max intensity and the max retention time for use later
            MaxIntensity = maxIntensity;
            MaxRetentionTime = maxTime;

            //Add the points to mark when the trigger ms2 scans were performed
            TriggerMS2Points = new PointPairList();
            foreach(MS2Event scan in TriggerMS2s)
            {
                TriggerMS2Points.Add(scan.RetentionTime, MaxIntensity * 1);
            }

            //Add the points to mark when the target ms2 scans were performed
            TargetMS2Points = new PointPairList();
            foreach (MS2Event scan in TargetMS2s)
            {
                TargetMS2Points.Add(scan.RetentionTime, MaxIntensity * 0.75);
            }

            //Add the points to mark when the target ms3 scans were performed
            TargetMS3Points = new PointPairList();
            foreach (MS3Event scan in TargetMS3s)
            {
                TargetMS3Points.Add(scan.RetentionTime, MaxIntensity * 0.5);
            }

            //Go through the target MS2s and match fragment and SPS ions when available
            List<MS2Event> targetMS2sDesc = TargetMS2sByTriggerInt.Values.Reverse().ToList();
            foreach (MS2Event ms2 in targetMS2sDesc)
            {
                //This will populate the matched MS2 points and matched SPS ions, if there was an MS3
                ms2.PopulateMatchedPeaks(TargetFrags, Charge);

                //Add the Scan Event Line to the GUI
                ScanEventLines.Add(new ScanEventLine(ms2));
            }
        }

        public void PopulatePrimingData()
        {
            //Populate the XIC data for the Trigger peptides
            TriggerMS1XicPoints = new PointPairList();
            foreach (KeyValuePair<double, double> kvp in TriggerMS1xic)
            {
                TriggerMS1XicPoints.Add(kvp.Key, kvp.Value);
            }

            //Look for the maximum trigger intensity
            double maxTime = 0;
            double maxIntensity = 0;
            foreach (MS2Event scan in TriggerMS2s)
            {
                if (scan.MS1Intensity > maxIntensity)
                {
                    maxTime = scan.RetentionTime;
                    maxIntensity = scan.MS1Intensity;
                }
            }

            //Set the max intensity and the max retention time for use later
            MaxIntensity = maxIntensity;
            MaxRetentionTime = maxTime;

            //Add the points to mark when the trigger ms2 scans were performed
            TriggerMS2Points = new PointPairList();
            foreach (MS2Event scan in TriggerMS2s)
            {
                TriggerMS2Points.Add(scan.RetentionTime, MaxIntensity * 1);
            }

            //Go through the trigger MS2s and match fragment ions
            List<MS2Event> triggerMS2Desc = TriggerMS2sByInt.Values.Reverse().ToList();
            foreach (MS2Event ms2 in triggerMS2Desc)
            {
                //This will populate the matched MS2 points and matched SPS ions, if there was an MS3
                ms2.PopulateMatchedPeaks(TriggerFrags, Charge);

                //Add the Scan Event Line to the GUI
                ScanEventLines.Add(new ScanEventLine(ms2));
            }

            AverageScanEventsLines(ScanEventLines, 5);
        }

        private void AverageScanEventsLines(List<ScanEventLine> scanEventLines, int topN)
        {
            if (scanEventLines.Count < topN)
            {
                topN = scanEventLines.Count;
            }

            Dictionary<string, Fragment> indexTargetFrags = new Dictionary<string, Fragment>();
            foreach(Fragment frag in TargetFrags)
            {
                indexTargetFrags.Add(frag.ToString(), frag);
            }
            


            TriggerCompositeSpectrum = new SortedList<double, double>();
            for (int i = 0; i < topN; i++)
            {
                ScanEventLine sel = scanEventLines[i];
                
                foreach(KeyValuePair<int, Dictionary<string, double>> kvp in sel.ScanEvent.MatchedFragDict)
                {
                    foreach(KeyValuePair<string, double> kvp2 in kvp.Value)
                    {
                        Fragment targetFrag = null;

                        if(indexTargetFrags.TryGetValue(kvp2.Key, out targetFrag))
                        {
                            double mz = targetFrag.ToMz(kvp.Key);
                            double intensity = kvp2.Value;

                            MzRange fragrange = new MzRange(mz, new Tolerance(ToleranceUnit.PPM, 10));

                            bool fragAdded = false;
                            foreach (KeyValuePair<double, double> kvp3 in TriggerCompositeSpectrum)
                            {
                                if (fragrange.Contains(kvp3.Key))
                                {
                                    TriggerCompositeSpectrum[kvp3.Key] += kvp3.Value;
                                    fragAdded = true;
                                    break;
                                }
                            }

                            if (!fragAdded)
                            {
                                TriggerCompositeSpectrum.Add(mz, intensity);
                            }
                        }
                    }
                }

            }

            foreach(KeyValuePair<double, double> kvp in TriggerCompositeSpectrum)
            {
                TriggerCompositeMS2Points.Add(kvp.Key, kvp.Value);
            }
        }

        public void UpdateTargetSPSIons(int numSPSIons)
        {
            SortedList<double, double> spectrumByInt = new SortedList<double, double>();
            foreach(KeyValuePair<double, double> kvp in TriggerCompositeSpectrum)
            {
                //dummy variable for the output
                double addDoub = 0;

                //This will be the double we will increment slowly so we don't add the same thing twice
                double testDoub = kvp.Value;
                while(spectrumByInt.TryGetValue(testDoub, out addDoub))
                {
                    testDoub += 0.01;
                }

                //Add the value
                spectrumByInt.Add(testDoub, kvp.Key);
            }

            if(spectrumByInt.Count < numSPSIons)
            {
                numSPSIons = spectrumByInt.Count;
            }

            TargetSPSIons.Clear();
            List<double> spsIonMZs = spectrumByInt.Values.Reverse().ToList();
            for (int i = 0;i<numSPSIons;i++)
            {
                TargetSPSIons.Add(spsIonMZs[i]);
            }

            foreach(MS2Event ms2 in TriggerMS2s)
            {
                ms2.PopulateMatchedSPSPeaks(TargetSPSIons);
            }
        }

        private ThermoMzPeak GetTallestPeak(MzRange range, ThermoSpectrum spectrum)
        {
            ThermoMzPeak retPeak = null;
            List<ThermoMzPeak> peaks = null;

            if(spectrum.TryGetPeaks(range, out peaks))
            {
                foreach(ThermoMzPeak peak in peaks)
                {
                    if(retPeak == null || peak.Intensity > retPeak.Intensity)
                    {
                        retPeak = peak;
                    }
                }
            }

            return retPeak;
        }
    }
}
