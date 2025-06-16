using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntdUI;
using Sunny.UI;
using MySql.Data.MySqlClient;

namespace AgriculturalRecycle
{
    public partial class Device_Store: UIForm
    {
        private readonly User _currentUser;
        private string[] arr = {"全部","粉碎设备","运输设备","打包设备","发酵设备","检测设备" };
        public Device_Store(User user)
        {
            InitializeComponent();
            _currentUser = user;
            this.KeyDown += new KeyEventHandler(Device_Store_KeyDown);
        }

        private void Device_Store_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uiButton4_Click(sender, e);
            }
        }


        public void CreateDevice(string sql)
        {
            DataTable dt = DBhelper.ExecuteQuery(sql, null);

            foreach (DataRow row in dt.Rows)
            {
                UIPanel panel = new UIPanel();
                panel.Size = new Size(272, 300);
                panel.Margin = new Padding(5);

                UIImageButton imgBtn = new UIImageButton();
                imgBtn.Size = new Size(196, 196);
                imgBtn.Location = new Point((panel.Width - imgBtn.Width) / 2, 10);
                imgBtn.BackgroundImageLayout = ImageLayout.Stretch;
                string imagePath = row["ImageURL"]?.ToString();
                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    using (var img = Image.FromFile(imagePath))
                    {
                        imgBtn.BackgroundImage = new Bitmap(img);
                    }
                }
                else
                {
                    imgBtn.BackgroundImage = null;
                }

                UILabel nameLabel = new UILabel();
                nameLabel.Text = row["ProductName"]?.ToString();
                nameLabel.AutoSize = false;
                nameLabel.BackColor = Color.Transparent;
                nameLabel.Size = new Size(panel.Width - 5, 18);
                nameLabel.TextAlign = ContentAlignment.MiddleCenter;
                nameLabel.Location = new Point(0, imgBtn.Bottom + 5);

                UILabel priceLabel = new UILabel();
                priceLabel.Text = "￥" + row["Price"]?.ToString();
                priceLabel.AutoSize = false;
                priceLabel.BackColor = Color.Transparent;
                priceLabel.Size = new Size(panel.Width - 5, 18);
                priceLabel.TextAlign = ContentAlignment.MiddleCenter;
                priceLabel.Location = new Point(0, nameLabel.Bottom + 2);

                UILabel categoryLabel = new UILabel();
                categoryLabel.Text = row["CategoryName"]?.ToString();
                categoryLabel.AutoSize = false;
                categoryLabel.BackColor = Color.Transparent;
                categoryLabel.Size = new Size(panel.Width - 5, 18);
                categoryLabel.TextAlign = ContentAlignment.MiddleCenter;
                categoryLabel.Location = new Point(0, priceLabel.Bottom + 2);

                imgBtn.Click += (s, e) =>
                {

                    int equipmentId = row.Table.Columns.Contains("EquipmentID") && row["EquipmentID"] != DBNull.Value ? Convert.ToInt32(row["EquipmentID"]) : 0;
                    string productName = row["ProductName"]?.ToString();
                    int productId = row.Table.Columns.Contains("ProductID") && row["ProductID"] != DBNull.Value ? Convert.ToInt32(row["ProductID"]) : 0;
                    int categoryId = row.Table.Columns.Contains("CategoryID") && row["CategoryID"] != DBNull.Value ? Convert.ToInt32(row["CategoryID"]) : 0;

                    Item item = new Item(equipmentId, productName, productId, categoryId);
                    List<Item> itemList = new List<Item> { item };
                    Commodity commodityForm = new Commodity(_currentUser, item);
                    commodityForm.Show();
                    this.Hide();
                };

                panel.Controls.Add(imgBtn);
                panel.Controls.Add(nameLabel);
                panel.Controls.Add(priceLabel);
                panel.Controls.Add(categoryLabel);

                flowLayoutPanel2.Controls.Add(panel);
            }
        }

        public void ClearPanel()
        {
            flowLayoutPanel2.Controls.Clear();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(_currentUser);
            this.Hide();
            mainForm.Show();
        }

        private void uiImageButton1_Click(object sender, EventArgs e)
        {

        }

        private void uiLabel1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(_currentUser);
            this.Hide();
            mainForm.Show();
            mainForm.uiTabControlMenu1.SelectedTab = mainForm.tabPage6;
        }

        private void Device_Store_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                uiComboBox1.Items.Add(arr[i]);
            }
            uiComboBox1.SelectedIndex = 0;

        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (uiComboBox1.SelectedIndex)
            {
                case 0:
                        ClearPanel();
                        string sql = @"
        SELECT 
            p.ProductID, 
            p.EquipmentID,
            p.CategoryID,
            p.ProductName, 
            p.Price, 
            c.CategoryName, 
            i.ImageURL
        FROM equipmentproducts p
        LEFT JOIN equipmentcategories c ON p.CategoryID = c.CategoryID
        LEFT JOIN productimages i ON p.ProductID = i.ProductID "; CreateDevice(sql);
                    break;
                case 1:
                        ClearPanel();
                        string sql2 = @"
        SELECT 
            p.ProductID, 
            p.EquipmentID,
            p.CategoryID,
            p.ProductName, 
            p.Price, 
            c.CategoryName, 
            i.ImageURL
        FROM equipmentproducts p
        LEFT JOIN equipmentcategories c ON p.CategoryID = c.CategoryID
        LEFT JOIN productimages i ON p.ProductID = i.ProductID where c.CategoryName = '粉碎设备'"; CreateDevice(sql2);
                    break;
                case 2:
                        ClearPanel();
                        string sql3 = @"
        SELECT 
            p.ProductID, 
            p.EquipmentID,
            p.CategoryID,
            p.ProductName, 
            p.Price, 
            c.CategoryName, 
            i.ImageURL
        FROM equipmentproducts p
        LEFT JOIN equipmentcategories c ON p.CategoryID = c.CategoryID
        LEFT JOIN productimages i ON p.ProductID = i.ProductID where c.CategoryName = '运输设备'"; CreateDevice(sql3);
                    break;
                case 3:
                        ClearPanel();
                        string sql4 = @"
        SELECT 
            p.ProductID, 
            p.EquipmentID,
            p.CategoryID,
            p.ProductName, 
            p.Price, 
            c.CategoryName, 
            i.ImageURL
        FROM equipmentproducts p
        LEFT JOIN equipmentcategories c ON p.CategoryID = c.CategoryID
        LEFT JOIN productimages i ON p.ProductID = i.ProductID where c.CategoryName = '打包设备'"; CreateDevice(sql4);
                    break;
                case 4:
                        ClearPanel();
                        string sql5 = @"
        SELECT 
            p.ProductID, 
            p.EquipmentID,
            p.CategoryID,
            p.ProductName, 
            p.Price, 
            c.CategoryName, 
            i.ImageURL
        FROM equipmentproducts p
        LEFT JOIN equipmentcategories c ON p.CategoryID = c.CategoryID
        LEFT JOIN productimages i ON p.ProductID = i.ProductID where c.CategoryName = '发酵设备'"; CreateDevice(sql5);
                    break;
                case 5:
                        ClearPanel();
                        string sql6 = @"
        SELECT 
            p.ProductID, 
            p.EquipmentID,
            p.CategoryID,
            p.ProductName, 
            p.Price, 
            c.CategoryName, 
            i.ImageURL
        FROM equipmentproducts p
        LEFT JOIN equipmentcategories c ON p.CategoryID = c.CategoryID
        LEFT JOIN productimages i ON p.ProductID = i.ProductID where c.CategoryName = '检测设备'"; CreateDevice(sql6);
                    break;
                default:
                    break;
            }
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void uiRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton1.Enabled = true;
            uiImageButton3.Enabled = false;
            uiImageButton4.Enabled = false;
            uiImageButton1.Visible = true;
            uiImageButton3.Visible = false;
            uiImageButton4.Visible = false;
        }

        private void uiRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton1.Enabled = false;
            uiImageButton3.Enabled = true;
            uiImageButton4.Enabled = false;
            uiImageButton1.Visible = false;
            uiImageButton3.Visible = true;
            uiImageButton4.Visible = false;
        }

        private void uiRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            uiImageButton1.Enabled = false;
            uiImageButton3.Enabled = false;
            uiImageButton4.Enabled = true;
            uiImageButton1.Visible = false;
            uiImageButton3.Visible = false;
            uiImageButton4.Visible = true;
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            string ProductName = uiTextBox1.Text;
            string sqlsearch = @"
        SELECT 
            p.ProductName, 
            p.Price, 
            c.CategoryName, 
            i.ImageURL
        FROM equipmentproducts p
        LEFT JOIN equipmentcategories c ON p.CategoryID = c.CategoryID
        LEFT JOIN productimages i ON p.ProductID = i.ProductID where p.ProductName like '%" + ProductName + "%'";
            ClearPanel();
            CreateDevice(sqlsearch);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            UserManage userManage = new UserManage(_currentUser);
            this.Hide();
            userManage.Show();
        }

        private void uiLabel4_Click(object sender, EventArgs e)
        {

        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            Shopping_Cart shopping_Cart = new Shopping_Cart(_currentUser);
            this.Hide();
            shopping_Cart.Show();
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            OrderHistory orderHistory = new OrderHistory(_currentUser);
            this.Hide();
            orderHistory.Show();
        }
    }
}
