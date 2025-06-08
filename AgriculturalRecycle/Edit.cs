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

namespace AgriculturalRecycle
{
    public partial class Edit: UIForm
    {
        private readonly User _currentuser;
        public Edit(User user)
        {
            InitializeComponent();
            _currentuser = user;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            UserManage usermanage = new UserManage(_currentuser);
            this.Hide();
            usermanage.Show();

        }
    }
}
