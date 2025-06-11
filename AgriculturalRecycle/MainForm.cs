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
using MySql.Data.MySqlClient;

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
            string sql = "select Avatar from UserInfo where UserID=@userid";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userid", _currentUser.UserID));
            if (dt.Rows.Count > 0)
            {
                string avatar = dt.Rows[0]["Avatar"].ToString();
                if (avatar != "")
                {
                    Image img = Image.FromFile(avatar);
                    uiAvatar1.Image = img;
                }
            }
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton1.Enabled = true;
            uiImageButton2.Enabled = false;
            uiImageButton3.Enabled = false;
            uiImageButton1.Visible = true;
            uiImageButton2.Visible = false;
            uiImageButton3.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton1.Enabled = false;
            uiImageButton2.Enabled = true;
            uiImageButton3.Enabled = false;
            uiImageButton1.Visible = false;
            uiImageButton2.Visible = true;
            uiImageButton3.Visible = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton1.Enabled = false;
            uiImageButton2.Enabled = false;
            uiImageButton3.Enabled = true;
            uiImageButton1.Visible = false;
            uiImageButton2.Visible = false;
            uiImageButton3.Visible = true;
        }

        private void uiImageButton1_Click(object sender, EventArgs e)
        {
            Device_BORE device_bore = new Device_BORE(_currentUser);
            this.Hide();
            device_bore.Show();
        }

        private void uiImageButton2_Click(object sender, EventArgs e)
        {
            uiTabControlMenu1.SelectedTab = tabPage3;
        }

        private void uiImageButton3_Click(object sender, EventArgs e)
        {
            Device_Store device_store = new Device_Store(_currentUser);
            this.Hide();
            device_store.Show();
        }

        private void uiLabel2_Click(object sender, EventArgs e)
        {

        }

        private void uiLine1_Click(object sender, EventArgs e)
        {

        }

        private void uiSplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void uiLabel1_Click(object sender, EventArgs e)
        {

        }

        private void uiLabel3_Click(object sender, EventArgs e)
        {

        }

        private void splitter1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uiSplitContainer1_SplitterMoved_1(object sender, SplitterEventArgs e)
        {

        }

        private void uiPanel2_Click(object sender, EventArgs e)
        {

        }

        private void uiAvatar1_Click(object sender, EventArgs e)
        {
            
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            Device_BORE device_bore = new Device_BORE(_currentUser);
            this.Hide();
            device_bore.Show();
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            Device_Store device_store = new Device_Store(_currentUser);
            this.Hide();
            device_store.Show();
        }
    }

}
