using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using BCrypt.Net;

namespace AgriculturalRecycle
{
    public partial class UpdateAccount: Form
    {
        private readonly User _currentUser;
        private string[] arr = { "@gmail.com", "@163.com", "@qq.com", "@yahoo.com", "@hotmail.com", "@outlook.com" };
        private bool isShow = false;
        private bool isShow2= false;
        private bool isShow3 = false;
        public UpdateAccount(User user)
        {
            InitializeComponent();
            _currentUser = user;
            this.KeyDown += new KeyEventHandler(Register_KeyDown);
        }

        private void Register_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uiButton1_Click(sender, e);
            }
        }

        private void UpdateAccount_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                uiComboBox1.Items.Add(arr[i]);
            }
            uiComboBox1.SelectedIndex = 0;
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string account=_currentUser.Username;
            string oldPassword = uiTextBox1.Text;
            string newPassword = uiTextBox2.Text;
            string confirmPassword = uiTextBox3.Text;
            string Email = uiTextBox4.Text.Trim()+arr[uiComboBox1.SelectedIndex];
            if(!String.IsNullOrEmpty(uiTextBox1.Text))
            {
                if (String.IsNullOrEmpty(uiTextBox2.Text))
                {
                    label1.Text = "请输入新密码";
                    uiTextBox2.RectColor = Color.Red;
                    label1.Visible = true;
                    uiTextBox2.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(uiTextBox3.Text))
                {
                    label1.Text = "请确认新密码";
                    uiTextBox3.RectColor = Color.Red;
                    label1.Visible = true;
                    uiTextBox3.Focus();
                    return;
                }
            }
            if (!String.IsNullOrEmpty(uiTextBox4.Text))
            {
                string checkEmailSql = "select count(*) from users where Email=@Email";
                object emailResult = DBhelper.ExecuteScalar(checkEmailSql, new MySqlParameter[] { new MySqlParameter("@Email", Email) });
                if (Convert.ToInt32(emailResult) > 0)
                {
                    MessageBox.Show("该邮箱已被注册，请更换邮箱！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    uiTextBox4.RectColor = Color.Red;
                    uiTextBox4.Focus();
                    return;
                }
                else
                {
                    string sqlE = "UPDATE Users SET Email=@Email WHERE Username =@account";
                    DBhelper.ExecuteNonQuery(sqlE, new MySqlParameter("@Email", Email), new MySqlParameter("@account", account));
                    MessageBox.Show("邮箱修改成功！");
                }
            }
            if (AuthService.ValidateUser(account, oldPassword) != null)
            {
                if(newPassword.Trim() == confirmPassword.Trim())
                {
                    string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    string sql = "UPDATE Users SET Password =@newpassword WHERE Username =@account";
                    DBhelper.ExecuteNonQuery(sql, new MySqlParameter("@newpassword", newPasswordHash), new MySqlParameter("@account", account));
                    MessageBox.Show("密码修改成功！");
                }
                else
                {
                    label2.Text = "两次输入的密码不一致！";
                    label3.Text = "两次输入的密码不一致！";
                    uiTextBox2.RectColor = Color.Red;
                    uiTextBox3.RectColor = Color.Red;
                    label2.Visible = true;
                    label3.Visible = true;
                    uiTextBox2.Text = "";
                    uiTextBox3.Text = "";
                    uiTextBox2.Focus();
                }
            }
            else
            {
                label1.Text = "旧密码错误！";
                uiTextBox1.RectColor = Color.Red;
                label1.Visible = true;
                uiTextBox1.Text = "";
                uiTextBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserManage um = new UserManage(_currentUser);
            this.Hide();
            um.Show();
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            uiTextBox1.RectColor = Color.Blue;
            label1.Visible = false;
        }

        private void uiTextBox2_TextChanged(object sender, EventArgs e)
        {
            uiTextBox2.RectColor = Color.Blue;
            label2.Visible = false;
        }

        private void uiTextBox3_TextChanged(object sender, EventArgs e)
        {
            uiTextBox3.RectColor = Color.Blue;
            label3.Visible = false;
        }

        private void uiTextBox4_TextChanged(object sender, EventArgs e)
        {
            uiTextBox4.RectColor = Color.Blue;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (isShow)
            {
                uiButton2.Text = "显示";
                isShow = false;
                uiTextBox1.PasswordChar = '*';
            }
            else
            {
                uiButton2.Text = "隐藏";
                isShow = true;
                uiTextBox1.PasswordChar = '\0';
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (isShow2)
            {
                uiButton3.Text = "显示";
                isShow2 = false;
                uiTextBox2.PasswordChar = '*';
            }
            else
            {
                uiButton3.Text = "隐藏";
                isShow2 = true;
                uiTextBox2.PasswordChar = '\0';
            }
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            if (isShow3)
            {
                uiButton4.Text = "显示";
                isShow3 = false;
                uiTextBox3.PasswordChar = '*';
            }
            else
            {
                uiButton4.Text = "隐藏";
                isShow3 = true;
                uiTextBox3.PasswordChar = '\0';
            }
        }

        private void uiLabel4_Click(object sender, EventArgs e)
        {

        }
    }
}
