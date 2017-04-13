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

        // Image file name
        string ImageFileName;

        // Uint16 List of image pixels read from the file.
        List<UInt16> ImageReadout = new List<UInt16>();

        // Header of original image file
        CameraImageHeader HeaderOrigImage;
        CameraImageHeader HeaderVoid;

        const string OrigImageName = "Original Image";
        const string ProcessedImageName = "Processed Image";

        // Original object, see CameraImage class desc.
        CameraImage OrigImage;
        // Processed object
        CameraImage ProcessedImage;

        // EmguCV Images for original
        Image<Bgr, UInt16> image16orig;
        Image<Bgr, byte> image8orig;

        // EmguCV Images for processed
        Image<Bgr, UInt16> image16proc;
        Image<Bgr, byte> image8proc;

        // List of "slice" objects for creating of profiles and calculations
        List<CameraImageSlice> SliceList;
        // Lists of slice ROI and color
        List<Rectangle> SliceROIlist;
        List<Color> SliceColorList;

        // Form for graphics
        Graphics1 GraphForm1;

        // Zoom factors
        private byte ZoomOrig;
        private byte ZoomOrigPrev;
        private byte ZoomProc;
        private byte ZoomProcPrev;

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
            SliceColor_comboBox.Items.AddRange(new object[] { Color.DeepPink,
            Color.Gold,
            Color.Aqua,
            Color.Blue,
            Color.BlueViolet,
            Color.Coral,
            Color.Crimson,
            Color.Cyan,
            Color.DarkBlue,
            Color.DarkGreen,
            Color.Orange,
            Color.Green,
            Color.Red });
            SliceColor_comboBox.SelectedIndex = 0;

            // Field of view input: focal plane object size in mm
            // EO lens L=1200: FOV = 410x340mm
            FOV_X_numericUpDown.Maximum = 1000;
            FOV_X_numericUpDown.Value = 410;
            FOV_Y_numericUpDown.Maximum = 1000;
            FOV_Y_numericUpDown.Value = 340;

            // Create a form for graphics
            GraphForm1 = new Graphics1();
            GraphForm1.Show();

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
                                ImageFileName = openFileDialog1.FileName;
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
                                    // Create Images to display, copy data
                                    image16orig = new Image<Bgr, ushort>(OrigImage.SizeX, OrigImage.SizeY);
                                    image8orig = new Image<Bgr, byte>(OrigImage.SizeX, OrigImage.SizeY);
                                    // Copy data from Mat
                                    OrigImage.Image(image16orig);
                                    OrigImage.Image(image8orig);
                                    ZoomOrigPrev = 1;
                                }
                                else
                                {
                                    //MessageBox.Show("CameraImage BAD; sizes: " + OrigImage.SizeX + ", " + OrigImage.SizeY, "", MessageBoxButtons.OK);
                                    comboBox1.Enabled = false;
                                }
                                // Create image for processing (no data)
                                ProcessedImage = new CameraImage((Int32)OrigImage.SizeY, (Int32)OrigImage.SizeX, 1, ProcessedImageName);
                                // Clone data array
                                ProcessedImage.Clone(OrigImage.SrcMat);
                                if (ProcessedImage.SizeX == OrigImage.SizeX && ProcessedImage.SizeY == OrigImage.SizeY)
                                {
                                    //MessageBox.Show("CameraImage OK; sizes: " + OrigImage.SizeX + ", " + OrigImage.SizeY, "", MessageBoxButtons.OK);
                                    groupBox1.Enabled = true;
                                    numericUpDownX.Maximum = ProcessedImage.SizeX;
                                    numericUpDownY.Maximum = ProcessedImage.SizeY;
                                    //label1.Text = OrigImage.GetProperties;
                                    // Create Images to display, copy data
                                    image16proc = new Image<Bgr, ushort>(ProcessedImage.SizeX, ProcessedImage.SizeY);
                                    image8proc = new Image<Bgr, byte>(ProcessedImage.SizeX, ProcessedImage.SizeY);
                                    // Copy data from Mat
                                    ProcessedImage.Image(image16proc);
                                    ProcessedImage.Image(image8proc);
                                    ZoomProcPrev = 1;
                                    // Create slice lists
                                    SliceList = new List<CameraImageSlice>();
                                    SliceROIlist = new List<Rectangle>();
                                    SliceColorList = new List<Color>();
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
            ZoomOrig = Convert.ToByte(item.Substring(item.Length-1, 1));
            OrigImage.ShowZoomed(image16orig, image8orig, OrigImageName, ZoomOrig, ZoomOrigPrev, null, null);
            ZoomOrigPrev = ZoomOrig;
            label1.Text = OrigImage.GetProperties;
        }
        // Mult-purpose progress bar to visualize some process (load image, processing image).
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Apply new color map to 8-bit mage and redraw
        private void ColorMapCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessedImage.ApplyColorMap(image8proc, (ColorMapType)ColorMapCombobox.SelectedItem);
            ProcessedImage.ShowZoomed(image16proc, image8proc, ProcessedImageName, ZoomProc, ZoomProcPrev, SliceROIlist, SliceColorList);
        }
        // Group of controls for processing image
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            OrigImage.Image(image16orig);
            OrigImage.Image(image8orig);
            ProcessedImage.Image(image16proc);
            ProcessedImage.Image(image8proc);
        }
        // Reset processed image to original
        private void ResetButton_Click(object sender, EventArgs e)
        {
            ProcessedImage.CloseAllWindows();
            SliceList.Clear();
            SliceROIlist.Clear();
            SliceColorList.Clear();
            SliceCount_label.Text = "Slice count: " + SliceList.Count.ToString();
            ProcessedImage.Clone(OrigImage.SrcMat);
            
            ProcessedImage.Image(image16proc);
            ProcessedImage.Image(image8proc);
            ProcessedImage.ShowZoomed(image16proc, image8proc, ProcessedImageName, ZoomProc, ZoomProcPrev, SliceROIlist, SliceColorList);
        }

        private void ZoomProcessedImagecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = ZoomProcessedImagecomboBox.SelectedItem.ToString();
            ZoomProc = Convert.ToByte(item.Substring(item.Length-1, 1));
            ProcessedImage.ShowZoomed(image16proc, image8proc, ProcessedImageName, ZoomProc, ZoomProcPrev, SliceROIlist, SliceColorList);
            ZoomProcPrev = ZoomProc;
            label2.Text = ProcessedImage.GetProperties;
        }

        private void label2_Click(object sender, EventArgs e)
        {

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
                SliceMargin2_numericUpDown.Maximum = ProcessedImage.SizeX - 1;
            }
            else if (ProcessedImage != null) // Horizontal
            {
                SliceMargin1_label.Text = "Top";
                SliceMargin2_label.Text = "Bottom";
                SliceMargin1_numericUpDown.Maximum = ProcessedImage.SizeY - 1;
                SliceMargin2_numericUpDown.Maximum = ProcessedImage.SizeY - 1;
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
                int h = ProcessedImage.SizeY - 2;
                Color col = (Color)SliceColor_comboBox.SelectedItem;
                Rectangle sliceROI = new Rectangle(x, 0, w, h);
                string name = "Slice-" + SliceList.Count.ToString();
                CameraImageSlice slice = new CameraImageSlice(ProcessedImage.SrcMat, sliceROI, name, col);
                // Update lists
                SliceList.Add(slice);
                SliceROIlist.Add(sliceROI);
                SliceColorList.Add(col);

                SliceCount_label.Text = "Slice count: " + SliceList.Count.ToString();
                GraphForm1.AddSliceProfile(SliceList.Last().AverageCols(), -slice.Ysize / 2, 1, name, col, true);
            }
            else if (HorV_slice_comboBox.SelectedIndex == 1) // Horizontal
            {
                // Add code here
            }
        }

        private void SliceColor_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ImageHisto_groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void CLAHE_button_Click(object sender, EventArgs e)
        {
            // Contrast Limited Adaptive Histogram Equalization (CLAHE)
            ProcessedImage.CLAHE();
            ProcessedImage.ShowZoomed(image16proc, image8proc, ProcessedImageName, ZoomProc, ZoomProcPrev, SliceROIlist, SliceColorList);
            label2.Text = ProcessedImage.GetProperties;
        }

        private void EqualizeHist_button_Click(object sender, EventArgs e)
        {
            ProcessedImage.EqualizeHisto(image8proc);
            ProcessedImage.ShowZoomed(image16proc, image8proc, ProcessedImageName, ZoomProc, ZoomProcPrev, SliceROIlist, SliceColorList);
            label2.Text = ProcessedImage.GetProperties;
        }

        private void SaveImages_button_Click(object sender, EventArgs e)
        {
            image16proc.Save(ImageFileName + "-16bit.png");
            image8proc.Save(ImageFileName + "-8bit.png");
        }

        private void SliceCount_label_Click(object sender, EventArgs e)
        {

        }

        private void ClearSliceList_button_Click(object sender, EventArgs e)
        {
            SliceList.Clear();
            SliceROIlist.Clear();
            SliceColorList.Clear();

            SliceCount_label.Text = "Slice count: " + SliceList.Count.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void FOV_X_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FOV_Y_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FrameImage_button_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Rescale X and Y of the Processed Image according to the FOV-X and FOV-Y.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyFOV_button_Click(object sender, EventArgs e)
        {
            ProcessedImage.FOVX = Convert.ToDouble(FOV_X_numericUpDown.Value);
            ProcessedImage.FOVY = Convert.ToDouble(FOV_Y_numericUpDown.Value);
            // Do not change the image - its X, Y are in pixels.
            // Rescale ALL slices from the list.
        }
    }
}
