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
using Newtonsoft.Json.Linq;

namespace AgriculturalRecycle
{
    public partial class Commodity: UIForm
    {
        private readonly User _currentUser;
        private readonly Item _currentItem;
        public double Price=0;
        public int Stock=0;
        public Commodity(User currentUser, Item currentItem)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _currentItem = currentItem;
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            Device_Store ds = new Device_Store(_currentUser);
            _currentItem.Turnout();
            this.Hide();
            ds.Show();
        }

        private void Commodity_Load(object sender, EventArgs e)
        {
            string sql = @"
                SELECT 
                e.ProductID,
                e.ProductName,
                e.Description,
                e.Price,
                e.Specification,
                e.CategoryID,
                e.EquipmentID,
                c.CategoryName,
                e.Stock,
                p.ImageURL
                FROM equipmentproducts e,productimages p,equipmentcategories c
                WHERE e.ProductID = p.ProductID AND e.CategoryID = c.CategoryID AND e.ProductID = @productid";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@productid", _currentItem.ProductID));
            if (dt.Rows.Count > 0)
            {
                string imageURL = dt.Rows[0]["ImageURL"].ToString();
                if (!string.IsNullOrEmpty(imageURL) && System.IO.File.Exists(imageURL))
                {
                    using (var img = Image.FromFile(imageURL))
                    {
                        pictureBox1.BackgroundImage = new Bitmap(img);
                    }
                }
                uiLabel1.Text = dt.Rows[0]["ProductName"].ToString();
                uiLabel2.Text = "剩余库存：" + dt.Rows[0]["Stock"].ToString();
                uiLabel3.Text = "商品价格：" + dt.Rows[0]["Price"].ToString();
                uiLabel4.Text = "商品类别：" + dt.Rows[0]["CategoryName"].ToString();
                uiLabel6.Text = dt.Rows[0]["Description"].ToString();
                string specJson = dt.Rows[0]["Specification"].ToString();
                string specDisplay = "";
                if (!string.IsNullOrWhiteSpace(specJson))
                {
                    try
                    {
                        var jObj = JObject.Parse(specJson);
                        foreach (var prop in jObj.Properties())
                        {
                            specDisplay += $"{prop.Name}：{prop.Value}\r\n";
                        }
                    }
                    catch
                    {
                        specDisplay = specJson;
                    }
                }
                uiLabel7.Text = specDisplay.TrimEnd();
                Price = double.Parse(dt.Rows[0]["Price"].ToString());
                Stock = int.Parse(dt.Rows[0]["Stock"].ToString());
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (Stock > 0)
            {
                string sqlcheck = @"SELECT COUNT(*) FROM shoppingcarts WHERE UserID = @userid AND ProductID = @productid";
                int count = (int)DBhelper.ExecuteScalar(sqlcheck,new MySqlParameter("@userid", _currentUser.UserID), new MySqlParameter("@productid", _currentItem.ProductID));
                if (count > 0)
                {
                    int newcount=count+1;
                    string sqlupdate = @"UPDATE shoppingcarts SET Quantity = @quantity WHERE UserID = @userid AND ProductID = @productid";
                    DBhelper.ExecuteNonQuery(sqlupdate, new MySqlParameter("@quantity", newcount), new MySqlParameter("@userid", _currentUser.UserID), new MySqlParameter("@productid", _currentItem.ProductID));
                }
                else
                {
                    string sqli = @"INSERT INTO shoppingcarts (UserID, ProductID, ProductName, CategoryID,EquipmentID) VALUES (@userid, @productid, @productname, @categoryid,@equipmentid)";
                    DBhelper.ExecuteNonQuery(sqli, new MySqlParameter("@userid", _currentUser.UserID), new MySqlParameter("@productid", _currentItem.ProductID), new MySqlParameter("@productname", uiLabel1.Text), new MySqlParameter("@categoryid", _currentItem.CategoryID), new MySqlParameter("@equipmentid", _currentItem.EquipmentID));
                }
                UIMessageBox.ShowSuccess("已加入购物车！");
                Device_Store ds = new Device_Store(_currentUser);
                _currentItem.Turnout();
                this.Hide();
                ds.Show();
            }
            else
            {
                UIMessageBox.ShowWarning("库存不足，无法加入购物车！");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
