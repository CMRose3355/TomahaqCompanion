using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace TomahaqCompanion
{
    public class MethodMassList
    {
        [XmlAttribute]
        public string MassListType;

        [XmlElement]
        public List<string> SourceNodePosition;

        public List<MassListRecord> MassList;

        [XmlAttribute]
        public bool Above;

        public MethodMassList() { }

        public MethodMassList(int treeIndex, string massListType, int msnLevel, bool above)
        {
            MassListType = massListType;

            Above = above;

            SourceNodePosition = new List<string>();
            SourceNodePosition.Add(treeIndex.ToString());

            for (int i = 1; i < msnLevel; i++)
            {
                SourceNodePosition.Add("0");
            }

            MassList = new List<MassListRecord>();
        }

        public void AddMassListRecord(double mz, int z)
        {
            MassList.Add(new MassListRecord(mz, z));
        }

        public void AddMassListRecord(double mz)
        {
            MassList.Add(new MassListRecord(mz));
        }
    }
}
