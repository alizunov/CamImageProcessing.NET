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
            Color.Beige,
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

            //zedGraphControl1.Invalidate();

        } // ctor

        // Methods 
        public void AddSliceProfile(List<double> SliceData, double Xshift, double Xscale)   // X = Xshift + Xscale*i_point
        {
            //zedGraphControl1.Validate();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: could not add new profile to the graph pane. " + ex.Message);
            }


            MessageBox.Show("Profile # " + NdrawnProfiles + " added to pane, Npoints =  " + SliceData.Count, "", MessageBoxButtons.OK);
        }

        private void Graphics1_Load(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
