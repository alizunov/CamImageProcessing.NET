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
    class CameraImageSlice
    {
        // Uses source (large) Mat, but dont modify it
        public Mat SrcMat
        { get; }

    }
}
