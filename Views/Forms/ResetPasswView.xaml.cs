using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp5.DBContext;
using WpfApp5.Views.MainRoles;


namespace WpfApp5.Views
{
    /// <summary>
    /// Interaction logic for ResetPasswView.xaml
    /// </summary>
    public partial class ResetPasswView : Window
    {
        private DataManager _dataManager = new DataManager();

        public ResetPasswView()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;

            if (txtUser.Text.Length == 0)
            {
                txtUser.BorderBrush = new SolidColorBrush(Colors.Red);
                txtInfor.Text = "Input UserName. ";
                error = true;
            }
            if (txtPass.Password.Length < 8)
            {
                txtPass.BorderBrush = new SolidColorBrush(Colors.Red);
                string msg = "Password less 8 symbols. ";
                txtInfor.Text = error? txtInfor.Text + msg : msg;
                error = true;
            }
            if (txtPassAgn.Password != txtPass.Password)
            {
                txtPassAgn.BorderBrush = new SolidColorBrush(Colors.Red);
                string msg = "Passwords are not the same.";
                txtInfor.Text = error ? txtInfor.Text + msg : msg;
                error = true;
            }

            if (error) { return; }

            resetView("Connect...");

            if (DataManager.resetUserPassword(txtUser.Text, txtPass.Password))
            {
  
                MessageBox.Show("Password successfully reset.\nPlease loging again with new password.");
                _dataManager.Logout.Execute(this);
            }
            else
            {
                resetView("RESET PASSWORD");
                MessageBox.Show("Reset password for user:\n\n[" + txtUser.Text +"]\n\nnot possible");
            }
        }

        private void resetView( string buttnLabel)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtPass.Focus();
                btnLogin.Content = buttnLabel;
                btnLogin.UpdateLayout();
            }));
        }


        private void input_Changed(object sender, RoutedEventArgs e)
        {
            txtUser.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            txtPass.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            txtPassAgn.BorderBrush = new SolidColorBrush(Colors.DarkGray);
        }

        private void input_Changed(object sender, TextChangedEventArgs e)
        {
            txtUser.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            txtPass.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            txtPassAgn.BorderBrush = new SolidColorBrush(Colors.DarkGray);
        }

        private void btnBack_click(object sender, RoutedEventArgs e)
        {
            _dataManager.Logout.Execute(this);
        }
    }
}
