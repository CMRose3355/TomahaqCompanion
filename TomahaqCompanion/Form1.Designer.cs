namespace TomahaqCompanion
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
            this.analysisTab = new System.Windows.Forms.TabPage();
            this.paramTab = new System.Windows.Forms.TabPage();
            this.targetOnlyFDB = new System.Windows.Forms.OpenFileDialog();
            this.templateMethodFDB = new System.Windows.Forms.OpenFileDialog();
            this.methodTab = new System.Windows.Forms.TabPage();
            this.targetSearchBox = new System.Windows.Forms.TextBox();
            this.targetTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.templateBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.analyzeRun = new System.Windows.Forms.Button();
            this.rawFileBox = new System.Windows.Forms.TextBox();
            this.createMethod = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.primingTargetList = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.addMS3TargetMassList = new System.Windows.Forms.CheckBox();
            this.addMS1TargetMassList = new System.Windows.Forms.CheckBox();
            this.addMS2IsolationOffset = new System.Windows.Forms.CheckBox();
            this.addMS2TriggerMassList = new System.Windows.Forms.CheckBox();
            this.modGridView = new System.Windows.Forms.DataGridView();
            this.userMods = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.addUserModifications = new System.Windows.Forms.Button();
            this.targetBoxBrowse = new System.Windows.Forms.Button();
            this.rawFileBrowser = new System.Windows.Forms.Button();
            this.templateMethodBrowse = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rawFileFDB = new System.Windows.Forms.OpenFileDialog();
            this.label100 = new System.Windows.Forms.Label();
            this.primingRawBox = new System.Windows.Forms.TextBox();
            this.primingRunBrowse = new System.Windows.Forms.Button();
            this.primingRawFDB = new System.Windows.Forms.OpenFileDialog();
            this.primingRawOFDia = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.targetGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanGridView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.analysisTab.SuspendLayout();
            this.methodTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userMods)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // targetGridView
            // 
            this.targetGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.targetGridView.Location = new System.Drawing.Point(4, 33);
            this.targetGridView.Name = "targetGridView";
            this.targetGridView.Size = new System.Drawing.Size(507, 496);
            this.targetGridView.TabIndex = 0;
            this.targetGridView.SelectionChanged += new System.EventHandler(this.targetGridView_SelectionChanged);
            // 
            // sortTargetsMZ
            // 
            this.sortTargetsMZ.Location = new System.Drawing.Point(359, 4);
            this.sortTargetsMZ.Name = "sortTargetsMZ";
            this.sortTargetsMZ.Size = new System.Drawing.Size(75, 23);
            this.sortTargetsMZ.TabIndex = 42;
            this.sortTargetsMZ.Text = "Sort by MZ";
            this.sortTargetsMZ.UseVisualStyleBackColor = true;
            this.sortTargetsMZ.Click += new System.EventHandler(this.sortTargetsMZ_Click);
            // 
            // sortTargetsRT
            // 
            this.sortTargetsRT.Location = new System.Drawing.Point(438, 4);
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
            this.label2.Location = new System.Drawing.Point(0, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Search Targets";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ms1GraphControl
            // 
            this.ms1GraphControl.Location = new System.Drawing.Point(517, 4);
            this.ms1GraphControl.Margin = new System.Windows.Forms.Padding(1);
            this.ms1GraphControl.Name = "ms1GraphControl";
            this.ms1GraphControl.ScrollGrace = 0D;
            this.ms1GraphControl.ScrollMaxX = 0D;
            this.ms1GraphControl.ScrollMaxY = 0D;
            this.ms1GraphControl.ScrollMaxY2 = 0D;
            this.ms1GraphControl.ScrollMinX = 0D;
            this.ms1GraphControl.ScrollMinY = 0D;
            this.ms1GraphControl.ScrollMinY2 = 0D;
            this.ms1GraphControl.Size = new System.Drawing.Size(966, 193);
            this.ms1GraphControl.TabIndex = 46;
            this.ms1GraphControl.UseExtendedPrintDialog = true;
            // 
            // spectrumGraphControl1
            // 
            this.spectrumGraphControl1.Location = new System.Drawing.Point(518, 203);
            this.spectrumGraphControl1.Margin = new System.Windows.Forms.Padding(1);
            this.spectrumGraphControl1.Name = "spectrumGraphControl1";
            this.spectrumGraphControl1.ScrollGrace = 0D;
            this.spectrumGraphControl1.ScrollMaxX = 0D;
            this.spectrumGraphControl1.ScrollMaxY = 0D;
            this.spectrumGraphControl1.ScrollMaxY2 = 0D;
            this.spectrumGraphControl1.ScrollMinX = 0D;
            this.spectrumGraphControl1.ScrollMinY = 0D;
            this.spectrumGraphControl1.ScrollMinY2 = 0D;
            this.spectrumGraphControl1.Size = new System.Drawing.Size(482, 192);
            this.spectrumGraphControl1.TabIndex = 47;
            this.spectrumGraphControl1.UseExtendedPrintDialog = true;
            // 
            // spectrumGraphControl2
            // 
            this.spectrumGraphControl2.Location = new System.Drawing.Point(1007, 203);
            this.spectrumGraphControl2.Margin = new System.Windows.Forms.Padding(1);
            this.spectrumGraphControl2.Name = "spectrumGraphControl2";
            this.spectrumGraphControl2.ScrollGrace = 0D;
            this.spectrumGraphControl2.ScrollMaxX = 0D;
            this.spectrumGraphControl2.ScrollMaxY = 0D;
            this.spectrumGraphControl2.ScrollMaxY2 = 0D;
            this.spectrumGraphControl2.ScrollMinX = 0D;
            this.spectrumGraphControl2.ScrollMinY = 0D;
            this.spectrumGraphControl2.ScrollMinY2 = 0D;
            this.spectrumGraphControl2.Size = new System.Drawing.Size(476, 192);
            this.spectrumGraphControl2.TabIndex = 49;
            this.spectrumGraphControl2.UseExtendedPrintDialog = true;
            // 
            // scanGridView
            // 
            this.scanGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scanGridView.Location = new System.Drawing.Point(518, 417);
            this.scanGridView.Name = "scanGridView";
            this.scanGridView.Size = new System.Drawing.Size(963, 112);
            this.scanGridView.TabIndex = 52;
            this.scanGridView.SelectionChanged += new System.EventHandler(this.scanGridView_SelectionChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(515, 401);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "MS/MS Events";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(2, 569);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(1494, 92);
            this.logBox.TabIndex = 57;
            this.logBox.Text = "";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.methodTab);
            this.tabControl.Controls.Add(this.analysisTab);
            this.tabControl.Controls.Add(this.paramTab);
            this.tabControl.Location = new System.Drawing.Point(2, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1498, 561);
            this.tabControl.TabIndex = 59;
            // 
            // analysisTab
            // 
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
            this.analysisTab.Size = new System.Drawing.Size(1490, 535);
            this.analysisTab.TabIndex = 0;
            this.analysisTab.Text = "Data Analysis";
            this.analysisTab.UseVisualStyleBackColor = true;
            this.analysisTab.Click += new System.EventHandler(this.analysisTab_Click);
            // 
            // paramTab
            // 
            this.paramTab.Location = new System.Drawing.Point(4, 22);
            this.paramTab.Name = "paramTab";
            this.paramTab.Padding = new System.Windows.Forms.Padding(3);
            this.paramTab.Size = new System.Drawing.Size(1490, 535);
            this.paramTab.TabIndex = 1;
            this.paramTab.Text = "Parameters";
            this.paramTab.UseVisualStyleBackColor = true;
            // 
            // targetOnlyFDB
            // 
            this.targetOnlyFDB.FileName = "openFileDialog1";
            // 
            // templateMethodFDB
            // 
            this.templateMethodFDB.FileName = "openFileDialog1";
            // 
            // methodTab
            // 
            this.methodTab.Controls.Add(this.groupBox5);
            this.methodTab.Controls.Add(this.addUserModifications);
            this.methodTab.Controls.Add(this.label11);
            this.methodTab.Controls.Add(this.userMods);
            this.methodTab.Controls.Add(this.label6);
            this.methodTab.Controls.Add(this.modGridView);
            this.methodTab.Controls.Add(this.groupBox3);
            this.methodTab.Controls.Add(this.groupBox2);
            this.methodTab.Location = new System.Drawing.Point(4, 22);
            this.methodTab.Name = "methodTab";
            this.methodTab.Size = new System.Drawing.Size(1490, 535);
            this.methodTab.TabIndex = 3;
            this.methodTab.Text = "TOMAHAQ File Loader and Method Builder";
            this.methodTab.UseVisualStyleBackColor = true;
            // 
            // targetSearchBox
            // 
            this.targetSearchBox.AllowDrop = true;
            this.targetSearchBox.Location = new System.Drawing.Point(82, 6);
            this.targetSearchBox.Margin = new System.Windows.Forms.Padding(1);
            this.targetSearchBox.Name = "targetSearchBox";
            this.targetSearchBox.Size = new System.Drawing.Size(272, 20);
            this.targetSearchBox.TabIndex = 59;
            // 
            // targetTextBox
            // 
            this.targetTextBox.AllowDrop = true;
            this.targetTextBox.Location = new System.Drawing.Point(7, 44);
            this.targetTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.targetTextBox.Name = "targetTextBox";
            this.targetTextBox.Size = new System.Drawing.Size(844, 20);
            this.targetTextBox.TabIndex = 59;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Targets (.csv - Columns = \"Peptide,z,MS3 Target m/z,MS2 Trigger m/z\")";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 22);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Template Method (.meth)";
            // 
            // templateBox
            // 
            this.templateBox.AllowDrop = true;
            this.templateBox.Location = new System.Drawing.Point(7, 38);
            this.templateBox.Margin = new System.Windows.Forms.Padding(1);
            this.templateBox.Name = "templateBox";
            this.templateBox.Size = new System.Drawing.Size(844, 20);
            this.templateBox.TabIndex = 65;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "TOMAHAQ Raw File (.Raw)";
            // 
            // analyzeRun
            // 
            this.analyzeRun.Location = new System.Drawing.Point(819, 76);
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
            this.rawFileBox.Location = new System.Drawing.Point(9, 38);
            this.rawFileBox.Margin = new System.Windows.Forms.Padding(1);
            this.rawFileBox.Name = "rawFileBox";
            this.rawFileBox.Size = new System.Drawing.Size(839, 20);
            this.rawFileBox.TabIndex = 61;
            // 
            // createMethod
            // 
            this.createMethod.Location = new System.Drawing.Point(819, 252);
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
            this.groupBox2.Location = new System.Drawing.Point(10, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(958, 82);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load Target Peptides";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.primingTargetList);
            this.groupBox3.Controls.Add(this.primingRunBrowse);
            this.groupBox3.Controls.Add(this.label100);
            this.groupBox3.Controls.Add(this.primingRawBox);
            this.groupBox3.Controls.Add(this.templateMethodBrowse);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.templateBox);
            this.groupBox3.Controls.Add(this.createMethod);
            this.groupBox3.Location = new System.Drawing.Point(10, 114);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(958, 284);
            this.groupBox3.TabIndex = 70;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "TOMAHAQ Method Creation";
            // 
            // primingTargetList
            // 
            this.primingTargetList.Location = new System.Drawing.Point(685, 252);
            this.primingTargetList.Name = "primingTargetList";
            this.primingTargetList.Size = new System.Drawing.Size(128, 23);
            this.primingTargetList.TabIndex = 68;
            this.primingTargetList.Text = "Print Priming Target List";
            this.primingTargetList.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.addMS3TargetMassList);
            this.groupBox1.Controls.Add(this.addMS1TargetMassList);
            this.groupBox1.Controls.Add(this.addMS2IsolationOffset);
            this.groupBox1.Controls.Add(this.addMS2TriggerMassList);
            this.groupBox1.Location = new System.Drawing.Point(7, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 165);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Method Modifications";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 30);
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
            this.addMS3TargetMassList.Location = new System.Drawing.Point(21, 131);
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
            this.addMS2IsolationOffset.Location = new System.Drawing.Point(21, 105);
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
            this.addMS2TriggerMassList.Location = new System.Drawing.Point(21, 79);
            this.addMS2TriggerMassList.Name = "addMS2TriggerMassList";
            this.addMS2TriggerMassList.Size = new System.Drawing.Size(267, 17);
            this.addMS2TriggerMassList.TabIndex = 9;
            this.addMS2TriggerMassList.Text = "\"Targeted Mass Trigger\" after Trigger Peptide MS2";
            this.addMS2TriggerMassList.UseVisualStyleBackColor = true;
            // 
            // modGridView
            // 
            this.modGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modGridView.Location = new System.Drawing.Point(977, 21);
            this.modGridView.Name = "modGridView";
            this.modGridView.Size = new System.Drawing.Size(507, 346);
            this.modGridView.TabIndex = 71;
            // 
            // userMods
            // 
            this.userMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.userMods.Location = new System.Drawing.Point(977, 419);
            this.userMods.Name = "userMods";
            this.userMods.Size = new System.Drawing.Size(507, 110);
            this.userMods.TabIndex = 73;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(974, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 72;
            this.label6.Text = "Modifications";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(974, 395);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(116, 13);
            this.label11.TabIndex = 74;
            this.label11.Text = "Add User Modifications";
            // 
            // addUserModifications
            // 
            this.addUserModifications.Location = new System.Drawing.Point(1364, 390);
            this.addUserModifications.Name = "addUserModifications";
            this.addUserModifications.Size = new System.Drawing.Size(120, 23);
            this.addUserModifications.TabIndex = 65;
            this.addUserModifications.Text = "Add User Mods";
            this.addUserModifications.UseVisualStyleBackColor = true;
            // 
            // targetBoxBrowse
            // 
            this.targetBoxBrowse.Location = new System.Drawing.Point(862, 42);
            this.targetBoxBrowse.Name = "targetBoxBrowse";
            this.targetBoxBrowse.Size = new System.Drawing.Size(85, 23);
            this.targetBoxBrowse.TabIndex = 75;
            this.targetBoxBrowse.Text = "Browse";
            this.targetBoxBrowse.UseVisualStyleBackColor = true;
            this.targetBoxBrowse.Click += new System.EventHandler(this.targetBoxBrowse_Click);
            // 
            // rawFileBrowser
            // 
            this.rawFileBrowser.Location = new System.Drawing.Point(857, 35);
            this.rawFileBrowser.Name = "rawFileBrowser";
            this.rawFileBrowser.Size = new System.Drawing.Size(90, 23);
            this.rawFileBrowser.TabIndex = 76;
            this.rawFileBrowser.Text = "Browse";
            this.rawFileBrowser.UseVisualStyleBackColor = true;
            this.rawFileBrowser.Click += new System.EventHandler(this.rawFileBrowser_Click);
            // 
            // templateMethodBrowse
            // 
            this.templateMethodBrowse.Location = new System.Drawing.Point(862, 36);
            this.templateMethodBrowse.Name = "templateMethodBrowse";
            this.templateMethodBrowse.Size = new System.Drawing.Size(85, 23);
            this.templateMethodBrowse.TabIndex = 76;
            this.templateMethodBrowse.Text = "Browse";
            this.templateMethodBrowse.UseVisualStyleBackColor = true;
            this.templateMethodBrowse.Click += new System.EventHandler(this.templateMethodBrowse_Click_1);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rawFileBrowser);
            this.groupBox5.Controls.Add(this.analyzeRun);
            this.groupBox5.Controls.Add(this.rawFileBox);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(10, 419);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(958, 110);
            this.groupBox5.TabIndex = 78;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Analyze TOMAHAQ Run";
            // 
            // rawFileFDB
            // 
            this.rawFileFDB.FileName = "openFileDialog1";
            // 
            // label100
            // 
            this.label100.AutoSize = true;
            this.label100.Location = new System.Drawing.Point(4, 67);
            this.label100.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(192, 13);
            this.label100.TabIndex = 78;
            this.label100.Text = "Priming Run Raw File (.Raw) *Optional*";
            // 
            // primingRawBox
            // 
            this.primingRawBox.AllowDrop = true;
            this.primingRawBox.Location = new System.Drawing.Point(7, 82);
            this.primingRawBox.Margin = new System.Windows.Forms.Padding(1);
            this.primingRawBox.Name = "primingRawBox";
            this.primingRawBox.Size = new System.Drawing.Size(844, 20);
            this.primingRawBox.TabIndex = 77;
            // 
            // primingRunBrowse
            // 
            this.primingRunBrowse.Location = new System.Drawing.Point(862, 80);
            this.primingRunBrowse.Name = "primingRunBrowse";
            this.primingRunBrowse.Size = new System.Drawing.Size(85, 23);
            this.primingRunBrowse.TabIndex = 79;
            this.primingRunBrowse.Text = "Browse";
            this.primingRunBrowse.UseVisualStyleBackColor = true;
            this.primingRunBrowse.Click += new System.EventHandler(this.primingRunBrowse_Click);
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
            // TomahaqCompanionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1502, 666);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.logBox);
            this.Name = "TomahaqCompanionForm";
            this.Text = "TomahaqCompanion";
            ((System.ComponentModel.ISupportInitialize)(this.targetGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanGridView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.analysisTab.ResumeLayout(false);
            this.analysisTab.PerformLayout();
            this.methodTab.ResumeLayout(false);
            this.methodTab.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userMods)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
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
        private System.Windows.Forms.DataGridView userMods;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView modGridView;
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
    }
}

