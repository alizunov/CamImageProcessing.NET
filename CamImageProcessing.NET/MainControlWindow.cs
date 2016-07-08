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

        // Mat of original and processed images.
        Mat MatOrigImage;

        public MainControlWindow()
        {
            InitializeComponent();
            this.button2.Enabled = false;
            this.button3.Enabled = false;
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
                                while (br1.BaseStream.Position != br1.BaseStream.Length)
                                {
                                    ImageReadout.Add( br1.ReadUInt16() );
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
                                UInt16[] ArrayOrigImage = ImageReadout.ToArray();
                                System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(ArrayOrigImage, System.Runtime.InteropServices.GCHandleType.Pinned);
                                MatOrigImage = new Mat((int)HeaderOrigImage.SizeY, (int)HeaderOrigImage.SizeX, DepthType.Cv16U, 1, handle.AddrOfPinnedObject(), (int)HeaderOrigImage.SizeX*2);
                                handle.Free();
                                string WindowImage1 = "Original Image";  //The name of the window
                                CvInvoke.NamedWindow(WindowImage1);      //Create the window using the specific name
                                CvInvoke.Imshow(WindowImage1, MatOrigImage); //Show the image
                                CvInvoke.WaitKey(0);  //Wait for the key pressing event
                                CvInvoke.DestroyWindow(WindowImage1); //Destroy the window if key is pressed

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

    }
}
