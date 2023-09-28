using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp5.DBContext;

namespace WpfApp5.Views.Tabs
{
    /// <summary>
    /// Логика взаимодействия для UserLog.xaml
    /// </summary>
    public partial class UserLog : UserControl
    {
        private string AllUsersLog = "All users";
        private DataManager dataManager = new DataManager();
        public DateTime fromDate, toDate;
        public bool OnlySelf = true;

        public bool GetAllUsersLog { get {
                if (userNameComboBox != null)
                    if (userNameComboBox.Text == AllUsersLog)
                        return true;
                return false;
           } 
        }

        public UserLog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = dataManager;
            dataManager.FetchLog.Execute(this);
            if (OnlySelf)
            {
                userNameComboBox.Visibility = Visibility.Collapsed;
                rowUserName.Visibility = Visibility.Collapsed;
            }
            userNameComboBox.Text = DataManager.LoggedUser.UserName;
            userNameComboBox.SelectedItem = DataManager.LoggedUser;
        }

        private void ClearFetchLog(object sender, RoutedEventArgs e)
        {
            LogFromDate.Text = "";
            LogToDate.Text = "";
            if (!OnlySelf)
            {
                userNameComboBox.SelectedItem = null;
                userNameComboBox.Text = AllUsersLog;
            }
        }

        private void logsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OnlySelf) return;

            if (logsDataGrid.SelectedItem is Log)
            {
                rowUserName.Content = ((Log)logsDataGrid.SelectedItem).theUserName ?? "";
            } else
            {
                rowUserName.Content = "User Name";
            }
        }

        private void FetchLog(object sender, RoutedEventArgs e)
        {
            bool error = false;
            string msg = "";

            fromDate = DateTime.MinValue;
            if (LogFromDate.Text != "" && !DateTime.TryParse(LogFromDate.Text, out fromDate))
            {
                msg = "Input wrong from date.\n";
                error = true;
            }

            toDate = DateTime.MinValue;
            if (LogToDate.Text != "" && !DateTime.TryParse(LogToDate.Text, out toDate))
            {
                msg = error ? msg + "Input wrong to date.\n" : msg;
                error = true;
            }

            if (error) {
                MessageBox.Show(msg);
                return; 
            }
            dataManager.FetchLog.Execute(this);
        }
    }
}
