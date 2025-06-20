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
    public partial class RentalSetting: UIForm
    {
        private readonly User _currentUser;
        private string[] arr = { };
        private string[] arr2 = {"长租", "短租"};
        public RentalSetting(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void RentalSetting_Load(object sender, EventArgs e)
        {
            string sqlcheck = "SELECT e.EquipmentName FROM userequipment u,equipment e WHERE u.EquipmentID=e.EquipmentID AND u.UserID=@userid";
            DataTable dt = DBhelper.ExecuteQuery(sqlcheck, new MySqlParameter("@userid", _currentUser.UserID));
            arr = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arr[i] = dt.Rows[i]["EquipmentName"].ToString();
            }
            for (int i = 0; i < arr2.Length; i++)
            {
                uiComboBox2.Items.Add(arr2[i]);
            }
            uiComboBox1.Items.AddRange(arr);
            uiComboBox2.SelectedIndex = 0;
            uiComboBox1.SelectedIndex = 0;

        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT 
                           u.EquipmentID,
                           u.UserID,
                           e.ProductID,
                           e.CategoryID,
                           u.Count from userequipment u,equipmentproducts e 
                           WHERE u.EquipmentID=e.EquipmentID AND u.UserID=@userid AND e.ProductName=@equipmentname";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userid", _currentUser.UserID), new MySqlParameter("@equipmentname",uiComboBox1.Text));
            if (dt.Rows.Count > 0)
            {
                int equipmentid = Convert.ToInt32(dt.Rows[0]["EquipmentID"]);
                int userid = Convert.ToInt32(dt.Rows[0]["UserID"]);
                int productid = Convert.ToInt32(dt.Rows[0]["ProductID"]);
                int rental = Convert.ToInt32(uiTextBox1.Text);
                int categoryid = Convert.ToInt32(dt.Rows[0]["CategoryID"]);
                int count =Convert.ToInt32(dt.Rows[0]["Count"]);
                string rentaltype = uiComboBox2.Text;
                string sql2 = @"INSERT INTO borrowequipment 
                                (EquipmentID,UserID,ProductID,CategoryID,Rental,AddDate,RentalDuration) 
                                VALUES(@equipmentid,@userid,@productid,@categoryid,@rental,@rentaldays,@rentaltype)";
                DBhelper.ExecuteNonQuery(sql2, new MySqlParameter("@equipmentid", equipmentid), new MySqlParameter("@userid", userid), new MySqlParameter("@productid", productid), new MySqlParameter("@categoryid", categoryid), new MySqlParameter("@rental", rental), new MySqlParameter("@rentaldays",DateTime.Now.ToString("yyyyMMddHHmmss")), new MySqlParameter("@rentaltype",rentaltype));
                if(count==1)
                {
                    string sql4 = "DELETE FROM userequipment WHERE EquipmentID=@equipmentid AND UserID=@userid";
                    DBhelper.ExecuteNonQuery(sql4, new MySqlParameter("@equipmentid", equipmentid), new MySqlParameter("@userid", userid));
                }
                else
                {
                    string sql3 = "UPDATE userequipment SET Count=Count-1 WHERE EquipmentID=@equipmentid AND UserID=@userid";
                    DBhelper.ExecuteNonQuery(sql3, new MySqlParameter("@equipmentid", equipmentid), new MySqlParameter("@userid", userid));
                }
                MessageBox.Show("租赁设置成功！");
            }
            else
            {
                MessageBox.Show("请选择设备！");
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
