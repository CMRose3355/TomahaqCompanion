using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TomahaqCompanion
{
    public class TargetPeptideLine
    { 
        public TargetPeptide Peptide { get; set; }
        public bool Selected { get; set; }
        public string PeptideString { get; set; }
        public double TargetMZ { get; set; }
        public double TriggerMZ { get; set; }
        public string Name { get; set; }
        public string Charge { get; set; }
        public string MaxIntensity { get; set; }

        public TargetPeptideLine(TargetPeptide peptide)
        {
            Selected = true;

            Peptide = peptide;

            PeptideString = peptide.PeptideString;
            Charge = peptide.Charge.ToString();

            TriggerMZ = Math.Round(peptide.TriggerMZ, 4);
            TargetMZ = Math.Round(peptide.TargetMZ, 4);
        }

        public TargetPeptideLine(TargetPeptide peptide, double maxInt)
        {
            Peptide = peptide;

            PeptideString = peptide.PeptideString;
            Charge = peptide.Charge.ToString();

            TriggerMZ = Math.Round(peptide.TriggerMZ, 4);
            TargetMZ = Math.Round(peptide.TargetMZ, 4);

            MaxIntensity = maxInt.ToString("G2", CultureInfo.InvariantCulture);

            Selected = true;
        }

        public override string ToString()
        {
            return Peptide.TargetToString();
        }
    }
}
