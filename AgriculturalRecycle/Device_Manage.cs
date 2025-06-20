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
    public partial class Device_Manage: UIForm
    {
        private readonly User _currentUser;
        private string[] arr = { "粉碎设备", "运输设备", "打包设备", "发酵设备", "检测设备" };
        private string[] arr2 = { "设备名称", "EquipmentID" };
        public Device_Manage(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void DGVLoad()
        {
            string sql = "SELECT * FROM equipmentproducts";
            DataTable dt=DBhelper.ExecuteQuery(sql);
            uiDataGridView1.DataSource = dt;
            string sql2 = "SELECT * FROM equipment";
            DataTable dt2 = DBhelper.ExecuteQuery(sql2);
            uiDataGridView2.DataSource = dt2;
            string sql3 = "SELECT * FROM equipmentcategories";
            DataTable dt3 = DBhelper.ExecuteQuery(sql3);
            uiDataGridView3.DataSource = dt3;
            string sql4 = "SELECT * FROM productimages";
            DataTable dt4 = DBhelper.ExecuteQuery(sql4);
            uiDataGridView4.DataSource = dt4;
        }

        private void Device_Manage_Load(object sender, EventArgs e)
        {
            DGVLoad();
            for (int i = 0; i < arr.Length; i++)
            {
                uiComboBox3.Items.Add(arr[i]);
                uiComboBox4.Items.Add(arr[i]);
            }
            uiComboBox3.SelectedIndex = 0;
            uiComboBox4.SelectedIndex = 0;
            for (int i = 0; i < arr2.Length; i++)
            {
                uiComboBox1.Items.Add(arr2[i]);
                uiComboBox2.Items.Add(arr2[i]);
                uiComboBox5.Items.Add(arr2[i]);
            }
            uiComboBox1.SelectedIndex = 0;
            uiComboBox2.SelectedIndex = 0;
            uiComboBox5.SelectedIndex = 0;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(_currentUser);
            mainForm.Show();
            this.Hide();
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            string productname = uiTextBox1.Text;
            string equipmentname = uiTextBox1.Text;
            string categoryname = uiComboBox3.Text;
            string description = uiRichTextBox1.Text;
            int price = int.Parse(uiTextBox3.Text);
            int costprice = int.Parse(uiTextBox4.Text);
            int stock = int.Parse(uiTextBox5.Text);
            string Model=uiTextBox8.Text;
            int lifespan = int.Parse(uiTextBox2.Text);
            var lines = uiRichTextBox2.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var dict = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    dict[parts[0].Trim()] = parts[1].Trim();
                }
            }
            string sqlsearch = "SELECT CategoryID FROM equipmentcategories WHERE CategoryName = @categoryname";
            int categoryid = (int)DBhelper.ExecuteScalar(sqlsearch, new MySqlParameter[] { new MySqlParameter("@categoryname", categoryname) });
            string specificationJson = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
            string sql2 = @"INSERT INTO equipment 
                           (EquipmentName, CategoryID,PurchasePrice,PurchaseDate, LifeSpan, Model)
                           VALUES (@equipmentname, @categoryid,@purchaseprice, @purchasedate, @lifespan, @model)";
            DBhelper.ExecuteNonQuery(sql2, new MySqlParameter[] {
                new MySqlParameter("@equipmentname", equipmentname),
                new MySqlParameter("@categoryid", categoryid),
                new MySqlParameter("@purchaseprice", costprice),
                new MySqlParameter("@purchasedate", DateTime.Now),
                new MySqlParameter("@lifespan", lifespan),
                new MySqlParameter("@model", Model)
            });
            string sql3 = "SELECT EquipmentID FROM equipment WHERE EquipmentName = @equipmentname";
            int equipmentid = (int)DBhelper.ExecuteScalar(sql3, new MySqlParameter[] { new MySqlParameter("@equipmentname", equipmentname) });
            string sql = @"INSERT INTO equipmentproducts 
                          (ProductName, EquipmentID, CategoryID, Description, Price, CostPrice, Stock, Specification) 
                           VALUES (@productname, @equipmentid, @categoryid, @description, @price, @costprice, @stock, @specification)";
            DBhelper.ExecuteNonQuery(sql, new MySqlParameter[] {
                new MySqlParameter("@productname", productname),
                new MySqlParameter("@equipmentid", equipmentid),
                new MySqlParameter("@categoryid", categoryid),
                new MySqlParameter("@description", description),
                new MySqlParameter("@price", price),
                new MySqlParameter("@costprice", costprice),
                new MySqlParameter("@stock", stock),
                new MySqlParameter("@specification", specificationJson)
            });
            UIMessageBox.Show("添加成功！");
            DGVLoad();
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            string productname = uiTextBox7.Text;
            string equipmentname = uiTextBox7.Text;
            string categoryname = uiComboBox4.Text;
            string description = uiRichTextBox3.Text;
            int price = int.Parse(uiTextBox3.Text);
            int costprice = int.Parse(uiTextBox4.Text);
            int stock = int.Parse(uiTextBox5.Text);
            string Model = uiTextBox8.Text;
            int lifespan = int.Parse(uiTextBox2.Text);
            var lines = uiRichTextBox4.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var dict = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    dict[parts[0].Trim()] = parts[1].Trim();
                }
            }
            string sqlsearch = "SELECT CategoryID FROM equipmentcategories WHERE CategoryName = @categoryname";
            int categoryid = (int)DBhelper.ExecuteScalar(sqlsearch, new MySqlParameter[] { new MySqlParameter("@categoryname", categoryname) });
            string specificationJson = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
            string sqlupdate = @"UPDATE equipment 
                               SET EquipmentName = @equipmentname, CategoryID = @categoryid, LifeSpan = @lifespan, Model = @model 
                               WHERE EquipmentID = @equipmentid";
            DBhelper.ExecuteNonQuery(sqlupdate, new MySqlParameter[] {
                new MySqlParameter("@equipmentname", equipmentname),
                new MySqlParameter("@categoryid", categoryid),
                new MySqlParameter("@lifespan", lifespan),
                new MySqlParameter("@model", Model),
                new MySqlParameter("@equipmentid", int.Parse(uiTextBox6.Text))
            });
            string sql2 = @"UPDATE equipmentproducts 
                           SET ProductName = @productname, CategoryID = @categoryid, Description = @description, Price = @price, CostPrice = @costprice, Stock = @stock, Specification = @specification 
                           WHERE ProductName = @productname";
            DBhelper.ExecuteNonQuery(sql2, new MySqlParameter[] {
                new MySqlParameter("@productname", productname),
                new MySqlParameter("@categoryid", categoryid),
                new MySqlParameter("@description", description),
                new MySqlParameter("@price", price),
                new MySqlParameter("@costprice", costprice),
                new MySqlParameter("@stock", stock),
                new MySqlParameter("@specification", specificationJson)
            });
            UIMessageBox.Show("修改成功！");
            DGVLoad();
        }

        private void uiComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            if(uiComboBox1.SelectedIndex==0)
            {
                string equipmentname = uiTextBox6.Text;
                string sql = "SELECT EquipmentID FROM equipment WHERE EquipmentName = @equipmentname";
                int equipmentid = (int)DBhelper.ExecuteScalar(sql, new MySqlParameter[] { new MySqlParameter("@equipmentname", equipmentname) });
                string sqldelete2 = "DELETE FROM equipmentproducts WHERE EquipmentID = @equipmentid";
                DBhelper.ExecuteNonQuery(sqldelete2, new MySqlParameter[] { new MySqlParameter("@equipmentid", equipmentid) });
                string sqldelete = "DELETE FROM equipment WHERE EquipmentID = @equipmentid";
                DBhelper.ExecuteNonQuery(sqldelete, new MySqlParameter[] { new MySqlParameter("@equipmentid", equipmentid) });
                UIMessageBox.Show("删除成功！");
            }
            else if(uiComboBox1.SelectedIndex==1)
            {
                int equipmentid = int.Parse(uiTextBox6.Text);
                string sqldelete2 = "DELETE FROM equipmentproducts WHERE EquipmentID = @equipmentid";
                DBhelper.ExecuteNonQuery(sqldelete2, new MySqlParameter[] { new MySqlParameter("@equipmentid", equipmentid) });
                string sqldelete = "DELETE FROM equipment WHERE EquipmentID = @equipmentid";
                DBhelper.ExecuteNonQuery(sqldelete, new MySqlParameter[] { new MySqlParameter("@equipmentid", equipmentid) });
                UIMessageBox.Show("删除成功！");
            }
            DGVLoad();
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            if (uiComboBox2.SelectedIndex == 0)
            {
                string equipmentname = uiTextBox12.Text;
                string sql = "SELECT EquipmentID FROM equipment WHERE EquipmentName = @equipmentname";
                DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter[] { new MySqlParameter("@equipmentname", equipmentname) });
                uiDataGridView2.DataSource = dt;
            }
            else if (uiComboBox2.SelectedIndex == 1)
            {
                int equipmentid = int.Parse(uiTextBox12.Text);
                string sql = "SELECT * FROM equipment WHERE EquipmentID = @equipmentid";
                DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter[] { new MySqlParameter("@equipmentid", equipmentid) });
                uiDataGridView2.DataSource = dt;
            }
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            DGVLoad();
        }

        private void uiComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void uiTextBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiButton8_Click(object sender, EventArgs e)
        {
            if(uiComboBox5.SelectedIndex==0)
            {
                string productname = uiTextBox13.Text;
                string sqlfind = "SELECT ProductID FROM equipmentproducts WHERE ProductName = @productname";
                int productid = (int)DBhelper.ExecuteScalar(sqlfind, new MySqlParameter[] { new MySqlParameter("@productname", productname) });
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "图片文件(*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName;
                    string sqladd = "INSERT INTO productimages (ProductID, ImagePath) VALUES (@productid, @imagepath)";
                    DBhelper.ExecuteNonQuery(sqladd, new MySqlParameter[] {
                        new MySqlParameter("@productid", productid),
                        new MySqlParameter("@imagepath", path)
                    });
                    UIMessageBox.Show("添加成功！");
                }
            }
            else if(uiComboBox5.SelectedIndex==1)
            {
                int productid = int.Parse(uiTextBox13.Text);
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "图片文件(*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName;
                    string sqladd = "INSERT INTO productimages (ProductID, ImagePath) VALUES (@productid, @imagepath)";
                    DBhelper.ExecuteNonQuery(sqladd, new MySqlParameter[] {
                        new MySqlParameter("@imagepath", path),
                        new MySqlParameter("@productid", productid)
                    });
                    UIMessageBox.Show("添加成功！");
                }
            }
        }
    }
}
