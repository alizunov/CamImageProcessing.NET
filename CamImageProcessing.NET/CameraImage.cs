using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// EmguCV namespaces
/// </summary>
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace CamImageProcessing.NET
{
    class CameraImage
    {
        // *** Properties ***
        // Image
        public Mat SrcMat
        { get; set; }

        // Currently only 1 channel (grayscale) implemented in most methods.
        public Int32 Nchannels
        { get; set; }

        public DepthType Depth
        { get; set; }

        public List<double> minList
        { get; set; }

        public List<double> maxList
        { get; set; }

        public List<Point> minLocationsList
        { get; set; }
        public List<Point> maxLocationsList
        { get; set; }

        // *** Methods ***
        // Constructors
        // ctor
        public CameraImage(Int32 SizeY, Int32 SizeX, Int32 Nch, List<UInt16> Data, string ImageName)
        {
            Nchannels = Nch;
            SrcMat = new Mat(SizeY, SizeX, DepthType.Cv16U, Nch);
            System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(Data.ToArray(), System.Runtime.InteropServices.GCHandleType.Pinned);
            using (Mat mat = new Mat(SizeY, SizeX, DepthType.Cv16U, Nchannels, handle.AddrOfPinnedObject(), SizeX * 2))
            {
                handle.Free();
                SrcMat = mat.Clone();
            }
            minList = new List<double>(Nchannels);
            maxList = new List<double>(Nchannels);
            minLocationsList = new List<Point>(Nchannels);
            maxLocationsList = new List<Point>(Nchannels);
            Depth = SrcMat.Depth;
            // Fill lists of min / max and location values per channel
            MinMax();
        }

        // ctor without copying data
        public CameraImage(Int32 SizeY, Int32 SizeX, Int32 Nch, string ImageName)
        {
            Nchannels = Nch;
            SrcMat = new Mat(SizeY, SizeX, DepthType.Cv16U, Nch);
            minList = new List<double>(Nchannels);
            maxList = new List<double>(Nchannels);
            minLocationsList = new List<Point>(Nchannels);
            maxLocationsList = new List<Point>(Nchannels);
            Depth = SrcMat.Depth;
            // Fill lists of min / max and location values per channel
            MinMax();
        }

        // Other methods
        // X-size of Mat
        public Int32 SizeX
        {
            get
            {
                return (Int32)SrcMat.Cols;
            }
        }
        // Y-size of Mat
        public Int32 SizeY
        {
            get
            {
                return (Int32)SrcMat.Rows;
            }
        }
        //Number of elements
        public Int32 TotalPixelNumber
        {
            get
            {
                return (Int32)SrcMat.Total;
            }
        }
        // Fills lists of min/max and locations
        private void MinMax()
        {
            // Works for up to THREE channels !!!
            double[] mins = {0, 0, 0};
            double[] maxs = {0, 0, 0};
            Point[] minLocs = new Point[3];
            Point[] maxLocs = new Point[3];
            try
            {
                SrcMat.MinMax(out mins, out maxs, out minLocs, out maxLocs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: Could not make zoomed image. Original error: " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            minList.Clear();
            maxList.Clear();
            minLocationsList.Clear();
            maxLocationsList.Clear();
            for (int i=0; i<Nchannels; i++)
            {
                minList.Add(mins[i]);
                maxList.Add(maxs[i]);
                minLocationsList.Add(minLocs[i]);
                maxLocationsList.Add(maxLocs[i]);
            }

        }
        //Returns some parameters as a multi-line string.
        public string GetProperties
        {
            get
            {
                string gen = "Size: " + this.SizeX.ToString() + "x" + this.SizeY.ToString() + "\n" +
                    "channels: " + Nchannels.ToString() + "\n" +
                    "Depth: " + Depth.ToString() + "\n";
                string mins = "Min. values: ";
                string maxs = "Max. values: ";
                string minLocs = "Min. locations: ";
                string maxLocs = "Max. locations";
                for (int i=0; i<Nchannels; i++)
                {
                    mins += minList.ElementAt(i) + " ";
                    maxs += maxList.ElementAt(i) + " ";
                    minLocs += minLocationsList.ElementAt(i) + " ";
                    maxLocs += maxLocationsList.ElementAt(i) + " ";
                }
                return gen + "\n" + mins + "\n" + maxs + "\n" + minLocs + "\n" + maxLocs;
            }
        }
        // Closes all named windows
        public void CloseAllWindows()
        {
            CvInvoke.DestroyAllWindows();
        }

        // Copies Mat to 16-bit image
        public void Image(Image<Bgr, UInt16> image16)
        {
            image16.SetZero();
            SrcMat.CopyTo(image16);
        }

        // Copies Mat to 8-bit image
        public void Image(Image<Bgr, byte> image8)
        {
            image8.SetZero();
            // Mat has 16-bit depth, so we need a 16-bit image first
            using (Mat mat8 = new Mat(SrcMat.Rows, SrcMat.Cols, DepthType.Cv8U, Nchannels))
            {
                SrcMat.ConvertTo(mat8, DepthType.Cv8U, 0.00390625, 0);  // scale = 1/256
                mat8.CopyTo(image8);
            }
        }

        // Shows zoomed (downsampled) image with one of preset factors from the AllowedDownsampleFactor enum.
        // Displayed (optonally scaled) picture is of EMGU Image class to allow drawing of geometric elements like a ROI rectangle.
        public void ShowZoomed(Image<Bgr, UInt16> image16, Image<Bgr, byte> image8, string ImageName, byte zoom, byte PrevZoom, List<Rectangle> SliceROI, List<Color> SliceColor)
        {
            byte[] AllowedDownsampleFactor = { 1, 2, 4, 8 };
            bool isAllowableZoom = false;

            foreach (byte iF in AllowedDownsampleFactor)
            {
                isAllowableZoom = (zoom == iF) ? true : false;
                if (isAllowableZoom) break;
            }
            // Check if image sizes are devisible by the zoom.
            if (SrcMat.Cols % zoom != 0 || SrcMat.Rows % zoom != 0)
            {
                Console.WriteLine("{0}: wrong zoom factor {1}: {2} and/or {3} are not multiple", MethodBase.GetCurrentMethod().Name, zoom, SrcMat.Cols, SrcMat.Rows);
                isAllowableZoom = false;
            }

            if (!isAllowableZoom)
            {
                Console.WriteLine("{0}: wrong zoom factor ", zoom);
                return;
            }

            Int32 ZoomedSizeX = (Int32)SrcMat.Cols / zoom;
            Int32 ZoomedSizeY = (Int32)SrcMat.Rows / zoom;
            try
            {
                Console.WriteLine("{0}: zoomed image sizes: {1}x{2} ", MethodBase.GetCurrentMethod().Name, ZoomedSizeX, ZoomedSizeY);
                Size DstSize = new Size(ZoomedSizeX, ZoomedSizeY);
                // Zoom = (1) clone Mat to Image; (2) resize Image; (3) Display Image
                if (image8 != null)
                {
                    using (Image<Bgr, byte> tmpImage = new Image<Bgr, byte>(image8.Cols, image8.Rows))
                    {
                        // Draw slice ROI
                        if (SliceROI != null)
                            foreach (Rectangle rect in SliceROI)
                            {
                                Bgr BGRcolor = new Bgr(SliceColor.ElementAt(SliceROI.IndexOf(rect)));
                                image8.Draw(rect, BGRcolor, 1, LineType.EightConnected);
                            }
                        CvInvoke.Resize(image8, tmpImage, DstSize, 0, 0);
                        // Destroy previous zoom if the window name is not empty
                        CvInvoke.DestroyWindow(ImageName + ", " + "byte" + ", scale 1:" + PrevZoom.ToString());
                        // Create new zoom
                        CvInvoke.NamedWindow(ImageName + ", " + "byte" + ", scale 1:" + zoom.ToString());             //Create the window using the specific name
                        CvInvoke.Imshow(ImageName + ", " + "byte" + ", scale 1:" + zoom.ToString(), tmpImage);          //Show the image
                    }
                }

                if (image16 != null)
                {
                    using (Image<Bgr, UInt16> tmpImage = new Image<Bgr, UInt16>(image16.Cols, image16.Rows))
                    {
                        // Draw slice ROI
                        if (SliceROI != null)
                            foreach (Rectangle rect in SliceROI)
                            {
                                Bgr BGRcolor = new Bgr(SliceColor.ElementAt(SliceROI.IndexOf(rect)));
                                image16.Draw(rect, BGRcolor, 1, LineType.EightConnected);
                            }
                        CvInvoke.Resize(image16, tmpImage, DstSize, 0, 0);
                        // Destroy previous zoom if the window name is not empty
                        CvInvoke.DestroyWindow(ImageName + ", " + "UInt16" + ", scale 1:" + PrevZoom.ToString());
                        // Create new zoom
                        CvInvoke.NamedWindow(ImageName + ", " + "UInt16" + ", scale 1:" + zoom.ToString());             //Create the window using the specific name
                        CvInvoke.Imshow(ImageName + ", " + "UInt16" + ", scale 1:" + zoom.ToString(), tmpImage);          //Show the image
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: Could not make zoomed image. Original error: " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }

        }

        // Copy method: copies Mat and updates min/max lists
        public void Copy(Mat mat)
        {
            //SrcMat.Dispose();
            mat.CopyTo(SrcMat);
            MinMax();
        }

        // Clone method
        public void Clone(Mat mat)
        {
            SrcMat = mat.Clone();
            MinMax();
        }
        // Get pixel value by its coordinates
        // WARNING: creates a new array of the full size!
        public UInt16 GetPixelValue(Point pixel)
        {
            UInt16 pv = 0;
            using (Matrix<UInt16> MatArr = new Matrix<UInt16>(SrcMat.Size))
            {
                SrcMat.CopyTo(MatArr);
                // Check ranges and align if necessary 
                pixel.X = Math.Abs(pixel.X);
                pixel.Y = Math.Abs(pixel.Y);
                pixel.X = (pixel.X >= SrcMat.Cols) ? SrcMat.Cols - 1 : pixel.X;
                pixel.Y = (pixel.Y >= SrcMat.Rows) ? SrcMat.Rows - 1 : pixel.Y;
                if (MatArr.Size != SrcMat.Size)
                    pv = 0;
                else
                    pv = MatArr[pixel.Y, pixel.X];
            }
            return pv;
        }

        // ****************************************
        // Methods for processing (changing) images
        // ****************************************

        // CvInvoke.ApplyColorMap() expects 8-bit image, otherwise returns original one (see docs).
        public void ApplyColorMap(Image<Bgr, byte> image8, ColorMapType ColorMap)
        {
            CvInvoke.ApplyColorMap(image8, image8, ColorMap);
        }
 
        // Enhancement of contrast and brightness by equalization of the 8-bit image histogram
        // Sequence: bgr_8-bit -> gray_8-bit -> equalization -> back
        public void EqualizeHisto(Image<Bgr, byte> image8)
        {
            using (Image<Gray, byte> imageGray = new Image<Gray, byte>(image8.Cols, image8.Rows))
            {
                try
                {
                    //CvInvoke.CvtColor(image8, imageGray, ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(image8, imageGray);
                    CvInvoke.CvtColor(imageGray, image8, ColorConversion.Gray2Bgr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}: Error: could equalize the 8-bit image histogram. " + ex.Message, MethodBase.GetCurrentMethod().Name);
                }
            }

        }

        // *** Methods modifying Mat ***
        // Linear transform NewMat = (OldMat - Offset)*Scale
        // Converts Mat to Cv32F type, otherwise AbsDiff() and AddWeighted() are not working
        public void OffsetAndScale(UInt16 offset)
        {
            try
            {
                double scale = System.Math.Abs(65535 / (maxList.ElementAt(0) - offset));
                using (Mat tmpMat = SrcMat.Clone())
                {
                    using (Mat tmpMat2 = SrcMat.Clone())
                    {
                        // Temp. Mat with all pixels set to Offset value
                        SrcMat.ConvertTo(tmpMat2, DepthType.Cv32F, 0, (double)offset);
                        SrcMat.ConvertTo(tmpMat, DepthType.Cv32F);
                        CvInvoke.AbsDiff(tmpMat, tmpMat2, tmpMat);
                        //CvInvoke.AddWeighted(tmpMat, 1, tmpMat, 0, 0, tmpMat2, DepthType.Cv32F);
                        CvInvoke.AddWeighted(tmpMat, scale, tmpMat, 0, 0, tmpMat2, DepthType.Cv32F);
                        //Console.WriteLine("{0}: Mat from scalar {1}: min = {2}, max = {3}. ", MethodBase.GetCurrentMethod().Name, offset, tmpMat2.GetValueRange().Min, tmpMat2.GetValueRange().Max );
                        //Console.WriteLine("{0}: tmpMat Depth: {1}, channels: {2} ", MethodBase.GetCurrentMethod().Name, tmpMat.Depth, tmpMat.NumberOfChannels );
                        Console.WriteLine("{0}: offset: {1}, scale factor: {2} ", MethodBase.GetCurrentMethod().Name, offset, scale);
                        tmpMat2.ConvertTo(SrcMat, DepthType.Cv16U);
                        // Call MinMax() to update min/max lists
                        MinMax();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could make offset subtraction and scaling. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            
        }

        // Contrast Limited Adaptive Histogram Equalization (CLAHE)
        // (applied on Mat)
        public void CLAHE()
        {
            // See documentation, default = 40
            double cliplimit = 40;
            // See documentation, default = 8x8
            Size tileGridSize = new Size(8, 8);
            try
            {
                CvInvoke.CLAHE(SrcMat, cliplimit, tileGridSize, SrcMat);
                MinMax();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could make CLAHE on SrcMat. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }

        }

    }
}
