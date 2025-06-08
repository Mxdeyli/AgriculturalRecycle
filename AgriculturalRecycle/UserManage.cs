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
    public partial class UserManage: UIForm
    {
        private readonly User _currentuser;
        public UserManage(User user)
        {
            InitializeComponent();
            _currentuser = user;
        }

        private void UserManage_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(_currentuser);
            this.Hide();
            mainForm.Show();
        }


        private void uiAvatar1_Click_1(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            Edit edit = new Edit(_currentuser);
            this.Hide();
            edit.Show();
        }
    }
}
