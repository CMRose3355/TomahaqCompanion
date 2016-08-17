using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace TomahaqCompanion
{
    public class MethodCopyAppendSN
    {
        public string SourceNodePosition;

        public MethodCopyAppendSN() { }

        public MethodCopyAppendSN(string position)
        {
            SourceNodePosition = position;
        }
    }
}
