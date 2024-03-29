﻿namespace TomahaqCompanion
{
    partial class TomahaqCompanionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.targetGridView = new System.Windows.Forms.DataGridView();
            this.sortTargetsMZ = new System.Windows.Forms.Button();
            this.sortTargetsRT = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ms1GraphControl = new ZedGraph.ZedGraphControl();
            this.spectrumGraphControl1 = new ZedGraph.ZedGraphControl();
            this.spectrumGraphControl2 = new ZedGraph.ZedGraphControl();
            this.scanGridView = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.methodTab = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.primingTargetList = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ms2DataAnalysis = new System.Windows.Forms.CheckBox();
            this.tomaAPIRawFile = new System.Windows.Forms.CheckBox();
            this.rawFileBrowser = new System.Windows.Forms.Button();
            this.analyzeRun = new System.Windows.Forms.Button();
            this.rawFileBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.addUserModifications = new System.Windows.Forms.Button();
            this.modGridView = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.userModGridView = new System.Windows.Forms.DataGridView();
            this.triggerCB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.targetCB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.massBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeCombo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.symbolBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sitesBox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.triggerIonMZMaxTB = new System.Windows.Forms.TextBox();
            this.triggerIonMZMinTB = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.minNumTriggerIonsNB = new System.Windows.Forms.NumericUpDown();
            this.groupIDCheckBox = new System.Windows.Forms.CheckBox();
            this.eclipseCheckbox = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.targetIonMax = new System.Windows.Forms.NumericUpDown();
            this.triggerIonMax = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.spsMaxMZ = new System.Windows.Forms.TextBox();
            this.spsMinMZ = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.precExHigh = new System.Windows.Forms.TextBox();
            this.precExLow = new System.Windows.Forms.TextBox();
            this.spsIonsAbovePrec = new System.Windows.Forms.CheckBox();
            this.chooseBestCharge = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.methodLengthBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.fragTolBox = new System.Windows.Forms.TextBox();
            this.ppmRB = new System.Windows.Forms.RadioButton();
            this.daRB = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.rtWindowTextBox = new System.Windows.Forms.TextBox();
            this.primingRunBrowse = new System.Windows.Forms.Button();
            this.label100 = new System.Windows.Forms.Label();
            this.primingRawBox = new System.Windows.Forms.TextBox();
            this.templateMethodBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.addMS3TargetMassList = new System.Windows.Forms.CheckBox();
            this.addMS1TargetMassList = new System.Windows.Forms.CheckBox();
            this.addMS2IsolationOffset = new System.Windows.Forms.CheckBox();
            this.addMS2TriggerMassList = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.templateBox = new System.Windows.Forms.TextBox();
            this.createMethod = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.targetBoxBrowse = new System.Windows.Forms.Button();
            this.targetTextBox = new System.Windows.Forms.TextBox();
            this.analysisTab = new System.Windows.Forms.TabPage();
            this.sortTargetProteins = new System.Windows.Forms.Button();
            this.proteinSearchBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.sortTargetsPeptide = new System.Windows.Forms.Button();
            this.displaySelectedLine = new System.Windows.Forms.CheckBox();
            this.displayTargetsWData = new System.Windows.Forms.CheckBox();
            this.displaySelected = new System.Windows.Forms.CheckBox();
            this.selectAll = new System.Windows.Forms.Button();
            this.deselectAll = new System.Windows.Forms.Button();
            this.exportSELs = new System.Windows.Forms.Button();
            this.updateMethod = new System.Windows.Forms.Button();
            this.targetSearchBox = new System.Windows.Forms.TextBox();
            this.paramTab = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.xmlTargetPepBrowse = new System.Windows.Forms.Button();
            this.xmlTemplateBrowse = new System.Windows.Forms.Button();
            this.xmlTemplateTB = new System.Windows.Forms.TextBox();
            this.xmlTargetPepTB = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.xmlBrowse = new System.Windows.Forms.Button();
            this.xmlTextBox = new System.Windows.Forms.TextBox();
            this.methodChangerAlone = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.printDebug = new System.Windows.Forms.CheckBox();
            this.modSaveLoadGroup = new System.Windows.Forms.GroupBox();
            this.loadUserModFile = new System.Windows.Forms.Button();
            this.modFileListBox = new System.Windows.Forms.ListBox();
            this.modFileName = new System.Windows.Forms.TextBox();
            this.exportModificationTable = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.exportTargetList = new System.Windows.Forms.Button();
            this.targetOnlyFDB = new System.Windows.Forms.OpenFileDialog();
            this.templateMethodFDB = new System.Windows.Forms.OpenFileDialog();
            this.rawFileFDB = new System.Windows.Forms.OpenFileDialog();
            this.primingRawFDB = new System.Windows.Forms.OpenFileDialog();
            this.primingRawOFDia = new System.Windows.Forms.OpenFileDialog();
            this.xmlOFDB = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.targetGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanGridView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.methodTab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userModGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minNumTriggerIonsNB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetIonMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerIonMax)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.analysisTab.SuspendLayout();
            this.paramTab.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.modSaveLoadGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // targetGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.targetGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.targetGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.targetGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.targetGridView.Location = new System.Drawing.Point(3, 34);
            this.targetGridView.Name = "targetGridView";
            this.targetGridView.Size = new System.Drawing.Size(472, 495);
            this.targetGridView.TabIndex = 0;
            this.targetGridView.SelectionChanged += new System.EventHandler(this.targetGridView_SelectionChanged);
            // 
            // sortTargetsMZ
            // 
            this.sortTargetsMZ.Location = new System.Drawing.Point(6, 535);
            this.sortTargetsMZ.Name = "sortTargetsMZ";
            this.sortTargetsMZ.Size = new System.Drawing.Size(75, 23);
            this.sortTargetsMZ.TabIndex = 42;
            this.sortTargetsMZ.Text = "Sort by MZ";
            this.sortTargetsMZ.UseVisualStyleBackColor = true;
            this.sortTargetsMZ.Click += new System.EventHandler(this.sortTargetsMZ_Click);
            // 
            // sortTargetsRT
            // 
            this.sortTargetsRT.Location = new System.Drawing.Point(87, 535);
            this.sortTargetsRT.Name = "sortTargetsRT";
            this.sortTargetsRT.Size = new System.Drawing.Size(75, 23);
            this.sortTargetsRT.TabIndex = 41;
            this.sortTargetsRT.Text = "Sort by RT";
            this.sortTargetsRT.UseVisualStyleBackColor = true;
            this.sortTargetsRT.Click += new System.EventHandler(this.sortTargetsRT_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Search Proteins";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ms1GraphControl
            // 
            this.ms1GraphControl.Location = new System.Drawing.Point(482, 6);
            this.ms1GraphControl.Margin = new System.Windows.Forms.Padding(1);
            this.ms1GraphControl.Name = "ms1GraphControl";
            this.ms1GraphControl.ScrollGrace = 0D;
            this.ms1GraphControl.ScrollMaxX = 0D;
            this.ms1GraphControl.ScrollMaxY = 0D;
            this.ms1GraphControl.ScrollMaxY2 = 0D;
            this.ms1GraphControl.ScrollMinX = 0D;
            this.ms1GraphControl.ScrollMinY = 0D;
            this.ms1GraphControl.ScrollMinY2 = 0D;
            this.ms1GraphControl.Size = new System.Drawing.Size(954, 190);
            this.ms1GraphControl.TabIndex = 46;
            this.ms1GraphControl.UseExtendedPrintDialog = true;
            this.ms1GraphControl.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.ms1GraphControl_MouseDownEvent);
            this.ms1GraphControl.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.ms1GraphControl_MouseUpEvent);
            this.ms1GraphControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ms1GraphControl_MouseClick);
            // 
            // spectrumGraphControl1
            // 
            this.spectrumGraphControl1.Location = new System.Drawing.Point(482, 203);
            this.spectrumGraphControl1.Margin = new System.Windows.Forms.Padding(1);
            this.spectrumGraphControl1.Name = "spectrumGraphControl1";
            this.spectrumGraphControl1.ScrollGrace = 0D;
            this.spectrumGraphControl1.ScrollMaxX = 0D;
            this.spectrumGraphControl1.ScrollMaxY = 0D;
            this.spectrumGraphControl1.ScrollMaxY2 = 0D;
            this.spectrumGraphControl1.ScrollMinX = 0D;
            this.spectrumGraphControl1.ScrollMinY = 0D;
            this.spectrumGraphControl1.ScrollMinY2 = 0D;
            this.spectrumGraphControl1.Size = new System.Drawing.Size(467, 179);
            this.spectrumGraphControl1.TabIndex = 47;
            this.spectrumGraphControl1.UseExtendedPrintDialog = true;
            this.spectrumGraphControl1.Load += new System.EventHandler(this.spectrumGraphControl1_Load);
            this.spectrumGraphControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.spectrumGraphControl1_MouseClick);
            this.spectrumGraphControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.spectrumGraphControl1_MouseDoubleClick);
            // 
            // spectrumGraphControl2
            // 
            this.spectrumGraphControl2.Location = new System.Drawing.Point(967, 203);
            this.spectrumGraphControl2.Margin = new System.Windows.Forms.Padding(1);
            this.spectrumGraphControl2.Name = "spectrumGraphControl2";
            this.spectrumGraphControl2.ScrollGrace = 0D;
            this.spectrumGraphControl2.ScrollMaxX = 0D;
            this.spectrumGraphControl2.ScrollMaxY = 0D;
            this.spectrumGraphControl2.ScrollMaxY2 = 0D;
            this.spectrumGraphControl2.ScrollMinX = 0D;
            this.spectrumGraphControl2.ScrollMinY = 0D;
            this.spectrumGraphControl2.ScrollMinY2 = 0D;
            this.spectrumGraphControl2.Size = new System.Drawing.Size(467, 179);
            this.spectrumGraphControl2.TabIndex = 49;
            this.spectrumGraphControl2.UseExtendedPrintDialog = true;
            // 
            // scanGridView
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scanGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.scanGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.scanGridView.DefaultCellStyle = dataGridViewCellStyle4;
            this.scanGridView.Location = new System.Drawing.Point(482, 401);
            this.scanGridView.Name = "scanGridView";
            this.scanGridView.Size = new System.Drawing.Size(952, 153);
            this.scanGridView.TabIndex = 52;
            this.scanGridView.SelectionChanged += new System.EventHandler(this.scanGridView_SelectionChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(479, 385);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "MS/MS Events";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(2, 662);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(1449, 77);
            this.logBox.TabIndex = 57;
            this.logBox.Text = "";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.methodTab);
            this.tabControl.Controls.Add(this.analysisTab);
            this.tabControl.Controls.Add(this.paramTab);
            this.tabControl.Location = new System.Drawing.Point(9, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1457, 654);
            this.tabControl.TabIndex = 59;
            // 
            // methodTab
            // 
            this.methodTab.AutoScroll = true;
            this.methodTab.Controls.Add(this.groupBox4);
            this.methodTab.Controls.Add(this.groupBox5);
            this.methodTab.Controls.Add(this.addUserModifications);
            this.methodTab.Controls.Add(this.modGridView);
            this.methodTab.Controls.Add(this.label11);
            this.methodTab.Controls.Add(this.userModGridView);
            this.methodTab.Controls.Add(this.label6);
            this.methodTab.Controls.Add(this.groupBox3);
            this.methodTab.Controls.Add(this.groupBox2);
            this.methodTab.Location = new System.Drawing.Point(4, 22);
            this.methodTab.Name = "methodTab";
            this.methodTab.Size = new System.Drawing.Size(1449, 628);
            this.methodTab.TabIndex = 3;
            this.methodTab.Text = "TOMAHAQ File Loader and Method Builder";
            this.methodTab.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.primingTargetList);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(10, 101);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(958, 58);
            this.groupBox4.TabIndex = 76;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Print TOMAHAQ Priming Run Inclusion List";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(17, 26);
            this.label8.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(643, 13);
            this.label8.TabIndex = 76;
            this.label8.Text = "If you are going to run the labeled trigger peptides alone (\"Priming Run\") use th" +
    "is target list to ensure selection of the monoisotopic peak";
            // 
            // primingTargetList
            // 
            this.primingTargetList.Location = new System.Drawing.Point(819, 21);
            this.primingTargetList.Name = "primingTargetList";
            this.primingTargetList.Size = new System.Drawing.Size(128, 23);
            this.primingTargetList.TabIndex = 68;
            this.primingTargetList.Text = "Priming Target List";
            this.primingTargetList.UseVisualStyleBackColor = true;
            this.primingTargetList.Click += new System.EventHandler(this.primingTargetList_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ms2DataAnalysis);
            this.groupBox5.Controls.Add(this.tomaAPIRawFile);
            this.groupBox5.Controls.Add(this.rawFileBrowser);
            this.groupBox5.Controls.Add(this.analyzeRun);
            this.groupBox5.Controls.Add(this.rawFileBox);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(10, 518);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(958, 100);
            this.groupBox5.TabIndex = 78;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Analyze TOMAHAQ Run";
            // 
            // ms2DataAnalysis
            // 
            this.ms2DataAnalysis.AutoSize = true;
            this.ms2DataAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ms2DataAnalysis.Location = new System.Drawing.Point(21, 69);
            this.ms2DataAnalysis.Name = "ms2DataAnalysis";
            this.ms2DataAnalysis.Size = new System.Drawing.Size(116, 17);
            this.ms2DataAnalysis.TabIndex = 77;
            this.ms2DataAnalysis.Text = "MS2 Quantification";
            this.ms2DataAnalysis.UseVisualStyleBackColor = true;
            // 
            // tomaAPIRawFile
            // 
            this.tomaAPIRawFile.AutoSize = true;
            this.tomaAPIRawFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tomaAPIRawFile.Location = new System.Drawing.Point(155, 69);
            this.tomaAPIRawFile.Name = "tomaAPIRawFile";
            this.tomaAPIRawFile.Size = new System.Drawing.Size(132, 17);
            this.tomaAPIRawFile.TabIndex = 60;
            this.tomaAPIRawFile.Text = "TomahaqAPI Raw File";
            this.tomaAPIRawFile.UseVisualStyleBackColor = true;
            // 
            // rawFileBrowser
            // 
            this.rawFileBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rawFileBrowser.Location = new System.Drawing.Point(857, 32);
            this.rawFileBrowser.Name = "rawFileBrowser";
            this.rawFileBrowser.Size = new System.Drawing.Size(90, 23);
            this.rawFileBrowser.TabIndex = 76;
            this.rawFileBrowser.Text = "Browse";
            this.rawFileBrowser.UseVisualStyleBackColor = true;
            this.rawFileBrowser.Click += new System.EventHandler(this.rawFileBrowser_Click);
            // 
            // analyzeRun
            // 
            this.analyzeRun.Location = new System.Drawing.Point(819, 69);
            this.analyzeRun.Name = "analyzeRun";
            this.analyzeRun.Size = new System.Drawing.Size(128, 23);
            this.analyzeRun.TabIndex = 64;
            this.analyzeRun.Text = "Analyze Run";
            this.analyzeRun.UseVisualStyleBackColor = true;
            this.analyzeRun.Click += new System.EventHandler(this.analyzeRun_Click);
            // 
            // rawFileBox
            // 
            this.rawFileBox.AllowDrop = true;
            this.rawFileBox.Location = new System.Drawing.Point(20, 35);
            this.rawFileBox.Margin = new System.Windows.Forms.Padding(1);
            this.rawFileBox.Name = "rawFileBox";
            this.rawFileBox.Size = new System.Drawing.Size(828, 20);
            this.rawFileBox.TabIndex = 61;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 21);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "TOMAHAQ Raw File (.Raw)";
            // 
            // addUserModifications
            // 
            this.addUserModifications.Location = new System.Drawing.Point(1324, 426);
            this.addUserModifications.Name = "addUserModifications";
            this.addUserModifications.Size = new System.Drawing.Size(120, 23);
            this.addUserModifications.TabIndex = 65;
            this.addUserModifications.Text = "Add User Mods";
            this.addUserModifications.UseVisualStyleBackColor = true;
            this.addUserModifications.Click += new System.EventHandler(this.addUserModifications_Click);
            // 
            // modGridView
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.modGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.modGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.modGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.modGridView.Location = new System.Drawing.Point(977, 21);
            this.modGridView.Name = "modGridView";
            this.modGridView.Size = new System.Drawing.Size(467, 399);
            this.modGridView.TabIndex = 71;
            this.modGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.modGridView_CellContentClick);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(974, 454);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 13);
            this.label11.TabIndex = 74;
            this.label11.Text = "User Modifications";
            // 
            // userModGridView
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.userModGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.userModGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.userModGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.triggerCB,
            this.targetCB,
            this.nameBox,
            this.massBox,
            this.typeCombo,
            this.symbolBox,
            this.sitesBox});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.userModGridView.DefaultCellStyle = dataGridViewCellStyle8;
            this.userModGridView.Location = new System.Drawing.Point(977, 478);
            this.userModGridView.Name = "userModGridView";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.userModGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.userModGridView.Size = new System.Drawing.Size(467, 137);
            this.userModGridView.TabIndex = 73;
            // 
            // triggerCB
            // 
            this.triggerCB.HeaderText = "Trigger";
            this.triggerCB.Name = "triggerCB";
            this.triggerCB.Width = 60;
            // 
            // targetCB
            // 
            this.targetCB.HeaderText = "Target";
            this.targetCB.Name = "targetCB";
            this.targetCB.Width = 60;
            // 
            // nameBox
            // 
            this.nameBox.HeaderText = "Name";
            this.nameBox.Name = "nameBox";
            this.nameBox.Width = 60;
            // 
            // massBox
            // 
            this.massBox.HeaderText = "Mono Mass";
            this.massBox.Name = "massBox";
            this.massBox.Width = 90;
            // 
            // typeCombo
            // 
            this.typeCombo.HeaderText = "Type";
            this.typeCombo.Items.AddRange(new object[] {
            "Static",
            "Dynamic"});
            this.typeCombo.Name = "typeCombo";
            this.typeCombo.Width = 70;
            // 
            // symbolBox
            // 
            this.symbolBox.HeaderText = "Symbol";
            this.symbolBox.Name = "symbolBox";
            this.symbolBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.symbolBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.symbolBox.Width = 45;
            // 
            // sitesBox
            // 
            this.sitesBox.HeaderText = "Sites";
            this.sitesBox.Name = "sitesBox";
            this.sitesBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sitesBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sitesBox.Width = 60;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(974, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 72;
            this.label6.Text = "Modifications";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.triggerIonMZMaxTB);
            this.groupBox3.Controls.Add(this.triggerIonMZMinTB);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.minNumTriggerIonsNB);
            this.groupBox3.Controls.Add(this.groupIDCheckBox);
            this.groupBox3.Controls.Add(this.eclipseCheckbox);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.targetIonMax);
            this.groupBox3.Controls.Add(this.triggerIonMax);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.spsMaxMZ);
            this.groupBox3.Controls.Add(this.spsMinMZ);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.precExHigh);
            this.groupBox3.Controls.Add(this.precExLow);
            this.groupBox3.Controls.Add(this.spsIonsAbovePrec);
            this.groupBox3.Controls.Add(this.chooseBestCharge);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.methodLengthBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.fragTolBox);
            this.groupBox3.Controls.Add(this.ppmRB);
            this.groupBox3.Controls.Add(this.daRB);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.rtWindowTextBox);
            this.groupBox3.Controls.Add(this.primingRunBrowse);
            this.groupBox3.Controls.Add(this.label100);
            this.groupBox3.Controls.Add(this.primingRawBox);
            this.groupBox3.Controls.Add(this.templateMethodBrowse);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.templateBox);
            this.groupBox3.Controls.Add(this.createMethod);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 174);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(958, 338);
            this.groupBox3.TabIndex = 70;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Create TOMAHAQ Method";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(425, 252);
            this.label22.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(120, 13);
            this.label22.TabIndex = 107;
            this.label22.Text = "Trigger Ion Range (m/z)";
            // 
            // triggerIonMZMaxTB
            // 
            this.triggerIonMZMaxTB.AllowDrop = true;
            this.triggerIonMZMaxTB.Location = new System.Drawing.Point(489, 275);
            this.triggerIonMZMaxTB.Margin = new System.Windows.Forms.Padding(1);
            this.triggerIonMZMaxTB.Name = "triggerIonMZMaxTB";
            this.triggerIonMZMaxTB.Size = new System.Drawing.Size(52, 20);
            this.triggerIonMZMaxTB.TabIndex = 106;
            this.triggerIonMZMaxTB.Text = "2000";
            this.triggerIonMZMaxTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // triggerIonMZMinTB
            // 
            this.triggerIonMZMinTB.AllowDrop = true;
            this.triggerIonMZMinTB.Location = new System.Drawing.Point(428, 275);
            this.triggerIonMZMinTB.Margin = new System.Windows.Forms.Padding(1);
            this.triggerIonMZMinTB.Name = "triggerIonMZMinTB";
            this.triggerIonMZMinTB.Size = new System.Drawing.Size(42, 20);
            this.triggerIonMZMinTB.TabIndex = 105;
            this.triggerIonMZMinTB.Text = "200";
            this.triggerIonMZMinTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(623, 277);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(165, 13);
            this.label14.TabIndex = 104;
            this.label14.Text = "Min # Trigger Ions to Include Pep";
            // 
            // minNumTriggerIonsNB
            // 
            this.minNumTriggerIonsNB.Location = new System.Drawing.Point(567, 274);
            this.minNumTriggerIonsNB.Name = "minNumTriggerIonsNB";
            this.minNumTriggerIonsNB.Size = new System.Drawing.Size(52, 20);
            this.minNumTriggerIonsNB.TabIndex = 103;
            this.minNumTriggerIonsNB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.minNumTriggerIonsNB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupIDCheckBox
            // 
            this.groupIDCheckBox.AutoSize = true;
            this.groupIDCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupIDCheckBox.Location = new System.Drawing.Point(802, 276);
            this.groupIDCheckBox.Name = "groupIDCheckBox";
            this.groupIDCheckBox.Size = new System.Drawing.Size(91, 17);
            this.groupIDCheckBox.TabIndex = 102;
            this.groupIDCheckBox.Text = "Use Group ID";
            this.groupIDCheckBox.UseVisualStyleBackColor = true;
            // 
            // eclipseCheckbox
            // 
            this.eclipseCheckbox.AutoSize = true;
            this.eclipseCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eclipseCheckbox.Location = new System.Drawing.Point(802, 248);
            this.eclipseCheckbox.Name = "eclipseCheckbox";
            this.eclipseCheckbox.Size = new System.Drawing.Size(100, 17);
            this.eclipseCheckbox.TabIndex = 101;
            this.eclipseCheckbox.Text = "Orbitrap Eclipse";
            this.eclipseCheckbox.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(623, 237);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(130, 13);
            this.label21.TabIndex = 100;
            this.label21.Text = "Max # of Target SPS Ions";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(623, 196);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(108, 13);
            this.label20.TabIndex = 99;
            this.label20.Text = "Max # of Trigger Ions";
            this.label20.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // targetIonMax
            // 
            this.targetIonMax.Location = new System.Drawing.Point(567, 234);
            this.targetIonMax.Name = "targetIonMax";
            this.targetIonMax.Size = new System.Drawing.Size(52, 20);
            this.targetIonMax.TabIndex = 98;
            this.targetIonMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.targetIonMax.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // triggerIonMax
            // 
            this.triggerIonMax.Location = new System.Drawing.Point(567, 192);
            this.triggerIonMax.Name = "triggerIonMax";
            this.triggerIonMax.Size = new System.Drawing.Size(52, 20);
            this.triggerIonMax.TabIndex = 97;
            this.triggerIonMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.triggerIonMax.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(846, 117);
            this.label18.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(103, 13);
            this.label18.TabIndex = 96;
            this.label18.Text = "SPS Ion Range (Th)";
            // 
            // spsMaxMZ
            // 
            this.spsMaxMZ.AllowDrop = true;
            this.spsMaxMZ.Location = new System.Drawing.Point(893, 134);
            this.spsMaxMZ.Margin = new System.Windows.Forms.Padding(1);
            this.spsMaxMZ.Name = "spsMaxMZ";
            this.spsMaxMZ.Size = new System.Drawing.Size(52, 20);
            this.spsMaxMZ.TabIndex = 95;
            this.spsMaxMZ.Text = "2000";
            this.spsMaxMZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // spsMinMZ
            // 
            this.spsMinMZ.AllowDrop = true;
            this.spsMinMZ.Location = new System.Drawing.Point(847, 134);
            this.spsMinMZ.Margin = new System.Windows.Forms.Padding(1);
            this.spsMinMZ.Name = "spsMinMZ";
            this.spsMinMZ.Size = new System.Drawing.Size(42, 20);
            this.spsMinMZ.TabIndex = 94;
            this.spsMinMZ.Text = "400";
            this.spsMinMZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(782, 155);
            this.label17.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 13);
            this.label17.TabIndex = 93;
            this.label17.Text = "Above";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(701, 154);
            this.label16.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 13);
            this.label16.TabIndex = 92;
            this.label16.Text = "Below";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(691, 116);
            this.label15.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(141, 13);
            this.label15.TabIndex = 91;
            this.label15.Text = "Prec Exclusion Window (Th)";
            // 
            // precExHigh
            // 
            this.precExHigh.AllowDrop = true;
            this.precExHigh.Location = new System.Drawing.Point(776, 133);
            this.precExHigh.Margin = new System.Windows.Forms.Padding(1);
            this.precExHigh.Name = "precExHigh";
            this.precExHigh.Size = new System.Drawing.Size(52, 20);
            this.precExHigh.TabIndex = 90;
            this.precExHigh.Text = "5";
            this.precExHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // precExLow
            // 
            this.precExLow.AllowDrop = true;
            this.precExLow.Location = new System.Drawing.Point(694, 133);
            this.precExLow.Margin = new System.Windows.Forms.Padding(1);
            this.precExLow.Name = "precExLow";
            this.precExLow.Size = new System.Drawing.Size(52, 20);
            this.precExLow.TabIndex = 89;
            this.precExLow.Text = "15";
            this.precExLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // spsIonsAbovePrec
            // 
            this.spsIonsAbovePrec.AutoSize = true;
            this.spsIonsAbovePrec.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spsIonsAbovePrec.Location = new System.Drawing.Point(802, 222);
            this.spsIonsAbovePrec.Name = "spsIonsAbovePrec";
            this.spsIonsAbovePrec.Size = new System.Drawing.Size(122, 17);
            this.spsIonsAbovePrec.TabIndex = 88;
            this.spsIonsAbovePrec.Text = "SPS MZ > Prec. MZ";
            this.spsIonsAbovePrec.UseVisualStyleBackColor = true;
            // 
            // chooseBestCharge
            // 
            this.chooseBestCharge.AutoSize = true;
            this.chooseBestCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseBestCharge.Location = new System.Drawing.Point(802, 195);
            this.chooseBestCharge.Name = "chooseBestCharge";
            this.chooseBestCharge.Size = new System.Drawing.Size(151, 17);
            this.chooseBestCharge.TabIndex = 86;
            this.chooseBestCharge.Text = "Choose Best Charge State";
            this.chooseBestCharge.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(424, 117);
            this.label12.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 13);
            this.label12.TabIndex = 85;
            this.label12.Text = "Method Length (min)";
            // 
            // methodLengthBox
            // 
            this.methodLengthBox.AllowDrop = true;
            this.methodLengthBox.Location = new System.Drawing.Point(425, 134);
            this.methodLengthBox.Margin = new System.Windows.Forms.Padding(1);
            this.methodLengthBox.Name = "methodLengthBox";
            this.methodLengthBox.Size = new System.Drawing.Size(103, 20);
            this.methodLengthBox.TabIndex = 84;
            this.methodLengthBox.Text = "120";
            this.methodLengthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(425, 169);
            this.label10.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 13);
            this.label10.TabIndex = 83;
            this.label10.Text = "Fragment Ion Tolerance";
            // 
            // fragTolBox
            // 
            this.fragTolBox.AllowDrop = true;
            this.fragTolBox.Location = new System.Drawing.Point(425, 190);
            this.fragTolBox.Margin = new System.Windows.Forms.Padding(1);
            this.fragTolBox.Name = "fragTolBox";
            this.fragTolBox.Size = new System.Drawing.Size(119, 20);
            this.fragTolBox.TabIndex = 82;
            this.fragTolBox.Text = "10";
            this.fragTolBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ppmRB
            // 
            this.ppmRB.AutoSize = true;
            this.ppmRB.Checked = true;
            this.ppmRB.Location = new System.Drawing.Point(435, 218);
            this.ppmRB.Name = "ppmRB";
            this.ppmRB.Size = new System.Drawing.Size(51, 17);
            this.ppmRB.TabIndex = 64;
            this.ppmRB.TabStop = true;
            this.ppmRB.Text = "PPM";
            this.ppmRB.UseVisualStyleBackColor = true;
            // 
            // daRB
            // 
            this.daRB.AutoSize = true;
            this.daRB.Location = new System.Drawing.Point(496, 219);
            this.daRB.Name = "daRB";
            this.daRB.Size = new System.Drawing.Size(42, 17);
            this.daRB.TabIndex = 65;
            this.daRB.TabStop = true;
            this.daRB.Text = "DA";
            this.daRB.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(540, 116);
            this.label9.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 13);
            this.label9.TabIndex = 81;
            this.label9.Text = "Targeting RT Window (min)";
            // 
            // rtWindowTextBox
            // 
            this.rtWindowTextBox.AllowDrop = true;
            this.rtWindowTextBox.Location = new System.Drawing.Point(541, 133);
            this.rtWindowTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.rtWindowTextBox.Name = "rtWindowTextBox";
            this.rtWindowTextBox.Size = new System.Drawing.Size(134, 20);
            this.rtWindowTextBox.TabIndex = 80;
            this.rtWindowTextBox.Text = "20";
            this.rtWindowTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // primingRunBrowse
            // 
            this.primingRunBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.primingRunBrowse.Location = new System.Drawing.Point(862, 80);
            this.primingRunBrowse.Name = "primingRunBrowse";
            this.primingRunBrowse.Size = new System.Drawing.Size(85, 23);
            this.primingRunBrowse.TabIndex = 79;
            this.primingRunBrowse.Text = "Browse";
            this.primingRunBrowse.UseVisualStyleBackColor = true;
            this.primingRunBrowse.Click += new System.EventHandler(this.primingRunBrowse_Click);
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label100.Location = new System.Drawing.Point(17, 68);
            this.label100.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(192, 13);
            this.label100.TabIndex = 78;
            this.label100.Text = "Priming Run Raw File (.Raw) *Optional*";
            // 
            // primingRawBox
            // 
            this.primingRawBox.AllowDrop = true;
            this.primingRawBox.Location = new System.Drawing.Point(20, 82);
            this.primingRawBox.Margin = new System.Windows.Forms.Padding(1);
            this.primingRawBox.Name = "primingRawBox";
            this.primingRawBox.Size = new System.Drawing.Size(831, 20);
            this.primingRawBox.TabIndex = 77;
            // 
            // templateMethodBrowse
            // 
            this.templateMethodBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateMethodBrowse.Location = new System.Drawing.Point(862, 36);
            this.templateMethodBrowse.Name = "templateMethodBrowse";
            this.templateMethodBrowse.Size = new System.Drawing.Size(85, 23);
            this.templateMethodBrowse.TabIndex = 76;
            this.templateMethodBrowse.Text = "Browse";
            this.templateMethodBrowse.UseVisualStyleBackColor = true;
            this.templateMethodBrowse.Click += new System.EventHandler(this.templateMethodBrowse_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.addMS3TargetMassList);
            this.groupBox1.Controls.Add(this.addMS1TargetMassList);
            this.groupBox1.Controls.Add(this.addMS2IsolationOffset);
            this.groupBox1.Controls.Add(this.addMS2TriggerMassList);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(20, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 185);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method Modifications";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 24);
            this.label7.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(380, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Instrument Filters To Edit - Unselect any filters you are not using in your metho" +
    "d!";
            // 
            // addMS3TargetMassList
            // 
            this.addMS3TargetMassList.AutoSize = true;
            this.addMS3TargetMassList.Checked = true;
            this.addMS3TargetMassList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addMS3TargetMassList.Location = new System.Drawing.Point(21, 141);
            this.addMS3TargetMassList.Name = "addMS3TargetMassList";
            this.addMS3TargetMassList.Size = new System.Drawing.Size(263, 17);
            this.addMS3TargetMassList.TabIndex = 11;
            this.addMS3TargetMassList.Text = "\"Targeted Mass Filter\" before Target Peptide MS3";
            this.addMS3TargetMassList.UseVisualStyleBackColor = true;
            // 
            // addMS1TargetMassList
            // 
            this.addMS1TargetMassList.AutoSize = true;
            this.addMS1TargetMassList.Checked = true;
            this.addMS1TargetMassList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addMS1TargetMassList.Location = new System.Drawing.Point(21, 53);
            this.addMS1TargetMassList.Name = "addMS1TargetMassList";
            this.addMS1TargetMassList.Size = new System.Drawing.Size(280, 17);
            this.addMS1TargetMassList.TabIndex = 8;
            this.addMS1TargetMassList.Text = "\"Targeted Mass Filter\" for Trigger Peptide before MS1";
            this.addMS1TargetMassList.UseVisualStyleBackColor = true;
            // 
            // addMS2IsolationOffset
            // 
            this.addMS2IsolationOffset.AutoSize = true;
            this.addMS2IsolationOffset.Checked = true;
            this.addMS2IsolationOffset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addMS2IsolationOffset.Location = new System.Drawing.Point(21, 111);
            this.addMS2IsolationOffset.Name = "addMS2IsolationOffset";
            this.addMS2IsolationOffset.Size = new System.Drawing.Size(219, 17);
            this.addMS2IsolationOffset.TabIndex = 10;
            this.addMS2IsolationOffset.Text = "\"Isolation Offset\" for Target Peptide MS2";
            this.addMS2IsolationOffset.UseVisualStyleBackColor = true;
            // 
            // addMS2TriggerMassList
            // 
            this.addMS2TriggerMassList.AutoSize = true;
            this.addMS2TriggerMassList.Checked = true;
            this.addMS2TriggerMassList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addMS2TriggerMassList.Location = new System.Drawing.Point(21, 81);
            this.addMS2TriggerMassList.Name = "addMS2TriggerMassList";
            this.addMS2TriggerMassList.Size = new System.Drawing.Size(267, 17);
            this.addMS2TriggerMassList.TabIndex = 9;
            this.addMS2TriggerMassList.Text = "\"Targeted Mass Trigger\" after Trigger Peptide MS2";
            this.addMS2TriggerMassList.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 24);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Template Method (.meth)";
            // 
            // templateBox
            // 
            this.templateBox.AllowDrop = true;
            this.templateBox.Location = new System.Drawing.Point(20, 38);
            this.templateBox.Margin = new System.Windows.Forms.Padding(1);
            this.templateBox.Name = "templateBox";
            this.templateBox.Size = new System.Drawing.Size(831, 20);
            this.templateBox.TabIndex = 65;
            this.templateBox.TextChanged += new System.EventHandler(this.templateBox_TextChanged);
            // 
            // createMethod
            // 
            this.createMethod.Location = new System.Drawing.Point(819, 304);
            this.createMethod.Name = "createMethod";
            this.createMethod.Size = new System.Drawing.Size(128, 23);
            this.createMethod.TabIndex = 63;
            this.createMethod.Text = "Create Method";
            this.createMethod.UseVisualStyleBackColor = true;
            this.createMethod.Click += new System.EventHandler(this.createMethod_Click_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.targetBoxBrowse);
            this.groupBox2.Controls.Add(this.targetTextBox);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(958, 82);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load Target Peptides";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(637, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Targets (.csv - Columns = \"Peptide,z,MS3 Target m/z,MS2 Trigger m/z\") - For m/z l" +
    "ists use semicolon to delimit (e.g. \"262.13;524.26\")";
            // 
            // targetBoxBrowse
            // 
            this.targetBoxBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.targetBoxBrowse.Location = new System.Drawing.Point(862, 42);
            this.targetBoxBrowse.Name = "targetBoxBrowse";
            this.targetBoxBrowse.Size = new System.Drawing.Size(85, 23);
            this.targetBoxBrowse.TabIndex = 75;
            this.targetBoxBrowse.Text = "Browse";
            this.targetBoxBrowse.UseVisualStyleBackColor = true;
            this.targetBoxBrowse.Click += new System.EventHandler(this.targetBoxBrowse_Click);
            // 
            // targetTextBox
            // 
            this.targetTextBox.AllowDrop = true;
            this.targetTextBox.Location = new System.Drawing.Point(20, 44);
            this.targetTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.targetTextBox.Name = "targetTextBox";
            this.targetTextBox.Size = new System.Drawing.Size(831, 20);
            this.targetTextBox.TabIndex = 59;
            this.targetTextBox.TextChanged += new System.EventHandler(this.targetTextBox_TextChanged);
            // 
            // analysisTab
            // 
            this.analysisTab.Controls.Add(this.sortTargetProteins);
            this.analysisTab.Controls.Add(this.proteinSearchBox);
            this.analysisTab.Controls.Add(this.label19);
            this.analysisTab.Controls.Add(this.sortTargetsPeptide);
            this.analysisTab.Controls.Add(this.displaySelectedLine);
            this.analysisTab.Controls.Add(this.displayTargetsWData);
            this.analysisTab.Controls.Add(this.displaySelected);
            this.analysisTab.Controls.Add(this.selectAll);
            this.analysisTab.Controls.Add(this.deselectAll);
            this.analysisTab.Controls.Add(this.exportSELs);
            this.analysisTab.Controls.Add(this.updateMethod);
            this.analysisTab.Controls.Add(this.targetSearchBox);
            this.analysisTab.Controls.Add(this.spectrumGraphControl2);
            this.analysisTab.Controls.Add(this.label5);
            this.analysisTab.Controls.Add(this.scanGridView);
            this.analysisTab.Controls.Add(this.spectrumGraphControl1);
            this.analysisTab.Controls.Add(this.ms1GraphControl);
            this.analysisTab.Controls.Add(this.label2);
            this.analysisTab.Controls.Add(this.sortTargetsMZ);
            this.analysisTab.Controls.Add(this.sortTargetsRT);
            this.analysisTab.Controls.Add(this.targetGridView);
            this.analysisTab.Location = new System.Drawing.Point(4, 22);
            this.analysisTab.Name = "analysisTab";
            this.analysisTab.Padding = new System.Windows.Forms.Padding(3);
            this.analysisTab.Size = new System.Drawing.Size(1449, 628);
            this.analysisTab.TabIndex = 0;
            this.analysisTab.Text = "Data Analysis";
            this.analysisTab.UseVisualStyleBackColor = true;
            this.analysisTab.Click += new System.EventHandler(this.analysisTab_Click);
            // 
            // sortTargetProteins
            // 
            this.sortTargetProteins.Location = new System.Drawing.Point(7, 560);
            this.sortTargetProteins.Name = "sortTargetProteins";
            this.sortTargetProteins.Size = new System.Drawing.Size(75, 23);
            this.sortTargetProteins.TabIndex = 70;
            this.sortTargetProteins.Text = "Sort by Pep";
            this.sortTargetProteins.UseVisualStyleBackColor = true;
            this.sortTargetProteins.Click += new System.EventHandler(this.sortTargetProteins_Click);
            // 
            // proteinSearchBox
            // 
            this.proteinSearchBox.AllowDrop = true;
            this.proteinSearchBox.Location = new System.Drawing.Point(82, 9);
            this.proteinSearchBox.Margin = new System.Windows.Forms.Padding(1);
            this.proteinSearchBox.Name = "proteinSearchBox";
            this.proteinSearchBox.Size = new System.Drawing.Size(148, 20);
            this.proteinSearchBox.TabIndex = 69;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(240, 12);
            this.label19.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(85, 13);
            this.label19.TabIndex = 68;
            this.label19.Text = "Search Peptides";
            // 
            // sortTargetsPeptide
            // 
            this.sortTargetsPeptide.Location = new System.Drawing.Point(88, 560);
            this.sortTargetsPeptide.Name = "sortTargetsPeptide";
            this.sortTargetsPeptide.Size = new System.Drawing.Size(75, 23);
            this.sortTargetsPeptide.TabIndex = 67;
            this.sortTargetsPeptide.Text = "Sort by Pep";
            this.sortTargetsPeptide.UseVisualStyleBackColor = true;
            this.sortTargetsPeptide.Click += new System.EventHandler(this.sortTargetsPeptide_Click);
            // 
            // displaySelectedLine
            // 
            this.displaySelectedLine.AutoSize = true;
            this.displaySelectedLine.Location = new System.Drawing.Point(243, 539);
            this.displaySelectedLine.Name = "displaySelectedLine";
            this.displaySelectedLine.Size = new System.Drawing.Size(166, 17);
            this.displaySelectedLine.TabIndex = 66;
            this.displaySelectedLine.Text = "Display only Selected Targets";
            this.displaySelectedLine.UseVisualStyleBackColor = true;
            this.displaySelectedLine.CheckedChanged += new System.EventHandler(this.displaySelectedLine_CheckedChanged);
            // 
            // displayTargetsWData
            // 
            this.displayTargetsWData.AutoSize = true;
            this.displayTargetsWData.Location = new System.Drawing.Point(243, 562);
            this.displayTargetsWData.Name = "displayTargetsWData";
            this.displayTargetsWData.Size = new System.Drawing.Size(169, 17);
            this.displayTargetsWData.TabIndex = 65;
            this.displayTargetsWData.Text = "Display only Targets with Data";
            this.displayTargetsWData.UseVisualStyleBackColor = true;
            this.displayTargetsWData.CheckedChanged += new System.EventHandler(this.displayTargetsWData_CheckedChanged);
            // 
            // displaySelected
            // 
            this.displaySelected.AutoSize = true;
            this.displaySelected.Location = new System.Drawing.Point(737, 564);
            this.displaySelected.Name = "displaySelected";
            this.displaySelected.Size = new System.Drawing.Size(160, 17);
            this.displaySelected.TabIndex = 60;
            this.displaySelected.Text = "Display only Selected Scans";
            this.displaySelected.UseVisualStyleBackColor = true;
            this.displaySelected.CheckedChanged += new System.EventHandler(this.displaySelected_CheckedChanged);
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(606, 560);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(118, 23);
            this.selectAll.TabIndex = 63;
            this.selectAll.Text = "Select All Scans";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // deselectAll
            // 
            this.deselectAll.Location = new System.Drawing.Point(482, 560);
            this.deselectAll.Name = "deselectAll";
            this.deselectAll.Size = new System.Drawing.Size(118, 23);
            this.deselectAll.TabIndex = 62;
            this.deselectAll.Text = "Deselect All Scans";
            this.deselectAll.UseVisualStyleBackColor = true;
            this.deselectAll.Click += new System.EventHandler(this.deselectAll_Click);
            // 
            // exportSELs
            // 
            this.exportSELs.Location = new System.Drawing.Point(1256, 560);
            this.exportSELs.Name = "exportSELs";
            this.exportSELs.Size = new System.Drawing.Size(180, 23);
            this.exportSELs.TabIndex = 60;
            this.exportSELs.Text = "Export Data for Selected Scans";
            this.exportSELs.UseVisualStyleBackColor = true;
            this.exportSELs.Click += new System.EventHandler(this.exportSELs_Click);
            // 
            // updateMethod
            // 
            this.updateMethod.Location = new System.Drawing.Point(1053, 560);
            this.updateMethod.Name = "updateMethod";
            this.updateMethod.Size = new System.Drawing.Size(197, 23);
            this.updateMethod.TabIndex = 61;
            this.updateMethod.Text = "Update Method w/ Selected Scans";
            this.updateMethod.UseVisualStyleBackColor = true;
            this.updateMethod.Click += new System.EventHandler(this.updateMethod_Click);
            // 
            // targetSearchBox
            // 
            this.targetSearchBox.AllowDrop = true;
            this.targetSearchBox.Location = new System.Drawing.Point(327, 9);
            this.targetSearchBox.Margin = new System.Windows.Forms.Padding(1);
            this.targetSearchBox.Name = "targetSearchBox";
            this.targetSearchBox.Size = new System.Drawing.Size(148, 20);
            this.targetSearchBox.TabIndex = 59;
            this.targetSearchBox.TextChanged += new System.EventHandler(this.targetSearchBox_TextChanged);
            // 
            // paramTab
            // 
            this.paramTab.Controls.Add(this.groupBox6);
            this.paramTab.Controls.Add(this.button1);
            this.paramTab.Controls.Add(this.printDebug);
            this.paramTab.Controls.Add(this.modSaveLoadGroup);
            this.paramTab.Controls.Add(this.exportTargetList);
            this.paramTab.Location = new System.Drawing.Point(4, 22);
            this.paramTab.Name = "paramTab";
            this.paramTab.Padding = new System.Windows.Forms.Padding(3);
            this.paramTab.Size = new System.Drawing.Size(1449, 628);
            this.paramTab.TabIndex = 1;
            this.paramTab.Text = "Advanced Parameters";
            this.paramTab.UseVisualStyleBackColor = true;
            this.paramTab.Click += new System.EventHandler(this.paramTab_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.label25);
            this.groupBox6.Controls.Add(this.xmlTargetPepBrowse);
            this.groupBox6.Controls.Add(this.xmlTemplateBrowse);
            this.groupBox6.Controls.Add(this.xmlTemplateTB);
            this.groupBox6.Controls.Add(this.xmlTargetPepTB);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Controls.Add(this.xmlBrowse);
            this.groupBox6.Controls.Add(this.xmlTextBox);
            this.groupBox6.Controls.Add(this.methodChangerAlone);
            this.groupBox6.Location = new System.Drawing.Point(695, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(748, 284);
            this.groupBox6.TabIndex = 70;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Modify Method with XML";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(6, 97);
            this.label24.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(125, 13);
            this.label24.TabIndex = 85;
            this.label24.Text = "Template Method (.meth)";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(6, 31);
            this.label25.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(637, 13);
            this.label25.TabIndex = 84;
            this.label25.Text = "Targets (.csv - Columns = \"Peptide,z,MS3 Target m/z,MS2 Trigger m/z\") - For m/z l" +
    "ists use semicolon to delimit (e.g. \"262.13;524.26\")";
            // 
            // xmlTargetPepBrowse
            // 
            this.xmlTargetPepBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xmlTargetPepBrowse.Location = new System.Drawing.Point(649, 49);
            this.xmlTargetPepBrowse.Name = "xmlTargetPepBrowse";
            this.xmlTargetPepBrowse.Size = new System.Drawing.Size(85, 23);
            this.xmlTargetPepBrowse.TabIndex = 83;
            this.xmlTargetPepBrowse.Text = "Browse";
            this.xmlTargetPepBrowse.UseVisualStyleBackColor = true;
            this.xmlTargetPepBrowse.Click += new System.EventHandler(this.button2_Click);
            // 
            // xmlTemplateBrowse
            // 
            this.xmlTemplateBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xmlTemplateBrowse.Location = new System.Drawing.Point(649, 111);
            this.xmlTemplateBrowse.Name = "xmlTemplateBrowse";
            this.xmlTemplateBrowse.Size = new System.Drawing.Size(85, 23);
            this.xmlTemplateBrowse.TabIndex = 82;
            this.xmlTemplateBrowse.Text = "Browse";
            this.xmlTemplateBrowse.UseVisualStyleBackColor = true;
            this.xmlTemplateBrowse.Click += new System.EventHandler(this.button3_Click);
            // 
            // xmlTemplateTB
            // 
            this.xmlTemplateTB.Location = new System.Drawing.Point(6, 113);
            this.xmlTemplateTB.Name = "xmlTemplateTB";
            this.xmlTemplateTB.Size = new System.Drawing.Size(632, 20);
            this.xmlTemplateTB.TabIndex = 79;
            // 
            // xmlTargetPepTB
            // 
            this.xmlTargetPepTB.Location = new System.Drawing.Point(6, 51);
            this.xmlTargetPepTB.Name = "xmlTargetPepTB";
            this.xmlTargetPepTB.Size = new System.Drawing.Size(632, 20);
            this.xmlTargetPepTB.TabIndex = 78;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 162);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(174, 13);
            this.label23.TabIndex = 77;
            this.label23.Text = "XML Method Modification File (.xml)";
            // 
            // xmlBrowse
            // 
            this.xmlBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xmlBrowse.Location = new System.Drawing.Point(649, 179);
            this.xmlBrowse.Name = "xmlBrowse";
            this.xmlBrowse.Size = new System.Drawing.Size(85, 23);
            this.xmlBrowse.TabIndex = 76;
            this.xmlBrowse.Text = "Browse";
            this.xmlBrowse.UseVisualStyleBackColor = true;
            this.xmlBrowse.Click += new System.EventHandler(this.xmlBrowse_Click);
            // 
            // xmlTextBox
            // 
            this.xmlTextBox.Location = new System.Drawing.Point(6, 179);
            this.xmlTextBox.Name = "xmlTextBox";
            this.xmlTextBox.Size = new System.Drawing.Size(632, 20);
            this.xmlTextBox.TabIndex = 1;
            // 
            // methodChangerAlone
            // 
            this.methodChangerAlone.Location = new System.Drawing.Point(591, 246);
            this.methodChangerAlone.Name = "methodChangerAlone";
            this.methodChangerAlone.Size = new System.Drawing.Size(143, 23);
            this.methodChangerAlone.TabIndex = 0;
            this.methodChangerAlone.Text = "Create Method";
            this.methodChangerAlone.UseVisualStyleBackColor = true;
            this.methodChangerAlone.Click += new System.EventHandler(this.methodChangerAlone_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 530);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 23);
            this.button1.TabIndex = 69;
            this.button1.Text = "Trigger Peptide Inclusion List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // printDebug
            // 
            this.printDebug.AutoSize = true;
            this.printDebug.Location = new System.Drawing.Point(6, 605);
            this.printDebug.Name = "printDebug";
            this.printDebug.Size = new System.Drawing.Size(153, 17);
            this.printDebug.TabIndex = 67;
            this.printDebug.Text = "Print Debugging Messages";
            this.printDebug.UseVisualStyleBackColor = true;
            // 
            // modSaveLoadGroup
            // 
            this.modSaveLoadGroup.Controls.Add(this.loadUserModFile);
            this.modSaveLoadGroup.Controls.Add(this.modFileListBox);
            this.modSaveLoadGroup.Controls.Add(this.modFileName);
            this.modSaveLoadGroup.Controls.Add(this.exportModificationTable);
            this.modSaveLoadGroup.Controls.Add(this.label13);
            this.modSaveLoadGroup.Location = new System.Drawing.Point(6, 6);
            this.modSaveLoadGroup.Name = "modSaveLoadGroup";
            this.modSaveLoadGroup.Size = new System.Drawing.Size(683, 284);
            this.modSaveLoadGroup.TabIndex = 66;
            this.modSaveLoadGroup.TabStop = false;
            this.modSaveLoadGroup.Text = "Save and Load Modifications";
            // 
            // loadUserModFile
            // 
            this.loadUserModFile.Location = new System.Drawing.Point(525, 246);
            this.loadUserModFile.Name = "loadUserModFile";
            this.loadUserModFile.Size = new System.Drawing.Size(152, 23);
            this.loadUserModFile.TabIndex = 70;
            this.loadUserModFile.Text = "Load User Modifications";
            this.loadUserModFile.UseVisualStyleBackColor = true;
            this.loadUserModFile.Click += new System.EventHandler(this.loadUserModFile_Click);
            // 
            // modFileListBox
            // 
            this.modFileListBox.FormattingEnabled = true;
            this.modFileListBox.Location = new System.Drawing.Point(12, 67);
            this.modFileListBox.Name = "modFileListBox";
            this.modFileListBox.Size = new System.Drawing.Size(665, 173);
            this.modFileListBox.TabIndex = 69;
            // 
            // modFileName
            // 
            this.modFileName.Location = new System.Drawing.Point(12, 37);
            this.modFileName.Name = "modFileName";
            this.modFileName.Size = new System.Drawing.Size(507, 20);
            this.modFileName.TabIndex = 68;
            this.modFileName.Text = "UserModifications";
            // 
            // exportModificationTable
            // 
            this.exportModificationTable.Location = new System.Drawing.Point(525, 35);
            this.exportModificationTable.Name = "exportModificationTable";
            this.exportModificationTable.Size = new System.Drawing.Size(152, 23);
            this.exportModificationTable.TabIndex = 67;
            this.exportModificationTable.Text = "Export Current Modifications";
            this.exportModificationTable.UseVisualStyleBackColor = true;
            this.exportModificationTable.Click += new System.EventHandler(this.exportModificationTable_Click_1);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 13);
            this.label13.TabIndex = 66;
            this.label13.Text = "Modification File Name";
            // 
            // exportTargetList
            // 
            this.exportTargetList.Location = new System.Drawing.Point(18, 559);
            this.exportTargetList.Name = "exportTargetList";
            this.exportTargetList.Size = new System.Drawing.Size(180, 23);
            this.exportTargetList.TabIndex = 61;
            this.exportTargetList.Text = "Export Target List";
            this.exportTargetList.UseVisualStyleBackColor = true;
            this.exportTargetList.Click += new System.EventHandler(this.exportTargetList_Click);
            // 
            // targetOnlyFDB
            // 
            this.targetOnlyFDB.FileName = "openFileDialog1";
            // 
            // templateMethodFDB
            // 
            this.templateMethodFDB.FileName = "openFileDialog1";
            // 
            // rawFileFDB
            // 
            this.rawFileFDB.FileName = "openFileDialog1";
            // 
            // primingRawFDB
            // 
            this.primingRawFDB.FileName = "openFileDialog1";
            // 
            // primingRawOFDia
            // 
            this.primingRawOFDia.FileName = "openFileDialog1";
            this.primingRawOFDia.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // xmlOFDB
            // 
            this.xmlOFDB.FileName = "xmlFDB";
            // 
            // TomahaqCompanionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1476, 751);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.logBox);
            this.Name = "TomahaqCompanionForm";
            this.Text = "TomahaqCompanion";
            ((System.ComponentModel.ISupportInitialize)(this.targetGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanGridView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.methodTab.ResumeLayout(false);
            this.methodTab.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userModGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minNumTriggerIonsNB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetIonMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerIonMax)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.analysisTab.ResumeLayout(false);
            this.analysisTab.PerformLayout();
            this.paramTab.ResumeLayout(false);
            this.paramTab.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.modSaveLoadGroup.ResumeLayout(false);
            this.modSaveLoadGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView targetGridView;
        private System.Windows.Forms.Button sortTargetsMZ;
        private System.Windows.Forms.Button sortTargetsRT;
        private System.Windows.Forms.Label label2;
        private ZedGraph.ZedGraphControl ms1GraphControl;
        private ZedGraph.ZedGraphControl spectrumGraphControl1;
        private ZedGraph.ZedGraphControl spectrumGraphControl2;
        private System.Windows.Forms.DataGridView scanGridView;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage analysisTab;
        private System.Windows.Forms.TabPage paramTab;
        private System.Windows.Forms.OpenFileDialog targetOnlyFDB;
        private System.Windows.Forms.OpenFileDialog templateMethodFDB;
        private System.Windows.Forms.TextBox targetSearchBox;
        private System.Windows.Forms.TabPage methodTab;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button primingTargetList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox templateBox;
        private System.Windows.Forms.Button createMethod;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox targetTextBox;
        private System.Windows.Forms.Button analyzeRun;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox rawFileBox;
        private System.Windows.Forms.Button addUserModifications;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView userModGridView;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox addMS3TargetMassList;
        private System.Windows.Forms.CheckBox addMS1TargetMassList;
        private System.Windows.Forms.CheckBox addMS2IsolationOffset;
        private System.Windows.Forms.CheckBox addMS2TriggerMassList;
        private System.Windows.Forms.Button rawFileBrowser;
        private System.Windows.Forms.Button targetBoxBrowse;
        private System.Windows.Forms.Button templateMethodBrowse;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.OpenFileDialog rawFileFDB;
        private System.Windows.Forms.Button primingRunBrowse;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.TextBox primingRawBox;
        private System.Windows.Forms.OpenFileDialog primingRawFDB;
        private System.Windows.Forms.OpenFileDialog primingRawOFDia;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewCheckBoxColumn triggerCB;
        private System.Windows.Forms.DataGridViewCheckBoxColumn targetCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn massBox;
        private System.Windows.Forms.DataGridViewComboBoxColumn typeCombo;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbolBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn sitesBox;
        private System.Windows.Forms.Button exportSELs;
        private System.Windows.Forms.Button updateMethod;
        private System.Windows.Forms.Button deselectAll;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox rtWindowTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox fragTolBox;
        private System.Windows.Forms.RadioButton ppmRB;
        private System.Windows.Forms.RadioButton daRB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox methodLengthBox;
        private System.Windows.Forms.TextBox xmlTextBox;
        private System.Windows.Forms.Button methodChangerAlone;
        private System.Windows.Forms.CheckBox displaySelected;
        private System.Windows.Forms.CheckBox displayTargetsWData;
        private System.Windows.Forms.Button exportTargetList;
        private System.Windows.Forms.GroupBox modSaveLoadGroup;
        private System.Windows.Forms.Button loadUserModFile;
        private System.Windows.Forms.ListBox modFileListBox;
        private System.Windows.Forms.TextBox modFileName;
        private System.Windows.Forms.Button exportModificationTable;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox displaySelectedLine;
        private System.Windows.Forms.Button sortTargetsPeptide;
        private System.Windows.Forms.DataGridView modGridView;
        private System.Windows.Forms.CheckBox tomaAPIRawFile;
        private System.Windows.Forms.CheckBox ms2DataAnalysis;
        private System.Windows.Forms.CheckBox chooseBestCharge;
        private System.Windows.Forms.CheckBox spsIonsAbovePrec;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox spsMaxMZ;
        private System.Windows.Forms.TextBox spsMinMZ;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox precExHigh;
        private System.Windows.Forms.TextBox precExLow;
        private System.Windows.Forms.CheckBox printDebug;
        private System.Windows.Forms.Button sortTargetProteins;
        private System.Windows.Forms.TextBox proteinSearchBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox groupIDCheckBox;
        private System.Windows.Forms.CheckBox eclipseCheckbox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown targetIonMax;
        private System.Windows.Forms.NumericUpDown triggerIonMax;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown minNumTriggerIonsNB;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox triggerIonMZMaxTB;
        private System.Windows.Forms.TextBox triggerIonMZMinTB;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button xmlBrowse;
        private System.Windows.Forms.OpenFileDialog xmlOFDB;
        private System.Windows.Forms.TextBox xmlTemplateTB;
        private System.Windows.Forms.TextBox xmlTargetPepTB;
        private System.Windows.Forms.Button xmlTargetPepBrowse;
        private System.Windows.Forms.Button xmlTemplateBrowse;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
    }
}

