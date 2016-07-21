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

        public Graphics1()
        {
            InitializeComponent();
            
            pane = new GraphPane();
            //zedGraphControl1.Invalidate();
            
        }

        private void Graphics1_Load(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void GetSliceData_button_Click(object sender, EventArgs e)
        {
            List<double> SliceList = MainControlWindow
            PointPairList plist = new PointPairList();
        }
    }
}
