using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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

        // Original image, see CameraImage class desc.
        CameraImage OrigImage;
        // Processed image
        CameraImage ProcessedImage;

        public MainControlWindow()
        {
            InitializeComponent();
            // Show header button
            this.button2.Enabled = false;
            // Open image button
            this.button3.Enabled = false;
            // Zoom factor for displaying original image combobox
            comboBox1.Name = "Zoom image";
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
        }

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
                                OrigImage = new CameraImage((Int32)HeaderOrigImage.SizeY, (Int32)HeaderOrigImage.SizeX, 1, ImageReadout, "Original Image");
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
                                ProcessedImage = OrigImage.Clone("Processed Image");
                                if (ProcessedImage.SizeX == OrigImage.SizeX && ProcessedImage.SizeY == OrigImage.SizeY)
                                {
                                    //MessageBox.Show("CameraImage OK; sizes: " + OrigImage.SizeX + ", " + OrigImage.SizeY, "", MessageBoxButtons.OK);
                                    groupBox1.Enabled = true;
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
            ProcessedImage.ApplyColorMap(ColorMapType.Hot);
            //ProcessedImage.ApplyColorMap((ColorMapType)ColorMapCombobox.SelectedItem);

        }
        // Group of controls for processing image
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ProcessedImage.ShowZoomed(2);
        }
    }
}
