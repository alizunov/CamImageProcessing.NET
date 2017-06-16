using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZedGraph;

namespace CamImageProcessing.NET
{
    public partial class Graphics1 : Form
    {
        /// <summary>
        /// Units of X-axis for all curves (profiles)
        /// </summary>
        public enum x_axis_units : byte { pixel=0, mm=1 };
        private x_axis_units xau;

        public x_axis_units XAxisUnits
        {
            get
            {
                return xau;
            }
            set
            {
                xau = value;
            }
        }

        /// <summary>
        /// Definition of X-axis shift for V-slices
        /// </summary>
        private double x0_V;
        /// <summary>
        /// Definition of X-axis shift for H-slices
        /// </summary>
        private double x0_H;
        /// <summary>
        /// Definition of X-axis scale for V-slices
        /// </summary>
        private double xscale_V;
        /// <summary>
        /// Definition of X-axis scale for H-slices
        /// </summary>
        private double xscale_H;
        
        /// <summary>
        /// Get/Set of X-axis shift for V-slices
        /// </summary>
        public double X0_V
        {
            get
            {
                return x0_V;
            }
            set
            {
                x0_V = value;
            }
        }
        /// <summary>
        /// Get/Set of X-axis shift for H-slices
        /// </summary>
        public double X0_H
        {
            get
            {
                return x0_H;
            }
            set
            {
                x0_H = value;
            }
        }
        /// <summary>
        /// Get/Set of X-axis scale for V-slices
        /// </summary>
        public double Xscale_V
        {
            get
            {
                return xscale_V;
            }
            set
            {
                xscale_V = value;
            }
        }
        /// <summary>
        /// Get/Set of X-axis scale for H-slices
        /// </summary>
        public double Xscale_H
        {
            get
            {
                return xscale_H;
            }
            set
            {
                xscale_H = value;
            }
        }
        



        // ZedGraph items
        public GraphPane pane
        { get; set; }

        // Normalization coefficient for all slice curves
        public double CurveNormCoefficient
        { get; set; }

        public Graphics1()
        {
            InitializeComponent();

            pane = zedGraphControl1.GraphPane;

            CurveNumber_numericUpDown.Value = 0;
            CurveNumber_numericUpDown.Maximum = 0;

            // Process slice groupbox
            ProcessSlice_groupBox.Enabled = false;

            // Lock function combo
            ApproxMethodOutside_comboBox.Items.AddRange(new object[] { "Linear",
                "Quad. positive",
                "Quad negative"});
            // Set X-axis units for slice profiles plotted in the GraphPane
            ChangeXaxisUnits_comboBox.Items.AddRange(new object[] {"Pixels",
                        "mm"});
            ChangeXaxisUnits_comboBox.SelectedIndex = 0;
            xau = x_axis_units.pixel;

        } // ctor

