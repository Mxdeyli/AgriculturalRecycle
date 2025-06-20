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
    public partial class SoldOut: UIForm
    {
        private readonly User _currentUser;
        private string[] arr = { };
        public SoldOut(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void SoldOut_Load(object sender, EventArgs e)
        {
            string sql = "SELECT e.EquipmentName FROM Equipment e,borrowequipment b WHERE e.EquipmentID = b.EquipmentID AND b.UserID =@userID";
            DataTable dt=DBhelper.ExecuteQuery(sql, new MySqlParameter("@userID", _currentUser.UserID));
            arr = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arr[i] = dt.Rows[i]["EquipmentName"].ToString();
            }
            uiComboBox1.Items.AddRange(arr);
            uiComboBox1.SelectedIndex = 0;
            string sql2 = "SELECT Rental FROM borrowequipment WHERE UserID =@userID AND EquipmentID = (SELECT EquipmentID FROM Equipment WHERE EquipmentName =@equipmentName)";
            DataTable dt2 = DBhelper.ExecuteQuery(sql2, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@equipmentName", uiComboBox1.Text));
            if (dt2.Rows.Count > 0)
            {
                uiLabel4.Text = dt2.Rows[0]["Rental"].ToString();
            }
            else
            {
                uiLabel4.Text = "0";
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string sqldelete = "DELETE FROM borrowequipment WHERE UserID =@userID AND EquipmentID = (SELECT EquipmentID FROM Equipment WHERE EquipmentName =@equipmentName)";
            DBhelper.ExecuteNonQuery(sqldelete, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@equipmentName", uiComboBox1.Text));
            string sqlsearch = "SELECT COUNT(*) FROM userequipment WHERE UserID =@userID AND EquipmentID = (SELECT EquipmentID FROM Equipment WHERE EquipmentName =@equipmentName)";
            DataTable dt = DBhelper.ExecuteQuery(sqlsearch, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@equipmentName", uiComboBox1.Text));
            if (dt.Rows[0][0].ToString() == "0")
            {
                string sqlinsert = "INSERT INTO userequipment(UserID,EquipmentID,Count) VALUES(@userID,(SELECT EquipmentID FROM Equipment WHERE EquipmentName =@equipmentName),1)";
                DBhelper.ExecuteNonQuery(sqlinsert, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@equipmentName", uiComboBox1.Text));
            }
            else
            {
                string sqlupdate = "UPDATE userequipment SET Count = Count + 1 WHERE UserID =@userID AND EquipmentID = (SELECT EquipmentID FROM Equipment WHERE EquipmentName =@equipmentName)";
                DBhelper.ExecuteNonQuery(sqlupdate, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@equipmentName", uiComboBox1.Text));
            }
            uiComboBox1.SelectedIndex = 0;
            uiLabel4.Text = "0";
            MessageBox.Show("商品下架成功！");
            
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql2 = "SELECT Rental FROM borrowequipment WHERE UserID =@userID AND EquipmentID = (SELECT EquipmentID FROM Equipment WHERE EquipmentName =@equipmentName)";
            DataTable dt2 = DBhelper.ExecuteQuery(sql2, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@equipmentName", uiComboBox1.Text));
            if (dt2.Rows.Count > 0)
            {
                uiLabel4.Text = dt2.Rows[0]["Rental"].ToString();
            }
            else
            {
                uiLabel1.Text = "0";
            }
        }
    }
}
