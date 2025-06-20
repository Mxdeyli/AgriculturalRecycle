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

namespace AgriculturalRecycle
{
    public partial class Device_BORE: UIForm
    {
        private readonly User _currentUser;
        private string[] arr = { "全部", "粉碎设备", "运输设备", "打包设备", "发酵设备", "检测设备" };
        public Device_BORE(User user)
        {
            InitializeComponent();
            _currentUser = user;
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
                nameLabel.Text = row["EquipmentName"]?.ToString();
                nameLabel.AutoSize = false;
                nameLabel.BackColor = Color.Transparent;
                nameLabel.Size = new Size(panel.Width - 5, 18);
                nameLabel.TextAlign = ContentAlignment.MiddleCenter;
                nameLabel.Location = new Point(0, imgBtn.Bottom + 5);

                UILabel priceLabel = new UILabel();
                priceLabel.Text = row["Rental"]?.ToString()+"元/月";
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

                UILabel durationLabel = new UILabel();
                durationLabel.Text = row["RentalDuration"]?.ToString();
                durationLabel.AutoSize = false;
                durationLabel.BackColor = Color.Transparent;
                durationLabel.Size = new Size(panel.Width - 5, 18);
                durationLabel.TextAlign = ContentAlignment.MiddleCenter;
                durationLabel.Location = new Point(0, categoryLabel.Bottom + 2);

                imgBtn.Click += (s, e) =>
                {

                    int equipmentId = row.Table.Columns.Contains("EquipmentID") && row["EquipmentID"] != DBNull.Value ? Convert.ToInt32(row["EquipmentID"]) : 0;
                    string productName = row["EquipmentName"]?.ToString();
                    int productId = row.Table.Columns.Contains("ProductID") && row["ProductID"] != DBNull.Value ? Convert.ToInt32(row["ProductID"]) : 0;
                    int categoryId = row.Table.Columns.Contains("CategoryID") && row["CategoryID"] != DBNull.Value ? Convert.ToInt32(row["CategoryID"]) : 0;

                    Item item = new Item(equipmentId, productName, productId, categoryId);
                    List<Item> itemList = new List<Item> { item };
                    BoreCommodity brcommodityForm = new BoreCommodity(_currentUser, item);
                    brcommodityForm.Show();
                    this.Hide();
                };

                panel.Controls.Add(imgBtn);
                panel.Controls.Add(nameLabel);
                panel.Controls.Add(priceLabel);
                panel.Controls.Add(categoryLabel);
                panel.Controls.Add(durationLabel);

                flowLayoutPanel2.Controls.Add(panel);
            }
        }

        public void ClearPanel()
        {
            flowLayoutPanel2.Controls.Clear();
        }

