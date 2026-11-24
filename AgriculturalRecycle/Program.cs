using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriculturalRecycle
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AGRICULTURALRECYCLE_CONNECTION_STRING")))
            {
                MessageBox.Show("请先配置 AGRICULTURALRECYCLE_CONNECTION_STRING。");
                return;
            }
            Application.Run(new Login());
        }
    }
}
