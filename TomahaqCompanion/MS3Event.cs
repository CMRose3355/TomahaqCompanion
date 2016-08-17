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
    public class MS3Event
    {
        public int ScanNumber { get; set; }
        public double RetentionTime { get; set; }
        public double InjectionTime { get; set; }
        public PointPairList AllPeaks { get; set; }
        public PointPairList QuantPeaks { get; set; }
        public int SPSIonCount { get; set; }

        public MS3Event(int scanNumber, double retentionTime, ThermoSpectrum ms3spectrum, List<ThermoMzPeak> peaks, double injectionTime, Dictionary<string, double> quantChannelDict, int numSPSIons)
        {
            SPSIonCount = numSPSIons;
            ScanNumber = scanNumber;
            RetentionTime = retentionTime;
            InjectionTime = injectionTime;

            AllPeaks = new PointPairList();
            QuantPeaks = new PointPairList();

            foreach (ThermoMzPeak peak in peaks)
            {
                AllPeaks.Add(peak.MZ, peak.Intensity);
            }

            int peakCount = 1;
            foreach(double quantMZ in quantChannelDict.Values)
            {
                MzRange quantRange = new MzRange(quantMZ, new Tolerance(ToleranceUnit.PPM, 10));
                ThermoMzPeak peak = ms3spectrum.GetClosestPeak(quantRange);

                if(peak == null)
                {
                    QuantPeaks.Add(peakCount, 0);
                }
                else
                {
                    QuantPeaks.Add(peakCount, peak.SignalToNoise);
                }

                peakCount++;
            }
        }
    }
}
