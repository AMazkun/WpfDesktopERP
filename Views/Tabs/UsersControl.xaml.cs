using System.Windows;
using System.Windows.Controls;
using WpfApp5.DBContext;

namespace WpfApp5.Views.Tabs
{
    /// <summary>
    /// Interaction logic for UsersControl.xaml
    /// </summary>
    public partial class UsersControl : UserControl
    {
        private DataManager dataManager = new DataManager();
        public UsersControl()
        {
            InitializeComponent();
            DataContext = dataManager;
        }


        private void del_user_btn(object sender, RoutedEventArgs e)
        {
/*          UserView user = new UserView();
            user.Show();
*/        }

    }
}
