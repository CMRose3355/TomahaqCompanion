using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace TomahaqCompanion
{
    [XmlRoot("MethodModifications")]
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

        [XmlElement("Modification")]
        public List<MethodModification> Modifications;

        public MethodModifications() { }

        public MethodModifications(string version, string model, string family, string type)
        {
            Version = version;
            Model = model;
            Family = family;
            Type = type;

            Modifications = new List<MethodModification>();
        }
    }

    public class MethodModification
    {
        [XmlAttribute]
        public string Order;

        [XmlElement("Experiment")]
        public List<Experiment> Experiments;

        public MethodModification() {
            Experiments = new List<Experiment>();
        }

        public MethodModification(int order, int expIndex)
        {
            Experiments = new List<Experiment>();

            Order = order.ToString();

            Experiments.Add(new Experiment(expIndex));
        }

        public MethodModification(int order, Experiment experiment)
        {
            Experiments = new List<Experiment>();

            Order = order.ToString();

            Experiments.Add(experiment);
        }
    }

    public class Experiment
    {
        [XmlAttribute]
        public string ExperimentIndex;

        [XmlElement("CopyAndAppendScanNode")]
        public CopyAndAppendScanNode CopyAndAppendScanNode;

        [XmlElement]
        public ScanNode ScanNode;

        [XmlElement]
        public MassListFilter MassListFilter;

        public Experiment() { }

        public Experiment(int experimentIndex)
        {
            ExperimentIndex = experimentIndex.ToString();
        }

        public void CopyAndPasteScanNode(int sourceNode)
        {
            CopyAndAppendScanNode = new CopyAndAppendScanNode(sourceNode.ToString());
        }

        public void AddMS1InclusionList(int treeIndex, Dictionary<double, int> mzAndzDict)
        {
            string type = "TargetedMassInclusion";
            bool above = true;
            int msnLevel = 1;

            MassListFilter = new MassListFilter(treeIndex, type, msnLevel, above);

            foreach (KeyValuePair<double, int> kvp in mzAndzDict)
            {
                MassListFilter.MassList.AddMassListRecord(kvp.Key, kvp.Value);
            }
        }

        public void AddMS1InclusionSingle(int treeIndex, double mz, int z)
        {
            string type = "TargetedMassInclusion";
            bool above = true;
            int msnLevel = 1;

            MassListFilter = new MassListFilter(treeIndex, type, msnLevel, above);

            MassListFilter.MassList.AddMassListRecord(mz, z);
        }

        public void AddMS1InclusionSingleWithRTWindow(int treeIndex, double mz, int z, double startTime, double endTime)
        {
            string type = "TargetedMassInclusion";
            bool above = true;
            int msnLevel = 1;

            MassListFilter = new MassListFilter(treeIndex, type, msnLevel, above);

            MassListFilter.MassList.AddMassListRecord(mz, z, startTime, endTime);
        }

        public void AddMS3InclusionList(int treeIndex, List<double> mzList)
        {
            string type = "TargetedMassInclusion";
            bool above = false;
            int msnLevel = 3 - 1;

            MassListFilter = new MassListFilter(treeIndex, type, msnLevel, above);

            foreach (double mz in mzList)
            {
                MassListFilter.MassList.AddMassListRecord(mz, 1);
            }
        }

        public void AddMS3InclusionList(int treeIndex, Dictionary<double, int> mzAndzDict)
        {
            string type = "TargetedMassInclusion";
            bool above = false;
            int msnLevel = 3 - 1;

            MassListFilter = new MassListFilter(treeIndex, type, msnLevel, above);

            foreach (KeyValuePair<double, int> mzAndZ in mzAndzDict)
            {
                MassListFilter.MassList.AddMassListRecord(mzAndZ.Key, mzAndZ.Value);
            }
        }

        public void AddMS2TriggerList(int treeIndex, List<double> mzList)
        {
            string type = "TargetedMassTrigger";
            bool above = false;
            int msnLevel = 2 - 1;

            MassListFilter = new MassListFilter(treeIndex, type, msnLevel, above);

            foreach (double mz in mzList)
            {
                MassListFilter.MassList.AddMassListRecord(mz, 1);
            }

        }

        public void AddMS2TriggerList(int treeIndex, Dictionary<double, int> mzAndzDict)
        {
            string type = "TargetedMassTrigger";
            bool above = false;
            int msnLevel = 2 - 1;

            MassListFilter = new MassListFilter(treeIndex, type, msnLevel, above);

            foreach (KeyValuePair<double, int> mzAndZ in mzAndzDict)
            {
                MassListFilter.MassList.AddMassListRecord(mzAndZ.Key, mzAndZ.Value);
            }
        }

        public void ChangeScanParams(int treeIndex, double isolationOffset)
        {
            int msnLevel = 2;
            ScanNode = new ScanNode(treeIndex, msnLevel);

            ScanNode.ChangeIsolationOffset(isolationOffset);
        }
    }

    public class CopyAndAppendScanNode
    {
        public string SourceNodePosition;

        public CopyAndAppendScanNode() { }

        public CopyAndAppendScanNode(string position)
        {
            SourceNodePosition = position;
        }
    }

    public class ScanNode
    {
        [XmlElement("SourceNodePosition")]
        public List<string> SourceNodePosition;

        [XmlElement("ScanParameters")]
        public ScanParameters ScanParameters;

        public ScanNode() { }

        public ScanNode(int treeIndex, int msnLevel)
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
            ScanParameters = new ScanParameters(newOffset);
        }
    }

    public class MassListFilter
    {
        [XmlAttribute("MassListType")]
        public string MassListType;

        [XmlAttribute("Above")]
        public bool Above;

        [XmlElement("SourceNodePosition")]
        public List<string> SourceNodePosition;

        [XmlElement("MassList")]
        public MassList MassList;

        public MassListFilter() { }

        public MassListFilter(int treeIndex, string massListType, int msnLevel, bool above)
        {
            MassListType = massListType;

            Above = above;

            SourceNodePosition = new List<string>();
            SourceNodePosition.Add(treeIndex.ToString());

            for (int i = 1; i < msnLevel; i++)
            {
                SourceNodePosition.Add("0");
            }

            MassList = new MassList();
        }
    }

    public class MassList
    {
        [XmlAttribute("StartEndTime")]
        public string StartEndTime;

        [XmlElement("MassListRecord")]
        public List<MassListRecord> Records;

        public MassList()
        {
            StartEndTime = "false";
            Records = new List<MassListRecord>();
        }

        public void AddMassListRecord(double mz, int z)
        {
            Records.Add(new MassListRecord(mz, z));
        }

        public void AddMassListRecord(double mz, int z, double startTime, double endTime)
        {
            StartEndTime = "true";
            Records.Add(new MassListRecord(mz, z, startTime, endTime));
        }

        public void AddMassListRecord(double mz)
        {
            Records.Add(new MassListRecord(mz));
        }
    }

    public class MassListRecord
    {
        public string MOverZ;
        public string Z;
        public string StartTime;
        public string EndTime;

        public MassListRecord() { }

        public MassListRecord(double mOverZ, int z)
        {
            MOverZ = mOverZ.ToString();
            Z = z.ToString();
        }

        public MassListRecord(double mOverZ)
        {
            MOverZ = mOverZ.ToString();
        }

        public MassListRecord(double mOverZ, int z, double startTime, double endTime)
        {
            MOverZ = mOverZ.ToString();
            Z = z.ToString();
            StartTime = Math.Round(startTime, 2).ToString();
            EndTime = Math.Round(endTime, 2).ToString();
        }
    }

    public class ScanParameters
    {
        public string IsolationOffset;

        public ScanParameters() { }

        public ScanParameters(double isolationOffset)
        {
            IsolationOffset = isolationOffset.ToString();
        }
    }
}
