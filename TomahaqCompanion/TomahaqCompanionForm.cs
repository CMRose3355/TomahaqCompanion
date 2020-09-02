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
using System.Diagnostics;
using XmlMethodChanger; 
using XmlMethodChanger.lib;



namespace TomahaqCompanion
{
    public partial class TomahaqCompanionForm : Form
    {
        private bool Analysis;
        private bool Priming;
        private bool SPSIonsEdited;
        private BindingList<TargetPeptideLine> Targets;
        private BindingList<TargetPeptideLine> TargetsDisplayed;
        private BindingList<ModificationLine> ModLines;
        private BindingList<ScanEventLine> ScanEvents;
        private BindingList<string> ModificationFiles;
        private Dictionary<string, Dictionary<string, double>> QuantChannelDict;
        private Dictionary<string, double> QuantChannelsInUse;
        private GraphPane MS1Pane;
        private GraphPane SpectrumPane1;
        private GraphPane SpectrumPane2;
        private double MouseDownTime;
        private double MouseUpTime;

        private Tolerance FragmentTol;

        public string FolderPath { get; set; }

        public TomahaqCompanionForm()
        {
            InitializeComponent();

            string newFolder = "TomahaqCompanionFiles";
            FolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), newFolder);
            if (!System.IO.Directory.Exists(FolderPath))
            {
                System.IO.Directory.CreateDirectory(FolderPath);
            }

            //
            Analysis = false;
            Priming = false;

            MouseDownTime = 0;
            MouseUpTime = 0;

            //Initialize the Target Grid View
            Targets = new BindingList<TargetPeptideLine>();
            TargetsDisplayed = new BindingList<TargetPeptideLine>();
            targetGridView.AutoGenerateColumns = false;
            targetGridView.DataSource = TargetsDisplayed;
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

            ModificationFiles = new BindingList<string>();
            modFileListBox.DataSource = ModificationFiles;

            UpdateMoficationFileList();

            InitializePrimingRunGraphs();

