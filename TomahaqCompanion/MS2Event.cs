using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;
using CSMSL.IO.Thermo;

namespace TomahaqCompanion
{
    public class MS2Event
    {
        public MS3Event MS3 {get; set;}

        public int ScanNumber { get; set; }
        public double RetentionTime { get; set; }
        public double InjectionTime { get; set; }

        public Tolerance FragmentTol { get; set; }

        public double MS1Intensity { get; set; }

        public double IsolationSpecificity { get; set; }

        public Dictionary<int, Dictionary<Fragment, double>> MatchedFragDict { get; set; }

        public PointPairList AllPeaks { get; set; }
        public PointPairList MatchedPeaks { get; set; }
        public PointPairList SPSPeaks { get; set; }

        public MS2Event(int scanNumber, double retentionTime, List<ThermoMzPeak> peaks, double injectionTime, double ms1Intensity, Tolerance fragTol)
        {
            ScanNumber = scanNumber;
            RetentionTime = retentionTime;
            InjectionTime = injectionTime;

            MS1Intensity = ms1Intensity;

            FragmentTol = fragTol;

            AllPeaks = new PointPairList();
            MatchedPeaks = new PointPairList();
            SPSPeaks = new PointPairList();
            
            foreach(ThermoMzPeak peak in peaks)
            {
                AllPeaks.Add(peak.MZ, peak.Intensity);
            }

            MatchedFragDict = new Dictionary<int, Dictionary<Fragment, double>>();
        }

        public void AddMS3Event(MS3Event ms3Event)
        {
            MS3 = ms3Event;
        }

        public void PopulateMatchedPeaks(List<Fragment> fragments, int parentCharge)
        {
            
            //Get the max charge when looking for fragments
            int maxCharge = parentCharge - 1;
            if(parentCharge >= 3) { maxCharge = 2; }

            //Cycle through the maximum charge
            for (int charge = 1; charge <= maxCharge; charge++)
            {
                //Add an entry in the 
                MatchedFragDict.Add(charge, new Dictionary<Fragment, double>());

                //Calculate the mz for each fragment and see if there is a peak in the spectrum around it
                foreach (Fragment frag in fragments)
                {
                    //Calculate the range to look in
                    MzRange fragRange = new MzRange(frag.ToMz(charge), FragmentTol);
                    double minMZ = fragRange.Minimum;
                    double maxMZ = fragRange.Maximum;

                    //Search the spectrum and return the peak that is the tallest
                    PointPair point = SearchSpectrum(minMZ, maxMZ, AllPeaks);
                    if(point != null)
                    {
                        double outDoub = 0;
                        if(!MatchedFragDict[charge].TryGetValue(frag, out outDoub))
                        {
                            MatchedPeaks.Add(point);
                            MatchedFragDict[charge].Add(frag, point.Y);
                        }
                       
                    }
                }
            }

            //If there is an MS3 with this MS2 then try to match the SPS Ions
            if(MS3 != null)
            {
                //Cycle through the SPS ions to mark them in the spectrum
                PopulateMatchedSPSPeaks(MS3.SPSIons);
            }
        }

        public void PopulateMatchedSPSPeaks(List<double> mzs, bool spsEdited = false)
        {
            PointPairList currentPeaks = new PointPairList();
            foreach(PointPair pair in SPSPeaks)
            {
                currentPeaks.Add(pair);
            }

            //Cycle through the SPS ions to mark them in the spectrum
            foreach (double spsMZ in mzs)
            {
                //Calculate the range to look in
                MzRange spsRange = new MzRange(spsMZ, FragmentTol);
                double minMZ = spsRange.Minimum;
                double maxMZ = spsRange.Maximum;

                //Search the spectrum and return the peak that is the tallest
                PointPair point = SearchSpectrum(minMZ, maxMZ, AllPeaks);
                if (point != null && !SPSPeaks.Contains(point) && (!spsEdited || (spsEdited && currentPeaks.Contains(point))))
                {
                    SPSPeaks.Add(point);
                }
            }
        }

        public void PopulateMatchedSPSPeaks(Dictionary<int, List<Fragment>> fragDict)
        {
            //Cycle through the SPS ions to mark them in the spectrum
            foreach (KeyValuePair<int, List<Fragment>> kvp in fragDict)
            {
                foreach(Fragment frag in kvp.Value)
                {
                    //Calculate the range to look in
                    MzRange spsRange = new MzRange(frag.ToMz(kvp.Key), FragmentTol);
                    double minMZ = spsRange.Minimum;
                    double maxMZ = spsRange.Maximum;

                    //Search the spectrum and return the peak that is the tallest
                    PointPair point = SearchSpectrum(minMZ, maxMZ, AllPeaks);
                    if (point != null)
                    {
                        SPSPeaks.Add(point);
                    }
                }
            }
        }

        private PointPair SearchSpectrum(double minMZ, double maxMZ, PointPairList searchList)
        {
            PointPair point = null;

            int minIndex = binarySearch(minMZ, searchList);

            while (minIndex < searchList.Count && searchList.ElementAt(minIndex).X <= maxMZ)
            {
                if(point == null || searchList.ElementAt(minIndex).Y > point.Y)
                {
                    point = searchList[minIndex];
                }

                minIndex++;
            }

            return point;
        }

        private int binarySearch(double minMZ, PointPairList searchList)
        {
            int max = searchList.Count - 1;
            int min = 0;
            int mid = 0;
            while (min <= max)
            {
                mid = (max + min) / 2;
                double midvalue = searchList.ElementAt(mid).X;
                if (minMZ < midvalue)
                {
                    max = mid - 1;
                }
                else if (minMZ > midvalue)
                {
                    min = mid + 1;
                }
                else
                {
                    return mid;
                }
            }

            return min;
        }
    }
}