        // Methods 
        public void AddSliceProfile(List<double> SliceData, double Xshift, double Xscale, string name, Color color, bool toScale = false)   // X = Xshift + Xscale*i_point
        {
            // Obtain X- and Y-limits for a new curve
            // *** For a normal data array (not extended set): x = 0 is the middle of x[] ***
            // Setting of a valid Xshift must be done on the calling side
            double xmin = Xshift;
            double xmax = xmin + Xscale * SliceData.Count;
            double ymin = 0;
            // Look at current curve count, calculate CurveNormCoefficient if no curves drawn so far
            if (pane.CurveList.Count == 0)
            {
                // CurveNormCoefficient = 10^n
                int n10 = (int)Math.Log10(SliceData.Max());
                CurveNormCoefficient = Math.Pow(10, -n10);
            }
            double yscale = (toScale) ? CurveNormCoefficient : 1;
            double ymax = SliceData.Max() * yscale;
            // Scale axis if necessary
            if (xmin < pane.XAxis.Scale.Min)
                pane.XAxis.Scale.Min = xmin;
            if (xmax > pane.XAxis.Scale.Max)
                pane.XAxis.Scale.Max = xmax;
            if (ymin < pane.XAxis.Scale.Min)
                pane.YAxis.Scale.Min = ymin;
            if (ymax > pane.XAxis.Scale.Max)
                pane.YAxis.Scale.Max = ymax;
            Console.WriteLine("New curve: xmin = {0}, xmax = {1}, ymin = {2}, ymax = {3} ", xmin, xmax, ymin, SliceData.Max());
            // ZedGraph list
            PointPairList pointlist = new PointPairList();
            for (int ip=0; ip<SliceData.Count(); ip++)
            {
                pointlist.Add(xmin + ip * Xscale, yscale * SliceData.ElementAt(ip));
            }
            // ZedGraph curve
            string CurveName = name;
            try
            {
                pane.Title.Text = "Slice profiles";
                //pane.XAxis.Title.Text = "Pixel";
                pane.YAxis.Title.Text = "Intensity, 10^" + (-1 * Math.Log10(CurveNormCoefficient)).ToString() + " counts";
                LineItem SliceCurve = pane.AddCurve(CurveName, pointlist, color, SymbolType.None);
                SliceCurve.Line.IsVisible = true;
                // Update axis
                zedGraphControl1.AxisChange();
                // Update graph pane
                zedGraphControl1.Invalidate();
                // Update the curve selection numUpDown control
                CurveNumber_numericUpDown.Maximum = pane.CurveList.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: could not add new profile to the graph pane. " + ex.Message);
            }


            MessageBox.Show("Profile # " + pane.CurveList.Count + " added to pane, Npoints =  " + SliceData.Count, "", MessageBoxButtons.OK);
            // Enable slice process box
            if (pane.CurveList.Count > 0)
            {
                ProcessSlice_groupBox.Enabled = true;
                ActiveSlice_numericUpDown.Maximum = pane.CurveList.Count - 1;
                CurveNumber_numericUpDown.Maximum = pane.CurveList.Count - 1;
                // Default order of a fit polynom
                FitPolyOrder_numericUpDown.Value = 17;
            }
            else
                ProcessSlice_groupBox.Enabled = false;
        }

        /// <summary>
        /// Rescale a profile using the formula NewX = k * OldX + b
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RescaleCurveXaxis(CurveItem crv, double k, double b)
        {
            PointPairList ppl = new PointPairList();
            for (int ip = 0; ip < crv.Points.Count; ip++)
                ppl.Add(crv.Points[ip].X * k + b, crv.Points[ip].Y);
            crv.Points = ppl;
        }

        private void Graphics1_Load(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load_1(object sender, EventArgs e)
        {

        }

        private void ClearPane_button_Click(object sender, EventArgs e)
        {
            pane.CurveList.Clear();
            pane.GraphObjList.Clear();
            ActiveSlice_numericUpDown.Maximum = 0;
            CurveNumber_numericUpDown.Maximum = 0;
            zedGraphControl1.Invalidate();
        }

        private void CurveNumber_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            CurveNumber_numericUpDown.Maximum = pane.CurveList.Count - 1;
        }

        private void RemoveCurve_button_Click(object sender, EventArgs e)
        {
            pane.CurveList.RemoveAt((int)CurveNumber_numericUpDown.Value);
            zedGraphControl1.Invalidate();
        }

