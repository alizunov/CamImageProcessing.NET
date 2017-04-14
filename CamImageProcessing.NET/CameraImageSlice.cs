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

namespace CamImageProcessing.NET
{
    // Subsection of the image Mat. Primary designation: vertical thin slice to measure the intensity profile.
    // SliceMatrix shares data with the large source (full-frame) Mat.
    // Calls for ROI drawings must be done from the CameraImage method displaying the image window (ShowZoomed())
    // Uses ZedGraph for graphics (NuGet packages inside this project).
    class CameraImageSlice
    {
        // *** Private members ***
        private Matrix<double> SliceMatrix;
        private string SliceName;

        // *** Properties ***
        public Rectangle ROI
        { get; set; }
        public Color ROIcontourColor
        { get; set; }

        public int Xsize
        { get; set; }

        public int Ysize
        { get; set; }

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
                SliceMatrix = new Matrix<double>(rect.Height, rect.Width);
                using (Mat ROImat = new Mat(mat, ROI))
                {
                    ROImat.ConvertTo(SliceMatrix, DepthType.Cv64F);
                    Xsize = SliceMatrix.Cols;
                    Ysize = SliceMatrix.Rows;
                }
                Console.WriteLine("{0}: Slice Matrix Xsize = {1}, Ysize = {2} ", MethodBase.GetCurrentMethod().Name, Xsize, Ysize);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could not create the slice Mat. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }

        }



        /// <summary>
        /// Averages columns like following: averaged_column = sum(columns)/Ncolumns, returns List<double>.
        /// </summary>
        /// <returns></returns>
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
    
        /// <summary>
        /// Averages rows like following: averaged_row = sum(rows)/Nrows, returns List<double>.
        /// </summary>
        /// <returns></returns>
        public List<double> AverageRows()
        {
            List<double> averagedList = new List<double>();
            double v = 0;
            for (int icol=0; icol<Xsize; icol++)
            {
                v = 0;
                for (int irow = 0; irow < Ysize; irow++)
                    v += SliceMatrix[irow, icol];
                averagedList.Add(v / Ysize);
            }
            return averagedList;
        }
    
        
        
        // class
    }
// namespace
}
