using System;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;

namespace Lilac.IDE
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            string About = string.Format(CultureInfo.InvariantCulture, @"Lilac IDE Version {0}.{1}.{2} (Build Number {3})", v.Major, v.Minor, v.Build, v.Revision);
            this.label1.Text = About;
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
            //linkLabel1.Links.Add(13, 19, "https://www.siaranite.co.uk");
        }

        private void OpenLink(object sender, LinkLabelLinkClickedEventArgs args)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.siaranite.co.uk");
        }
    }
}
