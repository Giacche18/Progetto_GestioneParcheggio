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
    public partial class formCheckOut : Form
    {
        ControllerCheckOut controllercheckout;
        ControllerCheckIn controllercheckin;

        public formCheckOut(ControllerCheckIn controllerCheckIn)
        {
            this.controllercheckin = controllerCheckIn;
            this.controllercheckout = new ControllerCheckOut();
            
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oraUscita.Text = DateTime.Now.ToString("HH:mm");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            controllercheckout.CalculatePrice(oraUscita);
            controllercheckin.RemoveData(textBoxNumDocumento.Text);

            textBoxNumDocumento.Clear();
            textBoxCognome.Clear();
            textBoxNome.Clear();
            textBoxVeicolo.Clear();
            textBoxTarga.Clear();
            oraIngresso.Text = null;  
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            controllercheckout.GetFullPath();
            controllercheckout.SearchAndRemoveClient(textBoxNumDocumento.Text, textBoxCognome, textBoxNome, textBoxVeicolo, textBoxTarga, oraIngresso);
        }
    }
}
