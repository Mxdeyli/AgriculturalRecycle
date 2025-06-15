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
using System.Diagnostics.Eventing.Reader;

namespace AgriculturalRecycle
{
    public partial class Shopping_Cart: UIForm
    {
        private readonly User _currentUser;
        public Shopping_Cart(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void NullShoppingCart()
        {
            if(flowLayoutPanel2.Controls.Count == 0)
            {
                if (flowLayoutPanel2.Controls.Count > 0)
                {
                    flowLayoutPanel2.Controls.Clear();
                    var panel = new Sunny.UI.UIPanel
                    {
                        Size = new Size(1023, 180),
                        Padding = new Padding(0),
                        Margin = new Padding(0, 5, 0, 5),
                        Text = "购物车为空"
                    };
                    flowLayoutPanel2.Controls.Add(panel);
                }
                else
                {
                    var panel = new Sunny.UI.UIPanel
                    {
                        Size = new Size(1023, 180),
                        Padding = new Padding(0),
                        Margin = new Padding(0, 5, 0, 5),
                        Text = "购物车为空"
                    };
                    flowLayoutPanel2.Controls.Add(panel);
                }
            }
        }
        private void CreatShoppingCart()
        {
            string sql = @"
                SELECT 
                    e.ProductID,
                    e.ProductName,
                    e.Description,
                    e.Price,
                    e.Specification,
                    e.CategoryID,
                    e.EquipmentID,
                    c.CategoryName,
                    e.Stock,
                    p.ImageURL,
                    SUM(s.Quantity) AS Quantity
                FROM equipmentproducts e
                JOIN productimages p ON e.ProductID = p.ProductID
                JOIN equipmentcategories c ON e.CategoryID = c.CategoryID
                JOIN shoppingcarts s ON e.ProductID = s.ProductID
                WHERE s.UserID = @userid
                GROUP BY 
                    e.ProductID, e.ProductName, e.Description, e.Price, e.Specification, 
                    e.CategoryID, e.EquipmentID, c.CategoryName, e.Stock, p.ImageURL";
            flowLayoutPanel2.Controls.Clear();
            DataTable dt = DBhelper.ExecuteQuery(sql, new MySqlParameter("@userid", _currentUser.UserID));
            if (dt.Rows.Count == 0)
            {
                NullShoppingCart();
                return;
            }
            foreach (DataRow row in dt.Rows)
            {
                int productId = Convert.ToInt32(row["ProductID"]);
                int equipmentId = Convert.ToInt32(row["EquipmentID"]);
                int categoryId = Convert.ToInt32(row["CategoryID"]);
                string productName = row["ProductName"].ToString();
                int count = Convert.ToInt32(row["Quantity"]);

                // 创建uiPanel
                var panel = new Sunny.UI.UIPanel
                {
                    Size = new Size(1023, 180),
                    Padding = new Padding(0),
                    Margin = new Padding(0, 5, 0, 5)
                };

                // uiCheckBox
                var checkBox = new Sunny.UI.UICheckBox
                {
                    BackColor = Color.Transparent,
                    Size = new Size(22, 22),
                    Location = new Point(10, (panel.Height - 22) / 2)
                };
                panel.Controls.Add(checkBox);

                // uiImageButton
                var imageButton = new Sunny.UI.UIImageButton
                {
                    Size = new Size(147, 147),
                    Location = new Point(checkBox.Right + 10, (panel.Height - 147) / 2),
                    BackgroundImageLayout = ImageLayout.Stretch
                };
                try
                {
                    string imgPath = row["ImageURL"].ToString();
                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        imageButton.BackgroundImage = Image.FromFile(imgPath);
                    }
                }
                catch { }
                panel.Controls.Add(imageButton);

                // 商品名称Label
                var nameLabel = new Sunny.UI.UILabel
                {
                    Name = "nameLabel",
                    BackColor = Color.Transparent,
                    Text = productName,
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(imageButton.Right + 30, 40)
                };
                panel.Controls.Add(nameLabel);

                // 商品类别Label
                var categoryLabel = new Sunny.UI.UILabel
                {
                    Name = "categoryLabel",
                    BackColor = Color.Transparent,
                    Text = row["CategoryName"].ToString(),
                    Font = new Font("微软雅黑", 12F),
                    AutoSize = true,
                    Location = new Point(imageButton.Right + 30, nameLabel.Bottom + 10)
                };
                panel.Controls.Add(categoryLabel);

                // 商品单价Label
                var priceLabel = new Sunny.UI.UILabel
                {
                    Name = "priceLabel",
                    BackColor = Color.Transparent,
                    Text = row["Price"].ToString(),
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(nameLabel.Right + 120, 40)
                };
                panel.Controls.Add(priceLabel);

                // 商品数量Label
                var countLabel = new Sunny.UI.UILabel
                {
                    Name = "countLabel",
                    BackColor = Color.Transparent,
                    Text = count.ToString(),
                    Font = new Font("微软雅黑", 14F),
                    AutoSize = true,
                    Location = new Point(priceLabel.Right + 120, 40)
                };
                panel.Controls.Add(countLabel);

                // 增加数量按钮
                var addBtn = new Sunny.UI.UIButton
                {
                    Text = "+",
                    Size = new Size(35, 28),
                    Font = new Font("微软雅黑", 12F),
                    FillColor = Color.FromArgb(0, 192, 0),
                    Location = new Point(countLabel.Left, countLabel.Bottom + 10)
                };
                // 减少数量按钮
                var subBtn = new Sunny.UI.UIButton
                {
                    Text = "-",
                    Size = new Size(35, 28),
                    Font = new Font("微软雅黑", 12F),
                    FillColor = Color.FromArgb(192, 192, 0),
                    Location = new Point(countLabel.Left + addBtn.Width + 10, countLabel.Bottom + 10)
                };

                addBtn.Click += (s, e) =>
                {
                    int newCount = count + 1;
                    string updateSql = "UPDATE shoppingcarts SET Quantity = @qty WHERE UserID = @uid AND ProductID = @pid";
                    DBhelper.ExecuteNonQuery(updateSql,
                        new MySqlParameter("@qty", newCount),
                        new MySqlParameter("@uid", _currentUser.UserID),
                        new MySqlParameter("@pid", productId));
                    countLabel.Text = newCount.ToString();
                    count = newCount;
                    UpdateChoosenum();
                    UpdateTotalCount();
                };

                subBtn.Click += (s, e) =>
                {
                    if (count <= 1)
                    {
                        string delSql = "DELETE FROM shoppingcarts WHERE UserID = @uid AND ProductID = @pid";
                        DBhelper.ExecuteNonQuery(delSql,
                            new MySqlParameter("@uid", _currentUser.UserID),
                            new MySqlParameter("@pid", productId));
                        flowLayoutPanel2.Controls.Remove(panel);
                        NullShoppingCart();
                    }
                    else
                    {
                        int newCount = count - 1;
                        string updateSql = "UPDATE shoppingcarts SET Quantity = @qty WHERE UserID = @uid AND ProductID = @pid";
                        DBhelper.ExecuteNonQuery(updateSql,
                            new MySqlParameter("@qty", newCount),
                            new MySqlParameter("@uid", _currentUser.UserID),
                            new MySqlParameter("@pid", productId));
                        countLabel.Text = newCount.ToString();
                        count = newCount;
                    }
                    UpdateChoosenum();
                    UpdateTotalCount();
                };

                checkBox.CheckedChanged += (s, e) =>
                {
                    if (checkBox.Checked)
                    {
                        UpdateChoosenum();
                    }
                    else
                    {
                        UpdateChoosenum();
                    }
                };

                panel.Controls.Add(addBtn);
                panel.Controls.Add(subBtn);
                panel.Controls.Add(countLabel);

                // 删除按钮
                var delBtn = new Sunny.UI.UIButton
                {
                    Text = "删除商品",
                    Size = new Size(115, 35),
                    Font = new Font("华文琥珀", 16F),
                    FillColor = Color.FromArgb(192, 0, 0),
                    Location = new Point(panel.Width - 125, (panel.Height - 35) / 2)
                };
                delBtn.Click += (s, e) =>
                {
                    string delSql = "DELETE FROM shoppingcarts WHERE UserID = @uid AND ProductID = @pid";
                    DBhelper.ExecuteNonQuery(delSql,
                        new MySqlParameter("@uid", _currentUser.UserID),
                        new MySqlParameter("@pid", productId));
                    flowLayoutPanel2.Controls.Remove(panel);
                    NullShoppingCart();
                    UpdateChoosenum();
                    UpdateTotalCount();
                };
                panel.Controls.Add(delBtn);
                flowLayoutPanel2.Controls.Add(panel);
            }
            UpdateTotalCount();
        }

