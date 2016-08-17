using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;
using CSMSL.IO.Thermo;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using ZedGraph;
using System.Xml.Serialization;
using System.Xml;
using XmlMethodChanger;
using XmlMethodChanger.lib;

namespace TomahaqCompanion
{
    public partial class TomahaqCompanionForm : Form
    {
        private bool Analysis;
        private bool Priming;
        private BindingList<TargetPeptideLine> Targets;
        private BindingList<ModificationLine> ModLines;
        private BindingList<ScanEventLine> ScanEvents;
        private Dictionary<string, Dictionary<string, double>> QuantChannelDict;
        private Dictionary<string, double> QuantChannelsInUse;
        private GraphPane MS1Pane;
        private GraphPane SpectrumPane1;
        private GraphPane SpectrumPane2;

        public TomahaqCompanionForm()
        {
            InitializeComponent();

            //
            Analysis = false;
            Priming = false;

            //Initialize the Target Grid View
            Targets = new BindingList<TargetPeptideLine>();
            targetGridView.AutoGenerateColumns = false;
            targetGridView.DataSource = Targets;
            InitializeTargetGrid();

            //Initialize the Modification Grid View
            modGridView.AutoGenerateColumns = false;
            ModLines = new BindingList<ModificationLine>();
            modGridView.DataSource = ModLines;
            InitializeModGrid();

            //Initialize the Scan Event Grid View
            scanGridView.AutoGenerateColumns = false;
            ScanEvents = new BindingList<ScanEventLine>();
            scanGridView.DataSource = ScanEvents;
            InitializeScanGrid();

            //Initialize the Quant Channels
            QuantChannelDict = BiuldQuantificationDictionary();

            InitializePrimingRunGraphs();
        }

        private void sortTargetsMZ_Click(object sender, EventArgs e)
        {

        }

        private void sortTargetsRT_Click(object sender, EventArgs e)
        {

        }

        private void primingTargetList_Click(object sender, EventArgs e)
        {
            //Import the modifications
            Dictionary<Modification, string> staticMods = new Dictionary<Modification, string>();
            Dictionary<Modification, char> dynMods = new Dictionary<Modification, char>();
            AddModifications(out staticMods, out dynMods);

            //Import the Peptides
            string targetFile = targetTextBox.Text;
            SortedList<double, TargetPeptide> targetList = ImportTargets(targetFile, staticMods, dynMods);

            //Print the Priming Run Target List
            using (StreamWriter writer = new StreamWriter(targetFile.Replace(".csv", "_primingRunInclusionList.csv")))
            {
                writer.WriteLine("Name,M,z range");
                foreach (TargetPeptide peptide in targetList.Values)
                {
                    writer.WriteLine(peptide.PeptideString + "," + peptide.Trigger.MonoisotopicMass + ",2-5");
                }
            }
        }

        private void createMethod_Click(object sender, EventArgs e)
        {
            try
            {
                Priming = true;
                Analysis = false;

                string templateMethod = templateBox.Text;
                string outputMethod = templateMethod.Replace(".meth", "_modified.meth");

                //Import the modifications
                UpdateLog("Importing Modifications");
                Dictionary<Modification, string> staticMods = new Dictionary<Modification, string>();
                Dictionary<Modification, char> dynMods = new Dictionary<Modification, char>();
                AddModifications(out staticMods, out dynMods);

                //Determine the quantification channels --Not necessary for priming run--
                UpdateLog("Determining Quantification Channels");
                Dictionary<string, double> quantChannelDict = AddQuantitationChannels(staticMods);

                //Import the Peptides
                UpdateLog("Importing Target Peptides");
                string targetFile = targetTextBox.Text;
                SortedList<double, TargetPeptide> targetList = ImportTargets(targetFile, staticMods, dynMods);

                //Iterate through the peptides, create ms1, ms2, and potentially ms3 lists
                if (rawPrimingRun.Checked)
                {
                    UpdateLog("Opening Raw File");
                    ThermoRawFile rawFile = new ThermoRawFile(rawFileBox.Text);
                    rawFile.Open();

                    //Build a map of the MS/MS scan events
                    UpdateLog("Mapping MS Scans");
                    Dictionary<int, int> TriggerMS2toMS1 = null;
                    Dictionary<int, int> TriggerMS2toTargetMS2 = null;
                    Dictionary<int, int> TargetMS2toTargetMS3 = null;
                    List<int> ms1Scans = MapMSDataScans(rawFile, out TriggerMS2toMS1, out TriggerMS2toTargetMS2, out TargetMS2toTargetMS3);

                    //Extract MS1 Information
                    UpdateLog("Extracting MS1 XICs");
                    ExtractMS1XIC(rawFile, ms1Scans, targetList);

                    //Populate the trigger ms2 data
                    UpdateLog("Extracting MS/MS Data");
                    ExtractData(rawFile, TriggerMS2toTargetMS2, TargetMS2toTargetMS3, targetList, quantChannelDict);

                    UpdateLog("Closing Raw File");
                    rawFile.Dispose();
                }

                //Populate Target SPS Ions
                foreach (TargetPeptide target in targetList.Values)
                {
                    target.PopulateTargetSPSIons();
                    target.PopulateTriggerIons();
                }

                //Make the XML
                string xmlFile = BuildMethodXML(targetFile, targetList.Values.ToList());

                //Export the method
                MethodChanger.ModifyMethod(templateMethod, xmlFile, outputMethod: outputMethod);

                //Plot the peptide selected
            }
            catch (Exception exp)
            {
                UpdateLog("Error! " + exp.Message);
            }
        }

