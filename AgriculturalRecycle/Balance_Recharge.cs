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
    public partial class Balance_Recharge: UIForm
    {
        private readonly User _currentUser;
        public Balance_Recharge(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void Balance_Recharge_Load(object sender, EventArgs e)
        {
            string UserID = _currentUser.UserID.ToString();
            uiTextBox1.Text = _currentUser.UserID.ToString();
            string sql = "SELECT Balance FROM UserInfo WHERE UserID = @UserID";
            DataTable dt = DBhelper.ExecuteQuery(sql, new  MySqlParameter("@UserID", UserID));
            uiLabel4.Text = "当前余额: " + dt.Rows[0][0].ToString();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string money = uiTextBox2.Text;
            string UserID = uiTextBox1.Text;
            if (uiTextBox1.Text == "")
            {
                UIMessageBox.ShowError("请输入用户ID！");
                return;
            }
            if (uiTextBox2.Text == "")
            {
                UIMessageBox.ShowError("请输入充值金额！");
                return;
            }
            string sqlidcheck = "SELECT UserID FROM UserInfo WHERE UserID = @UserID";
            DataTable dtidcheck = DBhelper.ExecuteQuery(sqlidcheck, new MySqlParameter("@UserID", uiTextBox1.Text));
            if (dtidcheck.Rows.Count == 0)
            {
                UIMessageBox.ShowError("用户ID不存在！");
                return;
            }
            string sql = "UPDATE UserInfo SET Balance = Balance + @money WHERE UserID = @UserID";
            DBhelper.ExecuteNonQuery(sql, new MySqlParameter("@money", money), new MySqlParameter("@UserID", UserID));
            UIMessageBox.ShowSuccess("充值成功！");
            Balance_Recharge_Load(sender, e);
            uiTextBox2.Text = "";   
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
