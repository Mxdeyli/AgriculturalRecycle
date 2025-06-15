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
    public partial class MainManager: UIForm
    {
        private readonly User _currentUser;
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
        }

        private void MainManager_Load(object sender, EventArgs e)
        {

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
    }
}
