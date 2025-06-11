using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AgriculturalRecycle
{
    public partial class Register: Form
    {
        private const int ValidCodeLength = 4;//验证码长度        
        private String strValidCode = "";//验证码   
        private string[] arr = { "@gmail.com", "@163.com", "@qq.com", "@yahoo.com", "@hotmail.com", "@outlook.com" };

        private void UpdateValidCode()
        {
            strValidCode = ReCaptcha.CreateRandomCode(ValidCodeLength);//生成随机验证码
            if (strValidCode == "") return;
            CreateImage.Createimage(strValidCode, pictureBox1);//创建验证码图片
        }

        private void Register_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uiButton1_Click(sender, e);
            }
        }

        public Register()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Register_KeyDown);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UpdateValidCode();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            UpdateValidCode();
            for(int i=0;i<arr.Length;i++)
            {
                uiComboBox1.Items.Add(arr[i]);
            }
            uiComboBox1.SelectedIndex = 0;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(uiTextBox1.Text)==true)
            {
                label7.Text = "用户名不能为空!";
                label7.Visible = true;
                uiTextBox1.RectColor=Color.Red;
                uiTextBox1.Focus();
                return;
            }
            if (String.IsNullOrEmpty(uiTextBox2.Text) == true)
            {
                label8.Text = "密码不能为空!";
                label8.Visible = true;
                uiTextBox2.RectColor = Color.Red;
                uiTextBox2.Focus();
                return;
            }
            if (String.IsNullOrEmpty(uiTextBox3.Text) == true)
            {
                label9.Text = "确认密码不能为空!";
                label9.Visible = true;
                uiTextBox3.RectColor = Color.Red;
                uiTextBox3.Focus();
                return;
            }
            if (String.IsNullOrEmpty(uiTextBox4.Text) == true)
            {
                label10.Text = "邮箱不能为空!";
                label10.Visible = true;
                uiTextBox4.RectColor = Color.Red;
                uiTextBox4.Focus();
                return;
            }
            if (String.IsNullOrEmpty(uiTextBox5.Text) == true)
            {
                label11.Text = "验证码不能为空!";
                label11.Visible = true;
                uiTextBox5.RectColor = Color.Red;
                uiTextBox5.Focus();
                return;
            }
            string validcode = uiTextBox5.Text.Trim();
            if (validcode.Length != ValidCodeLength)
            {
                MessageBox.Show("验证码错误", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                uiTextBox5.RectColor = Color.Red;
                uiTextBox5.Text = "";
                UpdateValidCode();
                uiTextBox5.Focus();
                return;
            }
            char[] ch1 = validcode.ToCharArray();
            char[] ch2 = strValidCode.ToCharArray();
            int Count1 = 0;//字母个数
            int Count2 = 0;//数字个数
            for (int i = 0; i < strValidCode.Length; i++)
            {
                if ((ch1[i] >= 'a' && ch1[i] <= 'z') || (ch1[i] >= 'A' && ch1[i] <= 'Z'))//字母
                {
                    if (ch1[i] == ch2[i])
                    {
                        Count1++;
                    }
                }
                else//数字
                {
                    if (ch1[i] == ch2[i])
                    {
                        Count2++;
                    }
                }

            }
            int CountSum = Count1 + Count2;
            if (CountSum == strValidCode.Length)
            {
                if(uiTextBox2.Text.Trim() == uiTextBox3.Text.Trim())
                {
                    string password = BCrypt.Net.BCrypt.HashPassword(uiTextBox2.Text.Trim());
                    string Account = uiTextBox1.Text.Trim(),UserName = uiTextBox1.Text.Trim(), Email = uiTextBox4.Text.Trim()+arr[uiComboBox1.SelectedIndex];
                    string checkSql = "select count(*) from users where Account=@Account";
                    object result = DBhelper.ExecuteScalar(checkSql, new MySqlParameter[] { new MySqlParameter("@Account", Account) });
                    if (Convert.ToInt32(result) > 0)
                    {
                        MessageBox.Show("用户名已存在，请更换用户名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        uiTextBox1.RectColor = Color.Red;
                        uiTextBox1.Focus();
                        return;
                    }
                    string checkEmailSql = "select count(*) from users where Email=@Email";
                    object emailResult = DBhelper.ExecuteScalar(checkEmailSql, new MySqlParameter[] { new MySqlParameter("@Email", Email) });
                    if (Convert.ToInt32(emailResult) > 0)
                    {
                        MessageBox.Show("该邮箱已被注册，请更换邮箱！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        uiTextBox4.RectColor = Color.Red;
                        uiTextBox4.Focus();
                        return;
                    }
                    string sql = "insert into users(Account,Password,UserName,Email) values (@Account,@Password,@UserName,@Email)";
                    var dt = DBhelper.ExecuteNonQuery(sql,new MySqlParameter[] { new MySqlParameter("@Account", Account), new MySqlParameter("@Password", password), new MySqlParameter("@UserName", UserName), new MySqlParameter("@Email", Email) });
                    if (dt> 0)
                    {
                        string sql2 = "select UserID from users where Account=@Account";
                        var userID = DBhelper.ExecuteQuery(sql2, new MySqlParameter[] { new MySqlParameter("@Account", Account) });
                        int UserID = Convert.ToInt32(userID.Rows[0]["UserID"]);
                        string sql1 = "insert into userinfo(UserID,UserName,Contact_Way) values (@UserID,@UserName,@Email)";
                        var dt1 = DBhelper.ExecuteNonQuery(sql1, new MySqlParameter[] { new MySqlParameter("@UserID", UserID), new MySqlParameter("@UserName", UserName), new MySqlParameter("@Email", Email) });
                        MessageBox.Show("注册成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        uiButton1.Enabled = false;
                        Login login = new Login();
                        this.Hide();
                        login.Show();
                    }
                    else
                    {
                        MessageBox.Show("注册失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("两次密码输入不一致", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    uiTextBox2.RectColor = Color.Red;
                    uiTextBox3.RectColor = Color.Red;
                    uiTextBox2.Text = "";
                    uiTextBox3.Text = "";
                    UpdateValidCode();
                    uiTextBox5.Text = "";
                    uiTextBox2.Focus();
                }
            }
            else
            {
                MessageBox.Show("验证码错误", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                UpdateValidCode();//更新验证码
                uiTextBox5.Text = "";
                uiTextBox5.Focus();
            }

        }

        private void uiTextBox5_TextChanged(object sender, EventArgs e)
        {
            if (label11.Visible == true) label11.Visible = false; uiTextBox5.RectColor = Color.Blue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (label7.Visible == true) label7.Visible = false; uiTextBox1.RectColor=Color.Blue;
        }

        private void uiTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (label8.Visible == true) label8.Visible = false; uiTextBox2.RectColor = Color.Blue;
        }

        private void uiTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (label9.Visible == true) label9.Visible = false; uiTextBox3.RectColor = Color.Blue;
        }

        private void uiTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (label10.Visible == true) label10.Visible = false; uiTextBox4.RectColor = Color.Blue;
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
