﻿using System;
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
        public string ProteinString { get; set; }
        public int Charge { get; set; }

        public double MassShift { get; set; }

        Tolerance FragmentTol { get; set; }

        public double MaxIntensity { get; set; }
        public double MaxRetentionTime { get; set; }

        public double StartRetentionTime { get; set; }
        public double EndRetentionTime { get; set; }

        public double StartSelectionTime { get; set; }
        public double EndSelectionTime { get; set; }

        public double RTWindow { get; set; }
        public double MethodEndTime { get; set; }

        public double TriggerMZ { get; set; }
        public double TargetMZ { get; set; }

        Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> ModificationDict { get; set; }

        public Peptide Trigger { get; set; }
        public Peptide Target { get; set; }

        public SortedList<double, double> TriggerMS1xic { get; set; }
        public SortedList<double, double> TargetMS1xic { get; set; }

        public SortedList<double, ThermoMzPeak> TriggerCompositeSpectrum { get; set; }
        public SortedList<double, ThermoMzPeak> TargetCompositeSpectrum { get; set; }

        public SortedDictionary<int, double> MS1toTriggerInt { get; set; }

        public SortedDictionary<double, MS2Event> TriggerMS2sByInt { get; set; }
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

        public Dictionary<double, int> TriggerIonsWithCharge { get; set; }
        public Dictionary<double, int> TargetSPSIonsWithCharge { get; set; }

        private Dictionary<string, Dictionary<int, Modification>> DynModDict { get; set; }

        public List<ScanEventLine> ScanEventLines { get; set; }
        public List<ScanEventLine> SelectedScanEventLines { get; set; }

        public PointPairList TriggerMS1XicPoints { get; set; }
        public PointPairList TargetMS1XicPoints { get; set; }
        public PointPairList TriggerMS2Points { get; set; }
        public PointPairList TargetMS2Points { get; set; }
        public PointPairList TargetMS3Points { get; set; }
        public PointPairList TargetCompositeMS2Points { get; set; }

        public bool Analyzing { get; set; }

        public double EO {get; set;}

        public MzRange SPSInclRange { get; set; } //Upon import set the min to the value, unless higher than Prec MZ is chosen - then switch

        public MzRange PrecExclRange { get; set; }


        public TargetPeptide(string peptideString, string protein, int charge, Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modificationDict, List<double> targetSPSIons, 
            List<double> triggerFragIons, double startTime, double endTime, Tolerance fragTol, double spsMin, double spsMax, double precExLow, double precExHigh,double eo, bool spsAbovePrec)
        {
            //Save the original peptide string
            PeptideString = peptideString;
            ProteinString = protein;
            Charge = charge;
            TargetSPSIons = targetSPSIons;
            TriggerIons = triggerFragIons;

            FragmentTol = fragTol;

            StartRetentionTime = startTime;
            EndRetentionTime = endTime;

            StartSelectionTime = 0;
            EndSelectionTime = 0;

            ModificationDict = modificationDict;

            //Build the dynamic modification dictionary and return the stripped string
            string strippedPepString = BuildDynamicModDict(peptideString, modificationDict["Dynamic"]);

            //Create the target and the trigger peptides
            Trigger = new Peptide(strippedPepString);
            Target = new Peptide(strippedPepString);

            //Add the modifications to the peptide
            Trigger = AddModifications(Trigger, modificationDict["Static"], "Trigger");
            Target = AddModifications(Target, modificationDict["Static"], "Target");

            TriggerMZ = Trigger.ToMz(Charge);
            TargetMZ = Target.ToMz(Charge);

            //Calculte the mass shift for the peptide
            MassShift = (Target.MonoisotopicMass - Trigger.MonoisotopicMass) / Charge;

            //Populate the fragments
            TriggerFrags = PopulateFragments(Trigger, "Trigger");
            TargetFrags = PopulateFragments(Target, "Target");

            TriggerMS1xic = new SortedList<double, double>();
            TargetMS1xic = new SortedList<double, double>();

            TriggerMS2s = new List<MS2Event>();
            TargetMS2s = new List<MS2Event>();
            TargetMS3s = new List<MS3Event>();

            ScanEventLines = new List<ScanEventLine>();
            SelectedScanEventLines = new List<ScanEventLine>();

            TriggerMS1XicPoints = new PointPairList();
            TargetMS1XicPoints = new PointPairList();
            TriggerMS2Points = new PointPairList();
            TargetMS2Points = new PointPairList();
            TargetMS3Points = new PointPairList();
            TargetCompositeMS2Points = new PointPairList();

            MS1toTriggerInt = new SortedDictionary<int, double>();

            TriggerMS2sByInt = new SortedDictionary<double, MS2Event>();

            TargetMS2sByTriggerInt = new SortedDictionary<double, MS2Event>();

            TriggerCompositeSpectrum = new SortedList<double, ThermoMzPeak>();
            TargetCompositeSpectrum = new SortedList<double, ThermoMzPeak>();

            TriggerIonsWithCharge = new Dictionary<double, int>();
            TargetSPSIonsWithCharge = new Dictionary<double, int>();

            InitializeTargetSPSFrags();

            Analyzing = false;

            if(spsAbovePrec && this.TargetMZ > spsMin)
            {
                spsMin = this.TargetMZ;
            }

            SPSInclRange = new MzRange(spsMin, spsMax);

            PrecExclRange = new MzRange(this.TargetMZ - precExLow, this.TargetMZ + precExHigh);

            EO = eo;
        }

        public TargetPeptide(string peptideString, string protein, int charge, Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modificationDict, Dictionary<double,int> targetSPSIons, Dictionary<double,int> triggerFragIons, double startTime, double endTime, Tolerance fragTol, double spsMin, double spsMax, double precExLow, double precExHigh, double eo, bool spsAbovePrec)
        {
            //Save the original peptide string
            PeptideString = peptideString;
            ProteinString = protein;
            Charge = charge;

            TargetSPSIonsWithCharge = targetSPSIons;
            TriggerIonsWithCharge = triggerFragIons;

            TargetSPSIons = TargetSPSIonsWithCharge.Keys.ToList();
            TriggerIons = TriggerIonsWithCharge.Keys.ToList();

            FragmentTol = fragTol;

            StartRetentionTime = startTime;
            EndRetentionTime = endTime;

            StartSelectionTime = 0;
            EndSelectionTime = 0;

            ModificationDict = modificationDict;

            //Build the dynamic modification dictionary and return the stripped string
            string strippedPepString = peptideString;

            Dictionary<string, Dictionary<Modification, string>> outDict = null;
            if (modificationDict.TryGetValue("Dynamic", out outDict))
            {
                strippedPepString = BuildDynamicModDict(peptideString, modificationDict["Dynamic"]);
            }

            //Create the target and the trigger peptides
            Trigger = new Peptide(strippedPepString);
            Target = new Peptide(strippedPepString);

            //Add the modifications to the peptide
            Trigger = AddModifications(Trigger, modificationDict["Static"], "Trigger");
            Target = AddModifications(Target, modificationDict["Static"], "Target");

            TriggerMZ = Trigger.ToMz(Charge);
            TargetMZ = Target.ToMz(Charge);

            //Calculte the mass shift for the peptide
            MassShift = (Target.MonoisotopicMass - Trigger.MonoisotopicMass) / Charge;

            //Populate the fragments
            TriggerFrags = PopulateFragments(Trigger, "Trigger");
            TargetFrags = PopulateFragments(Target, "Target");

            TriggerMS1xic = new SortedList<double, double>();
            TargetMS1xic = new SortedList<double, double>();

            TriggerMS2s = new List<MS2Event>();
            TargetMS2s = new List<MS2Event>();
            TargetMS3s = new List<MS3Event>();

            ScanEventLines = new List<ScanEventLine>();
            SelectedScanEventLines = new List<ScanEventLine>();

            TriggerMS1XicPoints = new PointPairList();
            TargetMS1XicPoints = new PointPairList();
            TriggerMS2Points = new PointPairList();
            TargetMS2Points = new PointPairList();
            TargetMS3Points = new PointPairList();
            TargetCompositeMS2Points = new PointPairList();

            MS1toTriggerInt = new SortedDictionary<int, double>();

            TriggerMS2sByInt = new SortedDictionary<double, MS2Event>();

            TargetMS2sByTriggerInt = new SortedDictionary<double, MS2Event>();

            TriggerCompositeSpectrum = new SortedList<double, ThermoMzPeak>();
            TargetCompositeSpectrum = new SortedList<double, ThermoMzPeak>();

            InitializeTargetSPSFrags();

            if (spsAbovePrec && this.TargetMZ > spsMin)
            {
                spsMin = this.TargetMZ;
            }

            SPSInclRange = new MzRange(spsMin, spsMax);

            PrecExclRange = new MzRange(this.TargetMZ - precExLow, this.TargetMZ + precExHigh);

            EO = eo;
        }

        public void AddMS1XICPoint(ThermoSpectrum spectrum, double rt, int scanNumber)
        {
            MzRange triggerRange = new MzRange(TriggerMZ, new Tolerance(ToleranceUnit.PPM, 15));
            MzRange targetRange = new MzRange(TargetMZ, new Tolerance(ToleranceUnit.PPM, 15));

            ThermoMzPeak triggerPeak = GetTallestPeak(triggerRange, spectrum);
            ThermoMzPeak targetPeak = GetTallestPeak(targetRange, spectrum);

            if(triggerPeak != null)
            {
                if(triggerPeak.Intensity > MaxIntensity)
                {
                    MaxIntensity = triggerPeak.Intensity;
                }

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
                double triggerMS1Int = 0;
                if(!MS1toTriggerInt.TryGetValue(ms1ScanNumber, out triggerMS1Int))
                {
                    triggerMS1Int = 0;
                }

                MS2Event ms2Event = new MS2Event(scanNumber, rt, peaks, it, triggerMS1Int, FragmentTol);
                TriggerMS2s.Add(ms2Event);

                MS2Event outEvent = null; 
                while(TriggerMS2sByInt.TryGetValue(triggerMS1Int, out outEvent))
                {
                    triggerMS1Int++;
                }

                TriggerMS2sByInt.Add(triggerMS1Int, ms2Event);
            }
        }

        public void AddTargetData(int ms1ScanNumber, int scanNumber, ThermoSpectrum spectrum, double rt, double it)
        {
            List<ThermoMzPeak> peaks = null;
            if (spectrum.TryGetPeaks(0, 2000, out peaks))
            {
                double triggerInt = 0;
                if (MS1toTriggerInt.TryGetValue(ms1ScanNumber, out triggerInt))
                {
                    MS2Event ms2Event = new MS2Event(scanNumber, rt, peaks, it, triggerInt, FragmentTol);
                    TargetMS2s.Add(ms2Event);

                    MS2Event outEvent = null;
                    while (TargetMS2sByTriggerInt.TryGetValue(triggerInt, out outEvent))
                    {
                        triggerInt += 1;
                    }

                    TargetMS2sByTriggerInt.Add(triggerInt, ms2Event);
                }
            }
        }

        public void AddTargetData(int ms1ScanNumber, int scanNumber, ThermoSpectrum spectrum, double rt, double it, int ms3scanNumber, ThermoSpectrum ms3spectrum, double ms3rt, double ms3it, List<double> spsIons, Dictionary<string, double> quantChannelDict)
        {
            if (MS1toTriggerInt == null) { return; }

            if (TargetMS2s == null) { return; }

            if(TargetMS3s == null) { return; }

            //See if there are any peaks within the MS2 event
            List<ThermoMzPeak> peaks = null;
            if (spectrum.TryGetPeaks(0, 2000, out peaks))
            {
                double triggerInt = 0;

                if(MS1toTriggerInt.TryGetValue(ms1ScanNumber, out triggerInt))
                {
                    triggerInt = MS1toTriggerInt[ms1ScanNumber];
                }

                MS2Event ms2Event = new MS2Event(scanNumber, rt, peaks, it, triggerInt, FragmentTol);
                TargetMS2s.Add(ms2Event);

                MS2Event outEvent = null;
                while(TargetMS2sByTriggerInt.TryGetValue(triggerInt, out outEvent))
                {
                    triggerInt += 1;
                }

                TargetMS2sByTriggerInt.Add(triggerInt, ms2Event);

                //If we have an MS3 then we will add the MS3 event to the MS2 event
                peaks = null;
                if (ms3spectrum.TryGetPeaks(0, 2000, out peaks))
                { 
                    MS3Event ms3Event = new MS3Event(ms3scanNumber, ms3rt, ms3spectrum, peaks, ms3it, quantChannelDict, spsIons);
                    ms2Event.AddMS3Event(ms3Event);
                    TargetMS3s.Add(ms3Event);

                    //Calculate Isolation Specificity
                    double isoSpec = CalculateIsolationSpecificity(spectrum, spsIons);
                    ms2Event.IsolationSpecificity = isoSpec;
                }
            }
        }

        private double CalculateIsolationSpecificity(ThermoSpectrum targetMS2Spectrum, List<double> spsIons)
        {
            double isoSpec = 0;

            if(spsIons.Count == 0)
            {
                return isoSpec;
            }

            double isoQ = 0.86;
            double isoWidth = 3;
            double totalPeakInt = 0;
            double matchedPeakInt = 0;

            spsIons.Sort();

            double firstMZ = spsIons.ElementAt(0);

            foreach(double mz in spsIons)
            {
                double spsWidth = isoWidth;

                if(mz != firstMZ)
                {
                    spsWidth = isoWidth / ((firstMZ * isoQ) / mz);
                }

                double minMZ = mz - spsWidth;
                double maxMZ = mz + spsWidth;

                MzRange totalRange = new MzRange(minMZ, maxMZ);
                MzRange matchRange = new MzRange(mz, FragmentTol);

                ThermoMzPeak matchedIon = GetTallestPeak(matchRange, targetMS2Spectrum); //TODO: Figure out why we get null for a matched ion...it is selected so there must be something there. 

                if(matchedIon != null)
                {
                    matchedPeakInt += matchedIon.SignalToNoise;

                    List<ThermoMzPeak> totalPeaks = null;
                    if (targetMS2Spectrum.TryGetPeaks(totalRange, out totalPeaks))
                    {
                        foreach (ThermoMzPeak peak in totalPeaks)
                        {
                            totalPeakInt += peak.SignalToNoise;
                        }
                    }
                }
            }

            isoSpec = matchedPeakInt / totalPeakInt;

            return isoSpec;
        }

        private void PopulateTargetSPSIons(int numSPSIons, bool force = false) //NOTE:This is where SPS Ion Filters are applied
        {
            if (TargetSPSIons.Count > 0 && !force)
            {
                return;
            }
            else
            {
                TargetSPSIons = new List<double>();
                TargetSPSIonsWithCharge = new Dictionary<double, int>();
                InitializeTargetSPSFrags();

                //Determine the maximum charge
                int maxCharge = Charge - 1;
                if (maxCharge > 2) { maxCharge = 2; }

                SortedList<double, double> mzsToAdd = new SortedList<double, double>();
                SortedDictionary<double, Dictionary<double, int>> mzsToAddWithCharge = new SortedDictionary<double, Dictionary<double, int>>();
                
                //Add in the fragment ions
                for (int i = 1; i <= maxCharge; i++)
                {
                    //Cycle through each fragment ions in the target fragments
                    foreach(Fragment frag in TargetFrags)
                    {
                        //Calculate the fragment charge and the distance from the precursor
                        double fragMZ = frag.ToMz(i);
                        double distanceFromPrec = fragMZ - TargetMZ;

                        if (SPSInclRange.Contains(fragMZ) && !PrecExclRange.Contains(fragMZ))
                        {
                            //check if there is another entry with the same distance from the precursor
                            double outDoub = 0;
                            if (!mzsToAdd.TryGetValue(distanceFromPrec, out outDoub))
                            {
                                mzsToAdd.Add(distanceFromPrec, fragMZ);
                                mzsToAddWithCharge.Add(distanceFromPrec, new Dictionary<double, int>());
                                mzsToAddWithCharge[distanceFromPrec].Add(fragMZ, i);

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
                    TargetSPSIonsWithCharge.Add(mzsToAddWithCharge.ElementAt(i).Value.ElementAt(0).Key, mzsToAddWithCharge.ElementAt(i).Value.ElementAt(0).Value);
                }
            }
        }

        private void PopulateTriggerIons(double triggerIonMinMZ, double triggerIonMaxMZ, int numIons, bool force = false)
        {
            if (TriggerIons.Count > 0 && !force)
            {
                return;
            }
            else
            {
                TriggerIons = new List<double>();
                TriggerIonsWithCharge = new Dictionary<double, int>();

                //Determine the maximum charge
                int maxCharge = Charge - 1;
                if (maxCharge > 2) { maxCharge = 2; }

                SortedList<double, double> mzsToAdd = new SortedList<double, double>();
                SortedDictionary<double, Dictionary<double, int>> mzsToAddWithCharge = new SortedDictionary<double, Dictionary<double, int>>();

                //Add in the fragment ions
                for (int i = 1; i <= maxCharge; i++)
                {
                    //Cycle through each fragment ions in the target fragments
                    foreach (Fragment frag in TriggerFrags)
                    {
                        //Calculate the fragment charge and the distance from the precursor
                        double fragMZ = frag.ToMz(i);
                        double distanceFromPrec = fragMZ - TriggerMZ;

                        bool addTriggerIon = true;
                        if(fragMZ <= triggerIonMinMZ || fragMZ >= triggerIonMaxMZ)
                        {
                            addTriggerIon = false;
                        }

                        //check if there is another entry with the same distance from the precursor
                        if (addTriggerIon)
                        {
                            double outDoub = 0;
                            if (!mzsToAdd.TryGetValue(distanceFromPrec, out outDoub))
                            {
                                mzsToAdd.Add(distanceFromPrec, fragMZ);
                                mzsToAddWithCharge.Add(distanceFromPrec, new Dictionary<double, int>());
                                mzsToAddWithCharge[distanceFromPrec].Add(fragMZ, i);
                            }
                        }
                    }
                }

                if (mzsToAdd.Count < numIons)
                {
                    numIons = mzsToAdd.Count;
                }

                for (int i = 0; i < numIons; i++)
                {
                    TriggerIons.Add(mzsToAdd.ElementAt(i).Value);
                    TriggerIonsWithCharge.Add(mzsToAddWithCharge.ElementAt(i).Value.ElementAt(0).Key, mzsToAddWithCharge.ElementAt(i).Value.ElementAt(0).Value);
                }
            }
        }

        public void PopulateTriggerAndTargetIons(int numTriggerIons, int numTargetIons, double triggerIonMinMZ, double triggerIonMaxMZ, bool force = false)
        {
            PopulateTriggerIons(triggerIonMinMZ, triggerIonMaxMZ, numTriggerIons, force);
            PopulateTargetSPSIons(numTargetIons, force);
        }

        private string BuildDynamicModDict(string peptideString, Dictionary<string, Dictionary<Modification, string>> allDynMods)
        {
            //This will be the dictionary that will tell us where to place the modifications
            DynModDict = new Dictionary<string, Dictionary<int, Modification>>();
            DynModDict.Add("Trigger", new Dictionary<int, Modification>());
            DynModDict.Add("Target", new Dictionary<int, Modification>());


            //Cycle through each peptide and set the appropriate modifications
            foreach (KeyValuePair<string, Dictionary<Modification, string>> kvp in allDynMods)
            {
                string peptideType = kvp.Key; //Trigger or target

                Dictionary<Modification, string> dynMods = kvp.Value;
                Dictionary<Modification, char> dynModsChar = new Dictionary<Modification, char>();
                List<char> modsChars = new List<char>();
                foreach(KeyValuePair<Modification, string> modPair in dynMods)
                {
                    dynModsChar.Add(modPair.Key, modPair.Value.ElementAt(0));
                    modsChars.Add(modPair.Value.ElementAt(0));
                }

                string currentPeptideString = peptideString;
                List<char> charToRemove = new List<char>();
                for (int i = 0; i < peptideString.Length; i++)
                {
                    char currentChar = peptideString.ElementAt(i);
                    if (!char.IsLetterOrDigit(currentChar))
                    {
                        if(!modsChars.Contains(currentChar) && !charToRemove.Contains(currentChar))
                        {
                            charToRemove.Add(currentChar);
                        }
                    }
                }


                foreach(char removeChar in charToRemove)
                {
                    currentPeptideString = currentPeptideString.Replace(removeChar.ToString(), "");
                }

                //Kepp track of the total modifications
                int totalMods = 0;

                //Cycle through the string to see if there is a dynamic modification
                for (int i = 0; i < currentPeptideString.Length; i++)
                {
                    //If there is a dynamic modification then save its location to a dictionary
                    if (dynModsChar.Values.Contains(currentPeptideString.ElementAt(i)))
                    {
                        //Increment and save the index
                        totalMods++;
                        int addIndexToMod = i - totalMods + 1;

                        //Get the actual modification -- this is a bit clunky, but there shouldn't be too many modifications to cycle through
                        Modification addMod = null;
                        foreach (KeyValuePair<Modification, char> kvp2 in dynModsChar)
                        {
                            //Grab the modification you will be using
                            if (kvp2.Value == currentPeptideString.ElementAt(i))
                            {
                                addMod = kvp2.Key;
                            }
                        }

                        //Add the modification with the correct index
                        DynModDict[peptideType].Add(addIndexToMod, addMod);
                    }
                }
            }
            
            //Save the stripped peptide which will be used to make the peptides
            string strippedPeptideString = peptideString;

            //Remove each of the dynamic modification chars from the string so that we can make a peptide
            //This will go through twice...but that shouldn't be a big deal
            foreach (KeyValuePair<string, Dictionary<Modification, string>> kvp in allDynMods)
            {
                //This isn't used here
                string peptideType = kvp.Key; //Trigger or target

                Dictionary<Modification, string> dynMods = kvp.Value;
                foreach (string mod in dynMods.Values)
                {
                    strippedPeptideString = strippedPeptideString.Replace(mod, "");
                }
            }

            //Return the stripped peptide sequence. 
            return strippedPeptideString;
        }

        private Peptide AddModifications(Peptide peptide, Dictionary<string, Dictionary<Modification, string>> allStaticMods, string type)
        {
            //Cycle through the static mods and only add the static mod to either a trigger or target peptide.
            foreach(KeyValuePair<Modification, string> kvp in allStaticMods[type])
            {
                peptide.SetModification(kvp.Key);
            }

            //Cycle through the dynamic mods and add them to both of the peptides. 
            if(DynModDict != null)
            {
                foreach (KeyValuePair<int, Modification> kvp in DynModDict[type])
                {
                    if (kvp.Key == 0)
                    {
                        peptide.SetModification(kvp.Value, Terminus.N);
                    }
                    else
                    {
                        peptide.SetModification(kvp.Value, kvp.Key);
                    }

                }
            }

            return peptide;
        }

        private List<Fragment> PopulateFragments(Peptide peptide, string type)
        {
            List<Fragment> retFrags = new List<Fragment>();
            List<Fragment> candidateFrags = new List<Fragment>();
            List<Fragment> frags = peptide.Fragment(FragmentTypes.b | FragmentTypes.y).ToList();

            //TODO: Add phospho neutral loss fragments. 
            double phosphoNL = 97.9766;
            List<Fragment> allFrags = new List<Fragment>();
            foreach(Fragment frag in frags)
            {
                allFrags.Add(frag);

                if(frag.Parent.Sequence != frag.Parent.SequenceWithModifications)
                {
                    List<IMass> modMasses = frag.GetModifications().ToList();
                    foreach (IMass mod in modMasses)
                    {
                        string modStr = mod.ToString();
                        if (mod.ToString().Equals("Phos"))
                        {
                            FragmentTypes localFragType = frag.Type;
                            if (frag.Type == FragmentTypes.b)
                            {
                                localFragType = FragmentTypes.bNeuLoss;
                            }
                            else if (frag.Type == FragmentTypes.y)
                            {
                                localFragType = FragmentTypes.yNeuLoss;
                            }

                            Fragment nLFrag = new Fragment(localFragType, frag.Number, frag.MonoisotopicMass - phosphoNL, frag.Parent);
                            allFrags.Add(nLFrag);
                        }
                    }
                }          
            }

            foreach (Fragment frag in allFrags)
            {
                if(Math.Abs(frag.Number) > 1)
                {
                    candidateFrags.Add(frag);
                }
            }

            //Ensure Lysine residues for any Y ions of target peptides
            if (type == "Target")
            {
                foreach(Fragment frag in candidateFrags)
                {
                    if(frag.GetSequence().Contains("K") || frag.Type == FragmentTypes.b || frag.Type == FragmentTypes.bNeuLoss)
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

        public void PopulatePrimingData(double methodLength)
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
            //MaxIntensity = maxIntensity; TODO: Change this as it was setting the max intensity back to 0
            MaxRetentionTime = maxTime; //TODO: Change this
            MethodEndTime = methodLength;

            //Only change the retention times if we found the peptide within the run
            //TODO: Change this so that it can be a user option
            UpdateRTWindow(maxTime);

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

        public void AverageScanEventsLines(List<ScanEventLine> scanEventLines, int topN, bool includeAll = false)
        {
            //Determine if you have less scan event lines than top N
            if (scanEventLines.Count < topN || includeAll)
            {
                topN = scanEventLines.Count;
            }

            //Index the target fragments for easier lookup later
            Dictionary<string, Fragment> indexTargetFrags = new Dictionary<string, Fragment>();
            foreach(Fragment frag in TargetFrags)
            {
                Fragment outFrag = null;
                if(!indexTargetFrags.TryGetValue(frag.ToString(), out outFrag))
                {
                    indexTargetFrags.Add(frag.ToString(), frag);
                }
                    
            }

            //Index the trigger fragments for easier lookup later
            Dictionary<string, Fragment> indexTriggerFrag = new Dictionary<string, Fragment>();
            foreach (Fragment frag in TriggerFrags)
            {
                Fragment outFrag = null;
                if (!indexTriggerFrag.TryGetValue(frag.ToString(), out outFrag))
                {
                    indexTriggerFrag.Add(frag.ToString(), frag);
                }
            }

            //Make the objects that will hold the trigger and the target spectrum
            TriggerCompositeSpectrum = new SortedList<double, ThermoMzPeak>();
            TargetCompositeSpectrum = new SortedList<double, ThermoMzPeak>();
            TargetCompositeMS2Points = new PointPairList();

            List<double> scanRTs = new List<double>();

            foreach(ScanEventLine sel in scanEventLines)
            {
                sel.Include = false;
            }

            //Average the spectrum
            for (int i = 0; i < topN; i++)
            {
                //Get the scan event line that you are going to average
                ScanEventLine sel = scanEventLines[i];

                sel.Include = true;

                scanRTs.Add(double.Parse(sel.MS2RetentionTime));

                //Cycle through the matched fragment dictionary
                foreach (KeyValuePair<int, Dictionary<Fragment, double>> kvp in sel.ScanEvent.MatchedFragDict)
                {
                    //The current charge
                    int charge = kvp.Key;

                    //We are now looking at a single charge state
                    foreach(KeyValuePair<Fragment, double> kvp2 in kvp.Value)
                    {
                        //The fragment from matched spectrum
                        string fragName = kvp2.Key.ToString();
                        Fragment triggerFrag = null;

                        //We will see if this fragment exsists within the target fragment list...it should
                        if(indexTriggerFrag.TryGetValue(fragName, out triggerFrag))
                        {
                            //We want to use the same list for the trigger and target fragments so calculate both m/z values here
                            double triggerMZ = triggerFrag.ToMz(charge);

                            //The same intensity will be used for each
                            double intensity = kvp2.Value;

                            //Calculate a narrow mass range to look for the peak
                            MzRange triggerRange = new MzRange(triggerMZ, FragmentTol);

                            //Then deal with the trigger peptide
                            bool triggerAdded = false;
                            foreach (KeyValuePair<double, ThermoMzPeak> kvp3 in TriggerCompositeSpectrum)
                            {
                                double testMZ = kvp3.Key;
                                ThermoMzPeak testPeak = kvp3.Value;

                                if (triggerRange.Contains(testMZ))
                                {
                                    TriggerCompositeSpectrum[testMZ].Intensity += testPeak.Intensity;
                                    triggerAdded = true;
                                    break;
                                }
                            }


                            if (!triggerAdded)
                            {
                                TriggerCompositeSpectrum.Add(triggerMZ, new ThermoMzPeak(triggerMZ, intensity, charge: charge));
                            }


                            Fragment targetFrag = null;
                            if (indexTargetFrags.TryGetValue(fragName, out targetFrag))
                            {
                                double targetMZ = targetFrag.ToMz(charge);
                                MzRange targetRange = new MzRange(targetMZ, FragmentTol);

                                //Deal with the target first to try and see if you can add the peak
                                bool targetAdded = false;
                                foreach (KeyValuePair<double, ThermoMzPeak> kvp3 in TargetCompositeSpectrum)
                                {
                                    double testMZ = kvp3.Key;
                                    ThermoMzPeak testPeak = kvp3.Value;

                                    if (targetRange.Contains(testMZ))
                                    {
                                        TargetCompositeSpectrum[testMZ].Intensity += testPeak.Intensity;
                                        targetAdded = true;
                                        break;
                                    }
                                }

                                if (!targetAdded)
                                {
                                    TargetCompositeSpectrum.Add(targetMZ, new ThermoMzPeak(targetMZ, intensity, charge: charge));
                                }
                            } 
                        }
                    }
                }

                //SelectedScanEventLines.Add(sel);
            }

            //Update the RT window based on the User input
            if(scanRTs.Count > 0 )
            {
                double avgRT = scanRTs.Average();
                UpdateRTWindow(avgRT, force: includeAll);

                StartSelectionTime = scanRTs.Min() - 0.01;
                EndSelectionTime = scanRTs.Max() + 0.01;
            }

            foreach (KeyValuePair<double, ThermoMzPeak> kvp in TargetCompositeSpectrum)
            {
                ThermoMzPeak peak = kvp.Value;
                TargetCompositeMS2Points.Add(peak.MZ, peak.Intensity);
            }
        }

        public void UpdateIons(int numTriggerIons, int numTargetIons, bool spsEdited = false)
        {

            //This will go through the composite spectrum and choose the topN most intense ions
            List<double> targetSPSIons = new List<double>();
            Dictionary<double, int> targetSPSIonsWithCharge = new Dictionary<double, int>();
            GetTopNIons(TargetMZ, TargetCompositeSpectrum, numTargetIons, out targetSPSIons, out targetSPSIonsWithCharge, "target");

            TargetSPSIons = targetSPSIons;
            TargetSPSIonsWithCharge = targetSPSIonsWithCharge;

            //This will go through the composite spectrum and choose the topN most intense ions
            List<double> triggerSPSIons = new List<double>();
            Dictionary<double, int> triggerSPSIonsWithCharge = new Dictionary<double, int>();
            GetTopNIons(TriggerMZ, TriggerCompositeSpectrum, numTriggerIons, out triggerSPSIons, out triggerSPSIonsWithCharge, "trigger");

            TriggerIons = triggerSPSIons;
            TriggerIonsWithCharge = triggerSPSIonsWithCharge;

            //This is to update the trigger ions correspond to the target SPS ions
            foreach (MS2Event ms2 in TriggerMS2s)
            {
                ms2.PopulateMatchedSPSPeaks(triggerSPSIons, spsEdited:spsEdited);
            }
        }

        private void GetTopNIons(double precMZ, SortedList<double, ThermoMzPeak> compositeSpectrum, int numIons, out List<double> mzValues, out Dictionary<double, int> mzValuesWithCharge, string type)
        {
            mzValues = new List<double>();
            mzValuesWithCharge = new Dictionary<double, int>();

            SortedList<double, ThermoMzPeak> spectrumByInt = new SortedList<double, ThermoMzPeak>();
            foreach (KeyValuePair<double, ThermoMzPeak> kvp in compositeSpectrum)
            {
                //Clarification
                double mz = kvp.Key;
                ThermoMzPeak peak = kvp.Value;

                //dummy variable for the output
                ThermoMzPeak outPeak = null;
                //This will be the double we will increment slowly so we don't add the same thing twice
                double testDoub = peak.Intensity;
                while (spectrumByInt.TryGetValue(testDoub, out outPeak))
                {
                    testDoub += 0.01;
                }

                //Add the value
                spectrumByInt.Add(testDoub, peak);
            }

            if (spectrumByInt.Count < numIons)
            {
                numIons = spectrumByInt.Count;
            }

            //Reverse the list base on the intensity so that we have the peaks most to least intense
            List<ThermoMzPeak> spsIonPeaks = spectrumByInt.Values.Reverse().ToList();

            int index = 0;
            int numIonsAdded = 0;
            while(numIonsAdded < numIons && index < spsIonPeaks.Count)
            {
                ThermoMzPeak peak = spsIonPeaks[index];
                double peakMZ = peak.MZ;
                int peakCharge = peak.Charge;

                if (SPSInclRange.Contains(peakMZ) && !PrecExclRange.Contains(peakMZ))
                {
                    mzValues.Add(peakMZ);
                    mzValuesWithCharge.Add(peakMZ, peakCharge);
                    numIonsAdded++;
                }
                
                index++;
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

        private void InitializeTargetSPSFrags()
        {
            TargetSPSFrags = new Dictionary<int, Dictionary<string, double>>();
            for (int i = 1; i < 10; i++)
            {
                TargetSPSFrags.Add(i, new Dictionary<string, double>());
            }
        }

        private void UpdateRTWindow(double centerRT, bool force = false)
        {
            //Only change the retention times if we found the peptide within the run
            //TODO: Change this so that it can be a user option
            if ((MaxRetentionTime > 0 && StartRetentionTime == -1) || force) 
            {
                StartRetentionTime = centerRT - (RTWindow / 2);
                EndRetentionTime = centerRT + (RTWindow / 2);

                if (EndRetentionTime > MethodEndTime) { EndRetentionTime = MethodEndTime; }
                if (StartRetentionTime < 0) { StartRetentionTime = 0; }
            }
        }

        public string GetTargetHeaders()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("PeptideString" + ",");
            sb.Append("ProteinString" + ",");
            sb.Append("Charge" + ",");
            sb.Append("StartRetentionTime" + ",");
            sb.Append("EndRetentionTime" + ",");
            sb.Append("EO" + ",");
            sb.Append("FragmentTol.Unit" + ",");
            sb.Append("FragmentTol.Value" + ",");

            sb.Append("ModificationDict" + ",");

            sb.Append("TriggerIonsWithCharge" + ",");

            sb.Append("TargetIonsWithCharge" + ",");

            return sb.ToString();
        }

        public string AnalysisToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PeptideString + ",");
            sb.Append(Charge + ",");
            sb.Append(MassShift + ",");
            sb.Append(FragmentTol.Unit + ",");
            sb.Append(FragmentTol.Value + ",");
            sb.Append(MaxIntensity + ",");
            sb.Append(MaxRetentionTime + ",");
            sb.Append(StartRetentionTime + ",");
            sb.Append(EndRetentionTime + ",");
            sb.Append(StartSelectionTime + ",");
            sb.Append(EndSelectionTime + ",");
            sb.Append(RTWindow + ",");
            sb.Append(MethodEndTime + ",");
            sb.Append(TriggerMZ + ",");
            sb.Append(TargetMZ + ",");
            sb.Append(ModificationDictToString(ModificationDict));

            //sb.Append(Trigger + ","); //This is the Trigger Peptide object - will probably need to re-make this upon re-import
            //sb.Append(Target + ","); //This is the Target Peptide object - will probably need to re-make this upon re-import



            return sb.ToString();
        }

        public string TargetToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PeptideString + ",");
            sb.Append(ProteinString + ",");
            sb.Append(Charge + ",");
            sb.Append(StartRetentionTime + ",");
            sb.Append(EndRetentionTime + ",");
            sb.Append(EO + ",");
            sb.Append(FragmentTol.Unit + ",");
            sb.Append(FragmentTol.Value + ",");

            sb.Append(ModificationDictToString(ModificationDict));

            sb.Append(IonsWithChargeToString(TriggerIonsWithCharge));

            sb.Append(IonsWithChargeToString(TargetSPSIonsWithCharge));

            return sb.ToString();
        }

        public string ModificationDictToString(Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modDict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<Modification, string>>> kvp in ModificationDict)
            {
                foreach (KeyValuePair<string, Dictionary<Modification, string>> kvp2 in kvp.Value)
                {
                    foreach (KeyValuePair<Modification, string> kvp3 in kvp2.Value)
                    {
                        sb.Append(kvp.Key + ">" + kvp2.Key + ">");
                        sb.Append(kvp3.Key.Name + ":" + kvp3.Key.MonoisotopicMass + ":" + kvp3.Key.Sites + ":" + kvp3.Value + "]");
                    }
                }
            }

            sb.Remove(sb.Length - 1, 1);

            sb.Replace(",", "|");
            sb.Replace(" ", "");

            sb.Append(',');

            return sb.ToString();
        }

        public string IonsWithChargeToString(Dictionary<double, int> fragDict)
        {
            StringBuilder sb = new StringBuilder();

            foreach(KeyValuePair<double, int> frag in fragDict)
            {
                sb.Append(frag.Key + ";" + frag.Value + "|");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(',');

            return sb.ToString();
        }
    }
}
