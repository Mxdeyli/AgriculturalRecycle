using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Sunny.UI;

namespace AgriculturalRecycle
{
    public partial class Edit: UIForm
    {
        private readonly User _currentuser;
        public Edit(User user)
        {
            InitializeComponent();
            _currentuser = user;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            UserManage usermanage = new UserManage(_currentuser);
            this.Hide();
            usermanage.Show();

        }

        private void Edit_Load(object sender, EventArgs e)
        {
            string sql = "select * from UserInfo where UserID=@userid";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userid", _currentuser.UserID));
            if (dt.Rows.Count > 0)
            {
                string avatar = dt.Rows[0]["Avatar"].ToString();
                if (avatar != "")
                {
                    Image img = Image.FromFile(avatar);
                    uiAvatar1.Image = img;
                }
                uiTextBox1.Text = dt.Rows[0]["UserName"].ToString();
                uiRichTextBox1.Text = dt.Rows[0]["Personalized_Signature"].ToString();
                uiTextBox2.Text = dt.Rows[0]["Contact_Way"].ToString();
                uiTextBox3.Text = dt.Rows[0]["region"].ToString();
                uiTextBox4.Text = dt.Rows[0]["Age"].ToString();
                uiTextBox5.Text = dt.Rows[0]["Hobby"].ToString();
                uiRichTextBox2.Text = dt.Rows[0]["ShippingAddress"].ToString();
                uiTextBox6.Text = dt.Rows[0]["Consignee"].ToString();
                uiTextBox7.Text = dt.Rows[0]["Phone"].ToString();
            }
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string sql = @"update UserInfo set 
                           UserName=@username, 
                           Personalized_Signature=@signature, 
                           Contact_Way=@contact, 
                           region=@region, 
                           Age=@age, 
                           Hobby=@hobby,
                           ShippingAddress=@address 
                           Consignee=@consignee 
                           Phone=@phone where UserID=@userid";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", uiTextBox1.Text),
                new MySqlParameter("@signature", uiRichTextBox1.Text),
                new MySqlParameter("@contact", uiTextBox2.Text),
                new MySqlParameter("@region", uiTextBox3.Text),
                new MySqlParameter("@age", uiTextBox4.Text),
                new MySqlParameter("@hobby", uiTextBox5.Text),
                new MySqlParameter("@userid", _currentuser.UserID),
                new MySqlParameter("@address", uiRichTextBox2.Text),
                new MySqlParameter("@consignee", uiTextBox6.Text),
                new MySqlParameter("@phone", uiTextBox7.Text)
            };
            DBhelper.ExecuteNonQuery(sql, parameters);
            MessageBox.Show("修改成功！");
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图片文件(*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filename = ofd.FileName;
                uiAvatar1.Image = Image.FromFile(filename);
                string sql = "update UserInfo set Avatar=@avatar where UserID=@userid";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@avatar", filename),
                    new MySqlParameter("@userid", _currentuser.UserID)
                };
                DBhelper.ExecuteNonQuery(sql, parameters);
            }
        }

        private void uiLabel9_Click(object sender, EventArgs e)
        {

        }
    }
}
