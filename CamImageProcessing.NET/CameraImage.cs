using System;
using System.Collections.Generic;
using System.Reflection;
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
        private string ShowImageName;
        // Currently only 1 channel (grayscale)
        public Int32 Nchannels;

        // ctor
        public CameraImage(Int32 SizeY, Int32 SizeX, Int32 Nch, List<UInt16> Data, string ImageName)
        {
            // Currently only 1 channel (grayscale)
            Nchannels = 1;
            System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(Data.ToArray(), System.Runtime.InteropServices.GCHandleType.Pinned);
            SrcMat = new Mat(SizeY, SizeX, DepthType.Cv16U, Nchannels, handle.AddrOfPinnedObject(), SizeX * 2);
            handle.Free();
            ShowImageName = ImageName;
            CurrentDownsampleFactor = 1;
        }
        // Shows zoomed (downsampled) image fith one of preset factors from the AllowedDownsampleFactor enum.
        public void ShowZoomed(byte zoom)
        {
            bool isAllowableZoom = false;
            foreach (byte iF in AllowedDownsampleFactor)
                isAllowableZoom = (zoom == iF) ? true : false;
            // Check if image sizes are devisible by the zoom.
            if (SrcMat.Cols % zoom != 0 || SrcMat.Rows % zoom != 0)
            {
                Console.WriteLine("{0}: wrong zoom factor {1}: {2} and/or {3} are not multiple", MethodBase.GetCurrentMethod().Name, zoom, SrcMat.Cols, SrcMat.Rows);
                isAllowableZoom = false;
            }

            if (zoom == CurrentDownsampleFactor || !isAllowableZoom) return;

            Int32 ZoomedSizeX = (Int32)SrcMat.Cols / zoom;
            Int32 ZoomedSizeY = (Int32)SrcMat.Rows / zoom;
            try
            {
                Mat DstMat = new Mat(ZoomedSizeY, ZoomedSizeX, DepthType.Cv16U, Nchannels);
                CvInvoke.PyrDown(SrcMat, DstMat);
                CvInvoke.NamedWindow(ShowImageName);             //Create the window using the specific name
                CvInvoke.Imshow(ShowImageName, DstMat);          //Show the image
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Could not make zoomed image. Original error: " + ex.Message);
            }

        }
    }
}
