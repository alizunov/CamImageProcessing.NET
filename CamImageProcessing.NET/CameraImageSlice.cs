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

// ROOT.NET
using ROOT;
using ROOTNET;

namespace CamImageProcessing.NET
{
    // Subsection of the image Mat. Primary designation: vertical thin slice to measure the intensity profile.
    // Uses ROOT 1-d histo for handling of profiles.
    // SliceMat shares data with the large source (full-frame) Mat.
    class CameraImageSlice
    {
        // *** Private members ***
        private string SliceName;
        private Rectangle ROI;
        private Color ROIcontourColor;
        private LineType linetype = LineType.EightConnected;
        private int linethickness = 1;

        // *** Properties ***
        public Mat SliceMat
        { get; set; }
        public CameraImage BaseCameraImage
        { get; }

        // ctor
        public CameraImageSlice(CameraImage CamImage, Rectangle rect, string name, Color color)
        {
            BaseCameraImage = CamImage;
            SliceName = name;
            ROI = rect;
            ROIcontourColor = color;
            // Check ROI, warn if wrong but try to create SliceMat hoping that the Mat ctor works safely.
            if (ROI.X<0 || ROI.Y<0 || ROI.Right>CamImage.SrcMat.Cols || ROI.Bottom>CamImage.SrcMat.Rows)
                Console.WriteLine("{0}: warning: wrong ROI. Will try to create the slice Mat anyway. ", MethodBase.GetCurrentMethod().Name);
            try
            {
                SliceMat = new Mat(CamImage.SrcMat, ROI);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could not create the slice Mat. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }

        // Draws rectangle defined by ROI in the base image (16-bit or 8-bit depending on the usage flag). The rectangle is scaled according to the current zoom factor set on the base image.
        public void DrawROIrectangle()
        {
            byte scale = BaseCameraImage.CurrentDownsampleFactor;
            int NewX = (int)ROI.X * scale;
            int NewY = (int)ROI.Y * scale;
            int NewWidth = (int)ROI.Width * scale;
            int NewHeight = (int)ROI.Height * scale;
            Bgr BGRcolor = new Bgr(ROIcontourColor);
            Rectangle ScaledROI = new Rectangle(NewX, NewY, NewWidth, NewHeight);
            if (BaseCameraImage.shouldUse8bit)
            {
                BaseCameraImage.SrcImage8bit.Draw(ScaledROI, BGRcolor, linethickness, linetype);
            }
        }
    // class
    }
// namspace
}
