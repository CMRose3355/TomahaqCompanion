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
        public PointPairList QuantPeaksMassError { get; set; }

        public List<double> SPSIons { get; set; }
        public int SPSIonCount { get; set; }

        public MS3Event(int scanNumber, double retentionTime, ThermoSpectrum ms3spectrum, List<ThermoMzPeak> peaks, double injectionTime, Dictionary<string, double> quantChannelDict, List<double> spsIons)
        {
            SPSIons = spsIons;
            SPSIonCount = spsIons.Count;
            ScanNumber = scanNumber;
            RetentionTime = retentionTime;
            InjectionTime = injectionTime;

            AllPeaks = new PointPairList();
            QuantPeaks = new PointPairList();
            QuantPeaksMassError = new PointPairList();

            foreach (ThermoMzPeak peak in peaks)
            {
                AllPeaks.Add(peak.MZ, peak.Intensity);
            }

            int peakCount = 1;
            foreach(double quantMZ in quantChannelDict.Values)
            {
                MzRange quantRange = new MzRange(quantMZ, new Tolerance(ToleranceUnit.PPM, 15));
                ThermoMzPeak peak = GetTallestPeak(quantRange, ms3spectrum);

                if(peak == null)
                {
                    QuantPeaks.Add(peakCount, 0);
                    QuantPeaksMassError.Add(peakCount, -100);
                }
                else
                {
                    QuantPeaks.Add(peakCount, peak.SignalToNoise);
                    QuantPeaksMassError.Add(peakCount, ((quantMZ - peak.MZ)/quantMZ*1000000));
                }

                peakCount++;
            }
        }

        private ThermoMzPeak GetTallestPeak(MzRange range, ThermoSpectrum spectrum)
        {
            ThermoMzPeak retPeak = null;
            List<ThermoMzPeak> peaks = null;

            if (spectrum.TryGetPeaks(range, out peaks))
            {
                foreach (ThermoMzPeak peak in peaks)
                {
                    if (retPeak == null || peak.Intensity > retPeak.Intensity)
                    {
                        retPeak = peak;
                    }
                }
            }

            return retPeak;
        }
    }
}
