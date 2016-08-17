using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace TomahaqCompanion
{
    public class MethodSN
    {
        [XmlElement]
        public List<string> SourceNodePosition;

        [XmlElement]
        public MethodScanParams ScanParameters;

        public MethodSN() { }

        public MethodSN(int treeIndex,int msnLevel)
        {
            SourceNodePosition = new List<string>();

            SourceNodePosition.Add(treeIndex.ToString());

            for (int i = 1; i < msnLevel; i++)
            {
                SourceNodePosition.Add("0");
            }
        }

        public void ChangeIsolationOffset(double newOffset)
        {
            ScanParameters = new MethodScanParams(newOffset);
        }

    }
}
