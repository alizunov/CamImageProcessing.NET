using System;
using System.Collections.Generic;
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
    // My class of 16-bit camera images
    // Nchannels - number of channels (1 for grayscale)
    // Some methods are implemented, mostly for basic processing of GDT plasma images from pco.edge.
    class CameraImage
    {
        private Mat SrcMat;
        // Using for show of a zoomed image (original data is copied to another Mat structure and plotted).
        private enum DownsampleFactor : byte { x1=1, x2=2, x4=4 };
        private string ShowImageName;
        // ctor
        public CameraImage(Int32 SizeY, Int32 SizeX, string ImageName, List<UInt16> Data, Int32 Nchannels)
        {
            try
            {
                System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(Data.ToArray(), System.Runtime.InteropServices.GCHandleType.Pinned);
                // SrcMat always preserve initial sizes (SizeX, SizeY) used for initialization.
                SrcMat = new Mat(SizeY, SizeX, DepthType.Cv16U, 1, handle.AddrOfPinnedObject(), SizeX * 2);
                handle.Free();
                ShowImageName = ImageName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Could not create Mat for the image. Original error: " + ex.Message);
            }
        }
        public void ShowZoomed(byte zoom)
        {
            CvInvoke.NamedWindow(ShowImageName);                //Create the window using the specific name
            CvInvoke.Imshow(ShowImageName, SrcMat);             //Show the image
            
        }
    }
}
