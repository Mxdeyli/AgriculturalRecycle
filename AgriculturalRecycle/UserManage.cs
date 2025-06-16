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

        private void CraeteDevice()
        {
            string sql = @"SELECT 
                           p.ImageURL,
                           e.ProductID,
                           e.CategoryID,
                           e.EquipmentID,
                           e.ProductName,
                           u.Count 
                           FROM UserEquipment u,EquipmentProducts e,productimages p
                           WHERE u.EquipmentID = e.EquipmentID AND e.ProductID = p.ProductID AND u.UserID = @userid";
            var dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userid", _currentuser.UserID));
            flowLayoutPanel2.Controls.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string imgurl = row["ImageURL"].ToString();
                string equipname = row["ProductName"].ToString();
                int count = int.Parse(row["Count"].ToString());
                int productId = int.Parse(row["ProductID"].ToString());
                int categoryId = int.Parse(row["CategoryID"].ToString());
                int equipmentId = int.Parse(row["EquipmentID"].ToString());
                string productname = row["ProductName"].ToString();

                var panel = new Sunny.UI.UIPanel
                {
                    Size = new Size(210,210),
                    Padding = new Padding(5),
                    Margin = new Padding(0, 5, 0, 5),
                    Tag = "OrderPanel"
                };

                var imageButton = new Sunny.UI.UIImageButton
                {
                    Size = new Size(180, 143),
                    Location = new Point(14,12),
                    BackgroundImageLayout = ImageLayout.Stretch
                };
                try
                {
                    string imgPath = row["ImageURL"].ToString();
                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        imageButton.BackgroundImage = Image.FromFile(imgPath);
                    }
                }
                catch { }
                imageButton.Click += (s, e) =>
                {
                    Item item = new Item(equipmentId, productname, productId, categoryId);
                    Commodity cd = new Commodity(_currentuser, item);
                    this.Hide();
                    cd.Show();
                };
                panel.Controls.Add(imageButton);

                var label1 = new Sunny.UI.UILabel
                {
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent,
                    Text = equipname,
                    Font = new Font("微软雅黑", 12),
                    Location = new Point(19, 158),
                    ForeColor = Color.Black,
                    Size = new Size(180, 20)
                };
                panel.Controls.Add(label1);

                var label2 = new Sunny.UI.UILabel
                {
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent,
                    Text = "X" + count,
                    Font = new Font("微软雅黑", 12),
                    Location = new Point(23, 181),
                    ForeColor = Color.Black,
                    Size = new Size(182, 20)
                };
                panel.Controls.Add(label2);

                flowLayoutPanel2.Controls.Add(panel);
            }
                
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
                uiLabel2.Text = "个性签名:\n"+dt.Rows[0]["Personalized_Signature"].ToString();
                uiLabel3.Text = "ID:" + dt.Rows[0]["UserID"].ToString();
                uiLabel4.Text = "性别:" + dt.Rows[0]["Gender"].ToString();
                uiLabel5.Text = "信用评分:" + dt.Rows[0]["CreditScore"].ToString();
                uiLabel6.Text = "年龄:" + dt.Rows[0]["Age"].ToString();
                uiLabel7.Text="余额:"+dt.Rows[0]["Balance"].ToString();
            }
            CraeteDevice();
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

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
