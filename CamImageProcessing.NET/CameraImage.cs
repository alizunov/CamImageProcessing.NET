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
        private Mat SrcMat;
        private byte[] AllowedDownsampleFactor = { 1, 2, 4, 8 };
        private byte CurrentDownsampleFactor;
        private string ImageNameBase;

        // Currently only 1 channel (grayscale) implemented in most methods.
        public Int32 Nchannels;
        public List<double> minList;
        public List<double> maxList;
        public List<Point> minLocationsList;
        public List<Point> maxLocationsList;

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
            // Fill lists of min / max and location values per channel
            this.MinMax();

            ImageNameBase = ImageName;
            // 0 - to activate zoom 1:1
            CurrentDownsampleFactor = 0;
        }
        // ctor which clones Mat
        public CameraImage(Mat RefMat, string ImageName)
        {
            SrcMat = RefMat.Clone();

            minList = new List<double>(Nchannels);
            maxList = new List<double>(Nchannels);
            minLocationsList = new List<Point>(Nchannels);
            maxLocationsList = new List<Point>(Nchannels);
            // Fill lists of min / max and location values per channel
            this.MinMax();

            ImageNameBase = ImageName;
            // 0 - to activate zoom 1:1
            CurrentDownsampleFactor = 0;
        }
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
        // Shows zoomed (downsampled) image with one of preset factors from the AllowedDownsampleFactor enum.
        public void ShowZoomed(byte zoom)
        {
            bool isAllowableZoom = false;
            string ShowImageName;

            //Console.WriteLine("{0}: start ", MethodBase.GetCurrentMethod().Name);
            //CvInvoke.WaitKey();

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

            if (zoom == CurrentDownsampleFactor || !isAllowableZoom)
            {
                Console.WriteLine("{0}: wrong zoom factor or the image already zoomed to this factor ", zoom);
                return;
            }

            Int32 ZoomedSizeX = (Int32)SrcMat.Cols / zoom;
            Int32 ZoomedSizeY = (Int32)SrcMat.Rows / zoom;
            try
            {
                Console.WriteLine("{0}: zoomed image sizes: {1}x{2} ", MethodBase.GetCurrentMethod().Name, ZoomedSizeX, ZoomedSizeY);
                Mat DstMat = new Mat(ZoomedSizeY, ZoomedSizeX, DepthType.Cv16U, Nchannels);
                Size DstSize = new Size(ZoomedSizeX, ZoomedSizeY);
                //CvInvoke.PyrDown(SrcMat, DstMat);
                CvInvoke.Resize(SrcMat, DstMat, DstSize, 0, 0);
                // Destroy previous zoom
                ShowImageName = ImageNameBase + ", scale 1:" + CurrentDownsampleFactor.ToString();
                CvInvoke.DestroyWindow(ShowImageName);
                // Create new zoom
                ShowImageName = ImageNameBase + ", scale 1:" + zoom.ToString();
                CvInvoke.NamedWindow(ShowImageName);             //Create the window using the specific name
                CvInvoke.Imshow(ShowImageName, DstMat);          //Show the image
                CurrentDownsampleFactor = zoom;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Could not make zoomed image. Original error: " + ex.Message);
            }

        }
        // Clone method: Clone() for Mat and copy for other members
        public CameraImage Clone(string NewName)
        {
            CameraImage NewImage = new CameraImage(SrcMat, NewName);
            return NewImage;
        }
        // ****************************************
        // Methods for processing (changing) images
        // ****************************************

        // This method modifies the source Mat!
        public void ApplyColorMap(ColorMapType ColorMap)
        {
            Mat tmpMat = SrcMat.Clone();
            SrcMat.ConvertTo(tmpMat, DepthType.Cv8U, 0.00390625);
            CvInvoke.ApplyColorMap(tmpMat, SrcMat, ColorMap);
        }
    }
}