            UpdateLog("Version = 1.1.0.0");
        }

        //These are the main buttons that perform functions within the program
        private void primingTargetList_Click(object sender, EventArgs e)
        {
            //Import the modifications
            UpdateLog("Importing Modifications");
            //<Static/Dynamic, <Trigger/Target, <Modification, Symbol>>>
            Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modificationDict = BuildModificationDictionary();

            //Import the Peptides
            string targetFile = targetTextBox.Text;
            bool chargeProvided = false;
            SortedList<double, TargetPeptide> targetList = ImportTargets(targetFile, modificationDict, out chargeProvided);

            Dictionary<string, TargetPeptide> addedTargets = new Dictionary<string, TargetPeptide>();

            //Print the Priming Run Target List
            using (StreamWriter writer = new StreamWriter(targetFile.Replace(".csv", "_primingRunInclusionList.csv")))
            {
                writer.WriteLine("Name,M,z range");
                foreach (TargetPeptide peptide in targetList.Values)
                {
                    TargetPeptide outPep = null;
                    if(!addedTargets.TryGetValue(peptide.PeptideString, out outPep))
                    {
                        writer.WriteLine(peptide.PeptideString + "," + peptide.Trigger.MonoisotopicMass + ",2-5");
                        addedTargets.Add(peptide.PeptideString, peptide);
                    }
                }
            }
        }

        private void createMethod_Click_1(object sender, EventArgs e)
        {
            //try
            //{
                Priming = true;
                Analysis = false;
                SPSIonsEdited = false;

                FragmentTol = new Tolerance(ToleranceUnit.PPM, double.Parse(fragTolBox.Text));
                if(daRB.Checked)
                {
                    FragmentTol = new Tolerance(ToleranceUnit.DA, double.Parse(fragTolBox.Text));
                }

                //Determine if a raw file was provided for the priming run
                bool rawFileProvided = false;
                if(primingRawBox.Text != "")
                {
                    rawFileProvided = true;
                }

                //Import the modifications
                UpdateLog("Importing Modifications");
                //This is the structure of this awful thing. <Static/Dynamic, <Trigger/Target, <Modification, Symbol>>>
                Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modificationDict = BuildModificationDictionary();

                //Determine the quantification channels
                //Here we just want to look at the target peptides that are static modifications
                UpdateLog("Determining Quantification Channels");
                QuantChannelsInUse = AddQuantitationChannels(modificationDict["Static"]["Target"]);

                //Import the Peptides
                UpdateLog("Importing Target Peptides");
                string targetFile = targetTextBox.Text;
                bool chargeProvided = false;
                SortedList<double, TargetPeptide> targetList = ImportTargets(targetFile, modificationDict, out chargeProvided);

                double methodLength = double.Parse(methodLengthBox.Text);
                //Iterate through the peptides, create ms1, ms2, and potentially ms3 lists
                if (rawFileProvided)
                {
                    UpdateLog("Opening Raw File");
                    ThermoRawFile rawFile = new ThermoRawFile(primingRawBox.Text);
                    rawFile.Open();

                    double actualMethodLength = rawFile.GetRetentionTime(rawFile.LastSpectrumNumber);
                    actualMethodLength = Math.Round(actualMethodLength, 0);

                    if(methodLength != actualMethodLength)
                    {
                        UpdateLog("Priming Run Method Length and User Input Method Length Do Not Match...Overriding User Input");
                        methodLengthBox.Text = Math.Round(methodLength, 0).ToString();
                        methodLength = actualMethodLength;
                    }

                    //Build a map of the MS/MS scan events
                    UpdateLog("Mapping MS Scans");
                    Dictionary<int, int> TriggerMS2toMS1 = null;
                    Dictionary<int, List<int>> TriggerMS2toTargetMS2 = null;
                    Dictionary<int, List<int>> TargetMS2toTargetMS3 = null;
                    List<int> ms1Scans = MapMSDataScans(rawFile, out TriggerMS2toMS1, out TriggerMS2toTargetMS2, out TargetMS2toTargetMS3);

                    //Set the default values times for each target
                    foreach(TargetPeptide targetPeptide in targetList.Values)
                    {
                        //Set the RT window
                        targetPeptide.RTWindow = double.Parse(rtWindowTextBox.Text);
                    }
                    
                    //Extract MS1 Information
                    UpdateLog("Extracting MS1 XICs");
                    ExtractMS1XIC(rawFile, ms1Scans, targetList);

                    //Populate the trigger ms2 data
                    UpdateLog("Extracting MS/MS Data");
                    ExtractData(rawFile, TriggerMS2toMS1, TriggerMS2toTargetMS2, TargetMS2toTargetMS3, targetList, QuantChannelsInUse);

                    UpdateLog("Closing Raw File");
                    rawFile.Dispose();
                }

                //Populate Target SPS Ions
                UpdateLog("Consolidating Data for GUI Tables");
                List<TargetPeptide> targets = new List<TargetPeptide>();
                foreach (TargetPeptide target in targetList.Values)
                {
                    //If a raw file was provided then we will use that data for the target list
                    if (rawFileProvided)
                    {
                        target.PopulatePrimingData(methodLength);
                        target.UpdateIons((int) triggerIonMax.Value, (int) targetIonMax.Value);
                    }

                    //This code will ensure that target or trigger ions are produced in the absence of a raw file
                    target.PopulateTriggerAndTargetIons((int)triggerIonMax.Value, (int)targetIonMax.Value); //TODO: Change this to a user param

                    //Add this to a list of targets that is not indexed on m/z
                    targets.Add(target);
                }

                //Choose the best charge state, but only if there is a raw file provided.
                //If the user provided the charge states skip this as well
                if(rawFileProvided && !chargeProvided && chooseBestCharge.Checked)
                {
                    targets = ChooseBestChargeState(targets);
                }

                //Now we can populate the final
                Targets.Clear();

                foreach (TargetPeptide target in targets)
                {
                    //This is building the target line that will go into the GUI
                    Targets.Add(new TargetPeptideLine(target));
                }

                DisplayTargets();

                printAllTomahaqData();

                //Make the XMLfile that will be used to alter the method
                //Only change the parameters the user wants to
                UpdateLog("Building XML");
                bool addMS1TargetedMass = addMS1TargetMassList.Checked;
                bool addMS2TriggerMass = addMS2TriggerMassList.Checked;
                bool addMS2IsoOffset = addMS2IsolationOffset.Checked;
                bool addMS3TargetedMass = addMS3TargetMassList.Checked;
                string xmlFile = null;               
                if(!groupIDCheckBox.Checked)
                {
                    xmlFile = BuildMethodXML(targetFile.Replace(".csv", "_method.xml"), targets, addMS1TargetedMass, addMS2TriggerMass, addMS2IsoOffset, addMS3TargetedMass); //TODO: Change Back to Not Large
                }
                else
                {
                    xmlFile = BuildLargeMethodXML(targetFile.Replace(".csv", "_method.xml"), targets, addMS1TargetedMass, addMS2TriggerMass, addMS2IsoOffset, addMS3TargetedMass); //TODO: Change Back to Not Large
                }
            

                //Export the method last in case it fails due to the program not being run on the instrument
                UpdateLog("Creating New Method");
                if (templateBox.Text != "")
                {
                    string templateMethod = templateBox.Text;
                    string outputMethod = targetFile.Replace(".csv", ".meth");
                    EditMethod(templateMethod, xmlFile, outputMethod);
                    ////XmlMethodChanger.lib.MethodChanger.ModifyMethod(templateMethod, xmlFile, outputMethod: outputMethod);
                    UpdateLog("Writing method to " + outputMethod);
                }
                else
                {
                    UpdateLog("Cannot Create New Method Because No Template Was Provided");
                }

                //This will switch the GUI to the data analysis tab, whose index is 1
                tabControl.SelectTab(1);
            //}
            //catch (Exception exp)
            //{
            //    UpdateLog("Error! " + exp.Message);
            //}

        }

        private void methodChangerAlone_Click(object sender, EventArgs e)
        {
            string templateMethod = templateBox.Text;
            string outputMethod = targetTextBox.Text.Replace(".csv", ".meth");
            string xmlFile = xmlTextBox.Text;

            EditMethod(templateMethod, xmlFile, outputMethod);
        }

        public void EditMethod(string templatePath, string modPath, string outputPath)
        {
            File.Copy(templatePath, ".\\XMLMethodChanger\\Template.meth", true);
            File.Copy(modPath, ".\\XMLMethodChanger\\Mods.xml", true);

            string exePath = ".\\XMLMethodChanger\\XmlMethodChanger.exe";
            string args = "-i .\\XMLMethodChanger\\Template.meth -m .\\XMLMethodChanger\\Mods.xml -o .\\XMLMethodChanger\\Output.meth";
            Console.WriteLine(args);
            ExecuteCommand(exePath, args);

            File.Copy(".\\XMLMethodChanger\\Output.meth", outputPath, true);

            File.Delete(".\\XMLMethodChanger\\Template.meth");
            File.Delete(".\\XMLMethodChanger\\Mods.xml");
            File.Delete(".\\XMLMethodChanger\\Output.meth");
        }

        public bool ExecuteCommand(string exePath, string args)
        {
            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo();

                procStartInfo.FileName = exePath;
                procStartInfo.Arguments = args;
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;

                using (Process process = new Process())
                {
                    process.StartInfo = procStartInfo;
                    process.Start();

                    process.WaitForExit();

                    string result = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(result);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("*** Error occured executing the following commands.");
                Console.WriteLine(exePath);
                Console.WriteLine(args);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void analyzeRun_Click(object sender, EventArgs e)
        {
           try
           {
                Priming = false;
                Analysis = true;

                FragmentTol = new Tolerance(ToleranceUnit.PPM, double.Parse(fragTolBox.Text));
                if (daRB.Checked)
                {
                    FragmentTol = new Tolerance(ToleranceUnit.DA, double.Parse(fragTolBox.Text));
                }

                //Import the modifications
                UpdateLog("Importing Modifications");
                //<Static/Dynamic, <Trigger/Target, <Modification, Symbol>>>
                Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modificationDict = BuildModificationDictionary();

                //Determine the quantification channels
                //Here we just want to look at the target peptides that are static modifications
                UpdateLog("Determining Quantification Channels");
                QuantChannelsInUse = AddQuantitationChannels(modificationDict["Static"]["Target"]);

                //Import the Peptides
                UpdateLog("Importing Target Peptides");
                string targetFile = targetTextBox.Text;
                bool chargeProvided = false;
                SortedList<double, TargetPeptide> targetList = ImportTargets(targetFile, modificationDict, out chargeProvided);

                //Iterate through the peptides, create ms1, ms2, and potentially ms3 lists
                UpdateLog("Opening Raw File");
                ThermoRawFile rawFile = new ThermoRawFile(rawFileBox.Text);
                rawFile.Open();

                //Build a map of the MS/MS scan events
                UpdateLog("Mapping MS Scans");
                Dictionary<int, int> TriggerMS2toMS1 = null;
                Dictionary<int, List<int>> TriggerMS2toTargetMS2 = null;
                Dictionary<int, List<int>> TargetMS2toTargetMS3 = null;

                //Get a spectrum number in the middle of the run
                int midSpectrum = rawFile.LastSpectrumNumber / 2;
                string scanDescription = rawFile.GetScanDescription(midSpectrum);

                List<int> ms1Scans = new List<int>();
                if (tomaAPIRawFile.Checked) //USER_INPUT
                {
                    
                    ms1Scans = MapAPIMSDataScans(rawFile, out TriggerMS2toMS1, out TriggerMS2toTargetMS2, out TargetMS2toTargetMS3, targetList);
                }
                else if(ms2DataAnalysis.Checked)
                {
                    ms1Scans = MapMS2DataScans(rawFile, out TriggerMS2toMS1, out TriggerMS2toTargetMS2, out TargetMS2toTargetMS3);
                }
                else
                {
                    ms1Scans = MapMSDataScans(rawFile, out TriggerMS2toMS1, out TriggerMS2toTargetMS2, out TargetMS2toTargetMS3);
                }

                //Extract MS1 Information
                UpdateLog("Extracting MS1 XICs");
                ExtractMS1XIC(rawFile, ms1Scans, targetList);

                //Populate the trigger ms2 data
                UpdateLog("Extracting MS/MS Data");
                ExtractData(rawFile, TriggerMS2toMS1, TriggerMS2toTargetMS2, TargetMS2toTargetMS3, targetList, QuantChannelsInUse);

                //Close the Raw file
                UpdateLog("Closing Raw File");
                rawFile.Dispose();

                //Build the User GUI data for each target
                UpdateLog("Consolidating Data for GUI Tables");
                Targets.Clear();
                TargetsDisplayed.Clear();

                foreach (TargetPeptide target in targetList.Values)
                {
                    //Here we will populate all of the data necessary for analysis of the data
                    target.PopulateAnalysisData();

                    //This is building the target line that will go into the GUI
                    Targets.Add(new TargetPeptideLine(target));
                    TargetsDisplayed.Add(new TargetPeptideLine(target));
                }

                printAllTomahaqData();

                //This will switch the GUI to the data analysis tab, whose index is 1
                tabControl.SelectTab(1);


            }
            catch (Exception exp)
            {
                UpdateLog("Error! " + exp.Message);
            }
        }

        private void updateMethod_Click(object sender, EventArgs e)
        {
            updateInstrumentMethod();
        }

        //Below are the methods and functions that are called from the buttons above
        private Dictionary<string, Dictionary<string, double>> BiuldQuantificationDictionary()
        {
            Dictionary<string, Dictionary<string, double>> retDict = new Dictionary<string, Dictionary<string, double>>();

            retDict.Add("TMT11", new Dictionary<string, double>());
            retDict.Add("TMT6", new Dictionary<string, double>());
            //retDict.Add("iTRAQ4", new Dictionary<string, double>());
            //retDict.Add("iTRAQ8", new Dictionary<string, double>());

            retDict["TMT11"].Add("126", 126.127725);
            retDict["TMT11"].Add("127n", 127.124760);
            retDict["TMT11"].Add("127c", 127.131079);
            retDict["TMT11"].Add("128n", 128.128114);
            retDict["TMT11"].Add("128c", 128.134433);
            retDict["TMT11"].Add("129n", 129.131468);
            retDict["TMT11"].Add("129c", 129.137787);
            retDict["TMT11"].Add("130n", 130.134822);
            retDict["TMT11"].Add("130c", 130.141141);
            retDict["TMT11"].Add("131n", 131.138176);
            retDict["TMT11"].Add("131c", 131.144499);

            retDict["TMT6"].Add("126", 126.127725);
            retDict["TMT6"].Add("127n", 127.124760);
            retDict["TMT6"].Add("127c", 127.131079);
            retDict["TMT6"].Add("128n", 128.128114);
            retDict["TMT6"].Add("128c", 128.134433);
            retDict["TMT6"].Add("129n", 129.131468);
            retDict["TMT6"].Add("129c", 129.137787);
            retDict["TMT6"].Add("130n", 130.134822);
            retDict["TMT6"].Add("130c", 130.141141);
            retDict["TMT6"].Add("131n", 131.138176);
            retDict["TMT6"].Add("131c", 131.144499);

            return retDict;
        }

        private Dictionary<string, double> AddQuantitationChannels(Dictionary<Modification, string> staticMods)
        {
            //This will be the dictionary of the quantitative channels
            Dictionary<string, double> retDict = null;

            //Cycle through the modifications that are being used
            foreach (KeyValuePair<Modification, string> kvp in staticMods)
            {
                //We need to check to see if this is actually being used for quantiation
                //There is a change you can have a static mod that varies between the two peptides but is not being used for quantiation
                Dictionary<string, double> outDict = null;
                if (QuantChannelDict.TryGetValue(kvp.Key.Name, out outDict))
                {
                    //If we are using a mod that has quantitative data then get it from the dictionary
                    //This quant channel dictionary is manually curated
                    retDict = QuantChannelDict[kvp.Key.Name];
                }
            }

            //Return the values
            return retDict;
        }

        private Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> BuildModificationDictionary()
        {
            Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> retDict = new Dictionary<string, Dictionary<string, Dictionary<Modification, string>>>();

            retDict.Add("Static", new Dictionary<string, Dictionary<Modification, string>>());
            retDict.Add("Dynamic", new Dictionary<string, Dictionary<Modification, string>>());

            retDict["Static"].Add("Trigger", new Dictionary<Modification, string>());
            retDict["Static"].Add("Target", new Dictionary<Modification, string>());

            retDict["Dynamic"].Add("Trigger", new Dictionary<Modification, string>());
            retDict["Dynamic"].Add("Target", new Dictionary<Modification, string>());

            foreach (ModificationLine modLine in ModLines)
            {
                Modification addMod = modLine.Modification;

                if (modLine.Trigger)
                {
                    retDict[modLine.Type]["Trigger"].Add(addMod, modLine.ModChar);
                }

                if (modLine.Target)
                {
                    retDict[modLine.Type]["Target"].Add(addMod, modLine.ModChar);
                }
            }

            return retDict;
        }

        private SortedList<double, TargetPeptide> ImportTargets(string targetFile, Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modificationDict, out bool chargeProvided)
        {
            //This will be the list to return, it is a sorted list so we can do a binary search if the list gets large in the future
            SortedList<double, TargetPeptide> retList = new SortedList<double, TargetPeptide>();
            chargeProvided = false;

            //Cycle through the csv and load in each peptide
            using (CsvReader reader = new CsvReader(new StreamReader(targetFile), true))
            {
                //Grab the headers from the csv
                List<string> headers = reader.GetFieldHeaders().ToList();

                //Reach each line
                Random rnd = new Random();
                while (reader.ReadNextRecord())
                {
                    //Get the peptide string
                    string peptideString = reader["Peptide"];

                    string proteinString = "";
                    if(headers.Contains("Protein"))
                    {
                        proteinString = reader["Protein"];
                    }

                    //Determine if a charge is provided or a range needs to be used
                    int minCharge = 2; //TODO: User Param
                    int maxCharge = 4; //TODO: User Param
                    if(headers.Contains("z"))
                    {
                        minCharge = int.Parse(reader["z"]);
                        maxCharge = minCharge;
                        chargeProvided = true;
                    }

                    //Determine if a start and end time is provided
                    double startTime = -1;
                    double endTime = -1;
                    if(headers.Contains("StartTime"))
                    {
                        startTime = double.Parse(reader["StartTime"]);
                        endTime = double.Parse(reader["EndTime"]);
                    }

                    //Determine if a start and end time is provided
                    double eo = -1;
                    if (headers.Contains("EO"))
                    {
                        eo = double.Parse(reader["EO"]);
                    }

                    //Determine if the user wants to use specific trigger masses
                    List<double> triggerFragIons = new List<double>();
                    if (headers.Contains("MS2 Trigger m/z"))
                    {
                        triggerFragIons = LoadUserIons(reader["MS2 Trigger m/z"]);
                    }

                    //Determine if the user input SPS ions that they want to use 
                    List<double> targetSPSIons = new List<double>();
                    if(headers.Contains("MS3 Target m/z"))
                    {
                        targetSPSIons = LoadUserIons(reader["MS3 Target m/z"]);
                    }

                    //
                    for (int charge = minCharge; charge <= maxCharge;charge++)
                    {
                        TargetPeptide target = new TargetPeptide(peptideString, proteinString, charge, modificationDict, targetSPSIons, triggerFragIons, startTime, endTime, FragmentTol, double.Parse(spsMinMZ.Text), double.Parse(spsMaxMZ.Text), double.Parse(precExLow.Text), double.Parse(precExHigh.Text), eo, spsIonsAbovePrec.Checked);

                        TargetPeptide outPep = null;
                        double mz = target.Trigger.ToMz(charge);
                        if (!retList.TryGetValue(mz, out outPep))
                        {
                            retList.Add(mz, target);
                        }
                        else
                        {
                            double rand = rnd.Next(1, 1000);
                            double newMass = mz + (rand * 0.000001);
                            retList.Add(newMass, target);
                            UpdateLog("Error! Multiple targets with same mass detected");
                            UpdateLog("Adding Peptide Anyway...Be Careful with analysis");
                        }
                    }
                }
            }

            return retList;
        }

        private List<double> LoadUserIons(string entry)
        {
            List<double> targetSPSIons = new List<double>();
            List<string> targetSPSIonsStrings = entry.Split(';').ToList();

            foreach (string ion in targetSPSIonsStrings)
            {
                targetSPSIons.Add(double.Parse(ion));
            }

            return targetSPSIons;
        }

        private List<int> MapMSDataScans(ThermoRawFile rawFile, out Dictionary<int, int> TriggerMS2toMS1, out Dictionary<int, List<int>> TriggerMS2toTargetMS2, out Dictionary<int, List<int>> TargetMS2toTargetMS3)
        {
            List<int> ms1List = new List<int>();
            TriggerMS2toMS1 = new Dictionary<int, int>();
            TriggerMS2toTargetMS2 = new Dictionary<int, List<int>>();
            TargetMS2toTargetMS3 = new Dictionary<int, List<int>>();

            int lastSpectrumNumber = rawFile.LastSpectrumNumber;

            //Cycle through the raw file
            for (int i = 1; i <= lastSpectrumNumber; i++)
            {
                //I am just querying the msn order of the scan, I think this is faster than getting the whole scan each time
                int msnOrder = rawFile.GetMsnOrder(i);

                //If it is an MS1 then just add the number to a list
                if (msnOrder == 1)
                {
                    ms1List.Add(i);
                }
                //If it is an MS3 then make the connection to the target MS2
                else if (msnOrder == 3)
                {
                    List<int> outList = null;
                    if(TargetMS2toTargetMS3.TryGetValue(rawFile.GetParentSpectrumNumber(i), out outList))
                    {
                        outList.Add(i);
                    }
                    else
                    {
                        TargetMS2toTargetMS3.Add(rawFile.GetParentSpectrumNumber(i), new List<int>());
                        TargetMS2toTargetMS3[rawFile.GetParentSpectrumNumber(i)].Add(i);
                    }
                    
                }
                //If it is an MS2 then it could be a trigger or target
                else if (msnOrder == 2)
                {
                    int parentScanNumber = rawFile.GetParentSpectrumNumber(i);
                    int parentOrder = rawFile.GetMsnOrder(parentScanNumber);

                    //If the parent order is 2 then this is a target ms2 and the parent is the trigger
                    if (parentOrder == 2)
                    {

                        List<int> outList = null;
                        if (TriggerMS2toTargetMS2.TryGetValue(parentScanNumber, out outList))
                        {
                            outList.Add(i);
                        }
                        else
                        {
                            TriggerMS2toTargetMS2.Add(parentScanNumber, new List<int>());
                            TriggerMS2toTargetMS2[parentScanNumber].Add(i);
                        }
                    }
                    //If the parent order is 1 then this is a trigger ms2 and the parent is an MS1
                    else if (parentOrder == 1)
                    {
                        TriggerMS2toMS1.Add(i, parentScanNumber);
                    }
                }
            }

            return ms1List;
        }

        private List<int> MapAPIMSDataScans(ThermoRawFile rawFile, out Dictionary<int, int> TriggerMS2toMS1, out Dictionary<int, List<int>> TriggerMS2toTargetMS2, out Dictionary<int, List<int>> TargetMS2toTargetMS3, SortedList<double, TargetPeptide> targetList)
        {
            List<int> ms1List = new List<int>();
            TriggerMS2toMS1 = new Dictionary<int, int>();
            TriggerMS2toTargetMS2 = new Dictionary<int, List<int>>();
            TargetMS2toTargetMS3 = new Dictionary<int, List<int>>();

            Dictionary<int, int> mappedTargetMS2s = new Dictionary<int, int>();

            int lastSpectrumNumber = rawFile.LastSpectrumNumber;

            for(int i = lastSpectrumNumber; i > 0; i--)
            {
                //I am just querying the msn order of the scan, I think this is faster than getting the whole scan each time
                int msnOrder = rawFile.GetMsnOrder(i);

                //If it is an MS1 then just add the number to a list
                if (msnOrder == 1)
                {
                    ms1List.Add(i);
                }
                //If it is an MS3 then make the connection to the target MS2
                else if (msnOrder == 3)
                {
                    //MS3 Find parent scan number
                    int ms3ParentScanNumber = FindParentMS3(rawFile, i, mappedTargetMS2s, out mappedTargetMS2s);

                    List<int> outList = null;
                    if (TargetMS2toTargetMS3.TryGetValue(ms3ParentScanNumber, out outList))
                    {
                        outList.Add(i);
                    }
                    else
                    {
                        TargetMS2toTargetMS3.Add(ms3ParentScanNumber, new List<int>());
                        TargetMS2toTargetMS3[ms3ParentScanNumber].Add(i);
                    }

                }
                //If it is an MS2 then it could be a trigger or target
                else if (msnOrder == 2)
                {
                    int triggerScanNumber = i;
                    string mzToParse = rawFile.GetScanFilterMZ(i);
                    double precMZ = double.Parse(mzToParse);
;
                    foreach(KeyValuePair<double, TargetPeptide> kvp in targetList)
                    {
                        MzRange triggerRange = new MzRange(kvp.Key, new Tolerance(ToleranceUnit.PPM, 15));
                        if(triggerRange.Contains(precMZ))
                        {
                            int targetMS2ScanNumber = FindTargetMS2(rawFile, triggerScanNumber, kvp.Value);

                            if(targetMS2ScanNumber > 0)
                            {
                                List<int> outList = null;
                                if (TriggerMS2toTargetMS2.TryGetValue(triggerScanNumber, out outList))
                                {
                                    outList.Add(i);
                                }
                                else
                                {
                                    TriggerMS2toTargetMS2.Add(triggerScanNumber, new List<int>());
                                    TriggerMS2toTargetMS2[triggerScanNumber].Add(targetMS2ScanNumber);
                                }
                            }

                            //FIND THE MS1 JUST GO BACKWARDS
                            int ms1ScanNumber = FindMS1(rawFile, triggerScanNumber);

                            int outNum = 0;
                            if(!TriggerMS2toMS1.TryGetValue(triggerScanNumber, out outNum))
                            {
                                TriggerMS2toMS1.Add(triggerScanNumber, ms1ScanNumber);
                            }
                        }
                    }
                }
            }

            return ms1List;
        }

        private List<int> MapMS2DataScans(ThermoRawFile rawFile, out Dictionary<int, int> TriggerMS2toMS1, out Dictionary<int, List<int>> TriggerMS2toTargetMS2, out Dictionary<int, List<int>> TargetMS2toTargetMS3)
        {
            List<int> ms1List = new List<int>();
            TriggerMS2toMS1 = new Dictionary<int, int>();
            TriggerMS2toTargetMS2 = new Dictionary<int, List<int>>();
            TargetMS2toTargetMS3 = new Dictionary<int, List<int>>();

            int lastSpectrumNumber = rawFile.LastSpectrumNumber;

            //Cycle through the raw file
            for (int i = 1; i <= lastSpectrumNumber; i++)
            {
                //I am just querying the msn order of the scan, I think this is faster than getting the whole scan each time
                int msnOrder = rawFile.GetMsnOrder(i);

                //If it is an MS1 then just add the number to a list
                if (msnOrder == 1)
                {
                    ms1List.Add(i);
                }
                //If it is an MS2 then it could be a trigger or target
                else if (msnOrder == 2)
                {
                    int parentScanNumber = rawFile.GetParentSpectrumNumber(i);
                    int parentOrder = rawFile.GetMsnOrder(parentScanNumber);

                    //If the parent order is 2 then this is a target ms2 and the parent is the trigger
                    if (parentOrder == 2)
                    {

                        List<int> outList = null;
                        if (TriggerMS2toTargetMS2.TryGetValue(parentScanNumber, out outList))
                        {
                            outList.Add(i);
                        }
                        else
                        {
                            TriggerMS2toTargetMS2.Add(parentScanNumber, new List<int>());
                            TriggerMS2toTargetMS2[parentScanNumber].Add(i);
                        }

                        outList = null;
                        if (TargetMS2toTargetMS3.TryGetValue(i, out outList))
                        {
                            outList.Add(i);
                        }
                        else
                        {
                            TargetMS2toTargetMS3.Add(i, new List<int>());
                            TargetMS2toTargetMS3[i].Add(i);
                        }
                    }
                    //If the parent order is 1 then this is a trigger ms2 and the parent is an MS1
                    else if (parentOrder == 1)
                    {
                        TriggerMS2toMS1.Add(i, parentScanNumber);
                    }
                }
            }

            return ms1List;
        }

        private int FindParentMS3(ThermoRawFile rawFile, int ms3ScanNumber, Dictionary<int, int> mappedTargetMS2s, out Dictionary<int, int> ret_mappedTargetMS2s)
        {
            int targetMS2ScanNumber = 0;
            string precursorMZ = rawFile.GetScanFilterMZ(ms3ScanNumber);
            ret_mappedTargetMS2s = new Dictionary<int, int>();

            for (int i = ms3ScanNumber; i >= 1; i--)
            {
                //I am just querying the msn order of the scan, I think this is faster than getting the whole scan each time
                int msnOrder = rawFile.GetMsnOrder(i);

                if(msnOrder == 2 && rawFile.GetScanFilterMZ(i).Equals(precursorMZ))
                {
                    int outSN = 0;
                    if(!mappedTargetMS2s.TryGetValue(i, out outSN))
                    {
                        mappedTargetMS2s.Add(i, i);
                        ret_mappedTargetMS2s = mappedTargetMS2s;
                        return i;
                    }
                }

            }

            return targetMS2ScanNumber;
        }

        private int FindTargetMS2(ThermoRawFile rawFile, int triggerMS2ScanNumber, TargetPeptide targetPeptide)
        {
            int targetMS2ScanNumber = 0;

            int endScanNumber = triggerMS2ScanNumber + 10;
            if(endScanNumber > rawFile.LastSpectrumNumber)
            {
                endScanNumber = rawFile.LastSpectrumNumber;
            }

            for (int i = triggerMS2ScanNumber; i <= endScanNumber; i++)
            {
                //I am just querying the msn order of the scan, I think this is faster than getting the whole scan each time
                int msnOrder = rawFile.GetMsnOrder(i);

                if (msnOrder == 2)
                {
                    double testMZ = double.Parse(rawFile.GetScanFilterMZ(i));

                    MzRange targetMZRange = new MzRange(targetPeptide.TargetMZ, new Tolerance(ToleranceUnit.PPM, 15));
                    if(targetMZRange.Contains(testMZ))
                    {
                        return i;
                    }
                }

            }

            return targetMS2ScanNumber;
        }

        private int FindMS1(ThermoRawFile rawFile, int triggerMS2ScanNumber)
        {
            int targetMS2ScanNumber = 0;

            for (int i = triggerMS2ScanNumber; i >= 1; i--)
            {
                //I am just querying the msn order of the scan, I think this is faster than getting the whole scan each time
                int msnOrder = rawFile.GetMsnOrder(i);

                if (msnOrder == 1)
                {
                    return i;
                }

            }

            return targetMS2ScanNumber;
        }

        private void ExtractMS1XIC(ThermoRawFile rawFile, List<int> ms1Scans, SortedList<double, TargetPeptide> targetList)
        {
            foreach (int scanNumber in ms1Scans)
            {
                ThermoSpectrum spectrum = rawFile.GetSpectrum(scanNumber);
                double rt = rawFile.GetRetentionTime(scanNumber);

                foreach (TargetPeptide target in targetList.Values)
                {
                    target.AddMS1XICPoint(spectrum, rt, scanNumber);
                }

                spectrum = null;
            }
        }

        private void ExtractData(ThermoRawFile rawFile, Dictionary<int, int> TriggerMS2toMS1, Dictionary<int, List<int>> TriggerMS2toTargetMS2, Dictionary<int, List<int>> TargetMS2toTargetMS3, SortedList<double, TargetPeptide> targetList, Dictionary<string, double> quantChannelDict)
        {
            //Cycle through the trigger MS2 scan numbers
            foreach (int scanNumber in TriggerMS2toMS1.Keys)
            {
                if (printDebug.Checked) { UpdateLog("Analyzing Scan Number " + scanNumber); }

                int ms1ScanNumber = TriggerMS2toMS1[scanNumber];

                double rt = rawFile.GetRetentionTime(scanNumber);
                double it = rawFile.GetInjectionTime(scanNumber);

                //Try to get the precursor mono mz
                //double precursorMZ = rawFile.GetPrecMonoMZ(scanNumber);

                //If this returns as 0, then just get the precursor mz
                //if (precursorMZ == 0)
                //{
                double precursorMZ = rawFile.GetPrecursorMz(scanNumber);
                //}

                //Define the range around the peak
                MassRange precursorRange = new MassRange(precursorMZ, new Tolerance(ToleranceUnit.PPM, 15));
                ThermoSpectrum spectrum = null;

                //Look through each target to find a match --This should be a binary search TODO: Binary Search
                foreach (TargetPeptide targetPeptide in targetList.Values)
                {
                    //If we pass this step then we triggered. We will extract all the data here
                    if (precursorRange.Contains(targetPeptide.TriggerMZ))
                    {
                        //If this is the first hit them get the spectrum
                        if (spectrum == null) { spectrum = rawFile.GetSpectrum(scanNumber); }

                        //Grab the trigger data for the Trigger MS2
                        targetPeptide.AddTriggerData(ms1ScanNumber, scanNumber, spectrum, rt, it);

                        //If we found a trigger peptide then see if we then targeted it
                        List<int> targetScanNumber = null;
                        if (TriggerMS2toTargetMS2.TryGetValue(scanNumber, out targetScanNumber))
                        {
                            if (printDebug.Checked) { UpdateLog("Extracting MS2/3 Data"); }

                            //If we did an ms2 on the target peptides then get that data as well
                            ExtractTargetData(targetPeptide, rawFile, ms1ScanNumber, targetScanNumber, TargetMS2toTargetMS3, quantChannelDict);

                            if (printDebug.Checked) { UpdateLog("Extracted MS2/3 Data"); }

                        }
                    }
                }
            }
        }

        private void ExtractTargetData(TargetPeptide targetPeptide, ThermoRawFile rawFile, int ms1ScanNumber, List<int> targetScanNumbers, Dictionary<int, List<int>> TargetMS2toTargetMS3, Dictionary<string, double> quantChannelDict)
        {

            foreach(int targetScanNumber in targetScanNumbers)
            {
                //Grab all of the data for the target and add that data
                double rt = rawFile.GetRetentionTime(targetScanNumber);
                double it = rawFile.GetInjectionTime(targetScanNumber);
                ThermoSpectrum spectrum = rawFile.GetSpectrum(targetScanNumber);

                //If we found a target peptide then see if we did an MS3
                List<int> ms3ScanNumbers = null;
                if (TargetMS2toTargetMS3.TryGetValue(targetScanNumber, out ms3ScanNumbers))
                {
                    foreach (int ms3ScanNumber in ms3ScanNumbers)
                    {

                        double ms3rt = rawFile.GetRetentionTime(ms3ScanNumber);
                        double ms3it = rawFile.GetInjectionTime(ms3ScanNumber);
                        ThermoSpectrum ms3spectrum = rawFile.GetSpectrum(ms3ScanNumber);
                        List<double> spsIons = new List<double>();

                        if (rawFile.GetMsnOrder(ms3ScanNumber) == 3)
                        {
                            spsIons = rawFile.GetSPSMasses(ms3ScanNumber);

                            if (spsIons == null)
                            {
                                spsIons = new List<double>();
                            }
                        }

                        if (ms3rt == null) { ms3rt = 0; }
                        if (ms3it == null) { ms3it = 0; }
                        if (spsIons == null) { spsIons = new List<double>(); }

                        if (printDebug.Checked)
                        {
                            if (ms3spectrum == null) { UpdateLog("No MS3 Spectrum"); }
                            if (spectrum == null) { UpdateLog("No MS2 Spectrum"); }
                            if (quantChannelDict == null) { UpdateLog("No Quant Dict"); }

                            UpdateLog("MS1 Scan Number" + ms1ScanNumber);
                            UpdateLog("Target Scan Number" + targetScanNumber);
                            UpdateLog("MS2 RT" + rt);
                            UpdateLog("MS2 IT" + it);
                            UpdateLog("MS3 Scan Number" + ms3ScanNumber);
                            UpdateLog("MS3 RT" + ms3rt);
                            UpdateLog("MS3 IT" + ms3it);
                            UpdateLog("Adding Target Data 1");
                        }

                        //Add the target data and if there was an MS3 add that data too
                        targetPeptide.AddTargetData(ms1ScanNumber, targetScanNumber, spectrum, rt, it, ms3ScanNumber, ms3spectrum, ms3rt, ms3it, spsIons, quantChannelDict);

                        if (printDebug.Checked) { UpdateLog("Added Target Data 1"); }
                    }
                }
                else
                {
                    targetPeptide.AddTargetData(ms1ScanNumber, targetScanNumber, spectrum, rt, it);
                }
            }
        }

        private List<TargetPeptide> ChooseBestChargeState(List<TargetPeptide> targets)
        {
            //This will be a list of unique peptides that will be returned with the best charge state
            List<TargetPeptide> retList = new List<TargetPeptide>();

            //This is a temporary dictionary that will help to organize the peptides based on the peptide string 
            //This string should be the same for all peptides. 
            Dictionary<string, List<TargetPeptide>> tempDict = new Dictionary<string, List<TargetPeptide>>();

            //Cycle through the peptides and index them based on the string
            foreach(TargetPeptide target in targets)
            {
                //First check to see if the the is already in the dictionary
                List<TargetPeptide> outList = null;
                if(tempDict.TryGetValue(target.PeptideString, out outList))
                {
                    //If it is already on the list add the target peptide to the list
                    tempDict[target.PeptideString].Add(target);
                }
                else
                {
                    //If it is not in the dictionary then make a new list, add the target, then add that list to the dict
                    List<TargetPeptide> addList = new List<TargetPeptide>();
                    addList.Add(target);
                    tempDict.Add(target.PeptideString, addList);
                }
            }

            //With the peptides indexed based on string we can now iterate through all of the entries and choose the best
            //peptide for each peptide
            foreach (List<TargetPeptide> targetList in tempDict.Values)
            {
                //This is the best target 
                TargetPeptide bestTarget = null;

                //We will determin the best target based on the intensity of the trigger peptide
                foreach (TargetPeptide target in targetList)
                {

                    if (bestTarget == null || target.MaxIntensity > bestTarget.MaxIntensity)
                    {
                        bestTarget = target;
                    }

                    //if(Math.Abs((target.MaxIntensity - bestTarget.MaxIntensity)/bestTarget.MaxIntensity) < 0.5)
                    //{
                    //    if (target.TargetSPSIons.Count > bestTarget.TargetSPSIons.Count)
                    //    {
                    //        bestTarget = target;
                    //    }
                    //}
                }

                //Add the best peptide to the list that will be returned. 
                retList.Add(bestTarget);
            }

            //Return the list. 
            return retList;
        }

        private string BuildMethodXML(string inputFile, List<TargetPeptide> targetList, bool ms1Target, bool ms2Trigger, bool ms2Offset, bool ms3target)
        {

            List<TargetPeptide> _targetList = new List<TargetPeptide>();

            foreach(TargetPeptide targetPep in targetList)
            {
                if(targetPep.TargetSPSIonsWithCharge.Count >0 && targetPep.TriggerMZ < 2000)
                {
                    _targetList.Add(targetPep);
                }
            }

            //Only change the parameters the user wants to
            bool addMS1TargetedMass = ms1Target;
            bool addMS2TriggerMass = ms2Trigger;
            bool addMS2IsoOffset = ms2Offset;
            bool addMS3TargetedMass = ms3target;

            //Save the xml for future use
            string file = inputFile;

            //Set up the serializer
            XmlSerializer serializer = new XmlSerializer(typeof(MethodModifications));

            //Set up the class that will hold all of the instructions
            string instrument = "OrbitrapFusion";
            if(eclipseCheckbox.Checked)
            {
                instrument = "OrbitrapEclipse";
            }
            MethodModifications methodMods = new MethodModifications("1", instrument, "Calcium", "SL");

            //First we will make sure we have enough trees for all of the peptides
            int modCount = 1;
            int experimentIndex = 0;
            for(int i = 1;i< _targetList.Count;i++)
            {
                int sourceNodeForCopy = modCount - 1;
                Experiment addExp = new Experiment(experimentIndex);
                addExp.CopyAndPasteScanNode(sourceNodeForCopy);

                MethodModification addMod = new MethodModification(modCount, addExp);
                methodMods.Modifications.Add(addMod);

                modCount++;
            }

            //Next, we will change all of the trees to include all of the attributes of the peptides
            int treeIndex = 0;
            foreach (TargetPeptide target in _targetList)
            {
                if(addMS2IsoOffset)
                {
                    //Add in the MS2 isolation offset
                    Experiment addExp = new Experiment(experimentIndex);
                    addExp.ChangeScanParams(treeIndex, target.MassShift);

                    MethodModification addMod = new MethodModification(modCount, addExp);
                    methodMods.Modifications.Add(addMod);

                    modCount++;
                }
                
                if(addMS1TargetedMass)
                {
                    //Add in the MS1 inclusion list, and MS2 isolation offset
                    Experiment addExp0 = new Experiment(experimentIndex);

                    //If the retention times have not been populated then don't change them
                    if (target.StartRetentionTime == -1)
                    {
                        addExp0.AddMS1InclusionSingle(treeIndex, target.TriggerMZ, target.Charge);
                    }
                    else
                    {
                        addExp0.AddMS1InclusionSingleWithRTWindow(treeIndex, target.TriggerMZ, target.Charge, 
                            target.StartRetentionTime, target.EndRetentionTime);
                    }

                    MethodModification addMod0 = new MethodModification(modCount, addExp0);
                    methodMods.Modifications.Add(addMod0);

                    modCount++;
                }

                if (addMS2TriggerMass)
                {
                    //Add the MS2 trigger mass List
                    Experiment addExp1 = new Experiment(experimentIndex);
                    addExp1.AddMS2TriggerList(treeIndex, target.TriggerIonsWithCharge);

                    MethodModification addMod1 = new MethodModification(modCount, addExp1);
                    methodMods.Modifications.Add(addMod1);

                    modCount++;
                }
                
                if(addMS3TargetedMass)
                {
                    //Add in the MS3 inclusion list
                    Experiment addExp2 = new Experiment(experimentIndex);
                    addExp2.AddMS3InclusionList(treeIndex, target.TargetSPSIonsWithCharge);

                    MethodModification addMod2 = new MethodModification(modCount, addExp2);
                    methodMods.Modifications.Add(addMod2);

                    modCount++;
                }
                
                treeIndex++;
            }

            //Write the XML
            using (TextWriter writer = new StreamWriter(file))
            {
                serializer.Serialize(writer, methodMods);
            }

            return file;
        }

        private string BuildLargeMethodXML(string inputFile, List<TargetPeptide> targetList, bool ms1Target, bool ms2Trigger, bool ms2Offset, bool ms3target)
        {
            SortedDictionary<double, TargetPeptide> _methodTargetList = new SortedDictionary<double, TargetPeptide>();
            List<TargetPeptide> _targetList = new List<TargetPeptide>();

            foreach (TargetPeptide targetPep in targetList)
            {
                if (targetPep.TargetSPSIonsWithCharge.Count > 0 && targetPep.TriggerMZ < 2000)
                {
                    double targetMZ = Math.Round(targetPep.TargetMZ,4);
                    double triggerMZ = Math.Round(targetPep.TriggerMZ, 4);

                    TargetPeptide outPep = null;
                    while(_methodTargetList.TryGetValue(triggerMZ, out outPep))
                    {
                        targetMZ += 0.0001;
                        triggerMZ += 0.0001;
                    }

                    targetPep.TargetMZ = targetMZ;
                    targetPep.TriggerMZ = triggerMZ;

                    _methodTargetList.Add(triggerMZ, targetPep);
                    _targetList.Add(targetPep);
                }
            }

            SortedDictionary<double, List<TargetPeptide>> massShift_to_TargetList = new SortedDictionary<double, List<TargetPeptide>>();
            foreach (TargetPeptide targetPep in _targetList)
            {
                double massShift = Math.Round(targetPep.TriggerMZ - targetPep.TargetMZ, 2);

                List<TargetPeptide> outList = null;
                if(massShift_to_TargetList.TryGetValue(massShift, out outList))
                {
                    massShift_to_TargetList[massShift].Add(targetPep);
                }
                else
                {
                    massShift_to_TargetList.Add(massShift, new List<TargetPeptide>());
                    massShift_to_TargetList[massShift].Add(targetPep);
                }
            }

            List<double> uniqueMassShifts = massShift_to_TargetList.Keys.ToList();

            //Only change the parameters the user wants to
            bool addMS1TargetedMass = ms1Target;
            bool addMS2TriggerMass = ms2Trigger;
            bool addMS2IsoOffset = ms2Offset;
            bool addMS3TargetedMass = ms3target;

            //Save the xml for future use
            string file = inputFile;

            //Set up the serializer
            XmlSerializer serializer = new XmlSerializer(typeof(MethodModifications));

            //Set up the class that will hold all of the instructions
            string instrument = "OrbitrapFusion";
            if (eclipseCheckbox.Checked)
            {
                instrument = "OrbitrapEclipse";
            }
            MethodModifications methodMods = new MethodModifications("1", instrument, "Calcium", "SL");

            //This will make a tree for all of the unique mass shifts
            int modCount = 1;
            int experimentIndex = 0;
            for (int i = 1; i < uniqueMassShifts.Count; i++)
            {
                int sourceNodeForCopy = 0;
                Experiment addExp = new Experiment(experimentIndex);
                addExp.CopyAndPasteScanNode(sourceNodeForCopy);

                MethodModification addMod = new MethodModification(modCount, addExp);
                methodMods.Modifications.Add(addMod);

                modCount++;
            }

            //Next, we will change all of the trees to include all of the attributes of the peptides
            int treeIndex = 0;
            foreach(KeyValuePair<double, List<TargetPeptide>> kvp in massShift_to_TargetList)
            {
                double massShift = kvp.Key;
                List<TargetPeptide> targetPeptides = kvp.Value;


                if (addMS1TargetedMass)
                {
                    //Add in the MS1 inclusion list, and MS2 isolation offset
                    Experiment addExp0 = new Experiment(experimentIndex);

                    addExp0.AddMS1Inclusion(treeIndex, targetPeptides);

                    MethodModification addMod0 = new MethodModification(modCount, addExp0);
                    methodMods.Modifications.Add(addMod0);

                    modCount++;
                }


                if (addMS2TriggerMass)
                {
                    //Add the MS2 trigger mass List
                    Experiment addExp1 = new Experiment(experimentIndex);

                    addExp1.AddMassShiftGroupedMS2TriggerList(treeIndex, targetPeptides);

                    MethodModification addMod1 = new MethodModification(modCount, addExp1);
                    methodMods.Modifications.Add(addMod1);

                    modCount++;
                }


                if (addMS2IsoOffset)
                {
                    //Add in the MS2 isolation offset
                    Experiment addExp = new Experiment(experimentIndex);

                    addExp.ChangeScanParams(treeIndex, massShift);

                    MethodModification addMod = new MethodModification(modCount, addExp);
                    methodMods.Modifications.Add(addMod);

                    modCount++;
                }


                if (addMS3TargetedMass)
                {
                    //Add in the MS3 inclusion list
                    Experiment addExp2 = new Experiment(experimentIndex);

                    addExp2.AddMassShiftGroupedMS3InclusionList(treeIndex, targetPeptides, massShift);

                    MethodModification addMod2 = new MethodModification(modCount, addExp2);
                    methodMods.Modifications.Add(addMod2);

                    modCount++;
                }

                treeIndex++;
            }


            //Write the XML
            using (TextWriter writer = new StreamWriter(file))
            {
                serializer.Serialize(writer, methodMods);
            }

            return file;
        }

        private void UpdateLog(string message)
        {
            logBox.AppendText(string.Format("[{0}]\t{1}\n", DateTime.Now.ToLongTimeString(), message));
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }

        #region Init and Update Plotting and Grids

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
            DataGridViewCheckBoxColumn selectedColumn = new DataGridViewCheckBoxColumn();
            selectedColumn.DataPropertyName = "Selected";
            selectedColumn.HeaderText = "Selected";
            selectedColumn.ReadOnly = false;
            selectedColumn.Width = 60;
            targetGridView.Columns.Add(selectedColumn);

            DataGridViewTextBoxColumn proteinColumn = new DataGridViewTextBoxColumn();
            proteinColumn.DataPropertyName = "ProteinString";
            proteinColumn.HeaderText = "Protein";
            proteinColumn.ReadOnly = true;
            proteinColumn.Width = 80;
            targetGridView.Columns.Add(proteinColumn);

            DataGridViewTextBoxColumn peptideColumn = new DataGridViewTextBoxColumn();
            peptideColumn.DataPropertyName = "PeptideString";
            peptideColumn.HeaderText = "Peptide";
            peptideColumn.ReadOnly = true;
            peptideColumn.Width = 100;
            targetGridView.Columns.Add(peptideColumn);


            DataGridViewTextBoxColumn triggerMZColumn = new DataGridViewTextBoxColumn();
            triggerMZColumn.DataPropertyName = "TriggerMZ";
            triggerMZColumn.HeaderText = "Trigger";
            triggerMZColumn.ReadOnly = true;
            triggerMZColumn.Width = 60;
            targetGridView.Columns.Add(triggerMZColumn);

            DataGridViewTextBoxColumn targetColumn = new DataGridViewTextBoxColumn();
            targetColumn.DataPropertyName = "TargetMZ";
            targetColumn.HeaderText = "Target";
            targetColumn.ReadOnly = true;
            targetColumn.Width = 60;
            targetColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            targetGridView.Columns.Add(targetColumn);

            DataGridViewTextBoxColumn chargeColumn = new DataGridViewTextBoxColumn();
            chargeColumn.DataPropertyName = "Charge";
            chargeColumn.HeaderText = "Charge";
            chargeColumn.ReadOnly = true;
            chargeColumn.Width = 45;
            targetGridView.Columns.Add(chargeColumn);

        }

        private void InitializeModGrid()
        {
            DataGridViewCheckBoxColumn triggerColumn = new DataGridViewCheckBoxColumn();
            triggerColumn.DataPropertyName = "Trigger";
            triggerColumn.HeaderText = "Trigger";
            triggerColumn.ReadOnly = false;
            triggerColumn.Width = 60;
            modGridView.Columns.Add(triggerColumn);

            DataGridViewCheckBoxColumn targetColumn = new DataGridViewCheckBoxColumn();
            targetColumn.DataPropertyName = "Target";
            targetColumn.HeaderText = "Target";
            targetColumn.ReadOnly = false;
            targetColumn.Width = 60;
            modGridView.Columns.Add(targetColumn);

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.ReadOnly = true;
            nameColumn.Width = 60;
            modGridView.Columns.Add(nameColumn);

            DataGridViewTextBoxColumn massColumn = new DataGridViewTextBoxColumn();
            massColumn.DataPropertyName = "Mass";
            massColumn.HeaderText = "Mono Mass";
            massColumn.ReadOnly = true;
            massColumn.Width = 70;
            modGridView.Columns.Add(massColumn);

            DataGridViewTextBoxColumn modTypeColumn = new DataGridViewTextBoxColumn();
            modTypeColumn.DataPropertyName = "Type";
            modTypeColumn.HeaderText = "Type";
            modTypeColumn.ReadOnly = true;
            modTypeColumn.Width = 60;
            modGridView.Columns.Add(modTypeColumn);

            DataGridViewTextBoxColumn modCharColumn = new DataGridViewTextBoxColumn();
            modCharColumn.DataPropertyName = "ModChar";
            modCharColumn.HeaderText = "Symbol";
            modCharColumn.ReadOnly = true;
            modCharColumn.Width = 30;
            modGridView.Columns.Add(modCharColumn);

            DataGridViewTextBoxColumn modSitesColumn = new DataGridViewTextBoxColumn();
            modSitesColumn.DataPropertyName = "ModSites";
            modSitesColumn.HeaderText = "Sites";
            modSitesColumn.ReadOnly = true;
            modSitesColumn.Width = 60;
            modGridView.Columns.Add(modSitesColumn);

            //Populate modded proteins - build a list of possible modifications
            Modification tmt0 = new Modification(224.152478, "TMT0", ModificationSites.K | ModificationSites.NPep);
            Modification tmt2 = new Modification(225.15833, "TMT2", ModificationSites.K | ModificationSites.NPep);
            Modification tmt11 = new Modification(229.162932, "TMT11", ModificationSites.K | ModificationSites.NPep);
            Modification tmtSH = new Modification(235.17677, "TMTsh", ModificationSites.K | ModificationSites.NPep);

            Modification cam = new Modification(57.02146, "CAM", ModificationSites.C);
            Modification nem = new Modification(125.04767, "NEM", ModificationSites.C);

            Modification ox = new Modification(15.99491, "OX", ModificationSites.M);
            Modification phos = new Modification(79.96633, "Phos", ModificationSites.S | ModificationSites.T | ModificationSites.Y);
            Modification acetyl = new Modification(42.01056, "Acetyl", ModificationSites.A | ModificationSites.M);

            Modification gg = new Modification(114.0429, "gg", ModificationSites.K);
            Modification ggTMT0 = new Modification(338.195378, "ggTMT0", ModificationSites.K);
            Modification ggTMT2 = new Modification(339.20123, "ggTMT2", ModificationSites.K);
            Modification ggTMT11 = new Modification(343.20583, "ggTMT11", ModificationSites.K);
            Modification ggTMTsh = new Modification(349.21967, "ggTMTsh", ModificationSites.K);

            Modification C2N1 = new Modification(3.004, "C2N1", ModificationSites.G);
            Modification C3N1 = new Modification(4.007, "C3N1", ModificationSites.A);
            Modification C5N1 = new Modification(6.0138, "C5N1", ModificationSites.V | ModificationSites.P);
            Modification C6N1 = new Modification(7.0171, "C6N1", ModificationSites.I | ModificationSites.L);
            Modification C9N1 = new Modification(10.0272, "C9N1", ModificationSites.Y | ModificationSites.F);

            Modification r10 = new Modification(10.00827, "R10", ModificationSites.R);
            Modification k8TMT11 = new Modification(237.1771, "K8TMT11", ModificationSites.K);

            Modification tmt11OL = new Modification(229.162932, "TMT11OL", ModificationSites.S | ModificationSites.T | ModificationSites.Y | ModificationSites.H);

            ModLines.Add(new ModificationLine("TMT0", Math.Round(tmt0.MonoisotopicMass, 5), "K,NPep", "", "Static", true, false, tmt0));
            //ModLines.Add(new ModificationLine("TMT2", Math.Round(tmt2.MonoisotopicMass, 5), "K,NPep", "", "Static", false, false, tmt2));
            ModLines.Add(new ModificationLine("SH-TMT", Math.Round(tmtSH.MonoisotopicMass, 5), "K,NPep", "", "Static", false, false, tmtSH));
            ModLines.Add(new ModificationLine("TMT11", Math.Round(tmt11.MonoisotopicMass, 5), "K,NPep", "", "Static", false, true, tmt11));
            ModLines.Add(new ModificationLine("R10", Math.Round(r10.MonoisotopicMass, 5), "R", "$", "Dynamic", false, false, r10));
            ModLines.Add(new ModificationLine("K8TMT11", Math.Round(k8TMT11.MonoisotopicMass, 5), "K", "@", "Dynamic", false, false, k8TMT11));
            ModLines.Add(new ModificationLine("CAM", Math.Round(cam.MonoisotopicMass, 5), "C", "", "Static", true, true, cam));
            ModLines.Add(new ModificationLine("NEM", Math.Round(nem.MonoisotopicMass, 5), "C", "", "Static", false, false, nem));
            ModLines.Add(new ModificationLine("Phos", Math.Round(phos.MonoisotopicMass, 5), "S,T,Y", "#", "Dynamic", false, false, phos));
            ModLines.Add(new ModificationLine("Acetyl", Math.Round(acetyl.MonoisotopicMass, 5), "NPep", "&", "Dynamic", false, false, acetyl));
            //ModLines.Add(new ModificationLine("gg", Math.Round(gg.MonoisotopicMass, 5), "K", "*", "Dynamic", false, false, gg));
            ModLines.Add(new ModificationLine("ggTMT0", Math.Round(ggTMT0.MonoisotopicMass, 5), "K", "^", "Dynamic", false, false, ggTMT0));
            //ModLines.Add(new ModificationLine("ggTMT2", Math.Round(ggTMT2.MonoisotopicMass, 5), "K", "^", "Dynamic", false, false, ggTMT2));
            ModLines.Add(new ModificationLine("ggTMT11", Math.Round(ggTMT11.MonoisotopicMass, 5), "K", "^", "Dynamic", false, false, ggTMT11));
            ModLines.Add(new ModificationLine("ggTMTsh", Math.Round(ggTMTsh.MonoisotopicMass, 5), "K", "^", "Dynamic", false, false, ggTMTsh));
            ModLines.Add(new ModificationLine("OX", Math.Round(ox.MonoisotopicMass, 5), "M", "*", "Dynamic", true, true, ox));
            //ModLines.Add(new ModificationLine("C2N1", Math.Round(C2N1.MonoisotopicMass, 5), "G", "^", "Dynamic", false, false, C2N1));
            //ModLines.Add(new ModificationLine("C3N1", Math.Round(C3N1.MonoisotopicMass, 5), "A", "#", "Dynamic", false, false, C3N1));
            //ModLines.Add(new ModificationLine("C5N1", Math.Round(C5N1.MonoisotopicMass, 5), "V,P", "&", "Dynamic", false, false, C5N1));
            //ModLines.Add(new ModificationLine("C6N1", Math.Round(C6N1.MonoisotopicMass, 5), "I,L", "@", "Dynamic", false, false, C6N1));
            //ModLines.Add(new ModificationLine("C9N1", Math.Round(C9N1.MonoisotopicMass, 5), "F,Y", "^", "Dynamic", false, false, C9N1));
            //ModLines.Add(new ModificationLine("TMT11OL", Math.Round(tmt11OL.MonoisotopicMass, 5), "S,T,Y,H", "$", "Dynamic", false, false, tmt11OL));
        }

        private void InitializeScanGrid()
        {
            DataGridViewCheckBoxColumn includeCol = new DataGridViewCheckBoxColumn();
            includeCol.DataPropertyName = "Include";
            includeCol.HeaderText = "Include";
            includeCol.ReadOnly = false;
            includeCol.Width = 50;
            scanGridView.Columns.Add(includeCol);

            DataGridViewTextBoxColumn ms2RTCol = new DataGridViewTextBoxColumn();
            ms2RTCol.DataPropertyName = "MS2RetentionTime";
            ms2RTCol.HeaderText = "MS2 RT";
            ms2RTCol.ReadOnly = true;
            ms2RTCol.Width = 50;
            scanGridView.Columns.Add(ms2RTCol);

            DataGridViewTextBoxColumn ms2TrigInt = new DataGridViewTextBoxColumn();
            ms2TrigInt.DataPropertyName = "MS1TriggerIntensity";
            ms2TrigInt.HeaderText = "Trigger Int (Log10)";
            ms2TrigInt.ReadOnly = true;
            ms2TrigInt.Width = 50;
            scanGridView.Columns.Add(ms2TrigInt);

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
            ms2ITCol.Width = 40;
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
            spsCol.Width = 30;
            scanGridView.Columns.Add(spsCol);

            DataGridViewTextBoxColumn quantCol1 = new DataGridViewTextBoxColumn();
            quantCol1.DataPropertyName = "MS3Quant1";
            quantCol1.HeaderText = "Quant #1";
            quantCol1.ReadOnly = true;
            quantCol1.Width = 45;
            quantCol1.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol1);

            DataGridViewTextBoxColumn quantCol2 = new DataGridViewTextBoxColumn();
            quantCol2.DataPropertyName = "MS3Quant2";
            quantCol2.HeaderText = "Quant #2";
            quantCol2.ReadOnly = true;
            quantCol2.Width = 45;
            scanGridView.Columns.Add(quantCol2);

            DataGridViewTextBoxColumn quantCol3 = new DataGridViewTextBoxColumn();
            quantCol3.DataPropertyName = "MS3Quant3";
            quantCol3.HeaderText = "Quant #3";
            quantCol3.ReadOnly = true;
            quantCol3.Width = 45;
            quantCol3.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol3);

            DataGridViewTextBoxColumn quantCol4 = new DataGridViewTextBoxColumn();
            quantCol4.DataPropertyName = "MS3Quant4";
            quantCol4.HeaderText = "Quant #4";
            quantCol4.ReadOnly = true;
            quantCol4.Width = 45;
            scanGridView.Columns.Add(quantCol4);

            DataGridViewTextBoxColumn quantCol5 = new DataGridViewTextBoxColumn();
            quantCol5.DataPropertyName = "MS3Quant5";
            quantCol5.HeaderText = "Quant #5";
            quantCol5.ReadOnly = true;
            quantCol5.Width = 45;
            quantCol5.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol5);

            DataGridViewTextBoxColumn quantCol6 = new DataGridViewTextBoxColumn();
            quantCol6.DataPropertyName = "MS3Quant6";
            quantCol6.HeaderText = "Quant #6";
            quantCol6.ReadOnly = true;
            quantCol6.Width = 45;
            scanGridView.Columns.Add(quantCol6);

            DataGridViewTextBoxColumn quantCol7 = new DataGridViewTextBoxColumn();
            quantCol7.DataPropertyName = "MS3Quant7";
            quantCol7.HeaderText = "Quant #7";
            quantCol7.ReadOnly = true;
            quantCol7.Width = 45;
            quantCol7.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol7);

            DataGridViewTextBoxColumn quantCol8 = new DataGridViewTextBoxColumn();
            quantCol8.DataPropertyName = "MS3Quant8";
            quantCol8.HeaderText = "Quant #8";
            quantCol8.ReadOnly = true;
            quantCol8.Width = 45;
            scanGridView.Columns.Add(quantCol8);

            DataGridViewTextBoxColumn quantCol9 = new DataGridViewTextBoxColumn();
            quantCol9.DataPropertyName = "MS3Quant9";
            quantCol9.HeaderText = "Quant #9";
            quantCol9.ReadOnly = true;
            quantCol9.Width = 45;
            quantCol9.SortMode = DataGridViewColumnSortMode.Automatic;
            scanGridView.Columns.Add(quantCol9);

            DataGridViewTextBoxColumn quantCol10 = new DataGridViewTextBoxColumn();
            quantCol10.DataPropertyName = "MS3Quant10";
            quantCol10.HeaderText = "Quant #10";
            quantCol10.ReadOnly = true;
            quantCol10.Width = 45;
            scanGridView.Columns.Add(quantCol10);

            DataGridViewTextBoxColumn quantCol11 = new DataGridViewTextBoxColumn();
            quantCol11.DataPropertyName = "MS3Quant11";
            quantCol11.HeaderText = "Quant #11";
            quantCol11.ReadOnly = true;
            quantCol11.Width = 45;
            scanGridView.Columns.Add(quantCol11);

            DataGridViewTextBoxColumn sumSNCol = new DataGridViewTextBoxColumn();
            sumSNCol.DataPropertyName = "MS3SumSN";
            sumSNCol.HeaderText = "Sum SN";
            sumSNCol.ReadOnly = true;
            sumSNCol.Width = 45;
            scanGridView.Columns.Add(sumSNCol);

            DataGridViewTextBoxColumn isoSpecCol = new DataGridViewTextBoxColumn();
            isoSpecCol.DataPropertyName = "MS3IsoSpec";
            isoSpecCol.HeaderText = "Iso Spec";
            isoSpecCol.ReadOnly = true;
            isoSpecCol.Width = 45;
            scanGridView.Columns.Add(isoSpecCol);

        }

        private void UpdateTargetPlots()
        {
            //Check to make sure there are targets being displayed in the GUI
            if (TargetsDisplayed.Count != 0)
            {
                //We need to get the index of the target that is selected in the GUI
                int index = 0;
                if (targetGridView.CurrentCell != null)
                {
                    index = targetGridView.CurrentCell.RowIndex;
                }

                //With the index we can get the target from the displayed list of targets
                TargetPeptide target = TargetsDisplayed[index].Peptide;

                //We will plot different things depending on wether this is analysis or a priming run
                //This is determined when either of the buttons is clicked - it should be the first thing in each method
                if (Analysis)
                {
                    UpdatePlotsAnalysis(target);
                }
                else if (Priming)
                {
                    UpdatePlotsPriming(target);
                }
            }
        }

        private void UpdatePlotsPriming(TargetPeptide target)
        {
            //Populate the scan Events Table
            UpdateScanGridMembers(target);

            //This will plot the MS1 XIC for the trigger peptide
            UpdateMS1XICsPlot(target);

            //We want the other two plots clear. These will be filled in when the program finishes because
            //the selection change will be called for the ScanDataGridView object
            if (target.TriggerMS2s.Count == 0)
            {
                SpectrumPane1.CurveList.Clear();
                spectrumGraphControl1.AxisChange();
                spectrumGraphControl1.Refresh();

                SpectrumPane2.CurveList.Clear();
                spectrumGraphControl2.AxisChange();
                spectrumGraphControl2.Refresh();

            }

        }

        private void UpdatePlotsAnalysis(TargetPeptide target)
        {
            //Populate the scan Events Table
            UpdateScanGridMembers(target);

            //This will update the XIC plots for the trigger and the target peptides
            UpdateMS1XICsPlot(target);

            //If there were not any target MS2s then clear the plots to make sure the plots
            //from the last peptide do not stay up. 
            if(target.TargetMS2s.Count == 0)
            {
                SpectrumPane1.CurveList.Clear();
                spectrumGraphControl1.AxisChange();
                spectrumGraphControl1.Refresh();

                SpectrumPane2.CurveList.Clear();
                spectrumGraphControl2.AxisChange();
                spectrumGraphControl2.Refresh();

            }
        }

        private void UpdateMS1XICsPlot(TargetPeptide target)
        {
            MS1Pane.CurveList.Clear();
            MS1Pane.Title.Text = "Precursor Ion XIC for " + target.PeptideString;

            LineItem triggerXIC = MS1Pane.AddCurve("Trigger XIC", target.TriggerMS1XicPoints, Color.Red, SymbolType.None);
            triggerXIC.Line.Width = 2.0F;

            LineItem targetXIC = MS1Pane.AddCurve("Target XIC", target.TargetMS1XicPoints, Color.Blue, SymbolType.None);
            targetXIC.Line.Width = 2.0F;

            LineItem triggerPoints = MS1Pane.AddCurve("Trigger MS2s", target.TriggerMS2Points, Color.Red, SymbolType.Circle);
            triggerPoints.Line.IsVisible = false;
            triggerPoints.Symbol.Border.Width = 2.0F;
            triggerPoints.Symbol.Size = 10.0F;

            LineItem targetMS2Points = MS1Pane.AddCurve("Target MS2s", target.TargetMS2Points, Color.Blue, SymbolType.Circle);
            targetMS2Points.Line.IsVisible = false;
            targetMS2Points.Symbol.Border.Width = 2.0F;
            targetMS2Points.Symbol.Size = 10.0F;

            LineItem targetMS3Points = MS1Pane.AddCurve("Target MS3s", target.TargetMS3Points, Color.Teal, SymbolType.Circle);
            targetMS3Points.Line.IsVisible = false;
            targetMS3Points.Symbol.Border.Width = 2.0F;
            targetMS3Points.Symbol.Size = 10.0F;

            MS1Pane.Legend.FontSpec.Size = 20f;

            HighlightPlot(target.StartSelectionTime, target.EndSelectionTime);

            ms1GraphControl.AxisChange();
            ms1GraphControl.Refresh();
        }

        private void UpdateTargetMS2Plot(MS2Event ms2)
        {
            SpectrumPane1.CurveList.Clear();
            SpectrumPane1.Title.Text = "Target MS2 Spectrum";
            SpectrumPane1.AddStick("Target Spectrum", ms2.AllPeaks, Color.Black);

            LineItem spsStars = SpectrumPane1.AddCurve("SPS Ions", ms2.SPSPeaks, Color.Red, SymbolType.Star);
            spsStars.Symbol.Fill.Color = Color.Red;
            spsStars.Line.IsVisible = false;
            spsStars.Symbol.Border.Width = 2.0F;
            spsStars.Symbol.Size = 10.0F;

            LineItem matchedStars = SpectrumPane1.AddCurve("Matched Fragments", ms2.MatchedPeaks, Color.Blue, SymbolType.Star);
            matchedStars.Symbol.Fill.Color = Color.Blue;
            matchedStars.Line.IsVisible = false;
            matchedStars.Symbol.Border.Width = 2.0F;
            matchedStars.Symbol.Size = 10.0F;

            spectrumGraphControl1.AxisChange();
            spectrumGraphControl1.Refresh();
        }

        private void UpdateTargetMS3Plot(MS3Event ms3)
        {
            SpectrumPane2.CurveList.Clear();
            SpectrumPane2.Title.Text = "Target MS3 Reporter Ion S/N";
            SpectrumPane2.XAxis.Title.Text = "Reporter Ion";
            SpectrumPane2.YAxis.Title.Text = "Signal to Noise";
            SpectrumPane2.AddBar("QuantData", ms3.QuantPeaks, Color.Blue);
            SpectrumPane2.XAxis.Type = AxisType.Text;
            SpectrumPane2.XAxis.Scale.TextLabels = QuantChannelsInUse.Keys.ToArray();
            
            spectrumGraphControl2.AxisChange();
            spectrumGraphControl2.Refresh();
        }

        private void UpdateTriggerMS2Plots(MS2Event ms2)
        {
            int index = 0;
            if (targetGridView.CurrentCell != null)
            {
                index = targetGridView.CurrentCell.RowIndex;
            }

            TargetPeptide target = TargetsDisplayed[index].Peptide;

            SpectrumPane1.CurveList.Clear();
            SpectrumPane1.Title.Text = "Trigger MS2 Spectrum";
            SpectrumPane1.AddStick("Trigger Spectrum", ms2.AllPeaks, Color.Black);

            LineItem spsStars = SpectrumPane1.AddCurve("SPS Ions", ms2.SPSPeaks, Color.Red, SymbolType.Star);
            spsStars.Symbol.Fill.Color = Color.Red;
            spsStars.Line.IsVisible = false;
            spsStars.Symbol.Border.Width = 2.0F;
            spsStars.Symbol.Size = 10.0F;

            LineItem matchedStars = SpectrumPane1.AddCurve("Matched Fragments", ms2.MatchedPeaks, Color.Blue, SymbolType.Star);
            matchedStars.Symbol.Fill.Color = Color.Blue;
            matchedStars.Line.IsVisible = false;
            matchedStars.Symbol.Border.Width = 2.0F;
            matchedStars.Symbol.Size = 10.0F;

            spectrumGraphControl1.AxisChange();
            spectrumGraphControl1.Refresh();

            //Update the composite Spectrum
            SpectrumPane2.CurveList.Clear();
            SpectrumPane2.Title.Text = "Averaged Target MS2 Spectrum (All Peaks)";
            SpectrumPane2.AddStick("Averaged Spectrum", target.TargetCompositeMS2Points, Color.Black);

            spectrumGraphControl2.AxisChange();
            spectrumGraphControl2.Refresh();
        }

        private void UpdateScanGridMembers(TargetPeptide target)
        {
            //Populate the scan Events Table
            ScanEvents.Clear();
            foreach (ScanEventLine scanEvent in target.ScanEventLines)
            {
                //For right now I am only including scans where an MS3 was performed
                //This could be something that we could change in the future or make it
                //so that people can choose to see scans w/o an MS3
                if ((Priming || (!Priming && scanEvent.ScanEvent.MS3 != null)) && (!displaySelected.Checked || (displaySelected.Checked && scanEvent.Include)))
                {
                    ScanEvents.Add(scanEvent);
                }
            }
        }

        #endregion

        #region User Interactions
        private void addUserModifications_Click(object sender, EventArgs e)
        {
            //Save the indexes for the rows that will be deleted
            List<int> indexToDelete = new List<int>();

            //Cycle through the rows
            foreach (DataGridViewRow row in userModGridView.Rows)
            {
                //If it is a new row then we don't want to do anything
                if (!row.IsNewRow)
                {
                    //Check the values entered by the user
                    bool trigger = CheckBoolValue(row.Cells["triggerCB"].Value);
                    bool target = CheckBoolValue(row.Cells["targetCB"].Value);
                    string name = CheckStringValue(row.Cells["nameBox"].Value);
                    double mass = CheckDoubValue(row.Cells["massBox"].Value);
                    string type = CheckStringValue(row.Cells["typeCombo"].Value);
                    string symbol = CheckStringValue(row.Cells["symbolBox"].Value);
                    string sites = CheckStringValue(row.Cells["sitesBox"].Value);

                    //Make sure there is enough to make a new modification
                    if (mass != 0 && name != "" && type != "" & sites != "")
                    {
                        //This is a bad way to add in multiple sites
                        List<string> sitesList = sites.Split(',').ToList();
                        List<ModificationSites> modSiteList = new List<ModificationSites>();
                        foreach (string site in sitesList)
                        {
                            modSiteList.Add(SwitchSite(site));
                        }

                        //If sites returned None then something is wrong
                        if (modSiteList[0] == ModificationSites.None) { sites = "None"; }

                        //First add the modification with just one of the mods
                        Modification addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0]);

                        //Check to see if there are 2 mods
                        if (modSiteList.Count == 2)
                        {
                            addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0] | modSiteList[1]);
                        }
                        //Check to see if there are 3 mods
                        else if (modSiteList.Count == 3)
                        {
                            addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0] | modSiteList[1] | modSiteList[2]);
                        }
                        //Check to see if there are 4 mods
                        else if (modSiteList.Count == 4)
                        {
                            addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0] | modSiteList[1] | modSiteList[2] | modSiteList[3]);
                        }

                        //If it is static then make sure the symbol is cleared
                        if (type == "Static") { symbol = ""; }

                        //Add a modification line
                        ModLines.Add(new ModificationLine(addMod.Name, Math.Round(addMod.MonoisotopicMass, 5), sites, symbol, type, trigger, target, addMod));

                        //Make a note of which row to delete
                        indexToDelete.Add(row.Index);
                    }
                }
            }

            //Remove the rows in the user mod grid
            int totalRemoved = 0;
            foreach (int rowIndex in indexToDelete)
            {
                userModGridView.Rows.Remove(userModGridView.Rows[rowIndex - totalRemoved]);
                totalRemoved++;
            }
        }

        private void sortTargetsMZ_Click(object sender, EventArgs e)
        {
            //Create a list that will be used to sort the peptides based on the trigger mz
            SortedList<double, TargetPeptideLine> sortedPeptideLines = new SortedList<double, TargetPeptideLine>();
            foreach (TargetPeptideLine pepLine in TargetsDisplayed)
            {
                TargetPeptideLine outPepLine = null;
                double currentMZ = pepLine.TriggerMZ;
                while (sortedPeptideLines.TryGetValue(currentMZ, out outPepLine))
                {
                    currentMZ += 0.01;
                }

                sortedPeptideLines.Add(currentMZ, pepLine);
            }

            //Add the targets back into what is being displayed
            TargetsDisplayed.Clear();
            foreach (TargetPeptideLine pepLine in sortedPeptideLines.Values)
            {
                TargetsDisplayed.Add(pepLine);
            }
        }

        private void sortTargetsRT_Click(object sender, EventArgs e)
        {
            //Create a list that will be used to sort the peptides based on the max rt
            SortedList<double, TargetPeptideLine> sortedPeptideLines = new SortedList<double, TargetPeptideLine>();
            foreach (TargetPeptideLine pepLine in TargetsDisplayed)
            {
                TargetPeptideLine outPepLine = null;
                double currentRT = pepLine.Peptide.MaxRetentionTime;
                while (sortedPeptideLines.TryGetValue(currentRT, out outPepLine))
                {
                    currentRT += 0.01;
                }

                sortedPeptideLines.Add(currentRT, pepLine);
            }

            //Add the targets back into what is being displayed
            TargetsDisplayed.Clear();
            foreach (TargetPeptideLine pepLine in sortedPeptideLines.Values)
            {
                TargetsDisplayed.Add(pepLine);
            }
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

                if(Analysis)
                {
                    UpdateTargetMS2Plot(ms2Event);
                    if (ms2Event.MS3 != null)
                    {
                        UpdateTargetMS3Plot(ms2Event.MS3);
                    }
                }
                else if (Priming)
                {
                    UpdateTriggerMS2Plots(ms2Event);
                }
            }
        }

        private void targetGridView_SelectionChanged(object sender, EventArgs e)
        {
            UpdateTargetPlots();
        }

        private void targetSearchBox_TextChanged(object sender, EventArgs e)
        {
            string text = targetSearchBox.Text;

            TargetsDisplayed.Clear();

            foreach (TargetPeptideLine pepLine in Targets)
            {
                if (pepLine.PeptideString.Contains(text))
                {
                    TargetsDisplayed.Add(pepLine);
                }
            }

            UpdateTargetPlots();
        }
        
        private void ms1GraphControl_MouseClick(object sender, MouseEventArgs e)
        {
            object nearestObject;
            int index;
            ms1GraphControl.GraphPane.FindNearestObject(new PointF(e.X, e.Y), this.CreateGraphics(), out nearestObject, out index);

            if (nearestObject != null && nearestObject.GetType() == typeof(LineItem))
            {
                LineItem lineitem = (LineItem)nearestObject;
                double xVal = lineitem[index].X;

                if (targetGridView.CurrentCell != null)
                {
                    index = targetGridView.CurrentCell.RowIndex;
                }

                TargetPeptide newTarget = TargetsDisplayed[index].Peptide;

                SortedList<double, ScanEventLine> sortedScanEvents = FindNearestScanEventRT(xVal);

                ScanEvents.Clear();
                foreach (ScanEventLine sel in sortedScanEvents.Values)
                {
                    ScanEvents.Add(sel);
                }
            }
        }

        private void spectrumGraphControl1_MouseClick(object sender, MouseEventArgs e)
        {
            //TODO: Add in option to pick peaks for priming run. 
        }

        private void spectrumGraphControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            return;

            object nearestObject;
            int index;
            spectrumGraphControl1.GraphPane.FindNearestObject(new PointF(e.X, e.Y), this.CreateGraphics(), out nearestObject, out index);

            if (nearestObject != null && nearestObject.GetType() == typeof(StickItem))
            {
                StickItem stickItem = (StickItem)nearestObject;
                double xVal = stickItem[index].X;

                if (scanGridView.CurrentCell != null)
                {
                    index = scanGridView.CurrentCell.RowIndex;
                }

                MS2Event ms2Event = ScanEvents[index].ScanEvent;

                ms2Event.SPSPeaks = UpdateSPSPeaks(ms2Event, xVal);
                
                if (Analysis)
                {
                    UpdateTargetMS2Plot(ms2Event);
                    if (ms2Event.MS3 != null)
                    {
                        UpdateTargetMS3Plot(ms2Event.MS3);
                    }
                }
                else if (Priming)
                {
                    UpdateTriggerMS2Plots(ms2Event);
                }
            }
        }

        private SortedList<double, ScanEventLine> FindNearestScanEventRT(double rt)
        {
            SortedList<double, ScanEventLine> sortedSEL = new SortedList<double, ScanEventLine>();

            foreach(ScanEventLine sel in ScanEvents)
            {
                double l_distance = Math.Abs(sel.ScanEvent.RetentionTime - rt);

                ScanEventLine outSel = null;
                while(sortedSEL.TryGetValue(l_distance, out outSel))
                {
                    l_distance += 0.0000001;
                }


                sortedSEL.Add(l_distance, sel);
            }

            return sortedSEL;
        }

        private PointPairList UpdateSPSPeaks(MS2Event scan, double mz)
        {
            SPSIonsEdited = true;

            //Find the MZ that is the closest
            SortedList<double, PointPair> allPeaks = new SortedList<double, PointPair>();
            foreach (PointPair pointPair in scan.MatchedPeaks)
            {
                //Cycle through and find the peak that is the closest on the matched list
                allPeaks.Add(Math.Abs(pointPair.X-mz), pointPair);
            }

            //The entry at element one will be the closest peak that is matching in the spectrum
            PointPair matchedPair = allPeaks.ElementAt(0).Value;
            double matchedMZ = matchedPair.X;

            //First see if it is in the sps peak list
            bool peakFound = false;
            SortedList<double, PointPair> spsPeaks = new SortedList<double, PointPair>();
            foreach (PointPair pointPair in scan.SPSPeaks)
            {
                //Cycle through and find the peak that is the closest on the matched list
                if(matchedMZ >= pointPair.X - 0.00001 && matchedMZ <= pointPair.X + 0.000001)
                {
                    peakFound = true;
                }
                else
                {
                    spsPeaks.Add(pointPair.X, pointPair);
                }
                
            }

            if(!peakFound)
            {
                spsPeaks.Add(matchedPair.X, matchedPair);
            }

            PointPairList retList = new PointPairList();
            foreach(PointPair pair in spsPeaks.Values)
            {
                retList.Add(pair);
            }

            return retList;
        }

        private void exportSELs_Click(object sender, EventArgs e)
        {
            printSelectedTomahaqData();
        }

        private void printAllTomahaqData()
        {
            //First see if there is a raw file in the analysis box
            string outputFile = rawFileBox.Text.Replace(".raw", "_AllScans.csv");

            //Next see if there is a raw file in the priming raw file box
            if (outputFile == null || outputFile == "")
            {
                outputFile = primingRawBox.Text.Replace(".raw", "_AllScans.csv");
            }

            //Lastly if there are no raw files then default to the target list
            if (outputFile == null || outputFile == "")
            {
                outputFile = targetTextBox.Text.Replace(".csv", "_AllScans.csv");
            }

            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Peptide,Protein,Charge,TriggerMZ,TargetMZ,MS1TriggerIntensity,MS2RetentionTime," +
                    "MS2ScanNumber,MS3ScanNumber,MS2InjectionTime,MS3InjectionTime,MS3SPSIons,MS3SumSN,MS3IS,MS3Quant1,MS3Quant2,MS3Quant3,MS3Quant4,MS3Quant5," +
                    "MS3Quant6,MS3Quant7,MS3Quant8,MS3Quant9,MS3Quant10,MSQuant11,QuantError1,QuantError2,QuantError3,QuantError4,QuantError5,QuantError6,QuantError7,QuantError8,QuantError9,QuantError10,QuantError11");

                foreach (TargetPeptideLine targetPepLine in TargetsDisplayed)
                {
                    TargetPeptide target = targetPepLine.Peptide;
                    foreach (ScanEventLine sel in target.ScanEventLines)
                    {
                        writer.WriteLine(target.PeptideString + "," + target.ProteinString + "," + target.Charge.ToString() + "," + target.TriggerMZ + "," + target.TargetMZ + "," + sel.ToString());
                    }
                }
            }
        }

        private void printSelectedTomahaqData()
        {
            //First see if there is a raw file in the analysis box
            string outputFile = rawFileBox.Text.Replace(".raw", "_SelectedScans.csv");

            //Next see if there is a raw file in the priming raw file box
            if (outputFile == null || outputFile == "")
            {
                outputFile = primingRawBox.Text.Replace(".raw", "_SelectedScans.csv");
            }

            //Lastly if there are no raw files then default to the target list
            if (outputFile == null || outputFile == "")
            {
                outputFile = targetTextBox.Text.Replace(".csv", "_SelectedScans.csv");
            }


            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("Peptide,Protein,Charge,TriggerMZ,TargetMZ,MS1TriggerIntensity,MS2RetentionTime," +
                    "MS2ScanNumber,MS3ScanNumber,MS2InjectionTime,MS3InjectionTime,MS3SPSIons,MS3SumSN,MS3IS,MS3Quant1,MS3Quant2,MS3Quant3,MS3Quant4,MS3Quant5," +
                    "MS3Quant6,MS3Quant7,MS3Quant8,MS3Quant9,MS3Quant10,MSQuant11,QuantError1,QuantError2,QuantError3,QuantError4,QuantError5,QuantError6,QuantError7,QuantError8,QuantError9,QuantError10,QuantError11");

                foreach (TargetPeptideLine targetPepLine in TargetsDisplayed)
                {
                    TargetPeptide target = targetPepLine.Peptide;
                    foreach (ScanEventLine sel in target.ScanEventLines)
                    {
                        if (sel.Include)
                        {
                            writer.WriteLine(target.PeptideString + "," + target.ProteinString + "," + target.Charge.ToString() + "," + target.TriggerMZ + "," + target.TargetMZ + "," + sel.ToString());
                        }
                    }
                }
            }
        }

        private void updateInstrumentMethod()
        {
            string targetFile = targetTextBox.Text;

            //Print out the scan data that you have selected
            printSelectedTomahaqData();

            //
            foreach (TargetPeptideLine targetPepLine in TargetsDisplayed)
            {
                //Only update the peptide if the peptide was seleted by the user
                if (targetPepLine.Selected)
                {
                    TargetPeptide target = targetPepLine.Peptide;
                    target.SelectedScanEventLines = new List<ScanEventLine>();
                    foreach (ScanEventLine sel in target.ScanEventLines)
                    {
                        if (sel.Include)
                        {
                            target.SelectedScanEventLines.Add(sel);
                        }
                    }

                    target.AverageScanEventsLines(target.SelectedScanEventLines, target.SelectedScanEventLines.Count, includeAll: true);
                    target.UpdateIons((int)triggerIonMax.Value, (int)targetIonMax.Value, spsEdited: SPSIonsEdited);

                    //target.PopulateTriggerAndTargetIons(20, force:true); //Is this necessary - if we are here there is a raw file
                }
            }

            //Make the XMLfile that will be used to alter the method
            //Only change the parameters the user wants to
            List<TargetPeptide> targets = new List<TargetPeptide>();
            foreach(TargetPeptideLine pep in TargetsDisplayed)
            {
                //Only add to the list of targets if the peptide was seleted by the user
                if(pep.Selected)
                {
                    targets.Add(pep.Peptide);
                }
            }

            UpdateLog("Building XML");
            bool addMS1TargetedMass = addMS1TargetMassList.Checked;
            bool addMS2TriggerMass = addMS2TriggerMassList.Checked;
            bool addMS2IsoOffset = addMS2IsolationOffset.Checked;
            bool addMS3TargetedMass = addMS3TargetMassList.Checked;
            string xmlFile = null;
            if (!groupIDCheckBox.Checked)
            {
                xmlFile = BuildMethodXML(targetFile.Replace(".csv", "_curatedMethod.xml"), targets, addMS1TargetedMass, addMS2TriggerMass, addMS2IsoOffset, addMS3TargetedMass); //TODO: Change Back to Not Large
            }
            else
            {
                xmlFile = BuildLargeMethodXML(targetFile.Replace(".csv", "_curatedMethod.xml"), targets, addMS1TargetedMass, addMS2TriggerMass, addMS2IsoOffset, addMS3TargetedMass); //TODO: Change Back to Not Large
            }

            //Export the method last in case it fails due to the program not being run on the instrument
            UpdateLog("Creating New Method");
            if (templateBox.Text != "")
            {
                string templateMethod = templateBox.Text;
                string outputMethod = targetFile.Replace(".csv", "_curated.meth");
                EditMethod(templateMethod, xmlFile, outputMethod);
                ////MethodChanger.ModifyMethod(templateMethod, xmlFile, outputMethod: outputMethod);
            }
            else
            {
                UpdateLog("Cannot Create New Method Because No Template Was Provided");
            }

            UpdateTargetPlots();

        }
        #endregion

        #region IO and Input Validation Code

        private ModificationSites SwitchSite(string aa)
        {
            switch (aa)
            {
                case "K":
                    return ModificationSites.K;
                case "C":
                    return ModificationSites.C;
                case "S":
                    return ModificationSites.S;
                case "T":
                    return ModificationSites.T;
                case "Y":
                    return ModificationSites.Y;
                case "M":
                    return ModificationSites.M;
                case "NTerminus":
                    return ModificationSites.NTerminus;
                case "NPep":
                    return ModificationSites.NPep;
                case "PepC":
                    return ModificationSites.PepC;
                case "NProt":
                    return ModificationSites.NProt;
                case "ProtC":
                    return ModificationSites.ProtC;
                case "A":
                    return ModificationSites.A;
                case "R":
                    return ModificationSites.R;
                case "N":
                    return ModificationSites.N;
                case "D":
                    return ModificationSites.D;
                case "E":
                    return ModificationSites.E;
                case "Q":
                    return ModificationSites.Q;
                case "G":
                    return ModificationSites.G;
                case "H":
                    return ModificationSites.H;
                case "I":
                    return ModificationSites.I;
                case "L":
                    return ModificationSites.I;
                case "P":
                    return ModificationSites.P;
                case "F":
                    return ModificationSites.F;
                case "W":
                    return ModificationSites.W;
                case "V":
                    return ModificationSites.V;
                default:
                    return ModificationSites.None;
            }
        }

        private bool CheckBoolValue(object value)
        {
            if (value == null)
            {
                return false;
            }

            return (bool)value;
        }

        private bool CheckStringBoolValue(object value)
        {
            if (value == null)
            {
                return false;
            }
            string val = (string)value;
            if (val == "FALSE")
            {
                val = "false";
            }
            else if (val == "True")
            {
                val = "true";
            }

            return bool.Parse(val);
        }

        private string CheckStringValue(object value)
        {
            if (value == null)
            {
                return "";
            }

            return (string)value;
        }

        private char CheckCharValue(object value)
        {
            if (value == null)
            {
                return ' ';
            }

            return (char)value;
        }

        private double CheckDoubValue(object value)
        {
            if (value == null)
            {
                return 0;
            }

            return double.Parse((string)value);
        }

        private void rawFileBrowser_Click(object sender, EventArgs e)
        {
            if (rawFileFDB.ShowDialog() == DialogResult.OK)
            {
                rawFileBox.Text = rawFileFDB.FileName;
            }
        }

        private void targetBoxBrowse_Click(object sender, EventArgs e)
        {
            if (targetOnlyFDB.ShowDialog() == DialogResult.OK)
            {
                targetTextBox.Text = targetOnlyFDB.FileName;
            }
        }

        private void templateMethodBrowse_Click_1(object sender, EventArgs e)
        {
            if (templateMethodFDB.ShowDialog() == DialogResult.OK)
            {
                templateBox.Text = templateMethodFDB.FileName;
            }
        }

        private void primingRunBrowse_Click(object sender, EventArgs e)
        {
            if (primingRawOFDia.ShowDialog() == DialogResult.OK)
            {
                primingRawBox.Text = primingRawOFDia.FileName;
            }
        }

        #endregion

        #region Unused Code
        private List<TargetPeptide> SearchPeptides(double precursorMZ, SortedList<double, TargetPeptide> targetList)
        {
            List<TargetPeptide> retList = new List<TargetPeptide>();



            return retList;
        }

        private void AddModifications(out Dictionary<Modification, string> staticMods, out Dictionary<Modification, char> dynMods)
        {
            dynMods = new Dictionary<Modification, char>();
            staticMods = new Dictionary<Modification, string>();

            foreach (ModificationLine modLine in ModLines)
            {
                if (modLine.Trigger || modLine.Target)
                {
                    Modification addMod = modLine.Modification;

                    if (addMod == null)
                    {
                        addMod = new Modification(modLine.Mass, modLine.Name);
                    }

                    if (modLine.Type == "Static")
                    {
                        string type = "";

                        if (modLine.Trigger && modLine.Target)
                        {
                            type = "Both";
                        }
                        else if (modLine.Trigger)
                        {
                            type = "Trigger";
                        }
                        else if (modLine.Target)
                        {
                            type = "Target";
                        }

                        staticMods.Add(addMod, type);
                    }
                    else if (modLine.Type == "Dynamic")
                    {

                        dynMods.Add(addMod, modLine.ModChar.ElementAt(0));
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void analysisTab_Click(object sender, EventArgs e)
        {

        }

        private void addMS1TargetMassList_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, EventArgs e)
        {
        }

        #endregion

        private void deselectAll_Click(object sender, EventArgs e)
        {
            //We need to get the index of the target that is selected in the GUI
            int index = 0;
            if (targetGridView.CurrentCell != null)
            {
                index = targetGridView.CurrentCell.RowIndex;
            }

            //With the index we can get the target from the displayed list of targets
            TargetPeptide target = TargetsDisplayed[index].Peptide;

            foreach (ScanEventLine sel in target.ScanEventLines)
            {
                sel.Include = false;
            }

            scanGridView.Refresh();
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            //We need to get the index of the target that is selected in the GUI
            int index = 0;
            if (targetGridView.CurrentCell != null)
            {
                index = targetGridView.CurrentCell.RowIndex;
            }

            //With the index we can get the target from the displayed list of targets
            TargetPeptide target = TargetsDisplayed[index].Peptide;

            foreach (ScanEventLine sel in target.ScanEventLines)
            {
                sel.Include = true;
            }

            scanGridView.Refresh();
        }

        private bool ms1GraphControl_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Alt) != 0)
            {
                double X = 0;
                double Y = 0;
                PointF mousePt = new PointF(e.X, e.Y);
                MS1Pane.ReverseTransform(mousePt, out X, out Y);

                MouseDownTime = X;
            }
            return false;
        }

        private bool ms1GraphControl_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {

            if ((Control.ModifierKeys & Keys.Alt) != 0)
            {
                double X = 0;
                double Y = 0;
                PointF mousePt = new PointF(e.X, e.Y);
                MS1Pane.ReverseTransform(mousePt, out X, out Y);

                MouseUpTime = X;

                if (MouseUpTime < MouseDownTime)
                {
                    double tempVar = MouseUpTime;
                    MouseUpTime = MouseDownTime;
                    MouseDownTime = tempVar;
                }

                HighlightPlot(MouseDownTime, MouseUpTime);

                SelectScansRange();
            }
            return false;
        }

        private void HighlightPlot(double start, double stop)
        {
            MS1Pane.GraphObjList.Clear();
            BoxObj box = new BoxObj(start, 1000000000000000, stop - start, 1000000000000000, Color.DeepSkyBlue, Color.White, Color.SkyBlue);
            box.IsVisible = true;
            box.Fill.Color = Color.Transparent;
            box.Location.CoordinateFrame = CoordType.AxisXYScale;

            box.IsClippedToChartRect = true;

            box.ZOrder = ZOrder.E_BehindCurves;
            ms1GraphControl.GraphPane.GraphObjList.Add(box);

            ms1GraphControl.Refresh();
        }

        private void SelectScansRange()
        {
            double start = MouseDownTime;
            double stop = MouseUpTime;

            
            //We need to get the index of the target that is selected in the GUI
            int index = 0;
            if (targetGridView.CurrentCell != null)
            {
                index = targetGridView.CurrentCell.RowIndex;
            }

            //With the index we can get the target from the displayed list of targets
            TargetPeptide target = TargetsDisplayed[index].Peptide;

            target.StartSelectionTime = start;
            target.EndSelectionTime = stop;

            foreach (ScanEventLine sel in target.ScanEventLines)
            {
                if (double.Parse(sel.MS2RetentionTime) >= start && double.Parse(sel.MS2RetentionTime) <= stop)
                {
                    sel.Include = true;
                }
                else
                {
                    sel.Include = false;
                }
            }

            UpdateScanGridMembers(target);

            scanGridView.Refresh();
        }

        private void displaySelected_CheckedChanged(object sender, EventArgs e)
        {
            //We need to get the index of the target that is selected in the GUI
            int index = 0;
            if (targetGridView.CurrentCell != null)
            {
                index = targetGridView.CurrentCell.RowIndex;
            }

            //With the index we can get the target from the displayed list of targets
            TargetPeptide target = TargetsDisplayed[index].Peptide;

            UpdateScanGridMembers(target);
        }

        private void DisplayTargets()
        {
            TargetsDisplayed.Clear();

            foreach (TargetPeptideLine targetLine in Targets)
            {
                //This is building the target line that will go into the GUI
                if(!displayTargetsWData.Checked || (displayTargetsWData.Checked && targetLine.Peptide.ScanEventLines.Count > 0))
                {
                    TargetsDisplayed.Add(targetLine);
                }
                
            }

        }

        private void displayTargetsWData_CheckedChanged(object sender, EventArgs e)
        {
            DisplayTargets();
        }

        private void exportTargetList_Click(object sender, EventArgs e)
        {
            string outputfile = targetTextBox.Text.Replace(".csv", "_targetlist.csv");

            using (StreamWriter writer = new StreamWriter(outputfile))
            {
                //Write the header line
                if(Targets.Count > 0)
                {
                    writer.WriteLine(Targets.ElementAt(0).Peptide.GetTargetHeaders());

                    foreach (TargetPeptideLine target in Targets)
                    {
                        //Write the output for each target
                        writer.WriteLine(target.ToString());
                    }
                }
            }
        }

        private void ExportModificationFile(string fileName)
        {
            Directory.CreateDirectory(".\\ModificationFiles");
            string outputfile = ".\\ModificationFiles\\" + fileName + ".csv";

            using (StreamWriter writer = new StreamWriter(outputfile))
            {
                writer.WriteLine("Name,Mass,ModSites,ModChar,Type,Trigger,Target");

                foreach (ModificationLine modline in ModLines)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(modline.Name + ",");
                    sb.Append(modline.Mass + ",");
                    sb.Append(modline.ModSites.Replace(",", ";") + ",");
                    sb.Append(modline.ModChar + ",");
                    sb.Append(modline.Type + ",");
                    sb.Append(modline.Trigger + ",");
                    sb.Append(modline.Target + ",");

                    writer.WriteLine(sb.ToString());
                }
            }

            UpdateMoficationFileList();
        }

        private void UpdateMoficationFileList()
        {
            Directory.CreateDirectory(".\\ModificationFiles");

            ModificationFiles.Clear();
            foreach (string fileName in Directory.GetFiles(".\\ModificationFiles", "*.csv", SearchOption.TopDirectoryOnly).ToList())
            {
                ModificationFiles.Add(fileName);
            }
        }

        private void exportModificationTable_Click_1(object sender, EventArgs e)
        {
            ExportModificationFile(modFileName.Text);
        }

        private void loadUserModFile_Click(object sender, EventArgs e)
        {
            ModLines.Clear();

            using (CsvReader reader = new CsvReader(new StreamReader(ModificationFiles[modFileListBox.SelectedIndex]), true))
            {
                while (reader.ReadNextRecord())
                {
                    string name = reader["Name"];
                    string mass = reader["Mass"];
                    string modSites = reader["ModSites"].Replace(";",",");
                    string modChar = reader["ModChar"];
                    string type = reader["Type"];
                    string trigger = reader["Trigger"];
                    string target = reader["Target"];

                    AddModLine(name, mass, type, modSites, modChar, trigger, target);
                }
            }
        }

        private void AddModLine(string Name, string Mass, string Type, string Sites, string Symbol, string Trigger, string Target)
        {
            //Check the values entered by the user
            bool trigger = CheckStringBoolValue(Trigger);
            bool target = CheckStringBoolValue(Target);
            string name = CheckStringValue(Name);
            double mass = CheckDoubValue(Mass);
            string type = CheckStringValue(Type);
            string symbol = CheckStringValue(Symbol);
            string sites = CheckStringValue(Sites);

            //Make sure there is enough to make a new modification
            if (mass != 0 && name != "" && type != "" & sites != "")
            {
                //This is a bad way to add in multiple sites
                List<string> sitesList = sites.Split(',').ToList();
                List<ModificationSites> modSiteList = new List<ModificationSites>();
                foreach (string site in sitesList)
                {
                    modSiteList.Add(SwitchSite(site));
                }

                //If sites returned None then something is wrong
                if (modSiteList[0] == ModificationSites.None) { sites = "None"; }

                //First add the modification with just one of the mods
                Modification addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0]);

                //Check to see if there are 2 mods
                if (modSiteList.Count == 2)
                {
                    addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0] | modSiteList[1]);
                }
                //Check to see if there are 3 mods
                else if (modSiteList.Count == 3)
                {
                    addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0] | modSiteList[1] | modSiteList[2]);
                }
                //Check to see if there are 4 mods
                else if (modSiteList.Count == 4)
                {
                    addMod = new Modification(monoMass: mass, name: name, sites: modSiteList[0] | modSiteList[1] | modSiteList[2] | modSiteList[3]);
                }

                //If it is static then make sure the symbol is cleared
                if (type == "Static") { symbol = ""; }

                //Add a modification line
                ModLines.Add(new ModificationLine(addMod.Name, Math.Round(addMod.MonoisotopicMass, 5), sites, symbol, type, trigger, target, addMod));
            }
        }

        private void displaySelectedLine_CheckedChanged(object sender, EventArgs e)
        {
            if(displaySelectedLine.Checked)
            {
                List<TargetPeptideLine> toDisplay = new List<TargetPeptideLine>();

                foreach (TargetPeptideLine targetLine in TargetsDisplayed)
                {
                    if (targetLine.Selected)
                    {
                        toDisplay.Add(targetLine);
                    }
                }

                TargetsDisplayed.Clear();

                foreach(TargetPeptideLine target in toDisplay)
                {
                    TargetsDisplayed.Add(target);
                }
            }
            else
            {
                DisplayTargets();
            }
        }

        private void modGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void sortTargetsPeptide_Click(object sender, EventArgs e)
        {
            //Create a list that will be used to sort the peptides based on the max rt
            SortedList<string, List<TargetPeptideLine>> sortedPeptideLines = new SortedList<string, List<TargetPeptideLine>>();

            foreach (TargetPeptideLine pepLine in TargetsDisplayed)
            {
                List<TargetPeptideLine> outList = new List<TargetPeptideLine>();
                if(sortedPeptideLines.TryGetValue(pepLine.Peptide.PeptideString, out outList))
                {
                    outList.Add(pepLine);
                }
                else
                {
                    sortedPeptideLines.Add(pepLine.Peptide.PeptideString, new List<TargetPeptideLine>());
                    sortedPeptideLines[pepLine.Peptide.PeptideString].Add(pepLine);
                }
            }

            //Add the targets back into what is being displayed
            TargetsDisplayed.Clear();

            foreach (KeyValuePair<string, List<TargetPeptideLine>> kvp in sortedPeptideLines)
            {
                foreach(TargetPeptideLine pepLine in kvp.Value)
                {
                    TargetsDisplayed.Add(pepLine);
                }
            }
        }

        private void spectrumGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void sortTargetProteins_Click(object sender, EventArgs e)
        {
            //Create a list that will be used to sort the peptides based on the max rt
            SortedList<string, List<TargetPeptideLine>> sortedPeptideLines = new SortedList<string, List<TargetPeptideLine>>();

            foreach (TargetPeptideLine pepLine in TargetsDisplayed)
            {
                List<TargetPeptideLine> outList = new List<TargetPeptideLine>();
                if (sortedPeptideLines.TryGetValue(pepLine.Peptide.ProteinString, out outList))
                {
                    outList.Add(pepLine);
                }
                else
                {
                    sortedPeptideLines.Add(pepLine.Peptide.ProteinString, new List<TargetPeptideLine>());
                    sortedPeptideLines[pepLine.Peptide.ProteinString].Add(pepLine);
                }
            }

            //Add the targets back into what is being displayed
            TargetsDisplayed.Clear();

            foreach (KeyValuePair<string, List<TargetPeptideLine>> kvp in sortedPeptideLines)
            {
                foreach (TargetPeptideLine pepLine in kvp.Value)
                {
                    TargetsDisplayed.Add(pepLine);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Import the modifications
            UpdateLog("Importing Modifications");
            //<Static/Dynamic, <Trigger/Target, <Modification, Symbol>>>
            Dictionary<string, Dictionary<string, Dictionary<Modification, string>>> modificationDict = BuildModificationDictionary();

            //Import the Peptides
            string targetFile = targetTextBox.Text;
            bool chargeProvided = false;
            SortedList<double, TargetPeptide> targetList = ImportTargets(targetFile, modificationDict, out chargeProvided);

            Dictionary<string, TargetPeptide> addedTargets = new Dictionary<string, TargetPeptide>();

            Dictionary<double, List<TargetPeptide>> addDicts = new Dictionary<double, List<TargetPeptide>>();

            foreach(TargetPeptide peptide in targetList.Values)
            {
                double offset = Math.Round(peptide.MassShift, 2);

                List<TargetPeptide> outList = new List<TargetPeptide>();
                if(!addDicts.TryGetValue(offset, out outList))
                {
                    addDicts.Add(offset, new List<TargetPeptide>());
                }

                addDicts[offset].Add(peptide);
            }

            foreach(KeyValuePair<double, List<TargetPeptide>> kvp in addDicts)
            {
                double offset = kvp.Key;

                using (StreamWriter writer = new StreamWriter(targetFile.Replace(".csv", "_triggerPeptideInclusion_Offset_" + offset + "mz.csv")))
                {
                    writer.WriteLine("Name,m/z,z");

                    foreach(TargetPeptide peptide in kvp.Value)
                    {
                        writer.WriteLine(peptide.PeptideString + "," + peptide.TriggerMZ + "," + peptide.Charge);
                    }
                }

            }
        }

        private void paramTab_Click(object sender, EventArgs e)
        {

        }

        private void eclipseCheckbox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
