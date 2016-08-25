using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;


namespace TomahaqCompanion
{
    public class MethodExp
    {
        [XmlAttribute]
        public string ExperimentIndex;

        [XmlElement]
        public MethodExp Experiment;

        [XmlElement]
        public MethodCopyAppendSN CopyAndAppendScanNode;

        [XmlElement]
        public MethodSN ScanNode;

        [XmlElement]
        public MethodMassList MassListFilter;

        public MethodExp() { }

        public MethodExp(int experimentIndex)
        {
            ExperimentIndex = experimentIndex.ToString();
        }

        public void CopyAndPasteScanNode(int sourceNode)
        {
            CopyAndAppendScanNode = new MethodCopyAppendSN(sourceNode.ToString());
        }

        public void AddMS1InclusionList(int treeIndex, Dictionary<double, int> mzAndzDict)
        {
            string type = "TargetedMassInclusion";
            bool above = true;
            int msnLevel = 1;

            MassListFilter = new MethodMassList(treeIndex, type, msnLevel, above);

            foreach(KeyValuePair<double, int> kvp in mzAndzDict)
            {
                MassListFilter.AddMassListRecord(kvp.Key, kvp.Value);
            }
        }

        public void AddMS1InclusionSingle(int treeIndex, double mz, int z)
        {
            string type = "TargetedMassInclusion";
            bool above = true;
            int msnLevel = 1;

            MassListFilter = new MethodMassList(treeIndex, type, msnLevel, above);

            MassListFilter.AddMassListRecord(mz, z);
        }

        public void AddMS3InclusionList(int treeIndex, List<double> mzList)
        {
            string type = "TargetedMassInclusion";
            bool above = false;
            int msnLevel = 3 - 1;

            MassListFilter = new MethodMassList(treeIndex, type, msnLevel, above);

            foreach (double mz in mzList)
            {
                MassListFilter.AddMassListRecord(mz, 1);
            }
        }

        public void AddMS3InclusionList(int treeIndex, Dictionary<double, int> mzAndzDict)
        {
            string type = "TargetedMassInclusion";
            bool above = false;
            int msnLevel = 3 - 1;

        }

        public void AddMS2TriggerList(int treeIndex, List<double> mzList)
        {
            string type = "TargetedMassTrigger";
            bool above = false;
            int msnLevel = 2 - 1;

            MassListFilter = new MethodMassList(treeIndex, type, msnLevel, above);

            foreach (double mz in mzList)
            {
                MassListFilter.AddMassListRecord(mz, 1);
            }

        }

        public void AddMS2TriggerList(int treeIndex, Dictionary<double, int> mzAndzDict)
        {
            string type = "TargetedMassTrigger";
            bool above = false;
            int msnLevel = 2 - 1;
        }

        public void ChangeScanParams(int treeIndex, double isolationOffset)
        {
            int msnLevel = 2;
            ScanNode = new MethodSN(treeIndex, msnLevel);

            ScanNode.ChangeIsolationOffset(isolationOffset);
        }
    }
}
