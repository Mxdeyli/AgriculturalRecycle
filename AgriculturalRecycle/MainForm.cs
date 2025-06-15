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

        private void Buttonrole()
        {
            string sql = "select UserType from Users where UserID=@userid";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userid", _currentUser.UserID));
            if (dt.Rows.Count > 0)
            {
                string usertype = dt.Rows[0]["UserType"].ToString();
                if (usertype == "admin")
                {
                    uiButton9.Visible = true;
                    uiButton10.Visible = true;
                }
            }
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
            flowLayoutPanel2.Controls.Clear();
            LoadMessagesToFlowPanel();
            Buttonrole();
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

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton4.Enabled = true;
            uiImageButton5.Enabled = false;
            uiImageButton6.Enabled = false;
            uiImageButton4.Visible = true;
            uiImageButton5.Visible = false;
            uiImageButton6.Visible = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton4.Enabled = false;
            uiImageButton5.Enabled = true;
            uiImageButton6.Enabled = false;
            uiImageButton4.Visible = false;
            uiImageButton5.Visible = true;
            uiImageButton6.Visible = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton4.Enabled = false;
            uiImageButton5.Enabled = false;
            uiImageButton6.Enabled = true;
            uiImageButton4.Visible = false;
            uiImageButton5.Visible = false;
            uiImageButton6.Visible = true;
        }

        private void uiLabel11_Click(object sender, EventArgs e)
        {

        }

        private void uiButton13_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void uiButton12_Click(object sender, EventArgs e)
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

        private void uiButton11_Click(object sender, EventArgs e)
        {
            UpdateAccount update = new UpdateAccount(_currentUser);
            this.Hide();
            update.Show();
        }

        private void uiButton9_Click(object sender, EventArgs e)
        {

        }

        private void uiButton10_Click(object sender, EventArgs e)
        {
            MainManager mainManager = new MainManager(_currentUser);
            this.Hide();
            mainManager.Show();
        }

        private void uiImageButton4_Click(object sender, EventArgs e)
        {
            Device_Store device_store = new Device_Store(_currentUser);
            this.Hide();
            device_store.Show();
        }

        private void uiImageButton5_Click(object sender, EventArgs e)
        {
            Device_Store device_store = new Device_Store(_currentUser);
            this.Hide();
            device_store.Show();
        }

        private void uiImageButton6_Click(object sender, EventArgs e)
        {
            Device_Store device_store = new Device_Store(_currentUser);
            this.Hide();
            device_store.Show();
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uiButton16_Click(object sender, EventArgs e)
        {
            string sqlusername = "select UserName from UserInfo where UserID=@userid";
            DataTable dt2 = DBhelper.ExecuteQuery(sqlusername, new MySqlParameter("@userid", _currentUser.UserID));
            string username = "";
            if (dt2.Rows.Count > 0)
            {
                username = dt2.Rows[0]["UserName"].ToString();
            }
            string message = uiRichTextBox1.Text;
            string sqlavatar = "select Avatar from UserInfo where UserID=@userid";
            DataTable dt = DBhelper.ExecuteQuery(sqlavatar, new MySqlParameter("@userid", _currentUser.UserID));
            string avatar = "";
            if (dt.Rows.Count > 0)
            {
                avatar = dt.Rows[0]["Avatar"].ToString();
            }
            if (message != "")
            {
                string sql = "insert into MessagesInfo(UserID,UserName,ImageUrl,MessageContent,MessageTime) values(@userid,@username,@imageurl,@messagecontent,now())";
                MySqlParameter[] parameters = new MySqlParameter[] {
                    new MySqlParameter("@userid",_currentUser.UserID),
                    new MySqlParameter("@username",username),
                    new MySqlParameter("@imageurl",avatar),
                    new MySqlParameter("@messagecontent",message)
                };
                DBhelper.ExecuteNonQuery(sql, parameters);
                uiRichTextBox1.Text = "";
                MainForm_Load(sender, e);
                MessageBox.Show("留言成功！");
            }
            else
            {
                MessageBox.Show("留言不能为空！"); 
            }
        }
        private void LoadMessagesToFlowPanel()
        {
            string sql = "select UserID,ImageUrl,UserName,MessageContent,MessageTime,MessageID from MessagesInfo order by MessageTime desc";
            DataTable dt = DBhelper.ExecuteQuery(sql);
            flowLayoutPanel2.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                UIPanel panel = new UIPanel();
                panel.Size = new Size(1404, 153);
                panel.Margin = new Padding(3);
                panel.Padding = new Padding(10);
                panel.BackColor = Color.White;

                UIAvatar avatar = new UIAvatar();
                avatar.Size = new Size(60, 60);
                avatar.Location = new Point(10, 10);
                avatar.BackColor = Color.Transparent;
                avatar.Icon=UIAvatar.UIIcon.Image;
                string imagePath = row["ImageUrl"]?.ToString();
                if(imagePath!= null && imagePath != "")
                {
                    Image img = Image.FromFile(imagePath);
                    avatar.Image = img;
                }
                else
                {
                    avatar.Image = Properties.Resources._76e802b42db9a6ce;   
                }
                panel.Controls.Add(avatar);

                UILabel lblUserName = new UILabel();
                lblUserName.BackColor = Color.Transparent;
                lblUserName.Text = row["UserName"].ToString();
                lblUserName.Font = new Font("微软雅黑", 12F, FontStyle.Bold);
                lblUserName.Location = new Point(avatar.Right + 10, avatar.Top);
                lblUserName.AutoSize = true;
                panel.Controls.Add(lblUserName);

                FlowLayoutPanel messagePanel = new FlowLayoutPanel();
                messagePanel.AutoSize = false;
                messagePanel.AutoScroll = true;
                messagePanel.WrapContents = true;
                messagePanel.FlowDirection = FlowDirection.LeftToRight;
                messagePanel.Location = new Point(avatar.Right + 10, lblUserName.Bottom + 5);
                messagePanel.Width = panel.Width - avatar.Right - 120;
                messagePanel.Height = 85;

                UILabel lblMessage = new UILabel();
                lblMessage.Text = row["MessageContent"].ToString();
                lblMessage.Font = new Font("微软雅黑", 10F);
                lblMessage.AutoSize = true;
                messagePanel.Controls.Add(lblMessage);
                panel.Controls.Add(messagePanel);

                UILabel lblTime = new UILabel();
                lblTime.BackColor = Color.Transparent;
                lblTime.Text = Convert.ToDateTime(row["MessageTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                lblTime.Font = new Font("微软雅黑", 9F, FontStyle.Italic);
                lblTime.ForeColor = Color.Gray;
                lblTime.AutoSize = true;
                lblTime.Location = new Point(panel.Width - lblTime.PreferredWidth - 120, panel.Height - lblTime.PreferredHeight - 10);
                panel.Controls.Add(lblTime);

                UIButton btnDelete = new UIButton();
                btnDelete.Text = "删除";
                btnDelete.Size = new Size(60, 30);
                btnDelete.Font = new Font("微软雅黑", 9F);
                btnDelete.Location = new Point(panel.Width - btnDelete.Width - 20, (panel.Height - btnDelete.Height) / 2);
                btnDelete.Tag = row["MessageID"];
                string userid = _currentUser.UserID.ToString();
                string messageid = row["UserID"].ToString();
                if (userid == messageid||userid == "1")
                {
                    btnDelete.Visible = true;
                }
                else
                {
                    btnDelete.Visible = false;
                }
                btnDelete.Click += (s, e) =>
                {
                    DialogResult dr = MessageBox.Show("确定要删除该留言吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string delSql = "delete from MessagesInfo where MessageID=@id";
                        DBhelper.ExecuteNonQuery(delSql, new MySqlParameter("@id", btnDelete.Tag));
                        LoadMessagesToFlowPanel();
                    }
                };
                panel.Controls.Add(btnDelete);

                flowLayoutPanel2.Controls.Add(panel);
            }
        }

        private void uiButton18_Click(object sender, EventArgs e)
        {
            MainForm_Load(sender, e);
        }

        private void uiButton15_Click(object sender, EventArgs e)
        {
            Balance_Recharge balance_recharge = new Balance_Recharge(_currentUser);
            balance_recharge.Show();
        }
    }

}
