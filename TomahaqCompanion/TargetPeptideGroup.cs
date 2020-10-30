using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomahaqCompanion
{
    public class TargetPeptideGroup
    {
        public string PeptideString { get; set; }
        public string ProteinString {get; set;}
        public SortedList<int, TargetPeptide> TargetPepByCharge { get; set; }
        public Dictionary<string, TargetPeptide> TargetPepByCV { get; set; }
        public Dictionary<string, TargetPeptide> TargetPeptides { get; set; }
        public bool Selected { get; set; }

        public TargetPeptideGroup(TargetPeptide targetPeptide)
        {
            TargetPepByCharge = new SortedList<int, TargetPeptide>();
            TargetPepByCV = new Dictionary<string, TargetPeptide>();
            TargetPeptides = new Dictionary<string, TargetPeptide>();
            Selected = true;

            PeptideString = targetPeptide.PeptideString;
            ProteinString = targetPeptide.ProteinString;
        }

        public void AddTargetPeptide(TargetPeptide targetPepetide)
        {

        }
    }

}
