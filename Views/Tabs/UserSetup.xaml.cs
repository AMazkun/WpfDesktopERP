using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для UserSetup.xaml
    /// </summary>
    public partial class UserSetup : UserControl
    {
        private DataManager _dataManager = new DataManager();

        public UserSetup()
        {
            InitializeComponent();
        }

        private void Click_Clear_PassFields(object sender, RoutedEventArgs e)
        {
            OldPassword.Password = "";
            NewPassword.Password = "";
            NewAgain.Password = "";
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            ChangePasswordPanet.Background = new SolidColorBrush{ Color = (Color)ColorConverter.ConvertFromString("#1F463C3C") };
        }

        private void Click_Change_Password(object sender, RoutedEventArgs e)
        {
            if (OldPassword.Password.Length == 0)
            {
                ChangePasswordPanet.Background = new SolidColorBrush(Colors.Red);
                DataManager.ResultStr = "Old Password not set";
                return;
            }
            if (NewPassword.Password.Length < 8)
            {
                ChangePasswordPanet.Background = new SolidColorBrush(Colors.Red);
                DataManager.ResultStr = "New Password les then 8 symbols";
                return;
            }
            if (NewPassword.Password != NewAgain.Password)
            {
                ChangePasswordPanet.Background = new SolidColorBrush(Colors.Red);
                DataManager.ResultStr = "New Password not identical to the next input";
                return;
            }

            _dataManager.ChangePass.Execute(null);
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {

            DataContext = _dataManager;
        }
    }
}
