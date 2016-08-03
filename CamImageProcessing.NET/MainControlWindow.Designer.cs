namespace CamImageProcessing.NET
{
    partial class MainControlWindow
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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SaveImages_button = new System.Windows.Forms.Button();
            this.ImageHisto_groupBox = new System.Windows.Forms.GroupBox();
            this.EqualizeHist_button = new System.Windows.Forms.Button();
            this.CLAHE_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Slice_groupBox = new System.Windows.Forms.GroupBox();
            this.ClearSliceList_button = new System.Windows.Forms.Button();
            this.SliceCount_label = new System.Windows.Forms.Label();
            this.SliceColor_comboBox = new System.Windows.Forms.ComboBox();
            this.CreateSlice_button = new System.Windows.Forms.Button();
            this.SliceMargin2_label = new System.Windows.Forms.Label();
            this.SliceMargin2_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SliceMargin1_label = new System.Windows.Forms.Label();
            this.SliceMargin1_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.HorV_slice_comboBox = new System.Windows.Forms.ComboBox();
            this.ShowValueButton = new System.Windows.Forms.Button();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.OffsetTextDone_button = new System.Windows.Forms.Button();
            this.BackgroundOffset_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ZoomProcessedImagecomboBox = new System.Windows.Forms.ComboBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.ColorMapCombobox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FOV_X_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.FOV_Y_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.ImageHisto_groupBox.SuspendLayout();
            this.Slice_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SliceMargin2_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliceMargin1_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOV_X_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOV_Y_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open Header file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Show header";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 77);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(114, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Open Image";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 106);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(114, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.Text = "Zoom Image";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 474);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(730, 23);
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Original image properties";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FOV_Y_numericUpDown);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.FOV_X_numericUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.SaveImages_button);
            this.groupBox1.Controls.Add(this.ImageHisto_groupBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Slice_groupBox);
            this.groupBox1.Controls.Add(this.ShowValueButton);
            this.groupBox1.Controls.Add(this.numericUpDownY);
            this.groupBox1.Controls.Add(this.numericUpDownX);
            this.groupBox1.Controls.Add(this.OffsetTextDone_button);
            this.groupBox1.Controls.Add(this.BackgroundOffset_textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ZoomProcessedImagecomboBox);
            this.groupBox1.Controls.Add(this.ResetButton);
            this.groupBox1.Controls.Add(this.ApplyButton);
            this.groupBox1.Controls.Add(this.ColorMapCombobox);
            this.groupBox1.Location = new System.Drawing.Point(238, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 456);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Image";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // SaveImages_button
            // 
            this.SaveImages_button.Location = new System.Drawing.Point(112, 427);
            this.SaveImages_button.Name = "SaveImages_button";
            this.SaveImages_button.Size = new System.Drawing.Size(75, 23);
            this.SaveImages_button.TabIndex = 14;
            this.SaveImages_button.Text = "Save";
            this.SaveImages_button.UseVisualStyleBackColor = true;
            this.SaveImages_button.Click += new System.EventHandler(this.SaveImages_button_Click);
            // 
            // ImageHisto_groupBox
            // 
            this.ImageHisto_groupBox.Controls.Add(this.EqualizeHist_button);
            this.ImageHisto_groupBox.Controls.Add(this.CLAHE_button);
            this.ImageHisto_groupBox.Location = new System.Drawing.Point(264, 147);
            this.ImageHisto_groupBox.Name = "ImageHisto_groupBox";
            this.ImageHisto_groupBox.Size = new System.Drawing.Size(200, 151);
            this.ImageHisto_groupBox.TabIndex = 13;
            this.ImageHisto_groupBox.TabStop = false;
            this.ImageHisto_groupBox.Text = "Histograms";
            this.ImageHisto_groupBox.Enter += new System.EventHandler(this.ImageHisto_groupBox_Enter);
            // 
            // EqualizeHist_button
            // 
            this.EqualizeHist_button.Location = new System.Drawing.Point(6, 20);
            this.EqualizeHist_button.Name = "EqualizeHist_button";
            this.EqualizeHist_button.Size = new System.Drawing.Size(75, 23);
            this.EqualizeHist_button.TabIndex = 1;
            this.EqualizeHist_button.Text = "Equalize";
            this.EqualizeHist_button.UseVisualStyleBackColor = true;
            this.EqualizeHist_button.Click += new System.EventHandler(this.EqualizeHist_button_Click);
            // 
            // CLAHE_button
            // 
            this.CLAHE_button.Location = new System.Drawing.Point(98, 20);
            this.CLAHE_button.Name = "CLAHE_button";
            this.CLAHE_button.Size = new System.Drawing.Size(75, 23);
            this.CLAHE_button.TabIndex = 0;
            this.CLAHE_button.Text = "CLAHE";
            this.CLAHE_button.UseVisualStyleBackColor = true;
            this.CLAHE_button.Click += new System.EventHandler(this.CLAHE_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Offset and scale";
            // 
            // Slice_groupBox
            // 
            this.Slice_groupBox.Controls.Add(this.ClearSliceList_button);
            this.Slice_groupBox.Controls.Add(this.SliceCount_label);
            this.Slice_groupBox.Controls.Add(this.SliceColor_comboBox);
            this.Slice_groupBox.Controls.Add(this.CreateSlice_button);
            this.Slice_groupBox.Controls.Add(this.SliceMargin2_label);
            this.Slice_groupBox.Controls.Add(this.SliceMargin2_numericUpDown);
            this.Slice_groupBox.Controls.Add(this.SliceMargin1_label);
            this.Slice_groupBox.Controls.Add(this.SliceMargin1_numericUpDown);
            this.Slice_groupBox.Controls.Add(this.HorV_slice_comboBox);
            this.Slice_groupBox.Location = new System.Drawing.Point(6, 147);
            this.Slice_groupBox.Name = "Slice_groupBox";
            this.Slice_groupBox.Size = new System.Drawing.Size(248, 151);
            this.Slice_groupBox.TabIndex = 11;
            this.Slice_groupBox.TabStop = false;
            this.Slice_groupBox.Text = "Image slice";
            this.Slice_groupBox.Enter += new System.EventHandler(this.Slice_groupBox_Enter);
            // 
            // ClearSliceList_button
            // 
            this.ClearSliceList_button.Location = new System.Drawing.Point(166, 102);
            this.ClearSliceList_button.Name = "ClearSliceList_button";
            this.ClearSliceList_button.Size = new System.Drawing.Size(75, 23);
            this.ClearSliceList_button.TabIndex = 8;
            this.ClearSliceList_button.Text = "Clear list";
            this.ClearSliceList_button.UseVisualStyleBackColor = true;
            this.ClearSliceList_button.Click += new System.EventHandler(this.ClearSliceList_button_Click);
            // 
            // SliceCount_label
            // 
            this.SliceCount_label.AutoSize = true;
            this.SliceCount_label.Location = new System.Drawing.Point(163, 135);
            this.SliceCount_label.Name = "SliceCount_label";
            this.SliceCount_label.Size = new System.Drawing.Size(66, 13);
            this.SliceCount_label.TabIndex = 7;
            this.SliceCount_label.Text = "Slice count: ";
            this.SliceCount_label.Click += new System.EventHandler(this.SliceCount_label_Click);
            // 
            // SliceColor_comboBox
            // 
            this.SliceColor_comboBox.FormattingEnabled = true;
            this.SliceColor_comboBox.Location = new System.Drawing.Point(7, 102);
            this.SliceColor_comboBox.Name = "SliceColor_comboBox";
            this.SliceColor_comboBox.Size = new System.Drawing.Size(121, 21);
            this.SliceColor_comboBox.TabIndex = 6;
            this.SliceColor_comboBox.Text = "Slice color";
            this.SliceColor_comboBox.SelectedIndexChanged += new System.EventHandler(this.SliceColor_comboBox_SelectedIndexChanged);
            // 
            // CreateSlice_button
            // 
            this.CreateSlice_button.Location = new System.Drawing.Point(133, 19);
            this.CreateSlice_button.Name = "CreateSlice_button";
            this.CreateSlice_button.Size = new System.Drawing.Size(75, 23);
            this.CreateSlice_button.TabIndex = 5;
            this.CreateSlice_button.Text = "Create Slice";
            this.CreateSlice_button.UseVisualStyleBackColor = true;
            this.CreateSlice_button.Click += new System.EventHandler(this.CreateSlice_button_Click);
            // 
            // SliceMargin2_label
            // 
            this.SliceMargin2_label.AutoSize = true;
            this.SliceMargin2_label.Location = new System.Drawing.Point(80, 82);
            this.SliceMargin2_label.Name = "SliceMargin2_label";
            this.SliceMargin2_label.Size = new System.Drawing.Size(48, 13);
            this.SliceMargin2_label.TabIndex = 4;
            this.SliceMargin2_label.Text = "Margin-2";
            this.SliceMargin2_label.Click += new System.EventHandler(this.SliceMargin2_label_Click);
            // 
            // SliceMargin2_numericUpDown
            // 
            this.SliceMargin2_numericUpDown.Location = new System.Drawing.Point(7, 75);
            this.SliceMargin2_numericUpDown.Name = "SliceMargin2_numericUpDown";
            this.SliceMargin2_numericUpDown.Size = new System.Drawing.Size(66, 20);
            this.SliceMargin2_numericUpDown.TabIndex = 3;
            this.SliceMargin2_numericUpDown.ValueChanged += new System.EventHandler(this.SliceMargin2_numericUpDown_ValueChanged);
            // 
            // SliceMargin1_label
            // 
            this.SliceMargin1_label.AutoSize = true;
            this.SliceMargin1_label.Location = new System.Drawing.Point(80, 55);
            this.SliceMargin1_label.Name = "SliceMargin1_label";
            this.SliceMargin1_label.Size = new System.Drawing.Size(48, 13);
            this.SliceMargin1_label.TabIndex = 2;
            this.SliceMargin1_label.Text = "Margin-1";
            this.SliceMargin1_label.Click += new System.EventHandler(this.SliceMargin1_label_Click);
            // 
            // SliceMargin1_numericUpDown
            // 
            this.SliceMargin1_numericUpDown.Location = new System.Drawing.Point(7, 48);
            this.SliceMargin1_numericUpDown.Name = "SliceMargin1_numericUpDown";
            this.SliceMargin1_numericUpDown.Size = new System.Drawing.Size(66, 20);
            this.SliceMargin1_numericUpDown.TabIndex = 1;
            this.SliceMargin1_numericUpDown.Tag = "Margin-1";
            this.SliceMargin1_numericUpDown.ValueChanged += new System.EventHandler(this.SliceMargin1_numericUpDown_ValueChanged);
            // 
            // HorV_slice_comboBox
            // 
            this.HorV_slice_comboBox.FormattingEnabled = true;
            this.HorV_slice_comboBox.Location = new System.Drawing.Point(7, 20);
            this.HorV_slice_comboBox.Name = "HorV_slice_comboBox";
            this.HorV_slice_comboBox.Size = new System.Drawing.Size(121, 21);
            this.HorV_slice_comboBox.TabIndex = 0;
            this.HorV_slice_comboBox.SelectedIndexChanged += new System.EventHandler(this.HorV_slice_comboBox_SelectedIndexChanged);
            // 
            // ShowValueButton
            // 
            this.ShowValueButton.Location = new System.Drawing.Point(139, 117);
            this.ShowValueButton.Name = "ShowValueButton";
            this.ShowValueButton.Size = new System.Drawing.Size(49, 23);
            this.ShowValueButton.TabIndex = 10;
            this.ShowValueButton.Text = "Get";
            this.ShowValueButton.UseVisualStyleBackColor = true;
            this.ShowValueButton.Click += new System.EventHandler(this.ShowValueButton_Click);
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.Location = new System.Drawing.Point(73, 121);
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownY.TabIndex = 9;
            this.numericUpDownY.ValueChanged += new System.EventHandler(this.numericUpDownY_ValueChanged);
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.Location = new System.Drawing.Point(7, 120);
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownX.TabIndex = 8;
            this.numericUpDownX.ValueChanged += new System.EventHandler(this.numericUpDownX_ValueChanged);
            // 
            // OffsetTextDone_button
            // 
            this.OffsetTextDone_button.Location = new System.Drawing.Point(113, 92);
            this.OffsetTextDone_button.Name = "OffsetTextDone_button";
            this.OffsetTextDone_button.Size = new System.Drawing.Size(75, 23);
            this.OffsetTextDone_button.TabIndex = 7;
            this.OffsetTextDone_button.Text = "Done";
            this.OffsetTextDone_button.UseVisualStyleBackColor = true;
            this.OffsetTextDone_button.Click += new System.EventHandler(this.OffsetTextDone_button_Click);
            // 
            // BackgroundOffset_textBox
            // 
            this.BackgroundOffset_textBox.Location = new System.Drawing.Point(6, 94);
            this.BackgroundOffset_textBox.Name = "BackgroundOffset_textBox";
            this.BackgroundOffset_textBox.Size = new System.Drawing.Size(100, 20);
            this.BackgroundOffset_textBox.TabIndex = 6;
            this.BackgroundOffset_textBox.TextChanged += new System.EventHandler(this.BackgroundOffset_textBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Proc. image properties";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ZoomProcessedImagecomboBox
            // 
            this.ZoomProcessedImagecomboBox.FormattingEnabled = true;
            this.ZoomProcessedImagecomboBox.Location = new System.Drawing.Point(133, 19);
            this.ZoomProcessedImagecomboBox.Name = "ZoomProcessedImagecomboBox";
            this.ZoomProcessedImagecomboBox.Size = new System.Drawing.Size(121, 21);
            this.ZoomProcessedImagecomboBox.TabIndex = 3;
            this.ZoomProcessedImagecomboBox.Text = "Zoom Image";
            this.ZoomProcessedImagecomboBox.SelectedIndexChanged += new System.EventHandler(this.ZoomProcessedImagecomboBox_SelectedIndexChanged);
            // 
            // ResetButton
            // 
            this.ResetButton.ForeColor = System.Drawing.Color.Red;
            this.ResetButton.Location = new System.Drawing.Point(423, 427);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 2;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ApplyButton.Location = new System.Drawing.Point(6, 427);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 23);
            this.ApplyButton.TabIndex = 1;
            this.ApplyButton.Text = "Update";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // ColorMapCombobox
            // 
            this.ColorMapCombobox.FormattingEnabled = true;
            this.ColorMapCombobox.Location = new System.Drawing.Point(6, 19);
            this.ColorMapCombobox.Name = "ColorMapCombobox";
            this.ColorMapCombobox.Size = new System.Drawing.Size(121, 21);
            this.ColorMapCombobox.TabIndex = 0;
            this.ColorMapCombobox.Text = "Color Map";
            this.ColorMapCombobox.SelectedIndexChanged += new System.EventHandler(this.ColorMapCombobox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 305);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "FOV-X";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // FOV_X_numericUpDown
            // 
            this.FOV_X_numericUpDown.Location = new System.Drawing.Point(58, 303);
            this.FOV_X_numericUpDown.Name = "FOV_X_numericUpDown";
            this.FOV_X_numericUpDown.Size = new System.Drawing.Size(75, 20);
            this.FOV_X_numericUpDown.TabIndex = 16;
            this.FOV_X_numericUpDown.ValueChanged += new System.EventHandler(this.FOV_X_numericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 332);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "FOV-Y";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // FOV_Y_numericUpDown
            // 
            this.FOV_Y_numericUpDown.Location = new System.Drawing.Point(57, 329);
            this.FOV_Y_numericUpDown.Name = "FOV_Y_numericUpDown";
            this.FOV_Y_numericUpDown.Size = new System.Drawing.Size(76, 20);
            this.FOV_Y_numericUpDown.TabIndex = 18;
            this.FOV_Y_numericUpDown.ValueChanged += new System.EventHandler(this.FOV_Y_numericUpDown_ValueChanged);
            // 
            // MainControlWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 509);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "MainControlWindow";
            this.Text = "Control panel";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ImageHisto_groupBox.ResumeLayout(false);
            this.Slice_groupBox.ResumeLayout(false);
            this.Slice_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SliceMargin2_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliceMargin1_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOV_X_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FOV_Y_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ColorMapCombobox;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.ComboBox ZoomProcessedImagecomboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox BackgroundOffset_textBox;
        private System.Windows.Forms.Button OffsetTextDone_button;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.Button ShowValueButton;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.GroupBox Slice_groupBox;
        private System.Windows.Forms.ComboBox HorV_slice_comboBox;
        private System.Windows.Forms.NumericUpDown SliceMargin1_numericUpDown;
        private System.Windows.Forms.Label SliceMargin1_label;
        private System.Windows.Forms.NumericUpDown SliceMargin2_numericUpDown;
        private System.Windows.Forms.Label SliceMargin2_label;
        private System.Windows.Forms.Button CreateSlice_button;
        private System.Windows.Forms.ComboBox SliceColor_comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox ImageHisto_groupBox;
        private System.Windows.Forms.Button CLAHE_button;
        private System.Windows.Forms.Button EqualizeHist_button;
        private System.Windows.Forms.Button SaveImages_button;
        private System.Windows.Forms.Label SliceCount_label;
        private System.Windows.Forms.Button ClearSliceList_button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown FOV_X_numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown FOV_Y_numericUpDown;
    }
}

