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
        public Graphics1()
        {
            InitializeComponent();
            
            GraphPane pane = new GraphPane();
            ArrowObj arrow = new ArrowObj(10, 100, 200, 200);
            pane.GraphObjList.Add(arrow);
            zedGraphControl1.Invalidate();
            
        }

        private void Graphics1_Load(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
