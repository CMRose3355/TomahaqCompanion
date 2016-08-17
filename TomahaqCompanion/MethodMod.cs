using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace TomahaqCompanion
{
    public class MethodMod
    {
        [XmlAttribute]
        public string Order;

        [XmlElement]
        public MethodExp Experiment;

        public MethodMod() { }

        public MethodMod(int order, int expIndex)
        {
            Order = order.ToString();

            Experiment = new MethodExp(expIndex);
        }

        public MethodMod(int order, MethodExp experiment)
        {
            Order = order.ToString();

            Experiment = experiment;
        }
    }
}