        private void ProcessSlice_groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void FitSlice_button_Click(object sender, EventArgs e)
        {
            // Prepare X,Y values
            int CurveNumber = (int)ActiveSlice_numericUpDown.Value;
            CurveItem ActiveSlice = pane.CurveList.ElementAt(CurveNumber);
            List<PointPair> pp = new List<PointPair>();
            pp = (List<PointPair>)ActiveSlice.Points.Clone();
            List<double> xl = new List<double>();
            List<double> yl = new List<double>();
            // Split poinpairs to x,y
            for (int i=0; i < pp.Count; i++)
            {
                xl.Add(pp.ElementAt(i).X);
                yl.Add(pp.ElementAt(i).Y);
            }
            // Create ProfileMath object
            string name = pane.CurveList.ElementAt(CurveNumber).Label.Text + "-fit";
            ProfileMath SliceMath = new ProfileMath(xl, yl, name);
            double Xshift = xl.ElementAt(0);
            double Xscale = xl.ElementAt(1) - Xshift;
            xl.Clear();
            yl.Clear();
            pp.Clear();
            List<double> yFit = new List<double>(SliceMath.Poly(SliceMath.Xlist.ToArray(), SliceMath.FitPoly((int)FitPolyOrder_numericUpDown.Value)));
            AddSliceProfile(yFit, Xshift, Xscale, name, Color.Black);
            // Display style
            LineItem myFit = (LineItem)pane.CurveList.Last();
            myFit.Line.IsVisible = true;
            myFit.Line.Width = 2;
            // Move fit curve one position higher in the CurveList
            int index = pane.CurveList.IndexOf(name);
            pane.CurveList.Move(index, -1);
            // Update 
            zedGraphControl1.Invalidate();

            ActiveSlice_numericUpDown.Maximum = pane.CurveList.Count - 1;
            CurveNumber_numericUpDown.Maximum = pane.CurveList.Count - 1;

        }

        private void ActiveSlice_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FitPolyOrder_numericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Abel_button_Click(object sender, EventArgs e)
        {
            try
            {
                // Prepare X,Y values
                int CurveNumber = (int)ActiveSlice_numericUpDown.Value;
                CurveItem ActiveSlice = pane.CurveList.ElementAt(CurveNumber);
                List<PointPair> pp = new List<PointPair>();
                pp = (List<PointPair>)ActiveSlice.Points.Clone();
                List<double> xl = new List<double>();
                List<double> yl = new List<double>();
                // Split poinpairs to x,y
                for (int i = 0; i < pp.Count; i++)
                {
                    xl.Add(pp.ElementAt(i).X);
                    yl.Add(pp.ElementAt(i).Y);
                }
                double Xscale = xl.ElementAt(1) - xl.ElementAt(0);
                string nameFit = pane.CurveList.ElementAt(CurveNumber).Label.Text + "-poly" + FitPolyOrder_numericUpDown.Value.ToString();
                string nameExtFit = nameFit + "-ext";
                // Create ProfileMath object
                ProfileMath SliceMath = new ProfileMath(xl, yl, nameFit);
                // Basic range fit
                List<double> Yfit = new List<double>(SliceMath.Poly(SliceMath.Xlist.ToArray(), SliceMath.FitPoly((int)FitPolyOrder_numericUpDown.Value)));
                xl.Clear();
                yl.Clear();
                pp.Clear();

                List<double> yAbel = new List<double>(SliceMath.AbelInversionPoly(SliceMath.GetApproximationMethod(ApproxMethodOutside_comboBox.SelectedItem.ToString())));
                // Extended range fit curve with left/right wing lock functions defined in the Abel inversion method
                List<double> Yext = new List<double>(SliceMath.BuildFitExtended());
                // Slice for Abel inversion profile
                AddSliceProfile(yAbel, SliceMath.Xext.ElementAt(0), Xscale, nameFit, ActiveSlice.Color);
                // Display style
                LineItem myFit = (LineItem)pane.CurveList.Last();
                myFit.Line.IsVisible = true;
                myFit.Line.Width = 2;
                // Move fit curve one position higher in the CurveList
                int index = pane.CurveList.IndexOf(nameFit);
                pane.CurveList.Move(index, -1);

                // Slice for extended fit
                AddSliceProfile(Yext, SliceMath.Xext.ElementAt(0), Xscale, nameExtFit, Color.DarkBlue);
                // Display style
                index = pane.CurveList.IndexOf(nameExtFit);
                LineItem myExtFit = (LineItem)pane.CurveList.ElementAt(index);
                myExtFit.Line.Width = 2;
                // Move fit curve one position higher in the CurveList
                //pane.CurveList.Move(index, -1);

                // Update 
                zedGraphControl1.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Could not make Abel inversion for curve #{0}. Original error: " + ex.Message, ActiveSlice_numericUpDown.Value);
            }

            ActiveSlice_numericUpDown.Maximum = pane.CurveList.Count - 1;
            CurveNumber_numericUpDown.Maximum = pane.CurveList.Count - 1;

        }

