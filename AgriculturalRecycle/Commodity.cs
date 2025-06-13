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
    public partial class Commodity: UIForm
    {
        private readonly User _currentUser;
        private readonly Item _currentItem;
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
            DataTable dt=DBhelper.ExecuteQuery(sql, new MySqlParameter("@productid", _currentItem.ProductID));
            if(dt.Rows.Count>0)
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
                uiLabel2.Text = "剩余库存："+dt.Rows[0]["Stock"].ToString();
                uiLabel3.Text = "商品价格："+dt.Rows[0]["Price"].ToString();
                uiLabel4.Text = "商品类别："+dt.Rows[0]["CategoryName"].ToString();
                uiLabel6.Text = dt.Rows[0]["Description"].ToString();
                uiLabel7.Text = dt.Rows[0]["Specification"].ToString();
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
