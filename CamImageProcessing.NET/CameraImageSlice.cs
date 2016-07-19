using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;

// Emgu.CV
using Emgu.CV;

// ROOT.NET
using ROOT;
using ROOTNET;

namespace CamImageProcessing.NET
{
    // Subsection of the image Mat. Primary designation: vertical thin slice to measure the intensity profile.
    // Uses ROOT 1-d histo for handling of profiles.
    class CameraImageSlice
    {
        private string SliceName;
        private Rectangle ROI;

        public Mat SliceMat
        { get; }

        // ctor
        public CameraImageSlice(Mat mat, Rectangle rect, string name)
        {
            SliceName = name;
            ROI = rect;
            // Check ROI, warn if wrong but try to create SliceMat hoping that the Mat ctor works safely.
            if (ROI.X<0 || ROI.Y<0 || ROI.Right>mat.Cols || ROI.Bottom>mat.Rows)
                Console.WriteLine("{0}: warning: wrong ROI. Will try to create the slice Mat. ", MethodBase.GetCurrentMethod().Name);
            try
            {
                SliceMat = new Mat(mat, ROI);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could not create the slice Mat. " + ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
