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

        public double TriggerMZ { get; set; }
        public double TargetMZ { get; set; }

        public Peptide Trigger { get; set; }
        public Peptide Target { get; set; }

        public SortedList<double, double> TriggerMS1xic {get; set;}
        public SortedList<double, double> TargetMS1xic { get; set; }

        public List<MS2Event> TriggerMS2s { get; set; }
        public List<MS2Event> TargetMS2s { get; set; }
        public List<MS3Event> TargetMS3s { get; set; }

        public MS2Event AvgTriggerMS2 { get; set; }

        public List<Fragment> TriggerFrags { get; set; }
        public List<Fragment> TargetFrags { get; set; }

        public List<double> TriggerIons { get; set; }
        public List<double> TargetSPSIons { get; set; }

        private Dictionary<int, Modification> DynModDict { get; set; }

        public List<ScanEventLine> ScanEventLines { get; set; }

        public PointPairList TriggerMS1XicPoints { get; set; }
        public PointPairList TargetMS1XicPoints { get; set; }

        public TargetPeptide(string peptideString, int charge, Dictionary<Modification, string> staticMods, Dictionary<Modification, char> dynMods, List<double> targetSPSIons)
        {
            //Save the original peptide string
            PeptideString = peptideString;
            Charge = charge;
            TargetSPSIons = targetSPSIons;
            TriggerIons = new List<double>();

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
        }

        public void AddMS1XICPoint(ThermoSpectrum spectrum, double rt)
        {
            MzRange triggerRange = new MzRange(TriggerMZ, new Tolerance(ToleranceUnit.PPM, 15));
            MzRange targetRange = new MzRange(TargetMZ, new Tolerance(ToleranceUnit.PPM, 15));

            ThermoMzPeak triggerPeak = spectrum.GetClosestPeak(triggerRange);
            ThermoMzPeak targetPeak = spectrum.GetClosestPeak(targetRange);

            if(triggerPeak != null)
            {
                TriggerMS1xic.Add(rt, triggerPeak.Intensity);
            }

            if(targetPeak != null)
            {
                TargetMS1xic.Add(rt, targetPeak.Intensity);
            }
        }

        public void AddTriggerData(int scanNumber, ThermoSpectrum spectrum, double rt, double it)
        {
            List<ThermoMzPeak> peaks = null;
            if(spectrum.TryGetPeaks(0,2000, out peaks))
            {
                TriggerMS2s.Add(new MS2Event(scanNumber, rt, peaks, it));
            }
        }

        public void AddTargetData(int scanNumber, ThermoSpectrum spectrum, double rt, double it)
        {
            List<ThermoMzPeak> peaks = null;
            if (spectrum.TryGetPeaks(0, 2000, out peaks))
            {
                TargetMS2s.Add(new MS2Event(scanNumber, rt, peaks, it));
            }
        }

        public void AddTargetData(int scanNumber, ThermoSpectrum spectrum, double rt, double it, int ms3scanNumber, ThermoSpectrum ms3spectrum, double ms3rt, double ms3it, int numSPSIons, Dictionary<string, double> quantChannelDict)
        {
            //See if there are any peaks within the MS2 event
            List<ThermoMzPeak> peaks = null;
            if (spectrum.TryGetPeaks(0, 2000, out peaks))
            {
                MS2Event ms2Event = new MS2Event(scanNumber, rt, peaks, it);
                TargetMS2s.Add(ms2Event);

                //If we have an MS3 then we will add the MS3 event to the MS2 event
                peaks = null;
                if (ms3spectrum.TryGetPeaks(0, 2000, out peaks))
                { 
                    MS3Event ms3Event = new MS3Event(ms3scanNumber, ms3rt, ms3spectrum, peaks, ms3it, quantChannelDict, numSPSIons);
                    ms2Event.AddMS3Event(ms3Event);
                    TargetMS3s.Add(ms3Event);
                }
            }
        }

        public void PopulateTargetSPSIons()
        {
            if (TargetSPSIons.Count > 0)
            {
                return;
            }
            else if (TargetMS2s.Count > 0)
            {
                //Calculate the best fragment ions based on spectra...this will be more work - but should be worth it. 
            }
            else
            {
                //Determine the maximum charge
                int maxCharge = Charge - 1;
                if (maxCharge > 2) { maxCharge = 2; }

                //Add in the fragment ions
                for (int i = 1; i <= maxCharge; i++)
                {
                    foreach(Fragment frag in TargetFrags)
                    {
                        double fragMZ = frag.ToMz(i);

                        if(fragMZ>400 && fragMZ<2000 && frag.Number >= 2)
                        {
                            TargetSPSIons.Add(frag.ToMz(i));
                        }
                    }
                }
            }
        }

        public void PopulateTriggerIons()
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
            TriggerMS1XicPoints = new PointPairList();
            foreach(KeyValuePair<double, double> kvp in TriggerMS1xic)
            {
                TriggerMS1XicPoints.Add(kvp.Key, kvp.Value);
            }

            TargetMS1XicPoints = new PointPairList();
            foreach (KeyValuePair<double, double> kvp in TargetMS1xic)
            {
                TargetMS1XicPoints.Add(kvp.Key, kvp.Value);
            }

            foreach (MS2Event ms2 in TargetMS2s)
            {
                ScanEventLines.Add(new ScanEventLine(ms2));
            }
        }

        public void PopulatePrimingData()
        {

        }
    }
}