        private void analyzeRun_Click(object sender, EventArgs e)
        {
            Priming = false;
            Analysis = true;

            //Import the modifications
            UpdateLog("Importing Modifications");
            Dictionary<Modification, string> staticMods = new Dictionary<Modification, string>();
            Dictionary<Modification, char> dynMods = new Dictionary<Modification, char>();
            AddModifications(out staticMods, out dynMods);

            //Determine the quantification channels --Not necessary for priming run--
            UpdateLog("Determining Quantification Channels");
            QuantChannelsInUse = AddQuantitationChannels(staticMods);

            //Import the Peptides
            UpdateLog("Importing Target Peptides");
            string targetFile = targetTextBox.Text;
            SortedList<double, TargetPeptide> targetList = ImportTargets(targetFile, staticMods, dynMods);

            //Iterate through the peptides, create ms1, ms2, and potentially ms3 lists
            UpdateLog("Opening Raw File");
            ThermoRawFile rawFile = new ThermoRawFile(rawFileBox.Text);
            rawFile.Open();

            //Build a map of the MS/MS scan events
            UpdateLog("Mapping MS Scans");
            Dictionary<int, int> TriggerMS2toMS1 = null;
            Dictionary<int, int> TriggerMS2toTargetMS2 = null;
            Dictionary<int, int> TargetMS2toTargetMS3 = null;
            List<int> ms1Scans = MapMSDataScans(rawFile, out TriggerMS2toMS1, out TriggerMS2toTargetMS2, out TargetMS2toTargetMS3);

            //Extract MS1 Information
            UpdateLog("Extracting MS1 XICs");
            ExtractMS1XIC(rawFile, ms1Scans, targetList);

            //Populate the trigger ms2 data
            UpdateLog("Extracting MS/MS Data");
            ExtractData(rawFile, TriggerMS2toTargetMS2, TargetMS2toTargetMS3, targetList, QuantChannelsInUse);

            //Close the Raw file
            UpdateLog("Closing Raw File");
            rawFile.Dispose();

            //Build the User GUI data for each target
            UpdateLog("Consolidating Data for GUI Tables");
            Targets.Clear();
            foreach (TargetPeptide target in targetList.Values)
            {
                //Here we will populate all of the data necessary for analysis of the data
                target.PopulateAnalysisData();

                //This is building the target line that will go into the GUI
                Targets.Add(new TargetPeptideLine(target));
            }

            //Plot the first peptide
            TargetPeptide firstPeptide = targetList.ElementAt(0).Value;

            //Update the plots
            UpdatePlotsAnalysis(firstPeptide);
        }

        private void AddModifications(out Dictionary<Modification, string> staticMods, out Dictionary<Modification, char> dynMods)
        {
            dynMods = new Dictionary<Modification, char>();
            staticMods = new Dictionary<Modification, string>();

            foreach (ModificationLine modLine in ModLines)
            {
                if (modLine.Trigger || modLine.Target || modLine.Both)
                {
                    Modification addMod = modLine.Modification;
                    if (addMod == null)
                    {
                        addMod = new Modification(modLine.Mass, modLine.Name);
                    }

                    if (modLine.Type == "Static")
                    {
                        string type = "Both";
                        if (modLine.Trigger) { type = "Trigger"; } else if (modLine.Target) { type = "Target"; }
                        staticMods.Add(addMod, type);
                    }
                    else
                    {
                        dynMods.Add(addMod, modLine.ModChar.ElementAt(0));
                    }
                }
            }
        }

