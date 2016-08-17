using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomahaqCompanion
{
    public class MethodScanParams
    {
        public string IsolationOffset;

        public MethodScanParams() { }

        public MethodScanParams(double isolationOffset)
        {
            IsolationOffset = isolationOffset.ToString();
        }
    }
}
