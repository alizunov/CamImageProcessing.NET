namespace CamImageProcessing.NET
{
    partial class Graphics1
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
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.ClearPane_button = new System.Windows.Forms.Button();
            this.CurveNumber_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RemoveCurve_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CurveNumber_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 12);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(646, 515);
            this.zedGraphControl1.TabIndex = 0;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            this.zedGraphControl1.Load += new System.EventHandler(this.zedGraphControl1_Load_1);
            // 
            // ClearPane_button
            // 
            this.ClearPane_button.Location = new System.Drawing.Point(156, 536);
            this.ClearPane_button.Name = "ClearPane_button";
            this.ClearPane_button.Size = new System.Drawing.Size(75, 23);
            this.ClearPane_button.TabIndex = 1;
            this.ClearPane_button.Text = "Clear pane";
            this.ClearPane_button.UseVisualStyleBackColor = true;
            this.ClearPane_button.Click += new System.EventHandler(this.ClearPane_button_Click);
            // 
            // CurveNumber_numericUpDown
            // 
            this.CurveNumber_numericUpDown.Location = new System.Drawing.Point(12, 536);
            this.CurveNumber_numericUpDown.Name = "CurveNumber_numericUpDown";
            this.CurveNumber_numericUpDown.Size = new System.Drawing.Size(57, 20);
            this.CurveNumber_numericUpDown.TabIndex = 2;
            this.CurveNumber_numericUpDown.ValueChanged += new System.EventHandler(this.CurveNumber_numericUpDown_ValueChanged);
            // 
            // RemoveCurve_button
            // 
            this.RemoveCurve_button.Location = new System.Drawing.Point(75, 536);
            this.RemoveCurve_button.Name = "RemoveCurve_button";
            this.RemoveCurve_button.Size = new System.Drawing.Size(75, 23);
            this.RemoveCurve_button.TabIndex = 3;
            this.RemoveCurve_button.Text = "Remove";
            this.RemoveCurve_button.UseVisualStyleBackColor = true;
            this.RemoveCurve_button.Click += new System.EventHandler(this.RemoveCurve_button_Click);
            // 
            // Graphics1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 592);
            this.Controls.Add(this.RemoveCurve_button);
            this.Controls.Add(this.CurveNumber_numericUpDown);
            this.Controls.Add(this.ClearPane_button);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "Graphics1";
            this.Text = "Graphics1";
            this.Load += new System.EventHandler(this.Graphics1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CurveNumber_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Button ClearPane_button;
        private System.Windows.Forms.NumericUpDown CurveNumber_numericUpDown;
        private System.Windows.Forms.Button RemoveCurve_button;
    }
}