        private SortedList<double, TargetPeptide> ImportTargets(string targetFile, Dictionary<Modification, string> staticMods, Dictionary<Modification, char> dynMods)
        {
            //This will be the list to return, it is a sorted list so we can do a binary search if the list gets large in the future
            SortedList<double, TargetPeptide> retList = new SortedList<double, TargetPeptide>();

            //Cycle through the csv and load in each peptide
            using (CsvReader reader = new CsvReader(new StreamReader(targetFile), true))
            {
                //Grab the headers from the csv
                List<string> headers = reader.GetFieldHeaders().ToList();

                //Reach each line
                while (reader.ReadNextRecord())
                {
                    //Get the peptide string
                    string peptideString = reader["Peptide"];
                    
                    //Determine if a charge is provided or a range needs to be used
                    int minCharge = 2;
                    int maxCharge = 4;
                    if(headers.Contains("z"))
                    {
                        minCharge = int.Parse(reader["z"]);
                        maxCharge = minCharge;
                    }

                    //Determine if the user input SPS ions that they want to use 
                    List<double> targetSPSIons = new List<double>();
                    if(headers.Contains("MS3 Inclusion m/z"))
                    {
                        targetSPSIons = LoadUserSPSIons(reader["MS3 Inclusion m/z"]);
                    }

                    //
                    for(int charge = minCharge; charge <= maxCharge;charge++)
                    {
                        TargetPeptide target = new TargetPeptide(peptideString, charge, staticMods, dynMods, targetSPSIons);

                        TargetPeptide outPep = null;
                        if (!retList.TryGetValue(target.Trigger.ToMz(charge), out outPep))
                        {
                            retList.Add(target.Trigger.ToMz(charge), target);
                        }
                        else
                        {
                            Console.WriteLine("Multiple targets with same mass detected");
                        }
                    }
                }
            }

            return retList;
        }

        private List<double> LoadUserSPSIons(string entry)
        {
            List<double> targetSPSIons = new List<double>();
            List<string> targetSPSIonsStrings = entry.Split(';').ToList();

            foreach (string ion in targetSPSIonsStrings)
            {
                targetSPSIons.Add(double.Parse(ion));
            }

            return targetSPSIons;
        }

        private List<int> MapMSDataScans(ThermoRawFile rawFile, out Dictionary<int, int> TriggerMS2toMS1, out Dictionary<int, int> TriggerMS2toTargetMS2, out Dictionary<int, int> TargetMS2toTargetMS3)
        {
            List<int> ms1List = new List<int>();
            TriggerMS2toMS1 = new Dictionary<int, int>();
            TriggerMS2toTargetMS2 = new Dictionary<int, int>();
            TargetMS2toTargetMS3 = new Dictionary<int, int>();

            int lastSpectrumNumber = rawFile.LastSpectrumNumber;

            //Cycle through the raw file
            for(int i = 1; i<= lastSpectrumNumber; i++)
            {
                //I am just querying the msn order of the scan, I think this is faster than getting the whole scan each time
                int msnOrder = rawFile.GetMsnOrder(i);

                //If it is an MS1 then just add the number to a list
                if(msnOrder == 1)
                {
                    ms1List.Add(i);
                }
                //If it is an MS3 then make the connection to the target MS2
                else if (msnOrder == 3)
                {
                    TargetMS2toTargetMS3.Add(rawFile.GetParentSpectrumNumber(i), i);
                }
                //If it is an MS2 then it could be a trigger or target
                else if (msnOrder == 2)
                {
                    int parentScanNumber = rawFile.GetParentSpectrumNumber(i);
                    int parentOrder = rawFile.GetMsnOrder(parentScanNumber);

                    //If the parent order is 2 then this is a target ms2 and the parent is the trigger
                    if(parentOrder == 2)
                    {
                        TriggerMS2toTargetMS2.Add(parentScanNumber, i);
                    }
                    //If the parent order is 1 then this is a trigger ms2 and the parent is an MS1
                    else if(parentOrder == 1)
                    {
                        TriggerMS2toMS1.Add(i, parentScanNumber);
                    }
                }
            }

            return ms1List;
        }

        private void ExtractMS1XIC(ThermoRawFile rawFile, List<int> ms1Scans, SortedList<double, TargetPeptide> targetList)
        {
            foreach(int scanNumber in ms1Scans)
            {
                ThermoSpectrum spectrum = rawFile.GetSpectrum(scanNumber);
                double rt = rawFile.GetRetentionTime(scanNumber);

                foreach(TargetPeptide target in targetList.Values)
                {
                    target.AddMS1XICPoint(spectrum, rt);
                }
            }
        }

