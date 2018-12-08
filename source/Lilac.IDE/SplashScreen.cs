using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lilac.IDE
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            if (progressBar1.Value == 100)
            {
                timer1.Stop();
                this.Close();
            }
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
            linkLabel1.Links.Add(13, 19, "https://www.siaranite.co.uk");
        }

        private void OpenLink(object sender, LinkLabelLinkClickedEventArgs args)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.siaranite.co.uk");
        }
    }
}
