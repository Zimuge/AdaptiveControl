using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaptiveControl
{
    public partial class AdaptiveControl : Form
    {
        public AdaptiveControl()
        {
            InitializeComponent();

            this.controlAlgorithm = new PidController(5.5, 100,
                paraChart, controlChart, paraDataGridView, controlDataGridView);

            controlAlgorithm.showData();
            controlAlgorithm.showParameters();
            controlAlgorithm.showParameters();
            controlAlgorithm.showData();
        }
        
        private void AdaptiveControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            //str_exit(Handle);
        }
        
    }
}
