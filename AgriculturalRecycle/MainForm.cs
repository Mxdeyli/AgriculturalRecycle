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
using AgriculturalRecycle.Layout;
using AntdUI;

namespace AgriculturalRecycle
{
    public partial class MainForm : UIForm
    {
        public AutoSizeFormClass autoSize = new AutoSizeFormClass();
        private readonly User _currentUser;
        public MainForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            autoSize.controllInitializeSize(this);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            autoSize.controllInitializeSize(this);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出账号？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login login = new Login();
                _currentUser.Logout();
                this.Hide();
                login.Show();

            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            autoSize.controllInitializeSize(this);
        }

        private void uiButton8_Click(object sender, EventArgs e)
        {
            UserManage userManage = new UserManage(_currentUser);
            this.Hide();
            userManage.Show();
        }
    }

}
