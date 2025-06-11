using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;

namespace AgriculturalRecycle
{
    public partial class Device_BORE: UIForm
    {
        private readonly User _currentUser;
        public Device_BORE(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(_currentUser);
            this.Hide();
            mainForm.Show();
        }
    }
}
