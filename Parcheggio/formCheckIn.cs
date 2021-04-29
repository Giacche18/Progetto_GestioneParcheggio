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
    public partial class formCheckIn : Form
    {
        private ControllerCheckIn controllercheckin;

        public formCheckIn(ControllerCheckIn controllerCheckIn)
        {
            controllercheckin = controllerCheckIn;

            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            controllercheckin.AddDati(textBoxCognome.Text, textBoxNome.Text, comboBoxDocumento.Text, textBoxNumDocumento.Text, comboBoxTipoVeicolo.Text, textBoxTarga.Text, oraIngresso.Text);

            textBoxCognome.Clear();
            textBoxNome.Clear();
            comboBoxDocumento.Text = null;
            comboBoxTipoVeicolo.Text = null;
            textBoxTarga.Clear(); 
            textBoxNumDocumento.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oraIngresso.Text = DateTime.Now.ToString("HH:mm");
        }
    }
}
