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

namespace AgriculturalRecycle
{
    public partial class OrderHistory: UIForm
    {
        private readonly User _currentUser;
        public OrderHistory(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void OrderHistory_Load(object sender, EventArgs e)
        {
            OderHistoryCart();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            Device_Store ds = new Device_Store(_currentUser);
            ds.Show();
            this.Hide();
        }

        private void OderHistoryCart()
        {
            string sql = @"SELECT 
                           o.OrderID,
                           o.Receivingtime, 
                           p.ImageURL,
                           e.ProductName, 
                           c.CategoryName,
                           o.Count,
                           o.TotalPrice,
                           o.Consignee,
                           o.ProductID,
                           o.CategoryID,
                           e.EquipmentID FROM OrderHistory o,equipmentproducts e,equipmentcategories c,productimages p
                           WHERE p.ProductID=e.ProductID AND e.CategoryID=c.CategoryID AND o.UserID=@UserID";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@UserID", _currentUser.UserID));
            foreach (DataRow row in dt.Rows)
            {
                string orderID = row["OrderID"].ToString();
                string orderDate = Convert.ToDateTime(row["Receivingtime"]).ToString("yyyy-MM-dd HH:mm:ss");
                string productname = row["ProductName"].ToString();
                string categoryname = row["CategoryName"].ToString();
                string imageurl = row["ImageURL"].ToString();
                int count = int.Parse(row["Count"].ToString());
                double totalprice = double.Parse(row["TotalPrice"].ToString());
                string consignee = row["Consignee"].ToString();
                int productId = int.Parse(row["ProductID"].ToString());
                int categoryId = int.Parse(row["CategoryID"].ToString());
                int equipmentId = int.Parse(row["EquipmentID"].ToString());

                var panel = new Sunny.UI.UIPanel
                {
                    Size = new Size(1045, 196),
                    Padding = new Padding(0),
                    Margin = new Padding(0, 5, 0, 5),
                    Tag = "OrderPanel"
                };

                var label1 = new Sunny.UI.UILabel
                {
                    Name = "nameLabel1",
                    BackColor = Color.FromArgb(0),
                    Text = orderID,
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(0, 0)
                };
                panel.Controls.Add(label1);

                var imageButton = new Sunny.UI.UIImageButton
                {
                    Size = new Size(172, 162),
                    Location = new Point(7, label1.Height+5),
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
                    Commodity cd = new Commodity(_currentUser, item);
                    this.Hide();
                    cd.Show();
                };
                panel.Controls.Add(imageButton);

                var label2 = new Sunny.UI.UILabel
                {
                    Name = "nameLabel2",
                    BackColor = Color.Transparent,
                    Text = productname,
                    Font = new Font("微软雅黑", 20F),
                    AutoSize = true,
                    Location = new Point(imageButton.Right + 110, label1.Height)
                };
                panel.Controls.Add(label2);
                
                var label3 = new Sunny.UI.UILabel
                {
                    Name = "nameLabel3",
                    BackColor = Color.Transparent,
                    Text = categoryname,
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(imageButton.Right + 110, label2.Height +85)
                };
                panel.Controls.Add(label3);

                var label4 = new Sunny.UI.UILabel
                {
                    Name = "nameLabel4",
                    BackColor = Color.Transparent,
                    Text = "X" + count,
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(label2.Right + 87, label2.Height)
                };
                panel.Controls.Add(label4);

                var label5 = new Sunny.UI.UILabel
                {
                    Name = "nameLabel5",
                    BackColor = Color.Transparent,
                    Text = "￥" + totalprice,
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(label4.Right + 215 , label2.Height)
                };
                panel.Controls.Add(label5);

                var label6 = new Sunny.UI.UILabel
                {
                    Name = "nameLabel6",
                    BackColor = Color.Transparent,
                    Text = "收货人:"+consignee,
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(label2.Right+40, label2.Height + 130)
                };
                panel.Controls.Add(label6);

                var label7 = new Sunny.UI.UILabel
                {
                    Name = "nameLabel7",
                    BackColor = Color.Transparent,
                    Text = "时间:"+orderDate,
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(label4.Right+150, label2.Height+130)
                };
                panel.Controls.Add(label7);

                flowLayoutPanel1.Controls.Add(panel);
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定要清除所有订单记录吗？此操作不可恢复。", "确认清除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string sql = "DELETE FROM OrderHistory WHERE UserID=@UserID";
                int rows = DBhelper.ExecuteNonQuery(sql, new MySqlParameter("@UserID", _currentUser.UserID));

                // 只移除Tag为"OrderPanel"的UIPanel
                for (int i = flowLayoutPanel1.Controls.Count - 1; i >= 0; i--)
                {
                    var ctrl = flowLayoutPanel1.Controls[i];
                    if (ctrl is Sunny.UI.UIPanel && ctrl.Tag != null && ctrl.Tag.ToString() == "OrderPanel")
                    {
                        flowLayoutPanel1.Controls.RemoveAt(i);
                    }
                }
                MessageBox.Show("订单记录已清除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
