using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlMethodChanger;
using XmlMethodChanger.lib;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Thermo.TNG.MethodXMLInterface;
using Thermo.TNG.MethodXMLFactory;

namespace TomahaqCompanion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void editMethod_Click(object sender, EventArgs e)
        {
            string templateMethod = "C:\\Users\\Orbitrap_Lumos\\Desktop\\XmlMethodModifications\\Examples\\Fusion\\SPS\\Template.meth";
            string modificationXML = "C:\\Users\\Orbitrap_Lumos\\Desktop\\XmlMethodModifications\\Examples\\Fusion\\SPS\\SPS_1.xml";
            string outputMethod = "C:\\Users\\Orbitrap_Lumos\\Desktop\\XmlMethodModifications\\Examples\\Fusion\\SPS\\SPS_ouput.meth";

            buildXML(modificationXML);

            using (IMethodXMLContext mxc = MethodChanger.CreateContext())
            {
                MethodChanger.ModifyMethod(templateMethod, modificationXML, outputMethod: outputMethod);
            }
        }

        private void buildXML(string modificationXML)
        {

        }
    }
}
