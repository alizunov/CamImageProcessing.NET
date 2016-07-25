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
        private static Color[] ProfileColors = { Color.DeepPink,
            Color.Gold,
            Color.Aqua,
            Color.Blue,
            Color.BlueViolet,
            Color.Coral,
            Color.Crimson,
            Color.Cyan,
            Color.DarkBlue,
            Color.DarkGreen,
            Color.Orange,
            Color.Green,
            Color.Red };

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

        } // ctor

        // Methods 
        public void AddSliceProfile(List<double> SliceData, double Xshift, double Xscale)   // X = Xshift + Xscale*i_point
        {
            // Obtain X- and Y-limits for a new curve
            double xmin = Xshift;
            double xmax = Xshift + Xscale * SliceData.Count();
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
                pointlist.Add(Xshift + ip * Xscale, SliceData.ElementAt(ip));
            }
            // ZedGraph curve
            string CurveName = "Slice-" + ++NdrawnProfiles;
            int iColor = NdrawnProfiles - 1;
            while (iColor >= ProfileColors.Length)
                iColor -= ProfileColors.Length;
            try
            {
                pane.Title.Text = "Slice profiles";
                pane.XAxis.Title.Text = "Pixel";
                pane.YAxis.Title.Text = "Intensity";
                LineItem SliceCurve = pane.AddCurve(CurveName, pointlist, ProfileColors[iColor], SymbolType.None);
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


            MessageBox.Show("Profile # " + NdrawnProfiles + " added to pane, Npoints =  " + SliceData.Count, "", MessageBoxButtons.OK);
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
            NdrawnProfiles = pane.CurveList.Count;
            zedGraphControl1.Invalidate();
        }

        private void CurveNumber_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            CurveNumber_numericUpDown.Maximum = pane.CurveList.Count;
        }

        private void RemoveCurve_button_Click(object sender, EventArgs e)
        {
            pane.CurveList.RemoveAt((int)CurveNumber_numericUpDown.Value - 1);
            zedGraphControl1.Invalidate();
        }
    }
}
