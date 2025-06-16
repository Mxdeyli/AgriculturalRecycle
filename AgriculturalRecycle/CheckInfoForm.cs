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
using System.Security.Cryptography.X509Certificates;

namespace AgriculturalRecycle
{
    public partial class CheckInfoForm: UIForm
    {
        private readonly User _currentUser;
        private List<int>Productids = new List<int>();
        private List<int>ProductCounts = new List<int>();

        public CheckInfoForm(User currentUser, List<int> productCounts, List<int> productids)
        {
            InitializeComponent();
            _currentUser = currentUser;
            Productids = new List<int>(productids);
            ProductCounts = new List<int>(productCounts);
        }

        private void CheckInfoForm_Load(object sender, EventArgs e)
        {
            string sql = "SELECT Phone,Consignee,ShippingAddress FROM UserInfo WHERE UserID = @UserID";
            DataTable dt=DBhelper.ExecuteQuery(sql, new MySqlParameter("@UserID", _currentUser.UserID));
            if (dt.Rows.Count > 0)
            {
                uiTextBox1.Text = dt.Rows[0]["Consignee"].ToString();
                uiTextBox2.Text = dt.Rows[0]["Phone"].ToString();
                uiRichTextBox1.Text = dt.Rows[0]["ShippingAddress"].ToString();
            }
            else
            {
                uiRichTextBox1.Text = "";
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            int j = 0;
            foreach (int i in Productids)
            {
                Random random = new Random();
                int num = random.Next(1, 10000);
                int count=ProductCounts[j++];
                string consignee = uiTextBox1.Text;
                string phone = uiTextBox2.Text;
                string shippingAddress = uiRichTextBox1.Text;
                string sqlsearch = "SELECT CategoryID FROM EquipmentProducts WHERE ProductID = @ProductID";
                DataTable dt = DBhelper.ExecuteQuery(sqlsearch, new MySqlParameter("@ProductID", i));
                int categoryid = Convert.ToInt32(dt.Rows[0]["CategoryID"]);
                double price = 0;
                string sqlprice = "SELECT Price FROM EquipmentProducts WHERE ProductID = @ProductID";
                DataTable dtprice = DBhelper.ExecuteQuery(sqlprice, new MySqlParameter("@ProductID", i));
                price = Convert.ToDouble(dtprice.Rows[0]["Price"]);
                string sql = "UPDATE UserInfo SET ShippingAddress = @ShippingAddress WHERE UserID = @UserID";
                DBhelper.ExecuteNonQuery(sql, new MySqlParameter("@UserID", _currentUser.UserID), new MySqlParameter("@ShippingAddress", shippingAddress));
                string sql2 = @"INSERT INTO OrderHistory(
                                OrderID,
                                UserID, 
                                ProductID,
                                CategoryID,
                                Consignee, 
                                Phone, 
                                ShippingAddress, 
                                Receivingtime,
                                Count,
                                TotalPrice) VALUES(
                                @OrderID,@UserID, @ProductID, @CategoryID, @Consignee, @Phone, @ShippingAddress, @Receivingtime, @Count, @TotalPrice)";
                DBhelper.ExecuteNonQuery(sql2, new MySqlParameter("@OrderID", DateTime.Now.ToString("yyyyMMddHHmmss") + _currentUser.UserID.ToString() + i.ToString() + num.ToString()),
                    new MySqlParameter("@ProductID", i),
                    new MySqlParameter("@UserID", _currentUser.UserID),
                    new MySqlParameter("@CategoryID", categoryid),
                    new MySqlParameter("@Consignee", consignee),
                    new MySqlParameter("@Phone", phone),
                    new MySqlParameter("@ShippingAddress", shippingAddress),
                    new MySqlParameter("@Receivingtime", DateTime.Now),
                    new MySqlParameter("@Count", count),
                    new MySqlParameter("@TotalPrice", count * price));
                string sql3 = "UPDATE EquipmentProducts SET Stock = Stock - @Count WHERE ProductID = @ProductID";
                DBhelper.ExecuteNonQuery(sql3, new MySqlParameter("@Count", count), new MySqlParameter("@ProductID", i));
            }
            UIMessageBox.ShowSuccess("下单成功！");
            this.Close();
        }

        private void uiTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
