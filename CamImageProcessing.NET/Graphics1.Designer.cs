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
            this.ProcessSlice_groupBox = new System.Windows.Forms.GroupBox();
            this.ApproxMethodOutside_comboBox = new System.Windows.Forms.ComboBox();
            this.Abel_button = new System.Windows.Forms.Button();
            this.FitPolyOrder_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ActiveSlice_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.FitSlice_button = new System.Windows.Forms.Button();
            this.SavePane_button = new System.Windows.Forms.Button();
            this.ChangeXaxisUnits_comboBox = new System.Windows.Forms.ComboBox();
            this.XaxisUnits_label = new System.Windows.Forms.Label();
            this.SaveCurve_button = new System.Windows.Forms.Button();
            this.saveAllcurves_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CurveNumber_numericUpDown)).BeginInit();
            this.ProcessSlice_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FitPolyOrder_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveSlice_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(12, 12);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.CurveNumber_numericUpDown.Location = new System.Drawing.Point(12, 565);
            this.CurveNumber_numericUpDown.Name = "CurveNumber_numericUpDown";
            this.CurveNumber_numericUpDown.Size = new System.Drawing.Size(57, 20);
            this.CurveNumber_numericUpDown.TabIndex = 2;
            this.CurveNumber_numericUpDown.ValueChanged += new System.EventHandler(this.CurveNumber_numericUpDown_ValueChanged);
            // 
            // RemoveCurve_button
            // 
            this.RemoveCurve_button.Location = new System.Drawing.Point(75, 565);
            this.RemoveCurve_button.Name = "RemoveCurve_button";
            this.RemoveCurve_button.Size = new System.Drawing.Size(75, 23);
            this.RemoveCurve_button.TabIndex = 3;
            this.RemoveCurve_button.Text = "Remove";
            this.RemoveCurve_button.UseVisualStyleBackColor = true;
            this.RemoveCurve_button.Click += new System.EventHandler(this.RemoveCurve_button_Click);
            // 
            // ProcessSlice_groupBox
            // 
            this.ProcessSlice_groupBox.Controls.Add(this.ApproxMethodOutside_comboBox);
            this.ProcessSlice_groupBox.Controls.Add(this.Abel_button);
            this.ProcessSlice_groupBox.Controls.Add(this.FitPolyOrder_numericUpDown);
            this.ProcessSlice_groupBox.Controls.Add(this.ActiveSlice_numericUpDown);
            this.ProcessSlice_groupBox.Controls.Add(this.FitSlice_button);
            this.ProcessSlice_groupBox.Location = new System.Drawing.Point(238, 536);
            this.ProcessSlice_groupBox.Name = "ProcessSlice_groupBox";
            this.ProcessSlice_groupBox.Size = new System.Drawing.Size(420, 81);
            this.ProcessSlice_groupBox.TabIndex = 4;
            this.ProcessSlice_groupBox.TabStop = false;
            this.ProcessSlice_groupBox.Text = "Process slice";
            this.ProcessSlice_groupBox.Enter += new System.EventHandler(this.ProcessSlice_groupBox_Enter);
            // 
            // ApproxMethodOutside_comboBox
            // 
            this.ApproxMethodOutside_comboBox.FormattingEnabled = true;
            this.ApproxMethodOutside_comboBox.Location = new System.Drawing.Point(295, 21);
            this.ApproxMethodOutside_comboBox.Name = "ApproxMethodOutside_comboBox";
            this.ApproxMethodOutside_comboBox.Size = new System.Drawing.Size(119, 21);
            this.ApproxMethodOutside_comboBox.TabIndex = 4;
            this.ApproxMethodOutside_comboBox.Text = "Lock function";
            this.ApproxMethodOutside_comboBox.SelectedIndexChanged += new System.EventHandler(this.ApproxMethodOutside_comboBox_SelectedIndexChanged);
            // 
            // Abel_button
            // 
            this.Abel_button.Location = new System.Drawing.Point(194, 20);
            this.Abel_button.Name = "Abel_button";
            this.Abel_button.Size = new System.Drawing.Size(94, 23);
            this.Abel_button.TabIndex = 3;
            this.Abel_button.Text = "Abel inversion";
            this.Abel_button.UseVisualStyleBackColor = true;
            this.Abel_button.Click += new System.EventHandler(this.Abel_button_Click);
            // 
            // FitPolyOrder_numericUpDown
            // 
            this.FitPolyOrder_numericUpDown.Location = new System.Drawing.Point(138, 20);
            this.FitPolyOrder_numericUpDown.Name = "FitPolyOrder_numericUpDown";
            this.FitPolyOrder_numericUpDown.Size = new System.Drawing.Size(50, 20);
            this.FitPolyOrder_numericUpDown.TabIndex = 2;
            this.FitPolyOrder_numericUpDown.ValueChanged += new System.EventHandler(this.FitPolyOrder_numericUpDown_ValueChanged);
            // 
            // ActiveSlice_numericUpDown
            // 
            this.ActiveSlice_numericUpDown.Location = new System.Drawing.Point(7, 21);
            this.ActiveSlice_numericUpDown.Name = "ActiveSlice_numericUpDown";
            this.ActiveSlice_numericUpDown.Size = new System.Drawing.Size(43, 20);
            this.ActiveSlice_numericUpDown.TabIndex = 1;
            this.ActiveSlice_numericUpDown.ValueChanged += new System.EventHandler(this.ActiveSlice_numericUpDown_ValueChanged);
            // 
            // FitSlice_button
            // 
            this.FitSlice_button.Location = new System.Drawing.Point(56, 19);
            this.FitSlice_button.Name = "FitSlice_button";
            this.FitSlice_button.Size = new System.Drawing.Size(75, 23);
            this.FitSlice_button.TabIndex = 0;
            this.FitSlice_button.Text = "Fit Slice";
            this.FitSlice_button.UseVisualStyleBackColor = true;
            this.FitSlice_button.Click += new System.EventHandler(this.FitSlice_button_Click);
            // 
            // SavePane_button
            // 
            this.SavePane_button.Location = new System.Drawing.Point(156, 565);
            this.SavePane_button.Name = "SavePane_button";
            this.SavePane_button.Size = new System.Drawing.Size(75, 23);
            this.SavePane_button.TabIndex = 5;
            this.SavePane_button.Text = "Save pane";
            this.SavePane_button.UseVisualStyleBackColor = true;
            this.SavePane_button.Click += new System.EventHandler(this.SavePane_button_Click);
            // 
            // ChangeXaxisUnits_comboBox
            // 
            this.ChangeXaxisUnits_comboBox.FormattingEnabled = true;
            this.ChangeXaxisUnits_comboBox.Location = new System.Drawing.Point(75, 538);
            this.ChangeXaxisUnits_comboBox.Name = "ChangeXaxisUnits_comboBox";
            this.ChangeXaxisUnits_comboBox.Size = new System.Drawing.Size(75, 21);
            this.ChangeXaxisUnits_comboBox.TabIndex = 6;
            this.ChangeXaxisUnits_comboBox.SelectedIndexChanged += new System.EventHandler(this.ChangeXaxisUnits_comboBox_SelectedIndexChanged);
            // 
            // XaxisUnits_label
            // 
            this.XaxisUnits_label.AutoSize = true;
            this.XaxisUnits_label.Location = new System.Drawing.Point(12, 541);
            this.XaxisUnits_label.Name = "XaxisUnits_label";
            this.XaxisUnits_label.Size = new System.Drawing.Size(60, 13);
            this.XaxisUnits_label.TabIndex = 7;
            this.XaxisUnits_label.Text = "X-axis units";
            // 
            // SaveCurve_button
            // 
            this.SaveCurve_button.Location = new System.Drawing.Point(75, 594);
            this.SaveCurve_button.Name = "SaveCurve_button";
            this.SaveCurve_button.Size = new System.Drawing.Size(75, 23);
            this.SaveCurve_button.TabIndex = 8;
            this.SaveCurve_button.Text = "Save curve";
            this.SaveCurve_button.UseVisualStyleBackColor = true;
            this.SaveCurve_button.Click += new System.EventHandler(this.SaveCurve_button_Click);
            // 
            // saveAllcurves_button
            // 
            this.saveAllcurves_button.Location = new System.Drawing.Point(157, 593);
            this.saveAllcurves_button.Name = "saveAllcurves_button";
            this.saveAllcurves_button.Size = new System.Drawing.Size(75, 23);
            this.saveAllcurves_button.TabIndex = 9;
            this.saveAllcurves_button.Text = "Save all curves";
            this.saveAllcurves_button.UseVisualStyleBackColor = true;
            this.saveAllcurves_button.Click += new System.EventHandler(this.saveAllcurves_button_Click);
            // 
            // Graphics1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 623);
            this.Controls.Add(this.saveAllcurves_button);
            this.Controls.Add(this.SaveCurve_button);
            this.Controls.Add(this.XaxisUnits_label);
            this.Controls.Add(this.ChangeXaxisUnits_comboBox);
            this.Controls.Add(this.SavePane_button);
            this.Controls.Add(this.ProcessSlice_groupBox);
            this.Controls.Add(this.RemoveCurve_button);
            this.Controls.Add(this.CurveNumber_numericUpDown);
            this.Controls.Add(this.ClearPane_button);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "Graphics1";
            this.Text = "Graphics1";
            this.Load += new System.EventHandler(this.Graphics1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CurveNumber_numericUpDown)).EndInit();
            this.ProcessSlice_groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FitPolyOrder_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveSlice_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Button ClearPane_button;
        private System.Windows.Forms.NumericUpDown CurveNumber_numericUpDown;
        private System.Windows.Forms.Button RemoveCurve_button;
        private System.Windows.Forms.GroupBox ProcessSlice_groupBox;
        private System.Windows.Forms.Button FitSlice_button;
        private System.Windows.Forms.NumericUpDown ActiveSlice_numericUpDown;
        private System.Windows.Forms.NumericUpDown FitPolyOrder_numericUpDown;
        private System.Windows.Forms.Button Abel_button;
        private System.Windows.Forms.ComboBox ApproxMethodOutside_comboBox;
        private System.Windows.Forms.Button SavePane_button;
        private System.Windows.Forms.ComboBox ChangeXaxisUnits_comboBox;
        private System.Windows.Forms.Label XaxisUnits_label;
        private System.Windows.Forms.Button SaveCurve_button;
        private System.Windows.Forms.Button saveAllcurves_button;
    }
}