using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility
{
    public partial class ConfigUtility : Form
    {
        private Common.Config conf;

        public ConfigUtility()
        {
            InitializeComponent();
            // Load config
            conf = Common.Config.Read();
            tokenInput.Text = conf.PushbulletToken;
        }

        private void OnSave(object sender, EventArgs e)
        {
            conf.PushbulletToken = tokenInput.Text;
            conf.Write();

            MessageBox.Show("Your token has been saved. It will be used the next time you start the overlay.", "Saved!");
        }
    }
}
