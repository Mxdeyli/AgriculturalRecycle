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
    public partial class MainManager: UIForm
    {
        private readonly User _currentUser;
        private string[] arr = {"用户名","UserID" };
        public MainManager(User user)
        {
            _currentUser = user;
            InitializeComponent();
            LoadUser();
        }

        private void LoadUser()
        {
            string sql="SELECT * FROM Users ";
            uiDataGridView1.DataSource=DBhelper.ExecuteQuery(sql);
            string sql1 = "SELECT * FROM userinfo";
            uiDataGridView2.DataSource = DBhelper.ExecuteQuery(sql1);
            uiComboBox1.SelectedIndex = 0;
            uiComboBox2.SelectedIndex = 0;
            uiComboBox3.SelectedIndex = 0;
            uiTextBox1.Text = "";
            uiTextBox2.Text = "";
            uiTextBox3.Text = "";
            uiTextBox4.Text = "";
            uiTextBox5.Text = "";
            uiTextBox6.Text = "";
            uiTextBox7.Text = "";
        }

        private void MainManager_Load(object sender, EventArgs e)
        {
            LoadUser();
            for (int i = 0; i < arr.Length; i++)
            {
                uiComboBox1.Items.Add(arr[i]);
                uiComboBox2.Items.Add(arr[i]);
                uiComboBox3.Items.Add(arr[i]);
            }
            uiComboBox1.SelectedIndex = 0;
            uiComboBox2.SelectedIndex = 0;
            uiComboBox3.SelectedIndex = 0;
        }

        private void uiDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(_currentUser);
            mainForm.Show();
            this.Hide();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            string input = uiTextBox1.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("请输入要删除的用户名或UserID！");
                return;
            }

            int userId = 0;
            if (uiComboBox1.SelectedItem.ToString() == "用户名")
            {
                string sqlFindUserId = "SELECT UserID FROM Users WHERE Account = @Account";
                var dt = DBhelper.ExecuteQuery(sqlFindUserId, new MySqlParameter("@Account", input));
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("未找到该用户名对应的用户！");
                    return;
                }
                userId = Convert.ToInt32(dt.Rows[0]["UserID"]);
            }
            else 
            {
                if (!int.TryParse(input, out userId))
                {
                    MessageBox.Show("请输入有效的UserID！");
                    return;
                }
            }

            string[] tables = {
                "borrowrecords", "messagesinfo", "orderhistory", "recyclerecords",
                "shoppingcarts", "userequipment", "userinfo", "userroles", "users"
            };

            using (var conn = DBhelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var table in tables)
                        {
                            string sql = $"DELETE FROM {table} WHERE UserID = @UserID";
                            using (var cmd = new MySqlCommand(sql, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@UserID", userId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                        MessageBox.Show("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("删除失败：" + ex.Message);
                    }
                }
            }
            LoadUser();
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            string oldname = uiTextBox5.Text.Trim();
            string newname = uiTextBox6.Text.Trim();
            if (string.IsNullOrEmpty(oldname) || string.IsNullOrEmpty(newname))
            {
                MessageBox.Show("请输入有效的用户名！");
                return;
            }

            string sqlFindUserId = "SELECT UserID FROM Users WHERE Account = @Account";
            var dt = DBhelper.ExecuteQuery(sqlFindUserId, new MySqlParameter("@Account", oldname));
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("未找到该用户名对应的用户！");
                return;
            }
            int userId = Convert.ToInt32(dt.Rows[0]["UserID"]);

            string sqlFindAccoount = "SELECT COUNT(*) FROM Users WHERE Account = @NewName";
            var dt1 = DBhelper.ExecuteScalar(sqlFindAccoount, new MySqlParameter("@NewName", newname));
            int countExist = Convert.ToInt32(dt1);
            if (countExist > 0)
            {
                MessageBox.Show("该用户名已存在！");
                return;
            }

            string sqlUpdate = "UPDATE Users SET Account = @NewName WHERE UserID = @UserID";
            int count = DBhelper.ExecuteNonQuery(sqlUpdate, new MySqlParameter("@NewName", newname), new MySqlParameter("@UserID", userId));
            if (count == 0)
            {
                MessageBox.Show("更新失败！");
                return;
            }
            MessageBox.Show("更新成功！");
            LoadUser();
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            string input=uiTextBox7.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                LoadUser();
                MessageBox.Show("请输入要查找的用户名或UserID！");
                return;
            }
            int userId = 0;
            if (uiComboBox2.SelectedItem.ToString() == "用户名")
            {
                string sqlFindUserId = "SELECT UserID FROM Users WHERE Account = @Account";
                var dt = DBhelper.ExecuteQuery(sqlFindUserId, new MySqlParameter("@Account", input));
                if (dt.Rows.Count == 0)
                {
                    uiTextBox7.Text = "";
                    MessageBox.Show("未找到该用户名对应的用户！");
                    return;
                }
                userId = Convert.ToInt32(dt.Rows[0]["UserID"]);
            }
            else
            {
                if (!int.TryParse(input, out userId))
                {
                    uiTextBox7.Text = "";
                    MessageBox.Show("请输入有效的UserID！");
                    return;
                }
            }

            string sql = "SELECT * FROM Users WHERE UserID = @UserID";
            var dt1 = DBhelper.ExecuteQuery(sql, new MySqlParameter("@UserID", userId));
            if (dt1.Rows.Count == 0)
            {
                uiTextBox7.Text = "";
                MessageBox.Show("未找到该用户！");
                return;
            }
            else
            {
                uiDataGridView1.DataSource = dt1;
            }
            string sql1 = "SELECT * FROM userinfo WHERE UserID = @UserID";
            var dt2 = DBhelper.ExecuteQuery(sql1, new MySqlParameter("@UserID", userId));
            if (dt2.Rows.Count == 0)
            {
                uiTextBox7.Text = "";
                MessageBox.Show("未找到该用户！");
                return;
            }
            else
            {
                uiDataGridView2.DataSource = dt2;
            }
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            string input = uiTextBox2.Text.Trim();
            string newPassword = uiTextBox3.Text.Trim();
            string confirmPassword = uiTextBox4.Text.Trim();

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("请输入用户名/UserID和新密码及确认密码！");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("两次输入的密码不一致！");
                return;
            }

            int userId = 0;
            if (uiComboBox3.SelectedItem.ToString() == "用户名")
            {
                string sqlFindUserId = "SELECT UserID FROM Users WHERE Account = @Account";
                var dt = DBhelper.ExecuteQuery(sqlFindUserId, new MySqlParameter("@Account", input));
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("未找到该用户名对应的用户！");
                    return;
                }
                userId = Convert.ToInt32(dt.Rows[0]["UserID"]);
            }
            else
            {
                if (!int.TryParse(input, out userId))
                {
                    MessageBox.Show("请输入有效的UserID！");
                    return;
                }
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            string sqlUpdate = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
            int count = DBhelper.ExecuteNonQuery(sqlUpdate,
                new MySqlParameter("@Password", hashedPassword),
                new MySqlParameter("@UserID", userId));
            if (count == 0)
            {
                MessageBox.Show("密码更新失败！");
                return;
            }
            MessageBox.Show("密码更新成功！");
            LoadUser();
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
