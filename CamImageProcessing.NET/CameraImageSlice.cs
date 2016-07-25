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
using ZedGraph;

namespace CamImageProcessing.NET
{
    // Subsection of the image Mat. Primary designation: vertical thin slice to measure the intensity profile.
    // SliceMatrix shares data with the large source (full-frame) Mat.
    // Calls for ROI drawings must be done from the CameraImage method displaying the image window (ShowZoomed())
    // Uses ZedGraph for graphics (NuGet packages inside this project).
    class CameraImageSlice
    {
        // *** Private members ***
        private string SliceName;
        private Rectangle ROI;
        private Color ROIcontourColor;
        private Emgu.CV.CvEnum.LineType linetype = Emgu.CV.CvEnum.LineType.EightConnected;
        private int linethickness = 1;

        // *** Properties ***
        public Matrix<double> SliceMatrix
        { get; set; }

        public int Xsize
        { get; set; }

        public int Ysize
        { get; set; }

        // *** ZedGraph properties ***

        // ctor
        public CameraImageSlice(Mat mat, Rectangle rect, string name, Color color)
        {
            SliceName = name;
            ROI = rect;
            ROIcontourColor = color;
            // Check ROI, warn if wrong but try to create SliceMat hoping that the Mat ctor works safely.
            if (ROI.X<0 || ROI.Y<0 || ROI.Right>mat.Cols || ROI.Bottom>mat.Rows)
                Console.WriteLine("{0}: warning: wrong ROI. Will try to create the slice Mat anyway. ", MethodBase.GetCurrentMethod().Name);
            try
            {
                //Mat SliceMat = new Mat(BaseMat, ROI);

                // ROI matrix (to where data will be copied)
                SliceMatrix = new Matrix<double>(rect.Height, rect.Width);
                SliceMatrix.SetZero();
                // Mask for copy operation: non-zero = ROI
                Matrix<double> CopyMask = new Matrix<double>(mat.Rows, mat.Cols);
                CopyMask.SetZero();
                for (int irow = rect.Top; irow < rect.Bottom; irow++)
                    for (int icol = rect.Left; icol < rect.Right; icol++)
                        CopyMask[irow, icol] = 1;
                // Copy data according to the mask
                mat.CopyTo(SliceMatrix, CopyMask);
                Xsize = SliceMatrix.Cols;
                Ysize = SliceMatrix.Rows;
                Console.WriteLine("{0}: Slice Matrix Xsize = {1}, Ysize = {2} ", MethodBase.GetCurrentMethod().Name, Xsize, Ysize);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could not create the slice Mat. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }

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

        // Averages columns like averaged_column = sum(columns)/Ncolumns, returns array.
        public List<double> AverageCols()
        {
            List<double> averagedList = new List<double>();
            double v = 0;
            for (int irow=0; irow<Ysize; irow++)
            {
                v = 0;
                for (int icol = 0; icol < Xsize; icol++)
                    v += SliceMatrix[irow, icol];
                averagedList.Add(v / Xsize);
            }
            return averagedList;
        }
    // class
    }
// namspace
}
