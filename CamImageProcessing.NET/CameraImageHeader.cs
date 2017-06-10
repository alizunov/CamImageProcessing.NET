using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamImageProcessing.NET
{
    /// <summary>
    /// Header of a image file (originally developed for pco.edge 5.5 sCMOS camera
    /// Rev. 10 June 2017: added TimeStamp in the Comment section of the header
    /// </summary>
    class CameraImageHeader : Tuple<string, string, string, string, double, double, double, Tuple<UInt32, UInt32, UInt32, UInt32, UInt32, UInt32, UInt32, Tuple<UInt32, double, double, double, double, double, string>>>
    {
        public const string Version = "1.0 for standalone image files";
        public const byte NumberOfFields = 21;
        public bool isGoodHeader;

        // Tuple with header field names
        Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string, string, string, string>>>HeaderFieldNames = 
            new Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string, string, string, string>>>("Camera Name",
            "Camera SN",
            "Shutter Mode",
            "Timing Mode",
            "Sensor Temperature",
            "Camera Temperature",
            "Power Temperature",
            new Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string, string, string, string>>("Image Max Size-X",
            "Image Max Size-Y",
            "ROI X1",
            "ROI X2",
            "ROI Y1",
            "ROI Y2",
            "Binning X",
            new Tuple<string, string, string, string, string, string, string>("Binning Y",
            "Exposure",
            "Delay Internal",
            "Delay External",
            "Pixel Rate",
            "Conversion Factor",
            "Comment")));

        // Basic ctor
        public CameraImageHeader(string CameraName, // Item1
            string CameraSN,                        // Item2
            string ShutterMode,                     // Item3
            string TimingMode,                      // Item4
            double SensorTemperature,               // Item5
            double CameraTemperature,               // Item6
            double PowerTemperature,                // Item7
            UInt32 ImageMaxSizeX,                   // Rest.Item1
            UInt32 ImageMaxSizeY,                   // Rest.Item2
            UInt32 ROI_X1,                          // Rest.Item3
            UInt32 ROI_X2,                          // Rest.Item4
            UInt32 ROI_Y1,                          // Rest.Item5
            UInt32 ROI_Y2,                          // Rest.Item6
            UInt32 BinningX,                        // Rest.Item7
            UInt32 BinningY,                        // Rest.Rest.Item1
            double Exposure,                        // Rest.Rest.Item2
            double DelayInternal,                   // Rest.Rest.Item3
            double DelayExternal,                   // Rest.Rest.Item4
            double PixelRate,                       // Rest.Rest.Item5
            double ConversionFactor,                // Rest.Rest.Item6
            string Comment)                         // Rest.Rest.Item7
            : base( CameraName, CameraSN, ShutterMode, TimingMode, SensorTemperature, CameraTemperature, PowerTemperature,
                  new Tuple<UInt32, UInt32, UInt32, UInt32, UInt32, UInt32, UInt32, Tuple<UInt32, double, double, double, double, double, string>>(ImageMaxSizeX,
                      ImageMaxSizeY,
                      ROI_X1,
                      ROI_X2,
                      ROI_Y1,
                      ROI_Y2,
                      BinningX,
                      new Tuple<uint, double, double, double, double, double, string>(BinningY, Exposure, DelayInternal, DelayExternal, PixelRate, ConversionFactor, Comment)) )
        {
            // Simple checks of sensor geometric parameters
            bool isGoodSize = (ImageMaxSizeX > 2) && (ImageMaxSizeY > 2);
            bool isGoodROI_X = (ROI_X2 > ROI_X1) && (ROI_X2 <= ImageMaxSizeX);
            bool isGoodROI_Y = (ROI_Y2 > ROI_Y1) && (ROI_Y2 <= ImageMaxSizeY);
            isGoodHeader = (isGoodSize && isGoodROI_X && isGoodROI_Y) ? true : false;
        }
        // Empty ctor
        public CameraImageHeader()
            : base( "Void", "", "", "", 0, 0, 0,
                  new Tuple<UInt32, UInt32, UInt32, UInt32, UInt32, UInt32, UInt32, Tuple<UInt32, double, double, double, double, double, string>>(0, 0, 0, 0, 0, 0, 0,
                      new Tuple<uint, double, double, double, double, double, string>(0, 0, 0, 0, 0, 0, "Empty header not corresponding to any existing image")) )
        {
            isGoodHeader = true;
        }
        // Check List<string> and create a header instance if good.
        public CameraImageHeader TryTextList( List<string> textlist )
        {
            // There are 21 header fields accroding to the approved specification. The list may contain more than 21 lines:
            // then the remaining starting from the line #21 will be considered as a multi-line comment string.
            int NumberOfLines = textlist.Count();

            string CameraName = textlist[0];
            string CameraSN = textlist[1];
            string ShutterMode = textlist[2];
            string TimingMode = textlist[3];

            string Comment = "";
            StringBuilder HeaderCommentMultiLine = new StringBuilder();

            if (NumberOfLines == NumberOfFields)
                Comment = textlist[20];
            else if (NumberOfLines > NumberOfFields)
            {
                for (int iline = NumberOfFields; iline < NumberOfLines; iline++)
                    HeaderCommentMultiLine.Append(textlist[iline]).Append("\n");
                Comment = HeaderCommentMultiLine.ToString();
            }
            else // NumberOfLines < NumberOfField
            {
                Console.WriteLine("Error parsing header: number of lines less than the number of fields (21).");
                return null;
            }

            double SensorTemperature = 0;
            double CameraTemperature = 0;
            double PowerTemperature = 0;
            double Exposure = 0;
            double DelayInternal = 0;
            double DelayExternal = 0;
            double PixelRate = 0;
            double ConversionFactor = 0;

            UInt32 ImageMaxSizeX = 0;
            UInt32 ImageMaxSizeY = 0;
            UInt32 ROI_X1 = 0;
            UInt32 ROI_X2 = 0;
            UInt32 ROI_Y1 = 0;
            UInt32 ROI_Y2 = 0;
            UInt32 BinningX = 0;
            UInt32 BinningY = 0;

            try
            {
                SensorTemperature = Convert.ToDouble(textlist[4]);
                CameraTemperature = Convert.ToDouble(textlist[5]);
                PowerTemperature = Convert.ToDouble(textlist[6]);
                Exposure = Convert.ToDouble(textlist[15]);
                DelayInternal = Convert.ToDouble(textlist[16]);
                DelayExternal = Convert.ToDouble(textlist[17]);
                PixelRate = Convert.ToDouble(textlist[18]);
                ConversionFactor = Convert.ToDouble(textlist[19]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: conversion from string to double failed. Original error: " + ex.Message);
            }

            try
            {
                ImageMaxSizeX = Convert.ToUInt32(textlist[7]);
                ImageMaxSizeY = Convert.ToUInt32(textlist[8]);
                ROI_X1 = Convert.ToUInt32(textlist[9]);
                ROI_X2 = Convert.ToUInt32(textlist[10]);
                ROI_Y1 = Convert.ToUInt32(textlist[11]);
                ROI_Y2 = Convert.ToUInt32(textlist[12]);
                BinningX = Convert.ToUInt32(textlist[13]);
                BinningY = Convert.ToUInt32(textlist[14]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: conversion from string to UInt32 failed. Original error: " + ex.Message);
            }

            CameraImageHeader header = new CameraImageHeader(CameraName,
                CameraSN,
                ShutterMode,
                TimingMode,
                SensorTemperature,
                CameraTemperature,
                PowerTemperature,
                ImageMaxSizeX,
                ImageMaxSizeY,
                ROI_X1,
                ROI_X2,
                ROI_Y1,
                ROI_Y2,
                BinningX,
                BinningY, 
                Exposure, 
                DelayInternal, 
                DelayExternal, 
                PixelRate, 
                ConversionFactor, 
                Comment);
            return header;
        }
        // Returns header as NumberOfFields lines with titles
        public string FormattedHeader
        {
            get
            {
                string[] fh = new string[] { HeaderFieldNames.Item1, ": ", Item1.ToString(), "\n",
                    HeaderFieldNames.Item2, ": ", Item2.ToString(), "\n",
                    HeaderFieldNames.Item3, ": ", Item3.ToString(), "\n",
                    HeaderFieldNames.Item4, ": ", Item4.ToString(), "\n",
                    HeaderFieldNames.Item5, ": ", Item5.ToString(), "\n",
                    HeaderFieldNames.Item6, ": ", Item6.ToString(), "\n",
                    HeaderFieldNames.Item7, ": ", Item7.ToString(), "\n",
                    HeaderFieldNames.Rest.Item1, ": ", Convert.ToString(Rest.Item1), "\n",
                    HeaderFieldNames.Rest.Item2, ": ", Convert.ToString(Rest.Item2), "\n",
                    HeaderFieldNames.Rest.Item3, ": ", Convert.ToString(Rest.Item3), "\n",
                    HeaderFieldNames.Rest.Item4, ": ", Convert.ToString(Rest.Item4), "\n",
                    HeaderFieldNames.Rest.Item5, ": ", Convert.ToString(Rest.Item5), "\n",
                    HeaderFieldNames.Rest.Item6, ": ", Convert.ToString(Rest.Item6), "\n",
                    HeaderFieldNames.Rest.Item7, ": ", Convert.ToString(Rest.Item7), "\n",
                    HeaderFieldNames.Rest.Rest.Item1, ": ", Convert.ToString(Rest.Rest.Item1), "\n",
                    HeaderFieldNames.Rest.Rest.Item2, ": ", Convert.ToString(Rest.Rest.Item2), "\n",
                    HeaderFieldNames.Rest.Rest.Item3, ": ", Convert.ToString(Rest.Rest.Item3), "\n",
                    HeaderFieldNames.Rest.Rest.Item4, ": ", Convert.ToString(Rest.Rest.Item4), "\n",
                    HeaderFieldNames.Rest.Rest.Item5, ": ", Convert.ToString(Rest.Rest.Item5), "\n",
                    HeaderFieldNames.Rest.Rest.Item6, ": ", Convert.ToString(Rest.Rest.Item6), "\n",
                    HeaderFieldNames.Rest.Rest.Item7, ": ", Rest.Rest.Item7 };
                return string.Concat(fh);
            }
        }
        // Returns actual image X-size = ROI_X2 - ROI_X1 + 1
        public UInt32 SizeX
        {
            get
            {
                return Rest.Item4 - Rest.Item3 + 1;
            }
        }
        // Returns actual image Y-size = ROI_Y2 - ROI_Y1 + 1
        public UInt32 SizeY
        {
            get
            {
                return Rest.Item6 - Rest.Item5 + 1;
            }
        }

    }
}
