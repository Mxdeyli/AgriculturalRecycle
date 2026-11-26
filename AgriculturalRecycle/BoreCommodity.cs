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
using Newtonsoft.Json.Linq;
using System.Collections;

namespace AgriculturalRecycle
{
    public partial class BoreCommodity: UIForm
    {
        private readonly User _currentUser;
        private Item _currentItem;
        private readonly int ownerId;
        private readonly object listingDate;
        private readonly decimal monthlyRent;
        private readonly decimal deposit;
        private readonly string duration;

        public BoreCommodity(User currentUser, Item currentItem, int ownerId, object listingDate,
            decimal monthlyRent, decimal deposit, string duration)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _currentItem = currentItem;
            this.ownerId = ownerId;
            this.listingDate = listingDate;
            this.monthlyRent = monthlyRent;
            this.deposit = deposit;
            this.duration = duration;
        }

        private void BoreCommodity_Load(object sender, EventArgs e)
        {
            string sql = @"
                SELECT 
                e.ProductName,
                e.Description,
                e.Specification,
                c.CategoryName,
                p.ImageURL
                FROM equipmentproducts e
                INNER JOIN equipmentcategories c ON e.CategoryID = c.CategoryID
                LEFT JOIN productimages p ON e.ProductID = p.ProductID
                WHERE e.ProductID = @productId;";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@productId", _currentItem.ProductID));
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
                uiLabel1.Text = _currentItem.ProductName;
                uiLabel2.Text = "商品租金：" + monthlyRent.ToString();
                uiLabel3.Text = "商品押金：" + deposit.ToString();
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
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            Rent rent = new Rent(_currentUser, _currentItem, ownerId, listingDate, monthlyRent, deposit, duration);
            rent.Show();
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            Device_BORE db = new Device_BORE(_currentUser);
            _currentItem.Turnout();
            db.Show();
            this.Hide();
        }
    }
}