        private void ExtractData(ThermoRawFile rawFile, Dictionary<int, int> TriggerMS2toTargetMS2, Dictionary<int, int> TargetMS2toTargetMS3, SortedList<double, TargetPeptide> targetList, Dictionary<string, double> quantChannelDict)
        {
            //Cycle through the trigger MS2 scan numbers
            foreach(int scanNumber in TriggerMS2toTargetMS2.Keys)
            {
                double rt = rawFile.GetRetentionTime(scanNumber);
                double it = rawFile.GetInjectionTime(scanNumber);
                double precursorMZ = rawFile.GetPrecursorMz(scanNumber);
                MassRange precursorRange = new MassRange(precursorMZ, new Tolerance(ToleranceUnit.PPM, 15));
                ThermoSpectrum spectrum = null;

                //Look through each target to find a match --This should be a binary search TODO: Binary Search
                foreach(TargetPeptide targetPeptide in targetList.Values)
                {
                    //If we pass this step then we triggered. We will extract all the data here
                    if(precursorRange.Contains(targetPeptide.TriggerMZ))
                    {
                        //If this is the first hit them get the spectrum
                        if(spectrum == null) { spectrum = rawFile.GetSpectrum(scanNumber); }

                        //Grab the trigger data for the Trigger MS2
                        targetPeptide.AddTriggerData(scanNumber, spectrum, rt, it);

                        //If we found a trigger peptide then see if we then targeted it
                        int targetScanNumber = 0;
                        if(TriggerMS2toTargetMS2.TryGetValue(scanNumber, out targetScanNumber))
                        {
                            //If we did an ms2 on the target peptides then get that data as well
                            ExtractTargetData(targetPeptide, rawFile, targetScanNumber, TargetMS2toTargetMS3, quantChannelDict);
                        }
                    }
                }
            }
        }

        private List<TargetPeptide> SearchPeptides(double precursorMZ, SortedList<double, TargetPeptide> targetList)
        {
            List<TargetPeptide> retList = new List<TargetPeptide>();



            return retList;
        }

        private void ExtractTargetData(TargetPeptide targetPeptide, ThermoRawFile rawFile, int targetScanNumber, Dictionary<int, int> TargetMS2toTargetMS3, Dictionary<string, double> quantChannelDict)
        {
            //Grab all of the data for the target and add that data
            double rt = rawFile.GetRetentionTime(targetScanNumber);
            double it = rawFile.GetInjectionTime(targetScanNumber);
            ThermoSpectrum spectrum = rawFile.GetSpectrum(targetScanNumber);

            //If we found a target peptide then see if we did an MS3
            int ms3ScanNumber = 0;
            double ms3rt = 0;
            double ms3it = 0;
            int numSPSIons = 0;
            ThermoSpectrum ms3spectrum = null;
            if (TargetMS2toTargetMS3.TryGetValue(targetScanNumber, out ms3ScanNumber))
            {
                ms3rt = rawFile.GetRetentionTime(ms3ScanNumber);
                ms3it = rawFile.GetInjectionTime(ms3ScanNumber);
                ms3spectrum = rawFile.GetSpectrum(ms3ScanNumber);
                numSPSIons = rawFile.getSPSMasses(ms3ScanNumber).Count;
            }

            //Add the target data and if there was an MS3 add that data too
            if(ms3spectrum == null)
            {
                targetPeptide.AddTargetData(targetScanNumber, spectrum, rt, it);
            }
            else
            {
                targetPeptide.AddTargetData(targetScanNumber, spectrum, rt, it, ms3ScanNumber, ms3spectrum, ms3rt, ms3it, numSPSIons, quantChannelDict);
            }
        }

        private void InitializePrimingRunGraphs()
        {
            MS1Pane = ms1GraphControl.GraphPane;
            MS1Pane.Title.Text = "Precursor Ion XIC";
            MS1Pane.Title.FontSpec.Size = 25f;
            MS1Pane.XAxis.Title.Text = "Retention Time";
            MS1Pane.XAxis.Title.FontSpec.Size = 20f;
            MS1Pane.YAxis.Title.Text = "Intensity";
            MS1Pane.YAxis.Title.FontSpec.Size = 20f;

            SpectrumPane1 = spectrumGraphControl1.GraphPane;
            SpectrumPane1.Title.Text = "Individual MS2 Trigger Spectrum";
            SpectrumPane1.Title.FontSpec.Size = 25f;
            SpectrumPane1.XAxis.Title.Text = "m/z";
            SpectrumPane1.XAxis.Title.FontSpec.Size = 20f;
            SpectrumPane1.YAxis.Title.Text = "Intensity";
            SpectrumPane1.YAxis.Title.FontSpec.Size = 20f;

            SpectrumPane2 = spectrumGraphControl2.GraphPane;
            SpectrumPane2.Title.Text = "Average MS2 Trigger Spectrum";
            SpectrumPane2.Title.FontSpec.Size = 25f;
            SpectrumPane2.XAxis.Title.Text = "m/z";
            SpectrumPane2.XAxis.Title.FontSpec.Size = 20f;
            SpectrumPane2.YAxis.Title.Text = "Intensity";
            SpectrumPane2.YAxis.Title.FontSpec.Size = 20f;
        }

