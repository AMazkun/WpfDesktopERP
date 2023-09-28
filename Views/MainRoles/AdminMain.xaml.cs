using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WpfApp5.Controllers;
using WpfApp5.DBContext;
using WpfApp5.Views.Tabs;


namespace WpfApp5.Views.MainRoles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminMain : Window
    {
        public AdminMain()
        {
            InitializeComponent();
            Closing += new CancelEventHandler(Handlers.AppClose);
            DataContext = new DataManager();
        }

        private bool Check_Exists(string title)
        {
            // check if alredy exist
            foreach (var item in mainTabControl.Items)
            {
                string itemTitle = (item as ClosableTab).Title;
                if (itemTitle == title)
                {
                    (item as TabItem).Focus();
                    return true;
                }
            }

            return false;
        }

        private void Show_Users(object sender, RoutedEventArgs e)
        {
            string title = "Users";

            if (Check_Exists(title) == false)
            {
                DataManager.ResultStr = "Show Users";
                ClosableTab tabitem = new ClosableTab();
                tabitem.Title = title;
                mainTabControl.Items.Add(tabitem);

                UsersControl userControl = new UsersControl();
                tabitem.Content = userControl;
                tabitem.Focus();
            }
        }

        private void Setup(object sender, RoutedEventArgs e)
        {
            string title = "Setup";

            if (Check_Exists(title) == false)
            {
                DataManager.ResultStr = "Show Setup";
                ClosableTab tabitem = new ClosableTab();
                tabitem.Title = title;
                mainTabControl.Items.Add(tabitem);

                UserSetup userSetup = new UserSetup();
                tabitem.Content = userSetup;
                tabitem.Focus();
            }
        }


        private void Show_Logs(object sender, RoutedEventArgs e)
        {
            string title = "Logs";

            if (Check_Exists(title) == false)
            {
                DataManager.ResultStr = "Show Logs";
                ClosableTab tabitem = new ClosableTab();
                tabitem.Title = title;
                mainTabControl.Items.Add(tabitem);

                UserLog userLog = new UserLog();
                tabitem.Content = userLog;
                tabitem.Focus();
            }
        }
    }
}
