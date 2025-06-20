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
    public partial class Rent: UIForm
    {
        private readonly User _currentUser;
        private Item _currentItem;
        public Rent(User currentUser, Item currentItem)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _currentItem = currentItem;
        }
        private void Rent_Load(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            int rentDate = Convert.ToInt32(uiTextBox1.Text);
            if (uiTextBox1.Text == "")
            {
                UIMessageBox.ShowError("Please enter the rent date.");
                return;
            }
            string sqlrent = @"INSERT INTO borrowrecords 
                              (EquipmentID,UserID,BorrowDate,ExpectedReturn,Deposit,Rental,PossessorID) 
                              VALUES(@EquipmentID,@UserID,@BorrowDate,@ExpectedReturn,@Deposit,@Rental,@PossessorID)";
            string sqlcheck = "SELECT Balance FROM userinfo WHERE UserID=@UserID";
            double balance = 0;
            DataTable dt=DBhelper.ExecuteQuery(sqlcheck, new MySqlParameter("@UserID", _currentUser.UserID));
            if (dt.Rows.Count > 0)
            {
                balance = Convert.ToDouble(dt.Rows[0]["Balance"]);
            }
            string sqlcheck2 = "SELECT Rental,UserID,Deposit,RentalDuration FROM borrowequipment WHERE EquipmentID=@EquipmentID";
            DataTable dt2 = DBhelper.ExecuteQuery(sqlcheck2, new MySqlParameter("@EquipmentID", _currentItem.EquipmentID));
            double rental = 0;
            int UserID = 0;
            decimal Deposit = 0;
            string duration = "";
            if (dt2.Rows.Count > 0)
            {
                 rental = Convert.ToDouble(dt2.Rows[0]["Rental"]);
                 UserID = Convert.ToInt32(dt2.Rows[0]["UserID"]);
                 Deposit = Convert.ToDecimal(dt2.Rows[0]["Deposit"]);
                 duration = dt2.Rows[0]["RentalDuration"].ToString();
            }
            if (rental > balance)
            {
                UIMessageBox.ShowError("余额不足，无法借入");
                return;
            }
            else
            {
               if(duration=="长租"&&rentDate<6)
                {
                    UIMessageBox.ShowError("长租借用时间不能小于6个月");
                    return;
                }
                else if(duration=="短租"&&rentDate>=6)
                {
                    UIMessageBox.ShowError("短租借用时间不能大于等于6个月");
                    return;
                }
                else
                {
                    DateTime borrowDate = DateTime.Now;
                    DateTime expectedReturn = DateTime.Now.AddMonths(rentDate);
                    double rent = rental * rentDate;
                    DBhelper.ExecuteNonQuery(sqlrent, new MySqlParameter("@EquipmentID", _currentItem.EquipmentID),
                                                      new MySqlParameter("@UserID", _currentUser.UserID),
                                                      new MySqlParameter("@BorrowDate", borrowDate),
                                                      new MySqlParameter("@ExpectedReturn", expectedReturn),
                                                      new MySqlParameter("@Deposit", Deposit),
                                                      new MySqlParameter("@Rental", rent),
                                                      new MySqlParameter("@PossessorID", UserID));
                    double newBalance = balance - rent-Convert.ToDouble(Deposit);
                    string updateSql = "UPDATE userinfo SET Balance = @balance WHERE UserID = @uid";
                    DBhelper.ExecuteNonQuery(updateSql,
                        new MySqlParameter("@balance", newBalance),
                        new MySqlParameter("@uid", _currentUser.UserID));
                    string sqldelete = "DELETE FROM borrowequipment WHERE EquipmentID=@EquipmentID AND UserID=@UserID AND Rental=@Rental";
                    DBhelper.ExecuteNonQuery(sqldelete, new MySqlParameter("@EquipmentID", _currentItem.EquipmentID),
                                                      new MySqlParameter("@UserID", UserID),
                                                      new MySqlParameter("@Rental", rental));
                    string sqlupdate = "UPDATE userinfo SET Balance =Balance+@Rental+@Deposit WHERE UserID = @uid";
                    DBhelper.ExecuteNonQuery(sqlupdate, new MySqlParameter("@Rental", rent),
                                                      new MySqlParameter("@uid", UserID),
                                                      new MySqlParameter("@Deposit", Deposit));
                    UIMessageBox.ShowSuccess("借用成功");
                    this.Close();
                }
            }

        }
    }
}
