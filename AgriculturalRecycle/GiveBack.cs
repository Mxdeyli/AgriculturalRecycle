using Sunny.UI;
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
    public partial class GiveBack: UIForm
    {
        private readonly User _currentUser;
        public GiveBack(User user)
        {
            InitializeComponent();
            _currentUser = user;

        }

        private void DGVLoad()
        {
            string sqlcheck = "SELECT * FROM borrowrecords WHERE UserID = @userID";
            DataTable dt = DBhelper.ExecuteQuery(sqlcheck, new MySqlParameter("@userID", _currentUser.UserID));
            if (dt.Rows.Count > 0)
            {
                uiDataGridView1.DataSource = dt;
            }
        }

        private void GiveBack_Load(object sender, EventArgs e)
        {
           DGVLoad();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            int equipmentID = 0;
            int possessorID = 0;
            string sql = "SELECT EquipmentID,PossessorID FROM borrowrecords WHERE UserID = @userID AND RecordID = @recordID";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@recordID", uiDataGridView1.CurrentRow.Cells[0].Value.ToString()));
            if (dt.Rows.Count > 0)
            {
                equipmentID = int.Parse(dt.Rows[0]["EquipmentID"].ToString());
                possessorID = int.Parse(dt.Rows[0]["PossessorID"].ToString());
            }
            string sql2 = "SELECT COUNT(*) FROM userequipment WHERE UserID = @userID AND EquipmentID = @equipmentID";
            int count = int.Parse(DBhelper.ExecuteScalar(sql2, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@equipmentID", equipmentID)).ToString());
            if (count == 0)
            {
                string sqladd = "INSERT INTO userequipment (UserID,EquipmentID,Count) VALUES (@userID,@equipmentID,1)";
                DBhelper.ExecuteNonQuery(sqladd, new MySqlParameter("@userID", possessorID), new MySqlParameter("@equipmentID", equipmentID));
                string sqldel = "DELETE FROM borrowrecords WHERE UserID = @userID AND RecordID = @recordID";
                DBhelper.ExecuteNonQuery(sqldel, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@recordID", uiDataGridView1.CurrentRow.Cells[0].Value.ToString()));
                MessageBox.Show("归还成功！");
                uiDataGridView1.DataSource = null;
                DGVLoad();
            }
            else
            {
                string sqlupdate = "UPDATE userequipment SET Count = Count + 1 WHERE UserID = @userID AND EquipmentID = @equipmentID";
                DBhelper.ExecuteNonQuery(sqlupdate, new MySqlParameter("@userID", possessorID), new MySqlParameter("@equipmentID", equipmentID));
                string sqldel = "DELETE FROM borrowrecords WHERE UserID = @userID AND RecordID = @recordID";
                DBhelper.ExecuteNonQuery(sqldel, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@recordID", uiDataGridView1.CurrentRow.Cells[0].Value.ToString()));
                MessageBox.Show("归还成功！");
                uiDataGridView1.DataSource = null;
                DGVLoad();
            }

        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = "SELECT e.EquipmentName FROM equipment e,borrowrecords br WHERE e.EquipmentID = br.EquipmentID AND br.UserID = @userID AND br.RecordID= @recordID";
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userID", _currentUser.UserID), new MySqlParameter("@recordID", uiTextBox1.Text));
            if (dt.Rows.Count > 0)
            {
                uiLabel2.Text ="归还设备名称："+ dt.Rows[0]["EquipmentName"].ToString();
            }
            else
            {
                uiLabel1.Text = "无此设备";
            }
        }
    }
}