        private void UpdateChoosenum()
        {
            int totalSelectedCount = 0;
            double totalSelectedPrice = 0;
            foreach (Control control in flowLayoutPanel2.Controls)
            {
                if (control is Sunny.UI.UIPanel panel)
                {
                    Sunny.UI.UICheckBox checkBox = null;
                    Sunny.UI.UILabel countLabel = null;
                    Sunny.UI.UILabel priceLabel = null;
                    foreach (Control subControl in panel.Controls)
                    {
                        if (subControl is Sunny.UI.UICheckBox cb)
                            checkBox = cb;
                        if (subControl is Sunny.UI.UILabel lbl && lbl.Name == "countLabel" && int.TryParse(lbl.Text, out _))
                            countLabel = lbl;
                        if (subControl is Sunny.UI.UILabel lbl2 && lbl2.Name == "priceLabel" && double.TryParse(lbl2.Text, out _))
                            priceLabel = lbl2;
                    }
                    if (checkBox != null && countLabel != null && priceLabel != null && checkBox.Checked)
                    {
                        int count = 0;
                        double price = 0;
                        int.TryParse(countLabel.Text, out count);
                        double.TryParse(priceLabel.Text, out price);
                        totalSelectedCount += count;
                        totalSelectedPrice += price * count;
                    }
                }
            }
            uiLabel12.Text = $"已选   ({totalSelectedCount}    件商品)";
            uiLabel11.Text = $"总价：￥{totalSelectedPrice:F2}";
        }
        private void UpdateTotalCount()
        {
            string sql = "SELECT IFNULL(SUM(Quantity),0) FROM (SELECT ProductID, SUM(Quantity) AS Quantity FROM shoppingcarts WHERE UserID = @uid GROUP BY ProductID) t";
            object result = DBhelper.ExecuteScalar(sql, new MySqlParameter("@uid", _currentUser.UserID));
            int total = Convert.ToInt32(result);
            uiLabel1.Text = $"总数量({total}件)";
        }
        private void Shopping_Cart_Load(object sender, EventArgs e)
        {
            UpdateChoosenum();
            uiLabel1.Text = $"总数量(0件)";
            CreatShoppingCart();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            Device_Store ds = new Device_Store(_currentUser);
            this.Hide();
            ds.Show();
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            UpdateChoosenum();
            ClearShoppingCart();
        }
        private void ClearShoppingCart()
        {
            string sql = "DELETE FROM shoppingcarts WHERE UserID = @uid";
            DBhelper.ExecuteNonQuery(sql, new MySqlParameter("@uid", _currentUser.UserID));
            flowLayoutPanel2.Controls.Clear();
            NullShoppingCart();
            UpdateTotalCount();
        }

