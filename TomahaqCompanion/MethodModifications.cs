using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace TomahaqCompanion
{
    public class MethodModifications
    {
        [XmlAttribute]
        public string Version;

        [XmlAttribute]
        public string Model;

        [XmlAttribute]
        public string Family;

        [XmlAttribute]
        public string Type;

        [XmlElement]
        public List<MethodMod> Modification;

        public MethodModifications() { }

        public MethodModifications(string version, string model, string family, string type)
        {
            Version = version;
            Model = model;
            Family = family;
            Type = type;

            Modification = new List<MethodMod>();
        }
    }
}