        private void ApproxMethodOutside_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Selected method:" + ApproxMethodOutside_comboBox.SelectedItem, "", MessageBoxButtons.OK);
        }

        /// <summary>
        /// "Save pane" button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePane_button_Click(object sender, EventArgs e)
        {
            zedGraphControl1.SaveAsBitmap();
        }

        private void ChangeXaxisUnits_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check FOV-X and FOV-Y, if both are not zero:
            // Go through curve list, rescale.
            if (xscale_H == 0 || xscale_V == 0)
                MessageBox.Show("FOV-X or FOV-Y is zero", "", MessageBoxButtons.OK);
            else
            {
                double kh, kv, bh, bv;
                double xmin = Double.MaxValue;
                double xmax = Double.MinValue;
                if (ChangeXaxisUnits_comboBox.SelectedIndex == 1) // Pixels -> mm
                {
                    bh = x0_H;
                    bv = x0_V;
                    kh = xscale_H;
                    kv = xscale_V;
                    // Change X axis title.
                    pane.XAxis.Title.Text = "Coordinate, mm";
                    // Change X axis units enum
                    XAxisUnits = x_axis_units.mm;
                }
                else // mm -> Pixels
                {
                    bh = -x0_H / xscale_H;
                    bv = -x0_V / xscale_V;
                    kh = 1 / xscale_H;
                    kv = 1 / xscale_V;
                    // Change X axis title.
                    pane.XAxis.Title.Text = "Coordinate, pixel";
                    // Change X axis units enum
                    XAxisUnits = x_axis_units.pixel;
                }
                foreach (CurveItem crv in pane.CurveList)
                {
                    // Separate H- and V-profiles by name
                    if (crv.Label.Text.Substring(6, 1) == "V")
                        RescaleCurveXaxis(crv, kv, bv);
                    else if (crv.Label.Text.Substring(6, 1) == "H")
                        RescaleCurveXaxis(crv, kh, bh);
                    else // Error parsing name
                        Console.WriteLine("Error in curve name: " + crv.Label.Text + " - neither Vertical nor Horizontal, rescale is not done.");
                    xmin = (crv.Points[0].X < xmin) ? crv.Points[0].X : xmin;
                    xmax = (crv.Points[crv.Points.Count - 1].X > xmax) ? crv.Points[crv.Points.Count - 1].X : xmax;
                }
                // Change XAxis margins
                pane.XAxis.Scale.Min = xmin;
                pane.XAxis.Scale.Max = xmax;

                // Update axis
                zedGraphControl1.AxisChange();
                // Update graph pane
                zedGraphControl1.Invalidate();

            }
        }

        private void SaveCurve_button_Click(object sender, EventArgs e)
        {

            System.IO.Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    // Code to write the stream goes here.
                    string crvFileName = saveFileDialog1.FileName;
                    try
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(crvFileName);
                        int CurveNumber = (int)ActiveSlice_numericUpDown.Value;
                        CurveItem ActiveCurve = pane.CurveList.ElementAt(CurveNumber);
                        for (int ip = 0; ip < ActiveCurve.Points.Count; ip++)
                            sw.Write(ActiveCurve.Points[ip].X + " " + ActiveCurve.Points[ip].Y / CurveNormCoefficient + "\n");
                        sw.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: Could not write curve to file. Original error: " + ex.Message);
                    }

                }
            }

        }
    }
}
