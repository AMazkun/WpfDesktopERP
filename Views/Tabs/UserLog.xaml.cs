using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp5.DBContext;

namespace WpfApp5.Views.Tabs
{
    /// <summary>
    /// Логика взаимодействия для UserLog.xaml
    /// </summary>
    public partial class UserLog : UserControl
    {
        private DataManager dataManager = new DataManager();

        public UserLog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataManager.userLogs = DataWorker.GetUserLog(DataManager.LoggedUser.Id);
            DataContext = dataManager;
        }
    }
}
