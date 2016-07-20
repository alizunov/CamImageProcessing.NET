using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

/// <summary>
/// EmguCV namespaces
/// </summary>
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace CamImageProcessing.NET
{
    public partial class MainControlWindow : Form
    {
        // String List containing header text readout from the selected file.
        // It must be checked before initialize the header class instance.
        List<string> TextReadout = new List<string>();

        // Uint16 List of image pixels read from the file.
        List<UInt16> ImageReadout = new List<UInt16>();

        // Header of original image file
        CameraImageHeader HeaderOrigImage;
        CameraImageHeader HeaderVoid;

        const string OrigImageName = "Original Image";
        const string ProcessedImageName = "Processed Image";

        // Original image, see CameraImage class desc.
        CameraImage OrigImage;
        // Processed image
        CameraImage ProcessedImage;

        // Slice
        CameraImageSlice Slice1;

        public MainControlWindow()
        {
            InitializeComponent();
            // Show header button
            this.button2.Enabled = false;
            // Open image button
            this.button3.Enabled = false;
            // Zoom factor for displaying original image combobox
            comboBox1.Name = "Zoom orig. image";
            comboBox1.TabIndex = 4;
            comboBox1.Items.AddRange(new object[] {"Zoom 1:1",
                        "Zoom 1:2",
                        "Zoom 1:4",
                        "Zoom 1:8"});
            
            comboBox1.Enabled = false;
            // Setup of controls for image processing grouped in the box
            groupBox1.Enabled = false;
            ColorMapCombobox.Items.AddRange(new object[] {ColorMapType.Autumn,
                        ColorMapType.Bone,
                        ColorMapType.Cool,
                        ColorMapType.Hot,
                        ColorMapType.Hsv,
                        ColorMapType.Jet,
                        ColorMapType.Ocean,
                        ColorMapType.Pink,
                        ColorMapType.Rainbow,
                        ColorMapType.Spring,
                        ColorMapType.Summer,
                        ColorMapType.Winter} );
            // Zoom processed image combobox
            ZoomProcessedImagecomboBox.Name = "Zoom proc. image";
            ZoomProcessedImagecomboBox.Items.AddRange(new object[] {"Zoom 1:1",
                        "Zoom 1:2",
                        "Zoom 1:4",
                        "Zoom 1:8"});
            // Use 8-bit image checkbox
            Use8bit_checkBox.Checked = false;
            // Setup UpDown inputs of X, Y coordinates for the ShowPixelValue() method
            // X-input
            numericUpDownX.Minimum = 0;
            numericUpDownX.Maximum = 100;   // Will be updated when SrcMat is obtained
            numericUpDownX.Increment = 1;
            // Y-input
            numericUpDownY.Minimum = 0;
            numericUpDownY.Maximum = 100;   // Will be updated when SrcMat is obtained
            numericUpDownY.Increment = 1;
            
            // Slice controls:
            // Horizontal or vertical combobox setup
            HorV_slice_comboBox.Name = "Slice direction";
            HorV_slice_comboBox.Items.AddRange(new object[] { "Vertical", "Horizontal" });
            HorV_slice_comboBox.SelectedIndex = 0;
            // Labels of numUpDown controls of slice margins
            SliceMargin1_label.Text = "Margin-1";
            SliceMargin2_label.Text = "Margin-2";
            // numUpDown controls of slice margins
            SliceMargin1_numericUpDown.Minimum = 0;
            SliceMargin2_numericUpDown.Minimum = 0;
            // Slice color combobox
            SliceColor_comboBox.Items.AddRange(new object[] { Color.AliceBlue,
                Color.AntiqueWhite,
                Color.Coral,
                Color.Cyan,
                Color.DarkBlue,
                Color.DarkGreen,
                Color.DarkMagenta,
                Color.DarkRed,
                Color.Blue,
                Color.Green,
                Color.Magenta,
                Color.Red});
            SliceColor_comboBox.SelectedIndex = 0;

        }   // MainControlWindow
        

         // Open header file
       private void button1_Click(object sender, EventArgs e)
        {
            System.IO.Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "Header files (*.imgh)|*.imgh|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

           if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            using (System.IO.StreamReader srReader = new System.IO.StreamReader(myStream))
                            {
                                // Insert code to read the stream here.
                                TextReadout.Clear();
                                string str1 = "";
                                while ((str1 = srReader.ReadLine()) != null)
                                {
                                    if (str1 != "\n")
                                    {
                                        TextReadout.Add(str1);
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            try
            {
                
                HeaderVoid = new CameraImageHeader();
                HeaderOrigImage = HeaderVoid.TryTextList(TextReadout);

                if (HeaderOrigImage.isGoodHeader)
                {
                    MessageBox.Show(TextReadout.Count().ToString() + " lines read from the file, header: OK", "", MessageBoxButtons.OK);
                    this.button2.Enabled = true;
                    this.button3.Enabled = true;
                }
                else
                {
                    MessageBox.Show(TextReadout.Count().ToString() + " lines read from the file, header: BAD", "", MessageBoxButtons.OK);
                    this.button2.Enabled = false;
                    this.button3.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: could not create headers. Original error: " + ex.Message);
            }

        }

        // Show header
        private void button2_Click(object sender, EventArgs e)
        {
            if ( HeaderOrigImage.isGoodHeader )
            {
                MessageBox.Show(HeaderOrigImage.FormattedHeader,"", MessageBoxButtons.OK);
            }
        }

        // Open Image
        private void button3_Click(object sender, EventArgs e)
        {
            System.IO.Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "Image files (*.img)|*.img|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            System.IO.BinaryReader br1 = new System.IO.BinaryReader(myStream);
                            using (br1)
                            {
                                br1.BaseStream.Position = 0;
                                ImageReadout.Clear();
                                // Prepare progress bar to show loading of an image
                                progressBar1.Minimum = 1;
                                progressBar1.Maximum = 100;
                                progressBar1.Step = 1;
                                progressBar1.Value = 1;
                                long posDelta = (long)br1.BaseStream.Length / 100;
                                long pbStep = 1;
                                while (br1.BaseStream.Position != br1.BaseStream.Length)
                                {
                                    ImageReadout.Add( br1.ReadUInt16() );
                                    if (br1.BaseStream.Position >= (long)posDelta * pbStep)
                                    {
                                        progressBar1.PerformStep();
                                        ++pbStep;
                                    }
                                }
                            }

                        }
                        Int32 SamplesRead = ImageReadout.Count() - 4;   // Do not know the reason.
                        if ( SamplesRead == HeaderOrigImage.SizeX*HeaderOrigImage.SizeY )
                        {
                            MessageBox.Show(Convert.ToString(SamplesRead) + " UInt16 samples read from the file.", "", MessageBoxButtons.OK);
                            // Read OK, so we can create EmguCV Mat structure
                            try
                            {
                                OrigImage = new CameraImage((Int32)HeaderOrigImage.SizeY, (Int32)HeaderOrigImage.SizeX, 1, ImageReadout, OrigImageName);
                                if (OrigImage.SizeX == HeaderOrigImage.SizeX && OrigImage.SizeY == HeaderOrigImage.SizeY)
                                {
                                    //MessageBox.Show("CameraImage OK; sizes: " + OrigImage.SizeX + ", " + OrigImage.SizeY, "", MessageBoxButtons.OK);
                                    comboBox1.Enabled = true;
                                    label1.Text = OrigImage.GetProperties;
                                }
                                else
                                {
                                    //MessageBox.Show("CameraImage BAD; sizes: " + OrigImage.SizeX + ", " + OrigImage.SizeY, "", MessageBoxButtons.OK);
                                    comboBox1.Enabled = false;
                                }
                                // Create image for processing
                                ProcessedImage = OrigImage.Clone(ProcessedImageName);
                                if (ProcessedImage.SizeX == OrigImage.SizeX && ProcessedImage.SizeY == OrigImage.SizeY)
                                {
                                    //MessageBox.Show("CameraImage OK; sizes: " + OrigImage.SizeX + ", " + OrigImage.SizeY, "", MessageBoxButtons.OK);
                                    groupBox1.Enabled = true;
                                    numericUpDownX.Maximum = ProcessedImage.SizeX;
                                    numericUpDownY.Maximum = ProcessedImage.SizeY;
                                    //label1.Text = OrigImage.GetProperties;
                                }
                                else
                                {
                                    //MessageBox.Show("CameraImage BAD; sizes: " + OrigImage.SizeX + ", " + OrigImage.SizeY, "", MessageBoxButtons.OK);
                                    comboBox1.Enabled = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: Could not create Mat for the image. Original error: " + ex.Message);
                            }

                        }
                        else
                        {
                            MessageBox.Show(Convert.ToString(SamplesRead) + " UInt16 samples read from the file, \n" +
                                "doesn't match to the image size from the header: " +
                                Convert.ToString(HeaderOrigImage.SizeX) + "x" + Convert.ToString(HeaderOrigImage.SizeY) + "\n" +
                                ": image processing structures not created.", "", MessageBoxButtons.OK);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        // Show zoomed original image (zoom factor from the combo).
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = comboBox1.SelectedItem.ToString();
            byte SelectedZoom = Convert.ToByte(item.Substring(item.Length-1, 1));
            OrigImage.ShowZoomed(SelectedZoom);
            label1.Text = OrigImage.GetProperties;
        }
        // Mult-purpose progress bar to visualize some process (load image, processing image).
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ColorMapCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessedImage.ApplyColorMap((ColorMapType)ColorMapCombobox.SelectedItem);

        }
        // Group of controls for processing image
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ProcessedImage.ShowZoomed(ProcessedImage.CurrentDownsampleFactor);
        }
        // Reset processed image to original
        private void ResetButton_Click(object sender, EventArgs e)
        {
            ProcessedImage = OrigImage.Clone(ProcessedImageName);
        }

        private void ZoomProcessedImagecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = ZoomProcessedImagecomboBox.SelectedItem.ToString();
            byte SelectedZoom = Convert.ToByte(item.Substring(item.Length-1, 1));
            ProcessedImage.ShowZoomed(SelectedZoom);
            label2.Text = ProcessedImage.GetProperties;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Use8bit_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ProcessedImage.shouldUse8bit = Use8bit_checkBox.Checked;
            if (Use8bit_checkBox.Checked) ProcessedImage.ConvertTo8bit();
        }

        private void BackgroundOffset_textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void OffsetTextDone_button_Click(object sender, EventArgs e)
        {
            ProcessedImage.OffsetAndScale(Convert.ToUInt16(BackgroundOffset_textBox.Text));
            label2.Text = ProcessedImage.GetProperties;
            //MessageBox.Show("Offset: " + Convert.ToUInt16(BackgroundOffset_textBox.Text).ToString(), "", MessageBoxButtons.OK);
        }

        private void numericUpDownX_ValueChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDownY_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ShowValueButton_Click(object sender, EventArgs e)
        {
            Point pixel = new Point((Int32)numericUpDownX.Value, (Int32)numericUpDownY.Value);
            UInt16 PixelValue = ProcessedImage.GetPixelValue(pixel);
            MessageBox.Show("Pixel(" + pixel.X + ", " + pixel.Y + ") = " + PixelValue, "", MessageBoxButtons.OK);
        }

        private void Slice_groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void HorV_slice_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HorV_slice_comboBox.SelectedIndex == 0 && ProcessedImage != null) // Vertical
            {
                SliceMargin1_label.Text = "Left";
                SliceMargin2_label.Text = "Right";
                SliceMargin1_numericUpDown.Maximum = ProcessedImage.SizeX - 1;
            }
            else if (ProcessedImage != null) // Horizontal
            {
                SliceMargin1_label.Text = "Top";
                SliceMargin2_label.Text = "Bottom";
                SliceMargin1_numericUpDown.Maximum = ProcessedImage.SizeY - 1;
            }
        }

        private void SliceMargin1_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SliceMargin1_label_Click(object sender, EventArgs e)
        {

        }

        private void SliceMargin2_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SliceMargin2_label_Click(object sender, EventArgs e)
        {

        }

        private void CreateSlice_button_Click(object sender, EventArgs e)
        {
            if (HorV_slice_comboBox.SelectedIndex == 0) // Vertical
            {
                int x = (int)SliceMargin1_numericUpDown.Value;
                int w = (int)(SliceMargin2_numericUpDown.Value - SliceMargin1_numericUpDown.Value);
                int h = ProcessedImage.SizeY - 1;
                Color col = (Color)SliceColor_comboBox.SelectedItem;
                Rectangle sliceROI = new Rectangle(x, 0, w, h);
                Slice1 = new CameraImageSlice(ProcessedImage, sliceROI, "Vertical slice", col);
            }
        }

        private void SliceColor_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
