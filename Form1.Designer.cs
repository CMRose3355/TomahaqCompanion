namespace TomahaqCompanion
{
    partial class Form1
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
            this.editMethod = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // editMethod
            // 
            this.editMethod.Location = new System.Drawing.Point(111, 223);
            this.editMethod.Name = "editMethod";
            this.editMethod.Size = new System.Drawing.Size(75, 23);
            this.editMethod.TabIndex = 0;
            this.editMethod.Text = "EditMethod";
            this.editMethod.UseVisualStyleBackColor = true;
            this.editMethod.Click += new System.EventHandler(this.editMethod_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.editMethod);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button editMethod;
    }
}