        public void ShowALLEquipment()
        {
            ClearPanel();
            string sql = @"
        SELECT 
            pe.ProductID, 
            pe.EquipmentID,
            pe.CategoryID,
            e.EquipmentName,
            pe.Rental, 
            c.CategoryName, 
            i.ImageURL,
            pe.RentalDuration
        FROM borrowequipment pe,equipmentcategories c,productimages i,equipment e
        WHERE pe.CategoryID = c.CategoryID  
        AND pe.ProductID = i.ProductID 
        AND pe.EquipmentID = e.EquipmentID";
            CreateDevice(sql);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(_currentUser);
            this.Hide();
            mainForm.Show();
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Device_BORE_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                uiComboBox1.Items.Add(arr[i]);
            }
            uiComboBox1.SelectedIndex = 0;
        }

        private void uiButton8_Click(object sender, EventArgs e)
        {
            UserManage userManage = new UserManage(_currentUser);
            this.Hide();
            userManage.Show();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            RentalSetting rentalSetting = new RentalSetting(_currentUser);
            rentalSetting.FormClosed += (s, args) => { this.ShowALLEquipment(); };
            rentalSetting.Show();
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            SoldOut soldOut = new SoldOut(_currentUser);
            soldOut.FormClosed += (s, args) => { this.ShowALLEquipment(); };
            soldOut.Show();
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (uiComboBox1.SelectedIndex)
            {
                case 0:
                    ShowALLEquipment();
                    break;
                case 1:
                    ClearPanel();
                    string sql2 = @"
        SELECT 
            pe.ProductID, 
            pe.EquipmentID,
            pe.CategoryID,
            e.EquipmentName,
            pe.Rental,
            c.CategoryName, 
            i.ImageURL,
            pe.RentalDuration
        FROM borrowequipment pe,equipmentcategories c,productimages i,equipment e
        WHERE pe.CategoryID = c.CategoryID  
        AND pe.ProductID = i.ProductID 
        AND c.CategoryName = '粉碎设备' 
        AND pe.EquipmentID = e.EquipmentID"; CreateDevice(sql2);
                    break;
                case 2:
                    ClearPanel();
                    string sql3 = @"
        SELECT 
            pe.ProductID, 
            pe.EquipmentID,
            pe.CategoryID,
            e.EquipmentName,
            pe.Rental,
            c.CategoryName, 
            i.ImageURL,
            pe.RentalDuration
        FROM borrowequipment pe,equipmentcategories c,productimages i,equipment e
        WHERE pe.CategoryID = c.CategoryID  
        AND pe.ProductID = i.ProductID 
        AND c.CategoryName = '运输设备' 
        AND pe.EquipmentID = e.EquipmentID"; CreateDevice(sql3);
                    break;
                case 3:
                    ClearPanel();
                    string sql4 = @"
        SELECT 
            pe.ProductID, 
            pe.EquipmentID,
            pe.CategoryID,
            e.EquipmentName,
            pe.Rental,
            c.CategoryName, 
            i.ImageURL,
            pe.RentalDuration
        FROM borrowequipment pe,equipmentcategories c,productimages i,equipment e
        WHERE pe.CategoryID = c.CategoryID  
        AND pe.ProductID = i.ProductID 
        AND c.CategoryName = '打包设备' 
        AND pe.EquipmentID = e.EquipmentID"; CreateDevice(sql4);
                    break;
                case 4:
                    ClearPanel();
                    string sql5 = @"
        SELECT 
            pe.ProductID, 
            pe.EquipmentID,
            pe.CategoryID,
            e.EquipmentName,
            pe.Rental,
            c.CategoryName, 
            i.ImageURL,
            pe.RentalDuration
        FROM borrowequipment pe,equipmentcategories c,productimages i,equipment e
        WHERE pe.CategoryID = c.CategoryID  
        AND pe.ProductID = i.ProductID 
        AND c.CategoryName = '发酵设备' 
        AND pe.EquipmentID = e.EquipmentID"; CreateDevice(sql5);
                    break;
                case 5:
                    ClearPanel();
                    string sql6 = @"
        SELECT 
            pe.ProductID, 
            pe.EquipmentID,
            pe.CategoryID,
            e.EquipmentName,
            pe.Rental,
            c.CategoryName, 
            i.ImageURL,
            pe.RentalDuration
        FROM borrowequipment pe,equipmentcategories c,productimages i,equipment e
        WHERE pe.CategoryID = c.CategoryID  
        AND pe.ProductID = i.ProductID 
        AND c.CategoryName = '检测设备' 
        AND pe.EquipmentID = e.EquipmentID"; CreateDevice(sql6);
                    break;
                default:
                    break;
            }
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            string ProductName = uiTextBox1.Text;
            string sqlsearch = @"
        SELECT 
            pe.ProductID, 
            pe.EquipmentID,
            pe.CategoryID,
            e.EquipmentName,
            pe.Rental,
            c.CategoryName, 
            i.ImageURL,
            pe.RentalDuration
        FROM borrowequipment pe,equipmentcategories c,productimages i,equipment e
        WHERE pe.CategoryID = c.CategoryID  
        AND pe.ProductID = i.ProductID 
        AND pe.EquipmentID = e.EquipmentID
        AND e.EquipmentName LIKE '%"+ProductName+"%'";
            ClearPanel();
            CreateDevice(sqlsearch);
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            GiveBack giveBack = new GiveBack(_currentUser);
            giveBack.Show();
        }
    }
}
