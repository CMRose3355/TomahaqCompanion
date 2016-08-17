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
        public PointPairList AllPeaks { get; set; }
        public PointPairList MatchedPeaks { get; set; }

        public MS2Event(int scanNumber, double retentionTime, List<ThermoMzPeak> peaks, double injectionTime)
        {
            ScanNumber = scanNumber;
            RetentionTime = retentionTime;
            InjectionTime = injectionTime;

            AllPeaks = new PointPairList();
            
            foreach(ThermoMzPeak peak in peaks)
            {
                AllPeaks.Add(peak.MZ, peak.Intensity);
            }
        }

        public void AddMS3Event(MS3Event ms3Event)
        {
            MS3 = ms3Event;
        }
    }
}
