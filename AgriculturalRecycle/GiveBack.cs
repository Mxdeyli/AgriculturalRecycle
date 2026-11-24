using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Sunny.UI;

namespace AgriculturalRecycle
{
    public partial class GiveBack : UIForm
    {
        private readonly User currentUser;

        public GiveBack(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void LoadRecords()
        {
            const string sql = "SELECT * FROM borrowrecords WHERE UserID = @userId";
            uiDataGridView1.DataSource = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userId", currentUser.UserID));
        }

        private void GiveBack_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (!TryGetRecordId(out int recordId))
            {
                MessageBox.Show("请选择或输入有效的借用记录编号。");
                return;
            }

            try
            {
                bool returned = RentalService.Return(currentUser.UserID, recordId);
                uiTextBox1.Clear();
                LoadRecords();
                MessageBox.Show(returned ? "归还成功！" : "这条借用记录无法归还，请刷新后重试。");
            }
            catch (MySqlException)
            {
                MessageBox.Show("归还失败，请稍后重试。");
            }
        }

        private bool TryGetRecordId(out int recordId)
        {
            if (!string.IsNullOrWhiteSpace(uiTextBox1.Text))
                return int.TryParse(uiTextBox1.Text, out recordId) && recordId > 0;

            var row = uiDataGridView1.CurrentRow?.DataBoundItem as DataRowView;
            return int.TryParse(row?["RecordID"]?.ToString(), out recordId) && recordId > 0;
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            uiLabel2.Text = "归还设备名称：";
            if (!int.TryParse(uiTextBox1.Text, out int recordId) || recordId <= 0)
                return;

            const string sql = @"SELECT e.EquipmentName FROM equipment e
                INNER JOIN borrowrecords br ON e.EquipmentID = br.EquipmentID
                WHERE br.UserID = @userId AND br.RecordID = @recordId";
            var records = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userId", currentUser.UserID),
                new MySqlParameter("@recordId", recordId));
            uiLabel2.Text += records.Rows.Count == 0 ? "未找到借用记录" : records.Rows[0]["EquipmentName"].ToString();
        }
    }
}
