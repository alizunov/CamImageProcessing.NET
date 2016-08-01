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
        // ZedGraph items
        public GraphPane pane
        { get; set; }

        // Number of slice profiles drawin in the pane.
        public int NdrawnProfiles;

        public Graphics1()
        {
            InitializeComponent();

            pane = zedGraphControl1.GraphPane;

            CurveNumber_numericUpDown.Value = 0;
            CurveNumber_numericUpDown.Maximum = 0;

            // Process slice groupbox
            ProcessSlice_groupBox.Enabled = false;

        } // ctor

        // Methods 
        public void AddSliceProfile(List<double> SliceData, double Xshift, double Xscale, string name, Color color)   // X = Xshift + Xscale*i_point
        {
            // Obtain X- and Y-limits for a new curve
            // Xshift input is not used. 
            // *** x = 0 is the middle of x[] ***
            double xmin = -0.5 * Xscale * SliceData.Count;
            double xmax = 0.5 * Xscale * SliceData.Count;
            double ymin = 0;
            double ymax = 1.2 * SliceData.Max();
            // Scale axis if necessary
            if (xmin < pane.XAxis.Scale.Min)
                pane.XAxis.Scale.Min = xmin;
            if (xmax > pane.XAxis.Scale.Max)
                pane.XAxis.Scale.Max = xmax;
            if (ymin < pane.XAxis.Scale.Min)
                pane.YAxis.Scale.Min = ymin;
            if (ymax > pane.XAxis.Scale.Max)
                pane.YAxis.Scale.Max = ymax;
            Console.WriteLine("New curve: xmin = {0}, xmax = {1}, ymin = {2}, ymax = {3} ", xmin, xmax, ymin, ymax);
            // ZedGraph list
            PointPairList pointlist = new PointPairList();
            for (int ip=0; ip<SliceData.Count(); ip++)
            {
                pointlist.Add(xmin + ip * Xscale, SliceData.ElementAt(ip));
            }
            // ZedGraph curve
            string CurveName = name;
            try
            {
                pane.Title.Text = "Slice profiles";
                pane.XAxis.Title.Text = "Pixel";
                pane.YAxis.Title.Text = "Intensity";
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
            List<double> yFit = new List<double>(SliceMath.FitPoly((int)FitPolyOrder_numericUpDown.Value));
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
    }
}
