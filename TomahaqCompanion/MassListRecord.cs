using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomahaqCompanion
{
    public class MassListRecord
    {
        public string MOverZ;
        public string Z;

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
    }
}
