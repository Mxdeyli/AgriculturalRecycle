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
using MySql.Data.MySqlClient;
using AntdUI;

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
            string sql = "select * from userinfo where UserID=@userid";
            var dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userid", _currentuser.UserID));
            if (dt.Rows.Count > 0)
            {
                string avatar = dt.Rows[0]["Avatar"].ToString();
                if (avatar!= "")
                {
                    Image img = Image.FromFile(dt.Rows[0]["Avatar"].ToString());
                    uiAvatar1.Image = img;
                }
                uiLabel1.Text = dt.Rows[0]["UserName"].ToString();
                uiLabel2.Text = dt.Rows[0]["Personalized_Signature"].ToString();
                uiLabel3.Text = "ID:" + dt.Rows[0]["UserID"].ToString();
                uiLabel4.Text = "性别:" + dt.Rows[0]["Gender"].ToString();
                uiLabel5.Text = "信用评分:" + dt.Rows[0]["CreditScore"].ToString();
                uiLabel6.Text = "年龄:" + dt.Rows[0]["Age"].ToString();
            }
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

        private void uiLabel1_Click(object sender, EventArgs e)
        {

        }

        private void uiLabel2_Click(object sender, EventArgs e)
        {

        }

        private void uiLabel6_Click(object sender, EventArgs e)
        {

        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            UpdateAccount update = new UpdateAccount(_currentuser);
            this.Hide();
            update.Show();
        }

        private void uiAvatar1_Click(object sender, EventArgs e)
        {

        }
    }
}