        private void InitializeTargetGrid()
        {
            DataGridViewTextBoxColumn peptideColumn = new DataGridViewTextBoxColumn();
            peptideColumn.DataPropertyName = "PeptideString";
            peptideColumn.HeaderText = "Peptide";
            peptideColumn.ReadOnly = true;
            peptideColumn.Width = 160;
            targetGridView.Columns.Add(peptideColumn);

            DataGridViewTextBoxColumn triggerMZColumn = new DataGridViewTextBoxColumn();
            triggerMZColumn.DataPropertyName = "TriggerMZ";
            triggerMZColumn.HeaderText = "Trigger";
            triggerMZColumn.ReadOnly = true;
            triggerMZColumn.Width = 80;
            targetGridView.Columns.Add(triggerMZColumn);

            DataGridViewTextBoxColumn targetColumn = new DataGridViewTextBoxColumn();
            targetColumn.DataPropertyName = "TargetMZ";
            targetColumn.HeaderText = "Target";
            targetColumn.ReadOnly = true;
            targetColumn.Width = 80;
            targetColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            targetGridView.Columns.Add(targetColumn);

            DataGridViewTextBoxColumn chargeColumn = new DataGridViewTextBoxColumn();
            chargeColumn.DataPropertyName = "Charge";
            chargeColumn.HeaderText = "Charge";
            chargeColumn.ReadOnly = true;
            chargeColumn.Width = 60;
            targetGridView.Columns.Add(chargeColumn);

        }

        private void InitializeModGrid()
        {
            DataGridViewCheckBoxColumn triggerColumn = new DataGridViewCheckBoxColumn();
            triggerColumn.DataPropertyName = "Trigger";
            triggerColumn.HeaderText = "Trigger";
            triggerColumn.ReadOnly = false;
            triggerColumn.Width = 50;
            modGridView.Columns.Add(triggerColumn);

            DataGridViewCheckBoxColumn targetColumn = new DataGridViewCheckBoxColumn();
            targetColumn.DataPropertyName = "Target";
            targetColumn.HeaderText = "Target";
            targetColumn.ReadOnly = false;
            targetColumn.Width = 50;
            modGridView.Columns.Add(targetColumn);

            DataGridViewCheckBoxColumn bothColumn = new DataGridViewCheckBoxColumn();
            bothColumn.DataPropertyName = "Both";
            bothColumn.HeaderText = "Both";
            bothColumn.ReadOnly = false;
            bothColumn.Width = 50;
            modGridView.Columns.Add(bothColumn);

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.ReadOnly = true;
            nameColumn.Width = 50;
            modGridView.Columns.Add(nameColumn);

            DataGridViewTextBoxColumn massColumn = new DataGridViewTextBoxColumn();
            massColumn.DataPropertyName = "Mass";
            massColumn.HeaderText = "Mono Mass";
            massColumn.ReadOnly = true;
            massColumn.Width = 80;
            modGridView.Columns.Add(massColumn);

            DataGridViewTextBoxColumn modTypeColumn = new DataGridViewTextBoxColumn();
            modTypeColumn.DataPropertyName = "Type";
            modTypeColumn.HeaderText = "Type";
            modTypeColumn.ReadOnly = true;
            modTypeColumn.Width = 50;
            modGridView.Columns.Add(modTypeColumn);

            DataGridViewTextBoxColumn modCharColumn = new DataGridViewTextBoxColumn();
            modCharColumn.DataPropertyName = "ModChar";
            modCharColumn.HeaderText = "Symbol";
            modCharColumn.ReadOnly = true;
            modCharColumn.Width = 50;
            modGridView.Columns.Add(modCharColumn);

            DataGridViewTextBoxColumn modSitesColumn = new DataGridViewTextBoxColumn();
            modSitesColumn.DataPropertyName = "ModSites";
            modSitesColumn.HeaderText = "Sites";
            modSitesColumn.ReadOnly = true;
            modSitesColumn.Width = 70;
            modGridView.Columns.Add(modSitesColumn);

            //Populate modded proteins - build a list of possible modifications
            Modification tmt10 = new Modification(229.162932, "TMT10", ModificationSites.K | ModificationSites.NTerminus);
            Modification tmt0 = new Modification(224.152478, "TMT0", ModificationSites.K | ModificationSites.NTerminus);
            Modification tmtSH = new Modification(235.17677, "TMTsh", ModificationSites.K | ModificationSites.NTerminus);
            Modification cam = new Modification(57.02146, "CAM", ModificationSites.C);
            Modification ox = new Modification(15.99491, "OX", ModificationSites.M);
            Modification phos = new Modification(79.96633, "Phos", ModificationSites.S | ModificationSites.T | ModificationSites.Y);

            ModLines.Add(new ModificationLine("TMT0", Math.Round(tmt0.MonoisotopicMass, 5), "K,NTerm", "", "Static", false, true, false, tmt0));
            ModLines.Add(new ModificationLine("TMT10", Math.Round(tmt10.MonoisotopicMass, 5), "K,NTerm", "", "Static", false, false, true, tmt10));
            ModLines.Add(new ModificationLine("TMTsh", Math.Round(tmtSH.MonoisotopicMass, 5), "K,NTerm", "", "Static", false, false, false, tmtSH));
            ModLines.Add(new ModificationLine("CAM", Math.Round(cam.MonoisotopicMass, 5), "C", "", "Static", true, false, false, cam));
            ModLines.Add(new ModificationLine("OX", Math.Round(ox.MonoisotopicMass, 5), "M", "*", "Dynamic", true, false, false, ox));
            ModLines.Add(new ModificationLine("Phos", Math.Round(phos.MonoisotopicMass, 5), "S,T,Y", "#", "Dynamic", false, false, false, phos));
        }

