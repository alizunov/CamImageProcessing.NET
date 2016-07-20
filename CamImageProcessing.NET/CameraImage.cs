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
        // *** Private members ***
        private byte[] AllowedDownsampleFactor = { 1, 2, 4, 8 };
        private string ImageNameBase;
        // Parameters of a linear transform NewMat = (OldMat - Offset)*Scale
        private UInt16 Offset;
        private double Scale;

        // *** Properties ***
        // Main source Mat
        public Mat SrcMat
        { get; set; }

        // Image clone of SrcMat to show up. Can be changed by CameraImageSlice methods.
        public Image<Bgr, UInt16> SrcImage
        { get; set; }
        
        // Image clone of SrcMat converted to the 8-bit depth to show up. Can be changed by CameraImageSlice methods.
        public Image<Bgr, byte> SrcImage8bit
        { get; set; }

        // Slice for profiling
        public CameraImageSlice Slice
        { get; set; }
        public byte CurrentDownsampleFactor
        { get; set; }
        
        // Currently only 1 channel (grayscale) implemented in most methods.
        public Int32 Nchannels
        { get; set; }

        public DepthType Depth
        { get; set; }

        public bool shouldUse8bit
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
            System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(Data.ToArray(), System.Runtime.InteropServices.GCHandleType.Pinned);
            SrcMat = new Mat(SizeY, SizeX, DepthType.Cv16U, Nchannels, handle.AddrOfPinnedObject(), SizeX * 2);
            handle.Free();
            minList = new List<double>(Nchannels);
            maxList = new List<double>(Nchannels);
            minLocationsList = new List<Point>(Nchannels);
            maxLocationsList = new List<Point>(Nchannels);
            Depth = SrcMat.Depth;
            // Fill lists of min / max and location values per channel
            MinMax();
            ImageNameBase = ImageName;
            // 0 - to activate zoom 1:1
            CurrentDownsampleFactor = 1;
            // Create the Image
            SrcImage = SrcMat.ToImage<Bgr, UInt16>();
            //SrcImage8bit = SrcImage.Convert<Bgr, byte>();
        }
        // ctor which clones Mat
        public CameraImage(Mat RefMat, string ImageName)
        {
            SrcMat = RefMat.Clone();

            Nchannels = RefMat.NumberOfChannels;
            minList = new List<double>(Nchannels);
            maxList = new List<double>(Nchannels);
            minLocationsList = new List<Point>(Nchannels);
            maxLocationsList = new List<Point>(Nchannels);
            Depth = SrcMat.Depth;
            // Fill lists of min / max and location values per channel
            MinMax();
            ImageNameBase = ImageName;
            // 0 - to activate zoom 1:1
            CurrentDownsampleFactor = 1;
            // Create the Images
            SrcImage = SrcMat.ToImage<Bgr, UInt16>();
            SrcImage8bit = SrcImage.Convert<Bgr, byte>();
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
                    "Depth: " + Depth.ToString() + "\n" +
                    "Zoom 1:" + CurrentDownsampleFactor.ToString();
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
        // Shows zoomed (downsampled) image with one of preset factors from the AllowedDownsampleFactor enum.
        // Displayed (optonally scaled) picture is of EMGU Image class to allow drawing of geometric elements like a ROI rectangle.
        public void ShowZoomed(byte zoom)
        {
            bool isAllowableZoom = false;
            string DisplayWindowName;
            string CurrentDepth;
            // zoom = 0 : only show 16-bit or 8-bit image, no scaling, no destroying previous window.
            // This is used, for instance, after creating of lines/rectangles/etc. in Slice methods.
            // The window must exists with the name DisplayWindowName !
            if (zoom == 0)
            {
                try
                {
                    if (shouldUse8bit)
                        CvInvoke.Imshow(ImageNameBase + ", " + "byte" + ", scale 1:" + CurrentDownsampleFactor.ToString(), SrcImage8bit);          //Show the image 8-bit
                    else
                        CvInvoke.Imshow(ImageNameBase + ", " + "UInt16" + ", scale 1:" + CurrentDownsampleFactor.ToString(), SrcImage);          //Show the image

                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}: Error: Could not update existing window. Original error: " + ex.Message, MethodBase.GetCurrentMethod().Name);
                }
                return;

            }
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
                if (shouldUse8bit)
                {
                    Image<Bgr, byte> tmpImage = SrcImage8bit;
                    CvInvoke.Resize(tmpImage, tmpImage, DstSize, 0, 0);
                    CurrentDepth = "byte";
                    // Destroy previous zoom if the window name is not empty
                    CvInvoke.DestroyWindow(ImageNameBase + ", " + CurrentDepth + ", scale 1:" + CurrentDownsampleFactor.ToString());
                    // Create new zoom
                    DisplayWindowName = ImageNameBase + ", " + CurrentDepth + ", scale 1:" + zoom.ToString();
                    CvInvoke.NamedWindow(DisplayWindowName);             //Create the window using the specific name
                    // Draw slice (ROI) rectangle
                    if (Slice != null)
                        Slice.DrawROIrectangle(true);
                    CvInvoke.Imshow(DisplayWindowName, tmpImage);          //Show the image
                }
                else
                {
                    Image<Bgr, UInt16> tmpImage = SrcImage;
                    CvInvoke.Resize(tmpImage, tmpImage, DstSize, 0, 0);
                    CurrentDepth = "UInt16";
                    // Destroy previous zoom if the window name is not empty
                    CvInvoke.DestroyWindow(ImageNameBase + ", " + CurrentDepth + ", scale 1:" + CurrentDownsampleFactor.ToString());
                    // Create new zoom
                    DisplayWindowName = ImageNameBase + ", " + CurrentDepth + ", scale 1:" + zoom.ToString();
                    CvInvoke.NamedWindow(DisplayWindowName);             //Create the window using the specific name
                    if (Slice != null)
                        Slice.DrawROIrectangle(false);
                    CvInvoke.Imshow(DisplayWindowName, tmpImage);          //Show the image
                }
                CurrentDownsampleFactor = zoom;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: Could not make zoomed image. Original error: " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }

        }
        // Clone method: Clone() for Mat and copy for other members
        public CameraImage Clone(string NewName)
        {
            CameraImage NewImage = new CameraImage(SrcMat, NewName);
            return NewImage;
        }
        // Get pixel value by its coordinates
        // WARNING: creates a new array of the full size!
        public UInt16 GetPixelValue(Point pixel)
        {
            Matrix<UInt16> MatArr;
            MatArr = new Matrix<UInt16>(SrcMat.Size);
            SrcMat.CopyTo(MatArr);
            UInt16 pv = 0;
            // Check ranges and align if necessary 
            pixel.X = Math.Abs(pixel.X);
            pixel.Y = Math.Abs(pixel.Y);
            pixel.X = (pixel.X >= SrcMat.Cols) ? SrcMat.Cols - 1 : pixel.X;
            pixel.Y = (pixel.Y >= SrcMat.Rows) ? SrcMat.Rows - 1 : pixel.Y;
            if (MatArr.Size != SrcMat.Size)
                pv = 0;
            else
                pv = MatArr[pixel.Y, pixel.X];
            MatArr.Dispose();
            return pv;
        }
        // ****************************************
        // Methods for processing (changing) images
        // ****************************************

        // Creates a 8-bit copy of the SrcImage
        // SrcMat and SrcImage are not changed
        public void ConvertTo8bit()
        {
            try
            {
                SrcImage8bit = SrcImage.ConvertScale<byte>(0.00390625, 0);  // scale = 1/256
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could make convert SrcImage to a 8-bit depth. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }

        }
        // CvInvoke.ApplyColorMap() expects 8-bit image, otherwise returns original one.
        // First calls ConvertTo8bit(), then applies a new color map.
        public void ApplyColorMap(ColorMapType ColorMap)
        {
            ConvertTo8bit();
            CvInvoke.ApplyColorMap(SrcImage8bit, SrcImage8bit, ColorMap);
        }
        // !!! OffsetAndScale() modifies the SrcMat itself !!!
        // Linear transform NewMat = (OldMat - Offset)*Scale
        // Converts Mat to Cv32F type, otherwise AbsDiff() and AddWeighted() are not working
        public void OffsetAndScale(UInt16 offset)
        {
            try
            {
                Offset = offset;
                Scale = System.Math.Abs(65535 / (maxList.ElementAt(0) - Offset));
                Mat tmpMat = SrcMat.Clone();
                Mat tmpMat2 = SrcMat.Clone();
                // Temp. Mat with all pixels set to Offset value
                SrcMat.ConvertTo(tmpMat2, DepthType.Cv32F, 0, (double)offset);
                SrcMat.ConvertTo(tmpMat, DepthType.Cv32F);
                CvInvoke.AbsDiff(tmpMat, tmpMat2, tmpMat);
                //CvInvoke.AddWeighted(tmpMat, 1, tmpMat, 0, 0, tmpMat2, DepthType.Cv32F);
                CvInvoke.AddWeighted(tmpMat, Scale, tmpMat, 0, 0, tmpMat2, DepthType.Cv32F);
                //Console.WriteLine("{0}: Mat from scalar {1}: min = {2}, max = {3}. ", MethodBase.GetCurrentMethod().Name, offset, tmpMat2.GetValueRange().Min, tmpMat2.GetValueRange().Max );
                //Console.WriteLine("{0}: tmpMat Depth: {1}, channels: {2} ", MethodBase.GetCurrentMethod().Name, tmpMat.Depth, tmpMat.NumberOfChannels );
                Console.WriteLine("{0}: offset: {1}, scale factor: {2} ", MethodBase.GetCurrentMethod().Name, offset, Scale);
                tmpMat2.ConvertTo(SrcMat, DepthType.Cv16U);
                // Call MinMax() to update min/max lists
                MinMax();

                tmpMat.Dispose();
                tmpMat2.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could make offset subtraction and scaling. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            
        }

        // ### Slice methods ###
        public void CreateSlice(Rectangle ROI, string SliceName, Color SliceColor)
        {
            Slice = new CameraImageSlice(SrcMat, SrcImage, SrcImage8bit, ROI, SliceName, SliceColor);
        }
    }
}
