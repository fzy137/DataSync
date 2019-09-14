using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DataSync
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            try{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //数据同步
                //Application.Run(new MainForm());
                //表空间查询
                Application.Run(new TableSpaceView());
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            



        }
    }
}
