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
            this.OffsetTextDone_button = new System.Windows.Forms.Button();
            this.BackgroundOffset_textBox = new System.Windows.Forms.TextBox();
            this.Use8bit_checkBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ZoomProcessedImagecomboBox = new System.Windows.Forms.ComboBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.ColorMapCombobox = new System.Windows.Forms.ComboBox();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.ShowValueButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
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
            this.groupBox1.Controls.Add(this.ShowValueButton);
            this.groupBox1.Controls.Add(this.numericUpDownY);
            this.groupBox1.Controls.Add(this.numericUpDownX);
            this.groupBox1.Controls.Add(this.OffsetTextDone_button);
            this.groupBox1.Controls.Add(this.BackgroundOffset_textBox);
            this.groupBox1.Controls.Add(this.Use8bit_checkBox);
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
            // Use8bit_checkBox
            // 
            this.Use8bit_checkBox.AutoSize = true;
            this.Use8bit_checkBox.Location = new System.Drawing.Point(7, 47);
            this.Use8bit_checkBox.Name = "Use8bit_checkBox";
            this.Use8bit_checkBox.Size = new System.Drawing.Size(99, 17);
            this.Use8bit_checkBox.TabIndex = 5;
            this.Use8bit_checkBox.Text = "Use 8-bit image";
            this.Use8bit_checkBox.UseVisualStyleBackColor = true;
            this.Use8bit_checkBox.CheckedChanged += new System.EventHandler(this.Use8bit_checkBox_CheckedChanged);
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
            // numericUpDownX
            // 
            this.numericUpDownX.Location = new System.Drawing.Point(7, 120);
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownX.TabIndex = 8;
            this.numericUpDownX.ValueChanged += new System.EventHandler(this.numericUpDownX_ValueChanged);
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.Location = new System.Drawing.Point(73, 121);
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownY.TabIndex = 9;
            this.numericUpDownY.ValueChanged += new System.EventHandler(this.numericUpDownY_ValueChanged);
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
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
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
        private System.Windows.Forms.CheckBox Use8bit_checkBox;
        private System.Windows.Forms.TextBox BackgroundOffset_textBox;
        private System.Windows.Forms.Button OffsetTextDone_button;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.Button ShowValueButton;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
    }
}

