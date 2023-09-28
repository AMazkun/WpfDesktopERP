using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApp5.DBContext;

namespace WpfApp5.Controllers
{
    public static class Handlers
    {
        internal static void AppClose(object sender, CancelEventArgs e)
        {
            DataManager.LoggedResultStr = DataWorker.SaveLoginUser(false);
        }

        internal static void ShowMessage(string msg)
        {
            if (msg.Contains(DataManager.ERROR))
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(msg);
            }));

        }
    }
}
