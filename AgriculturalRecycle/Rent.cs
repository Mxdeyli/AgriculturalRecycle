using System;
using MySql.Data.MySqlClient;
using Sunny.UI;

namespace AgriculturalRecycle
{
    public partial class Rent : UIForm
    {
        private readonly User currentUser;
        private readonly Item currentItem;
        private readonly int ownerId;
        private readonly object listingDate;
        private readonly decimal monthlyRent;
        private readonly decimal deposit;
        private readonly string duration;

        public Rent(User user, Item item, int ownerId, object listingDate, decimal monthlyRent, decimal deposit, string duration)
        {
            InitializeComponent();
            currentUser = user;
            currentItem = item;
            this.ownerId = ownerId;
            this.listingDate = listingDate;
            this.monthlyRent = monthlyRent;
            this.deposit = deposit;
            this.duration = duration;
        }

        private void Rent_Load(object sender, EventArgs e)
        {
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Now;
            int maxMonths = (DateTime.MaxValue.Year - today.Year) * 12 + DateTime.MaxValue.Month - today.Month;
            if (!int.TryParse(uiTextBox1.Text, out int months) || months < 1 || months > maxMonths)
            {
                UIMessageBox.ShowError("请输入有效的租赁月数。");
                return;
            }
            if (currentUser.UserID == ownerId)
            {
                UIMessageBox.ShowError("不能租用自己发布的设备。");
                return;
            }
            if (duration == "长租" && months < 6)
            {
                UIMessageBox.ShowError("长租借用时间不能小于 6 个月。");
                return;
            }
            if (duration == "短租" && months >= 6)
            {
                UIMessageBox.ShowError("短租借用时间必须小于 6 个月。");
                return;
            }

            try
            {
                string error = RentalService.Borrow(currentUser.UserID, ownerId, currentItem.EquipmentID,
                    currentItem.ProductID, listingDate, duration, monthlyRent, deposit, months, today);
                if (error != null)
                {
                    UIMessageBox.ShowError(error);
                    return;
                }

                UIMessageBox.ShowSuccess("借用成功");
                Close();
            }
            catch (MySqlException)
            {
                UIMessageBox.ShowError("借用失败，请稍后重试。");
            }
        }
    }
}