        private void InitializeScanGrid()
        {
            DataGridViewTextBoxColumn ms2RTCol = new DataGridViewTextBoxColumn();
            ms2RTCol.DataPropertyName = "MS2RetentionTime";
            ms2RTCol.HeaderText = "MS2 RT";
            ms2RTCol.ReadOnly = true;
            ms2RTCol.Width = 50;
            scanGridView.Columns.Add(ms2RTCol);

            DataGridViewTextBoxColumn ms2SNCol = new DataGridViewTextBoxColumn();
            ms2SNCol.DataPropertyName = "MS2ScanNumber";
            ms2SNCol.HeaderText = "MS2 Scan#";
            ms2SNCol.ReadOnly = true;
            ms2SNCol.Width = 50;
            scanGridView.Columns.Add(ms2SNCol);

            DataGridViewTextBoxColumn ms2ITCol = new DataGridViewTextBoxColumn();
            ms2ITCol.DataPropertyName = "MS2InjectionTime";
            ms2ITCol.HeaderText = "MS2 IT";
            ms2ITCol.ReadOnly = true;
            ms2ITCol.Width = 50;
            scanGridView.Columns.Add(ms2ITCol);

            DataGridViewTextBoxColumn ms3SNCol = new DataGridViewTextBoxColumn();
            ms3SNCol.DataPropertyName = "MS3ScanNumber";
            ms3SNCol.HeaderText = "MS3 Scan#";
            ms3SNCol.ReadOnly = true;
            ms3SNCol.Width = 50;
            ms3SNCol.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(ms3SNCol);

            DataGridViewTextBoxColumn ms3ITCol = new DataGridViewTextBoxColumn();
            ms3ITCol.DataPropertyName = "MS3InjectionTime";
            ms3ITCol.HeaderText = "MS3 IT";
            ms3ITCol.ReadOnly = true;
            ms3ITCol.Width = 50;
            scanGridView.Columns.Add(ms3ITCol);

            DataGridViewTextBoxColumn spsCol = new DataGridViewTextBoxColumn();
            spsCol.DataPropertyName = "MS3SPSIons";
            spsCol.HeaderText = "# SPS";
            spsCol.ReadOnly = true;
            spsCol.Width = 50;
            scanGridView.Columns.Add(spsCol);

            DataGridViewTextBoxColumn quantCol1 = new DataGridViewTextBoxColumn();
            quantCol1.DataPropertyName = "MS3Quant1";
            quantCol1.HeaderText = "Quant #1";
            quantCol1.ReadOnly = true;
            quantCol1.Width = 50;
            quantCol1.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol1);

            DataGridViewTextBoxColumn quantCol2 = new DataGridViewTextBoxColumn();
            quantCol2.DataPropertyName = "MS3Quant2";
            quantCol2.HeaderText = "Quant #2";
            quantCol2.ReadOnly = true;
            quantCol2.Width = 50;
            scanGridView.Columns.Add(quantCol2);

            DataGridViewTextBoxColumn quantCol3 = new DataGridViewTextBoxColumn();
            quantCol3.DataPropertyName = "MS3Quant3";
            quantCol3.HeaderText = "Quant #3";
            quantCol3.ReadOnly = true;
            quantCol3.Width = 50;
            quantCol3.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol3);

            DataGridViewTextBoxColumn quantCol4 = new DataGridViewTextBoxColumn();
            quantCol4.DataPropertyName = "MS3Quant4";
            quantCol4.HeaderText = "Quant #4";
            quantCol4.ReadOnly = true;
            quantCol4.Width = 50;
            scanGridView.Columns.Add(quantCol4);