        private void uiLabel11_Click(object sender, EventArgs e)
        {

        }

        private void uiLabel12_Click(object sender, EventArgs e)
        {

        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            double totalPrice = 0;
            string totalPriceText = uiLabel11.Text.Replace("总价：￥", "").Trim();
            double.TryParse(totalPriceText, out totalPrice);
            if (totalPrice <= 0)
            {
                UIMessageBox.ShowWarning("请选择要结算的商品！");
                return;
            }

            string sql = "SELECT Balance FROM userinfo WHERE UserID = @uid";
            var result = DBhelper.ExecuteQuery(sql, new MySqlParameter("@uid", _currentUser.UserID));
            if (result.Rows.Count == 0)
            {
                UIMessageBox.ShowError("用户信息不存在！");
                return;
            }
            double balance = Convert.ToDouble(result.Rows[0]["Balance"]);

            if (balance < totalPrice)
            {
                UIMessageBox.ShowWarning("余额不足，请充值！");
                Balance_Recharge br = new Balance_Recharge(_currentUser);
                br.Show();
                return;
            }

            double newBalance = balance - totalPrice;
            string updateSql = "UPDATE userinfo SET Balance = @balance WHERE UserID = @uid";
            DBhelper.ExecuteNonQuery(updateSql,
                new MySqlParameter("@balance", newBalance),
                new MySqlParameter("@uid", _currentUser.UserID));

            List<int> toDeleteProductIds = new List<int>();
            List<Control> toRemovePanels = new List<Control>();
            List<int>toProductCounts = new List<int>();
            foreach (Control control in flowLayoutPanel2.Controls)
            {
                if (control is Sunny.UI.UIPanel panel)
                {
                    Sunny.UI.UICheckBox checkBox = null;
                    int productId = -1;
                    foreach (Control subControl in panel.Controls)
                    {
                        if (subControl is Sunny.UI.UICheckBox cb)
                            checkBox = cb;
                        if (subControl is Sunny.UI.UILabel lbl && lbl.Name == "nameLabel")
                        {
                            string pname = lbl.Text;
                            string pidSql = "SELECT ProductID FROM equipmentproducts WHERE ProductName = @pname";
                            var pidResult = DBhelper.ExecuteQuery(pidSql, new MySqlParameter("@pname", pname));
                            if (pidResult.Rows.Count > 0)
                                productId = Convert.ToInt32(pidResult.Rows[0]["ProductID"]);
                        }
                        if (subControl is Sunny.UI.UILabel lbl2 && lbl2.Name == "countLabel" && int.TryParse(lbl2.Text, out _)&&checkBox.Checked)
                        {
                            int count = 0;
                            int.TryParse(lbl2.Text, out count);
                            toProductCounts.Add(count);
                        }
                    }
                    if (checkBox != null && checkBox.Checked && productId > 0)
                    {
                        toDeleteProductIds.Add(productId);
                        toRemovePanels.Add(panel);
                    }
                }
            }
            foreach (int pid in toDeleteProductIds)
            {
                string delSql = "DELETE FROM shoppingcarts WHERE UserID = @uid AND ProductID = @pid";
                DBhelper.ExecuteNonQuery(delSql,
                    new MySqlParameter("@uid", _currentUser.UserID),
                    new MySqlParameter("@pid", pid));
            }
            foreach (var panel in toRemovePanels)
            {
                flowLayoutPanel2.Controls.Remove(panel);
            }
            CheckInfoForm cif = new CheckInfoForm(_currentUser, toProductCounts,toDeleteProductIds);
            cif.Show();
            NullShoppingCart();
            UpdateChoosenum();
            UpdateTotalCount();
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (uiCheckBox1.Checked)
            {
                foreach (Control control in flowLayoutPanel2.Controls)
                {
                    if (control is Sunny.UI.UIPanel panel)
                    {
                        Sunny.UI.UICheckBox checkBox = null;
                        foreach (Control subControl in panel.Controls)
                        {
                            if (subControl is Sunny.UI.UICheckBox cb)
                                checkBox = cb;
                        }
                        if (checkBox != null)
                        {
                            checkBox.Checked = true;
                        }
                    }
                }
            }
            else
            {
                foreach (Control control in flowLayoutPanel2.Controls)
                {
                    if (control is Sunny.UI.UIPanel panel)
                    {
                        Sunny.UI.UICheckBox checkBox = null;
                        foreach (Control subControl in panel.Controls)
                        {
                            if (subControl is Sunny.UI.UICheckBox cb)
                                checkBox = cb;
                        }
                        if (checkBox != null)
                        {
                            checkBox.Checked = false;
                        }
                    }
                }
            }
            UpdateChoosenum();
        }
    }
}
