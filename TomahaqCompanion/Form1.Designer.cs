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
            this.targetTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modGridView = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.sortTargetsMZ = new System.Windows.Forms.Button();
            this.sortTargetsRT = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rawFileBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ms1GraphControl = new ZedGraph.ZedGraphControl();
            this.spectrumGraphControl1 = new ZedGraph.ZedGraphControl();
            this.spectrumGraphControl2 = new ZedGraph.ZedGraphControl();
            this.createMethod = new System.Windows.Forms.Button();
            this.analyzeRun = new System.Windows.Forms.Button();
            this.scanGridView = new System.Windows.Forms.DataGridView();
            this.templateBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rawPrimingRun = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.primingTargetList = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.analysisTab = new System.Windows.Forms.TabPage();
            this.paramTab = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.targetGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanGridView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.analysisTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // targetGridView
            // 
            this.targetGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.targetGridView.Location = new System.Drawing.Point(3, 63);
            this.targetGridView.Name = "targetGridView";
            this.targetGridView.Size = new System.Drawing.Size(507, 246);
            this.targetGridView.TabIndex = 0;
            this.targetGridView.SelectionChanged += new System.EventHandler(this.targetGridView_SelectionChanged);
            // 
            // targetTextBox
            // 
            this.targetTextBox.AllowDrop = true;
            this.targetTextBox.Location = new System.Drawing.Point(3, 20);
            this.targetTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.targetTextBox.Name = "targetTextBox";
            this.targetTextBox.Size = new System.Drawing.Size(314, 20);
            this.targetTextBox.TabIndex = 4;
            this.targetTextBox.Text = "C:\\Users\\Orbitrap_Lumos\\Desktop\\XmlMethodModifications\\Examples\\Fusion\\SPS\\HumanP" +
    "eptides13.csv";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Targets (.csv - Columns = \"Peptide,z\")";
            // 
            // modGridView
            // 
            this.modGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modGridView.Location = new System.Drawing.Point(3, 354);
            this.modGridView.Name = "modGridView";
            this.modGridView.Size = new System.Drawing.Size(507, 173);
            this.modGridView.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 337);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Modifications";
            // 
            // sortTargetsMZ
            // 
            this.sortTargetsMZ.Location = new System.Drawing.Point(357, 319);
            this.sortTargetsMZ.Name = "sortTargetsMZ";
            this.sortTargetsMZ.Size = new System.Drawing.Size(75, 23);
            this.sortTargetsMZ.TabIndex = 42;
            this.sortTargetsMZ.Text = "Sort by MZ";
            this.sortTargetsMZ.UseVisualStyleBackColor = true;
            this.sortTargetsMZ.Click += new System.EventHandler(this.sortTargetsMZ_Click);
            // 
            // sortTargetsRT
            // 
            this.sortTargetsRT.Location = new System.Drawing.Point(436, 319);
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
            this.label2.Location = new System.Drawing.Point(0, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Targets";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // rawFileBox
            // 
            this.rawFileBox.AllowDrop = true;
            this.rawFileBox.Location = new System.Drawing.Point(463, 20);
            this.rawFileBox.Margin = new System.Windows.Forms.Padding(1);
            this.rawFileBox.Name = "rawFileBox";
            this.rawFileBox.Size = new System.Drawing.Size(293, 20);
            this.rawFileBox.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(463, 7);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "RawFile";
            // 
            // ms1GraphControl
            // 
            this.ms1GraphControl.Location = new System.Drawing.Point(517, 63);
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
            this.spectrumGraphControl1.Location = new System.Drawing.Point(518, 264);
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
            this.spectrumGraphControl2.Location = new System.Drawing.Point(1007, 264);
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
            // createMethod
            // 
            this.createMethod.Location = new System.Drawing.Point(1366, 19);
            this.createMethod.Name = "createMethod";
            this.createMethod.Size = new System.Drawing.Size(120, 23);
            this.createMethod.TabIndex = 50;
            this.createMethod.Text = "Create Method";
            this.createMethod.UseVisualStyleBackColor = true;
            this.createMethod.Click += new System.EventHandler(this.createMethod_Click);
            // 
            // analyzeRun
            // 
            this.analyzeRun.Location = new System.Drawing.Point(1240, 19);
            this.analyzeRun.Name = "analyzeRun";
            this.analyzeRun.Size = new System.Drawing.Size(120, 23);
            this.analyzeRun.TabIndex = 51;
            this.analyzeRun.Text = "Analyze Run";
            this.analyzeRun.UseVisualStyleBackColor = true;
            this.analyzeRun.Click += new System.EventHandler(this.analyzeRun_Click);
            // 
            // scanGridView
            // 
            this.scanGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scanGridView.Location = new System.Drawing.Point(518, 478);
            this.scanGridView.Name = "scanGridView";
            this.scanGridView.Size = new System.Drawing.Size(963, 119);
            this.scanGridView.TabIndex = 52;
            this.scanGridView.SelectionChanged += new System.EventHandler(this.scanGridView_SelectionChanged);
            // 
            // templateBox
            // 
            this.templateBox.AllowDrop = true;
            this.templateBox.Location = new System.Drawing.Point(896, 20);
            this.templateBox.Margin = new System.Windows.Forms.Padding(1);
            this.templateBox.Name = "templateBox";
            this.templateBox.Size = new System.Drawing.Size(340, 20);
            this.templateBox.TabIndex = 53;
            this.templateBox.Text = "C:\\Users\\Orbitrap_Lumos\\Desktop\\XmlMethodModifications\\Examples\\Fusion\\SPS\\Templa" +
    "te.meth";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(893, 7);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Template Method";
            // 
            // rawPrimingRun
            // 
            this.rawPrimingRun.AutoSize = true;
            this.rawPrimingRun.Location = new System.Drawing.Point(760, 23);
            this.rawPrimingRun.Name = "rawPrimingRun";
            this.rawPrimingRun.Size = new System.Drawing.Size(137, 17);
            this.rawPrimingRun.TabIndex = 55;
            this.rawPrimingRun.Text = "Raw File is Priming Run";
            this.rawPrimingRun.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(515, 461);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "MS/MS Events";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(3, 533);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(507, 64);
            this.logBox.TabIndex = 57;
            this.logBox.Text = "";
            // 
            // primingTargetList
            // 
            this.primingTargetList.Location = new System.Drawing.Point(324, 18);
            this.primingTargetList.Name = "primingTargetList";
            this.primingTargetList.Size = new System.Drawing.Size(132, 23);
            this.primingTargetList.TabIndex = 58;
            this.primingTargetList.Text = "Print Priming Target List";
            this.primingTargetList.UseVisualStyleBackColor = true;
            this.primingTargetList.Click += new System.EventHandler(this.primingTargetList_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.analysisTab);
            this.tabControl.Controls.Add(this.paramTab);
            this.tabControl.Location = new System.Drawing.Point(2, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1498, 624);
            this.tabControl.TabIndex = 59;
            // 
            // analysisTab
            // 
            this.analysisTab.Controls.Add(this.targetTextBox);
            this.analysisTab.Controls.Add(this.spectrumGraphControl2);
            this.analysisTab.Controls.Add(this.label5);
            this.analysisTab.Controls.Add(this.logBox);
            this.analysisTab.Controls.Add(this.scanGridView);
            this.analysisTab.Controls.Add(this.primingTargetList);
            this.analysisTab.Controls.Add(this.label1);
            this.analysisTab.Controls.Add(this.spectrumGraphControl1);
            this.analysisTab.Controls.Add(this.label4);
            this.analysisTab.Controls.Add(this.ms1GraphControl);
            this.analysisTab.Controls.Add(this.rawPrimingRun);
            this.analysisTab.Controls.Add(this.label2);
            this.analysisTab.Controls.Add(this.label6);
            this.analysisTab.Controls.Add(this.templateBox);
            this.analysisTab.Controls.Add(this.label3);
            this.analysisTab.Controls.Add(this.analyzeRun);
            this.analysisTab.Controls.Add(this.sortTargetsMZ);
            this.analysisTab.Controls.Add(this.rawFileBox);
            this.analysisTab.Controls.Add(this.sortTargetsRT);
            this.analysisTab.Controls.Add(this.createMethod);
            this.analysisTab.Controls.Add(this.targetGridView);
            this.analysisTab.Controls.Add(this.modGridView);
            this.analysisTab.Location = new System.Drawing.Point(4, 22);
            this.analysisTab.Name = "analysisTab";
            this.analysisTab.Padding = new System.Windows.Forms.Padding(3);
            this.analysisTab.Size = new System.Drawing.Size(1490, 598);
            this.analysisTab.TabIndex = 0;
            this.analysisTab.Text = "Method Construction and Data Analysis";
            this.analysisTab.UseVisualStyleBackColor = true;
            this.analysisTab.Click += new System.EventHandler(this.analysisTab_Click);
            // 
            // paramTab
            // 
            this.paramTab.Location = new System.Drawing.Point(4, 22);
            this.paramTab.Name = "paramTab";
            this.paramTab.Padding = new System.Windows.Forms.Padding(3);
            this.paramTab.Size = new System.Drawing.Size(1490, 598);
            this.paramTab.TabIndex = 1;
            this.paramTab.Text = "Paramaters";
            this.paramTab.UseVisualStyleBackColor = true;
            // 
            // TomahaqCompanionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1502, 626);
            this.Controls.Add(this.tabControl);
            this.Name = "TomahaqCompanionForm";
            this.Text = "TomahaqCompanion";
            ((System.ComponentModel.ISupportInitialize)(this.targetGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scanGridView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.analysisTab.ResumeLayout(false);
            this.analysisTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView targetGridView;
        private System.Windows.Forms.TextBox targetTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView modGridView;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button sortTargetsMZ;
        private System.Windows.Forms.Button sortTargetsRT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox rawFileBox;
        private System.Windows.Forms.Label label3;
        private ZedGraph.ZedGraphControl ms1GraphControl;
        private ZedGraph.ZedGraphControl spectrumGraphControl1;
        private ZedGraph.ZedGraphControl spectrumGraphControl2;
        private System.Windows.Forms.Button createMethod;
        private System.Windows.Forms.Button analyzeRun;
        private System.Windows.Forms.DataGridView scanGridView;
        private System.Windows.Forms.TextBox templateBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox rawPrimingRun;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.Button primingTargetList;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage analysisTab;
        private System.Windows.Forms.TabPage paramTab;
    }
}