            DataGridViewTextBoxColumn quantCol5 = new DataGridViewTextBoxColumn();
            quantCol5.DataPropertyName = "MS3Quant5";
            quantCol5.HeaderText = "Quant #5";
            quantCol5.ReadOnly = true;
            quantCol5.Width = 50;
            quantCol5.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol5);

            DataGridViewTextBoxColumn quantCol6 = new DataGridViewTextBoxColumn();
            quantCol6.DataPropertyName = "MS3Quant6";
            quantCol6.HeaderText = "Quant #6";
            quantCol6.ReadOnly = true;
            quantCol6.Width = 50;
            scanGridView.Columns.Add(quantCol6);

            DataGridViewTextBoxColumn quantCol7 = new DataGridViewTextBoxColumn();
            quantCol7.DataPropertyName = "MS3Quant7";
            quantCol7.HeaderText = "Quant #7";
            quantCol7.ReadOnly = true;
            quantCol7.Width = 50;
            quantCol7.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol7);

            DataGridViewTextBoxColumn quantCol8 = new DataGridViewTextBoxColumn();
            quantCol8.DataPropertyName = "MS3Quant8";
            quantCol8.HeaderText = "Quant #8";
            quantCol8.ReadOnly = true;
            quantCol8.Width = 50;
            scanGridView.Columns.Add(quantCol8);

            DataGridViewTextBoxColumn quantCol9 = new DataGridViewTextBoxColumn();
            quantCol9.DataPropertyName = "MS3Quant9";
            quantCol9.HeaderText = "Quant #9";
            quantCol9.ReadOnly = true;
            quantCol9.Width = 50;
            quantCol9.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol9);

            DataGridViewTextBoxColumn quantCol10 = new DataGridViewTextBoxColumn();
            quantCol10.DataPropertyName = "MS3Quant10";
            quantCol10.HeaderText = "Quant #10";
            quantCol10.ReadOnly = true;
            quantCol10.Width = 50;
            scanGridView.Columns.Add(quantCol10);

            DataGridViewTextBoxColumn sumSNCol = new DataGridViewTextBoxColumn();
            sumSNCol.DataPropertyName = "MS3SumSN";
            sumSNCol.HeaderText = "Sum SN";
            sumSNCol.ReadOnly = true;
            sumSNCol.Width = 75;
            scanGridView.Columns.Add(sumSNCol);

        }

        private Dictionary<string, double> AddQuantitationChannels(Dictionary<Modification, string> staticMods)
        {
            Modification targetMod = null;
            foreach (KeyValuePair<Modification, string> kvp in staticMods)
            {
                if (kvp.Value == "Target")
                {
                    targetMod = kvp.Key;
                }
            }

            return QuantChannelDict[targetMod.Name];
        }

        private Dictionary<string, Dictionary<string, double>> BiuldQuantificationDictionary()
        {
            Dictionary<string, Dictionary<string, double>> retDict = new Dictionary<string, Dictionary<string, double>>();

            retDict.Add("TMT10", new Dictionary<string, double>());
            //retDict.Add("TMT6", new Dictionary<string, double>());
            //retDict.Add("iTRAQ4", new Dictionary<string, double>());
            //retDict.Add("iTRAQ8", new Dictionary<string, double>());

            retDict["TMT10"].Add("126", 126.127725);
            retDict["TMT10"].Add("127n", 127.124760);
            retDict["TMT10"].Add("127c", 127.131079);
            retDict["TMT10"].Add("128n", 128.128114);
            retDict["TMT10"].Add("128c", 128.134433);
            retDict["TMT10"].Add("129n", 129.131468);
            retDict["TMT10"].Add("129c", 129.137787);
            retDict["TMT10"].Add("130n", 130.134822);
            retDict["TMT10"].Add("130c", 130.141141);
            retDict["TMT10"].Add("131", 131.138176);

            return retDict;
        }

        private void UpdateLog(string message)
        {
            logBox.AppendText(string.Format("[{0}]\t{1}\n", DateTime.Now.ToLongTimeString(), message));
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }

        private void UpdatePlotsPriming(TargetPeptide target)
        {
            
        }

        private void UpdatePlotsAnalysis(TargetPeptide target)
        {
            //Populate the scan Events Table
            ScanEvents.Clear();
            foreach (ScanEventLine scanEvent in target.ScanEventLines)
            {
                if(scanEvent.ScanEvent.MS3 != null)
                {
                    ScanEvents.Add(scanEvent);
                }
            }

            UpdateMS1XICsPlot(target);

            if(target.TargetMS2s.Count > 0)
            {
                MS2Event ms2Event = target.TargetMS2s.ElementAt(0);
                UpdateTargetMS2Plot(ms2Event);

                if (ms2Event.MS3 != null)
                {
                    UpdateTargetMS3Plot(ms2Event.MS3);
                }
            }
        }

        private void UpdateMS1XICsPlot(TargetPeptide target)
        {
            MS1Pane.CurveList.Clear();

            LineItem triggerXIC = MS1Pane.AddCurve("Trigger XIC", target.TriggerMS1XicPoints, Color.Red, SymbolType.None);
            triggerXIC.Line.Width = 2.0F;

            LineItem targetXIC = MS1Pane.AddCurve("Target XIC", target.TargetMS1XicPoints, Color.Blue, SymbolType.None);
            targetXIC.Line.Width = 2.0F;

            MS1Pane.Legend.FontSpec.Size = 20f;

            ms1GraphControl.AxisChange();
            ms1GraphControl.Refresh();
        }

        private void UpdateTargetMS2Plot(MS2Event ms2)
        {
            SpectrumPane1.CurveList.Clear();

            SpectrumPane1.Title.Text = "Target MS2 Spectrum";
            SpectrumPane1.AddStick("Target Spectrum", ms2.AllPeaks, Color.Black);

            spectrumGraphControl1.AxisChange();
            spectrumGraphControl1.Refresh();
        }

        private void UpdateTargetMS3Plot(MS3Event ms3)
        {
            SpectrumPane2.CurveList.Clear();

            SpectrumPane2.Title.Text = "Reporter Ion Intensity (Signal to Noise)";
            SpectrumPane2.XAxis.Title.Text = "Reporter Ion";
            SpectrumPane2.YAxis.Title.Text = "Signal to Noise";
            SpectrumPane2.AddBar("QuantData", ms3.QuantPeaks, Color.Blue);
            SpectrumPane2.XAxis.Type = AxisType.Text;
            SpectrumPane2.XAxis.Scale.TextLabels = QuantChannelsInUse.Keys.ToArray();
            
            spectrumGraphControl2.AxisChange();
            spectrumGraphControl2.Refresh();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void scanGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (ScanEvents.Count != 0)
            {
                int index = 0;
                if (scanGridView.CurrentCell != null)
                {
                    index = scanGridView.CurrentCell.RowIndex;
                }

                MS2Event ms2Event = ScanEvents[index].ScanEvent;
                UpdateTargetMS2Plot(ms2Event);

                if (ms2Event.MS3 != null)
                {
                    UpdateTargetMS3Plot(ms2Event.MS3);
                }

            }
        }

        private void targetGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (Targets.Count != 0)
            {
                int index = 0;
                if (targetGridView.CurrentCell != null)
                {
                    index = targetGridView.CurrentCell.RowIndex;
                }

                TargetPeptide target = Targets[index].Peptide;

                if(Analysis)
                {
                    UpdatePlotsAnalysis(target);
                }
                else if (Priming)
                {
                    UpdatePlotsPriming(target);
                }
            }
        }

        private string BuildMethodXML(string inputFile, List<TargetPeptide> targetList)
        {
            //Save the xml for future use
            string file = inputFile.Replace(".csv","_method.xml");

            //Set up the serializer
            XmlSerializer serializer = new XmlSerializer(typeof(MethodModifications));

            //Set up the class that will hold all of the instructions
            MethodModifications methodMods = new MethodModifications("1", "OrbitrapFusion", "Calcium", "SL");

            //First we will make sure we have enough trees for all of the peptides
            int modCount = 1;
            int experimentIndex = 0;
            foreach (TargetPeptide target in targetList)
            {
                int sourceNodeForCopy = modCount - 1;
                MethodExp addExp = new MethodExp(experimentIndex);
                addExp.CopyAndPasteScanNode(sourceNodeForCopy);

                MethodMod addMod = new MethodMod(modCount, addExp);
                methodMods.Modification.Add(addMod);

                modCount++;
            }

            //Next, we will change all of the trees to include all of the attributes of the peptides
            int treeIndex = 0;
            foreach (TargetPeptide target in targetList)
            {
                //Add in the MS1 inclusion list, and MS2 isolation offset
                MethodExp addExp = new MethodExp(experimentIndex);
                addExp.ChangeScanParams(treeIndex, target.MassShift);
                addExp.AddMS1InclusionSingle(treeIndex, target.TriggerMZ, target.Charge);

                MethodMod addMod = new MethodMod(modCount, addExp);
                methodMods.Modification.Add(addMod);

                modCount++;

                //Add the MS2 trigger mass List
                MethodExp addExp1 = new MethodExp(experimentIndex);
                addExp1.AddMS2TriggerList(treeIndex, target.TriggerIons);

                MethodMod addMod1 = new MethodMod(modCount, addExp1);
                methodMods.Modification.Add(addMod1);

                modCount++;

                //Add in the MS3 inclusion list - This needs to be done seperately because I don't think there can be multiple mass lists in one experiment
                MethodExp addExp2 = new MethodExp(experimentIndex);
                addExp2.AddMS3InclusionList(treeIndex, target.TargetSPSIons);

                MethodMod addMod2 = new MethodMod(modCount, addExp2);
                methodMods.Modification.Add(addMod2);

                modCount++;
                treeIndex++;
            }

            //Write the XML
            using (TextWriter writer = new StreamWriter(file))
            {
                serializer.Serialize(writer, methodMods);
            }

            return file;
        }

        private void analysisTab_Click(object sender, EventArgs e)
        {

        }
    }
}
