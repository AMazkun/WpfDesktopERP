using System;
using System.Windows;
using System.Windows.Input;
using WpfApp5.DBContext;
using WpfApp5.Views.MainRoles;


namespace WpfApp5.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new DataManager();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            ResetView("Logging..");

            if (DataManager.validateUser(txtUser.Text, txtPass.Password))
            {
                Window mainWindow = null;

                switch (DataManager.LoggedUser.RoleName)
                {
                    case "Administrator":
                        mainWindow = new AdminMain();
                        break;
                    case "Sales":
                    case "Constructor":
                    case "Employer":
                    case "Shipper":
                    case "Supplier":
                    case "Operator":
                    case "Manager":
                    case "User":
                    case "Guest":
                    case "Developer":
                        mainWindow = new DeveloperMain();
                        break;
                }

                if (mainWindow == null) {
                    DataManager.ErrorResultStr = "User Main window init error.";
                    return;
                }
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ResetView("LOG IN");
            }
        }
        private void ResetView(string buttnLabel)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtPass.Focus();
                btnLogin.Content = buttnLabel;
                btnLogin.UpdateLayout();
            }));
        }

    }
}
