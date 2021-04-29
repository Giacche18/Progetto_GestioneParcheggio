using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parcheggio.Controller;

namespace Parcheggio
{
    public partial class formUpdateClient : Form
    {
        private ControllerCheckIn controllerUpdateClient;

        public formUpdateClient(ControllerCheckIn controlleupdateclient)
        {
            this.controllerUpdateClient = controlleupdateclient;

            InitializeComponent();
        }
   
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            controllerUpdateClient.UpdateClient(listClient);
            toolStripButton2.Enabled = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
