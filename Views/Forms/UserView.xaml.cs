using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfApp5.DBContext;


namespace WpfApp5.Views.Forms
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        DataManager dataManager = new DataManager();

        public UserView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataManager.PointedUser == null)
            {
                Title = "New User";
                dataManager.pointedUser = new User();
                dataManager.pointedUser.CreatedOn = DateTime.Now;
                dataManager.pointedUser.CreatedBy = DataManager.LoggedUser.Id;

                dataManager.PointedUserRole = new Role();

                dataManager.resultStr = "New User Created";

            }
            DataContext = dataManager;
        }

 
        private void Click_Close_Window(object sender, RoutedEventArgs e)
        {
            DataManager.ResultStr = "";
            this.Close();
        }

        // verify input

        private void btnApply_click(object sender, RoutedEventArgs e)
        {
            if (dataManager.pointedUser == null) {
                MessageBox.Show("No user selected");
                return;
            }
            if (dataManager.pointedUser.UserName == null || dataManager.pointedUser.UserName == "")
            {
                MessageBox.Show("UserName should be set");
                return;
            }
            if (dataManager.pointedUser.Role == 0)
            {
                MessageBox.Show("Role should be set");
                return;
            }

            if (dataManager.pointedUser.LastName == null)
            {
                MessageBox.Show("Last name should be set");
                return;
            }
            if (dataManager.pointedUser.FirstName == null)
            {
                MessageBox.Show("FirstName should be set");
                return;
            }

            DataManager.PointedUser = dataManager.pointedUser;
            dataManager.SaveUser.Execute(this);
        }

    }
}
