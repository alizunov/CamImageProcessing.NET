using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;

// Emgu.CV
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

// Graphics
using OxyPlot;
using OxyPlot.Series;

namespace CamImageProcessing.NET
{
    // Subsection of the image Mat. Primary designation: vertical thin slice to measure the intensity profile.
    // SliceMat shares data with the large source (full-frame) Mat.
    // Calls for ROI drawings must be done from the CameraImage method displaying the image window (ShowZoomed())
    // Uses OxyPlot for graphics (NuGet packages inside this project).
    class CameraImageSlice
    {
        // *** Private members ***
        private string SliceName;
        private Rectangle ROI;
        private Color ROIcontourColor;
        private LineType linetype = LineType.EightConnected;
        private int linethickness = 1;

        // *** Properties ***
        public Mat BaseMat
        { get; }
        public Mat SliceMat
        { get; set; }

        public Image<Bgr, UInt16> BaseImage16
        { get; set; }

        public Image<Bgr,byte> BaseImage8
        { get; set; }

        // *** OxyPlot properties ***
        public PlotModel SliceCanvas
        { get; set; }

        // ctor
        public CameraImageSlice(Mat mat, Image<Bgr, UInt16> img16, Image<Bgr, byte> img8, Rectangle rect, string name, Color color)
        {
            BaseMat = mat;
            BaseImage16 = img16;
            BaseImage8 = img8;
            SliceName = name;
            ROI = rect;
            ROIcontourColor = color;
            // Check ROI, warn if wrong but try to create SliceMat hoping that the Mat ctor works safely.
            if (ROI.X<0 || ROI.Y<0 || ROI.Right>BaseMat.Cols || ROI.Bottom>BaseMat.Rows)
                Console.WriteLine("{0}: warning: wrong ROI. Will try to create the slice Mat anyway. ", MethodBase.GetCurrentMethod().Name);
            try
            {
                SliceMat = new Mat(BaseMat, ROI);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could not create the slice Mat. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }
            //SliceCanvas = new PlotModel { Title = "Slice canvas" };

        }

        // Draws rectangle defined by ROI in the base image (16-bit or 8-bit depending on the usage flag). The rectangle is scaled according to the current zoom factor set on the base image.
        public void DrawROIrectangle(bool is8bit, Image<Bgr, UInt16> img16, Image<Bgr, byte> img8, int scale)
        {
            try
            {
                int NewX = (int)ROI.X / scale;
                int NewY = (int)ROI.Y / scale;
                int NewWidth = (int)ROI.Width / scale;
                int NewHeight = (int)ROI.Height / scale;
                Console.WriteLine("{0}: ROI-X = {1}, ROI-Y = {2}, ROI-W = {3}, ROI-H = {4}, scale = {5}. ", MethodBase.GetCurrentMethod().Name, NewX, NewY, NewWidth, NewHeight, scale);
                Bgr BGRcolor = new Bgr(ROIcontourColor);
                Rectangle ScaledROI = new Rectangle(NewX, NewY, NewWidth, NewHeight);
                if (is8bit)
                    img8.Draw(ScaledROI, BGRcolor, linethickness, linetype);
                else
                    img16.Draw(ScaledROI, BGRcolor, linethickness, linetype);
           }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could not create ROI or draw rectangle: scale = {1}. " + ex.Message, MethodBase.GetCurrentMethod().Name, scale);
            }

        }
    // class
    }
// namspace
}
