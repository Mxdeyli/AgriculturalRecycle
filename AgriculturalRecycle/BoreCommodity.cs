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
        private int Rental;
        private decimal Deposit;
        public BoreCommodity(User currentUser, Item currentItem)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _currentItem = currentItem;
        }

        private void BoreCommodity_Load(object sender, EventArgs e)
        {
            string sql = @"
                SELECT 
                be.ProductID,
                e.ProductName,
                e.Description,
                be.Rental,
                e.Specification,
                be.CategoryID,
                be.EquipmentID,
                c.CategoryName,
                be.Deposit,
                p.ImageURL
                FROM equipmentproducts e,productimages p,equipmentcategories c,borrowequipment be
                WHERE be.ProductID = e.ProductID AND e.ProductID = p.ProductID AND e.CategoryID = c.CategoryID AND be.EquipmentID = @equipmentID AND be.ProductID = @productID;";
            DataTable dt = DBhelper.ExecuteQuery(sql,new MySqlParameter("@equipmentID", _currentItem.EquipmentID), new MySqlParameter("@productID", _currentItem.ProductID) );
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
                uiLabel2.Text = "商品租金：" + dt.Rows[0]["Rental"].ToString();
                uiLabel3.Text = "商品押金：" + dt.Rows[0]["Deposit"].ToString();
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
                Rental= int.Parse(dt.Rows[0]["Rental"].ToString());
                Deposit = decimal.Parse(dt.Rows[0]["Deposit"].ToString());
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            Rent rent = new Rent(_currentUser, _currentItem);
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
