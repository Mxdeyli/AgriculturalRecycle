using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using AgriculturalRecycle.Layout;
using AntdUI;

namespace AgriculturalRecycle
{
    public partial class Login: Form
    {
        private bool isShow = true;
        public AutoSizeFormClass autoSize = new AutoSizeFormClass();
        public Login()
        {
            InitializeComponent();
            this.KeyDown+=new KeyEventHandler(Login_KeyDown);
            autoSize.controllInitializeSize(this);
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(sender, e);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string account = textBox1.Text;
            string password = textBox2.Text;
            if (String.IsNullOrEmpty(account))
            {
                label4.Text = "请输入账号!"; 
                label4.Visible = true;
                textBox1.Focus();
                return;
            }
            if (String.IsNullOrEmpty(password))
            {
                label5.Text = "请输入密码!"; 
                label5.Visible = true;
                textBox2.Focus();
                return;
            }
            if (AuthService.ValidateUser(account, password)!= null)
            {
                User currentUser = AuthService.ValidateUser(account, password);
                MainForm mainForm = new MainForm(currentUser);
                this.Hide();
                mainForm.Show();

            }
            else
            {
                MessageBox.Show("账号或密码错误!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (label4.Visible) label4.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (label5.Visible) label5.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void uiButton1_Click(object sender, EventArgs e)
        { 
            if (isShow)
            {
                uiButton1.Text = "隐藏";
                isShow = false;
                textBox2.UseSystemPasswordChar =false ;
                textBox2.PasswordChar = '\0';
            }
            else
            {
                uiButton1.Text = "显示";
                isShow = true;
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            this.Hide();
            register.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            autoSize.controllInitializeSize(this);
            label4.Visible = false;
            label5.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Resize(object sender, EventArgs e)
        {

        }

        private void Login_SizeChanged(object sender, EventArgs e)
        {
            autoSize.controllInitializeSize(this);
        }
    }
}
