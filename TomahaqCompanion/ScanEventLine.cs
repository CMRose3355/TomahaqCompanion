using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace TomahaqCompanion
{
    public class ScanEventLine
    {
        public MS2Event ScanEvent { get; set; }
        public string MS2RetentionTime { get; set; }
        public string MS2ScanNumber { get; set; }
        public string MS3ScanNumber { get; set; }
        public string MS2InjectionTime { get; set; }
        public string MS3InjectionTime { get; set; }
        public string MS3SPSIons { get; set; }
        public string MS3SumSN { get; set; }
        public string MS3Quant1 { get; set; }
        public string MS3Quant2 { get; set; }
        public string MS3Quant3 { get; set; }
        public string MS3Quant4 { get; set; }
        public string MS3Quant5 { get; set; }
        public string MS3Quant6 { get; set; }
        public string MS3Quant7 { get; set; }
        public string MS3Quant8 { get; set; }
        public string MS3Quant9 { get; set; }
        public string MS3Quant10 { get; set; }

        public ScanEventLine(MS2Event ms2ScanEvent)
        {
            ScanEvent = ms2ScanEvent;
            MS2RetentionTime = Math.Round(ms2ScanEvent.RetentionTime,2).ToString();
            MS2ScanNumber = ms2ScanEvent.ScanNumber.ToString();
            MS2InjectionTime = Math.Round(ms2ScanEvent.InjectionTime,1).ToString();

            if(ms2ScanEvent.MS3 != null)
            {
                MS3Event ms3 = ms2ScanEvent.MS3;
                MS3ScanNumber = ms3.ScanNumber.ToString();
                MS3InjectionTime = Math.Round(ms3.InjectionTime,1).ToString();
                MS3SPSIons = ms3.SPSIonCount.ToString();

                //Add up the quant information for the sum SN
                double ms3SN = 0;
                foreach(PointPair xy in ms3.QuantPeaks)
                {
                    ms3SN += xy.Y;
                }

                MS3SumSN = Math.Round(ms3SN,2).ToString();
                MS3Quant1 = Math.Round(ms3.QuantPeaks[0].Y,1).ToString();
                MS3Quant2 = Math.Round(ms3.QuantPeaks[1].Y, 1).ToString();
                MS3Quant3 = Math.Round(ms3.QuantPeaks[2].Y, 1).ToString();
                MS3Quant4 = Math.Round(ms3.QuantPeaks[3].Y, 1).ToString();
                MS3Quant5 = Math.Round(ms3.QuantPeaks[4].Y, 1).ToString();
                MS3Quant6 = Math.Round(ms3.QuantPeaks[5].Y, 1).ToString();
                MS3Quant7 = Math.Round(ms3.QuantPeaks[6].Y, 1).ToString();
                MS3Quant8 = Math.Round(ms3.QuantPeaks[7].Y, 1).ToString();
                MS3Quant9 = Math.Round(ms3.QuantPeaks[8].Y, 1).ToString();
                MS3Quant10 = Math.Round(ms3.QuantPeaks[9].Y, 1).ToString();
            }
            else
            {
                MS3ScanNumber = "";
                MS3InjectionTime = "";
                MS3SPSIons = "";
                MS3SumSN = "";
                MS3Quant1 = "";
                MS3Quant2 = "";
                MS3Quant3 = "";
                MS3Quant4 = "";
                MS3Quant5 = "";
                MS3Quant6 = "";
                MS3Quant7 = "";
                MS3Quant8 = "";
                MS3Quant9 = "";
                MS3Quant10 = "";
            }
        }
    }
}
