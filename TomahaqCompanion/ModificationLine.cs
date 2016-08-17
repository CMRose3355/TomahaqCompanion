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

namespace TomahaqCompanion
{
    public class ModificationLine
    {
        public string Name { get; set; }
        public double Mass { get; set; }
        public string ModSites { get; set; }
        public string ModChar { get; set; }
        public string Type { get; set; }
        public bool Both { get; set; }
        public bool Trigger { get; set; }
        public bool Target { get; set; }
        public Modification Modification { get; set; }


        public ModificationLine(string name, double mass, string modSites, string modChar, string type, bool both, bool trigger, bool target)
        {
            Name = name;
            Mass = mass;
            ModSites = modSites;
            ModChar = modChar;
            Type = type;
            Both = both;
            Trigger = trigger;
            Target = target;
        }

        public ModificationLine(string name, double mass, string modSites, string modChar, string type, bool both, bool trigger, bool target, Modification mod)
        {
            Name = name;
            Mass = mass;
            ModSites = modSites;
            ModChar = modChar;
            Type = type;
            Both = both;
            Modification = mod;
            Trigger = trigger;
            Target = target;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}