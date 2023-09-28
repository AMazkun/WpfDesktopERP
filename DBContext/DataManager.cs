using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using WpfApp5.Controllers;
using WpfApp5.ViewControls;
using WpfApp5.Views;
using WpfApp5.Views.Forms;

namespace WpfApp5.DBContext
{

    public class DataManager : INotifyPropertyChanged
    {
        public const string ERROR = "ERROR: ";

        public DataManager()
        {
            // This should use a weak event handler instead of normal handler
            GlobalPropertyChanged += this.HandleGlobalPropertyChanged;
        }

        private List<Log> _allLogs;

        public List<Log> allLogs
        {
            get { return _allLogs; }

            set
            {
                _allLogs = value;
                OnPropertyChanged("allLogs");
            }
        }

        private List<Log> _userLogs;

        public List<Log> userLogs
        {
            get { return _userLogs; }

            set
            {
                _userLogs = value;
                OnPropertyChanged("userLogs");
            }
        }


        #region DB USER ROLE
        private List<User> _allUsers = DataWorker.GetAllUsers();

        public List<User> allUsers
        {   
            get { return _allUsers; }

            set
            {
                _allUsers = value;
                OnPropertyChanged("allUsers");
            }
        }

        private List<Setting> _allSettings = DataWorker.GetAllSettings();

        public List<Setting> allSettings
        {
            get { return _allSettings; }

            set
            {
                _allSettings = value;
                OnPropertyChanged("allSettings");
            }
        }

        private List<Role> _allRoles = DataWorker.GetAllRoles();

        public List<Role> AllRoles
        {
            get { return _allRoles; }

            set
            {
                _allRoles = value;
                OnPropertyChanged("allRoles");
            }
        }
        #endregion DB USER ROLE

        #region GLOBAL VARS
        public static User LoggedUser { get; set; }
        public static User PointedUser { get; set; }


        private User _pointedUser { get; set; } = null;
        public  User  pointedUser {
            get {
                if(_pointedUser == null)
                {
                    if (PointedUser != null && PointedUser.Id != 0) _pointedUser = DataWorker.GeyUserById(PointedUser.Id);
                }
                return _pointedUser;
            }
            set {
                _pointedUser = value;
            }
        }


        private Role pointedUserRole { get ; set; }
        public Role PointedUserRole
        {
            get { return pointedUserRole; }
            set
            {
                pointedUserRole = value ?? new Role();
                User pu = pointedUser;
                     pu.Role = pointedUserRole.Id;
            }
        }

        private static string _resultStr { get; set; }

        public static string ResultStr
        {
            get { return _resultStr; }
            set
            {
                if (IsError)
                {
                    _resultStr += value;
                }
                else
                {
                    _resultStr = value;
                }
                OnGlobalPropertyChanged(typeof(string), "ResultStr");
            }
        }

        public static string LoggedResultStr
        {
            get { return _resultStr; }
            set
            {
                if (LoggedUser != null) DataWorker.Log(LoggedUser.Id, value);
                ResultStr = value;
            }
        }

        public static string ErrorResultStr
        {
            set
            {
                if (value != "") { 
                    DataWorker.LogError(LoggedUser.Id, value);
                    _resultStr = ERROR;
                    Handlers.ShowMessage(value);
                }
                else
                {
                    _resultStr = "";
                }
                OnGlobalPropertyChanged(typeof(string), "ResultStr");
            }
        }

        public static bool IsError { get { return _resultStr.StartsWith(ERROR); } }

        public string resultStr
        {
            get { return _resultStr; }
            set
            {
                ResultStr = value;
                //OnPropertyChanged("resultStr");
            }
        }

        #endregion GLOBAL VARS

        #region LOGIN

        // check if user can be logged
        public static bool validateUser(string user, string password)
        {
            LoggedUser = DataWorker.UserCredentals(user, password);
            if (LoggedUser != null)
            {
                // clear used password for security reasons
                LoggedUser.Active = true;
                LoggedResultStr = DataWorker.SaveLoginUser(true);
                PointedUser = null;
                return true;
            }
            ErrorResultStr = "Username or password incorrect,\nor prohibited to access.\n\nAsk you Admin to solve.";
            return false;
        }

        // reset user password
        internal static bool resetUserPassword(string user, string password)
        {
            string msg = "Password for [" + user + "] ";
            if (DataWorker.UserResetPassword(user, password))
            {
                // clear used password for security reasons
                LoggedResultStr = msg + "sucsessfuly reset";

                PointedUser = null;
                LoggedUser = null;
                return true;
            }
            ErrorResultStr = msg + "cannot reset";
            return false;
        }

        private RelayCommand newPassword;
        public RelayCommand NewPassword
        {
            get
            {
                return newPassword ?? new RelayCommand(obj =>
                {
                    ResetPasswView resetPasswView = new ResetPasswView();
                    resetPasswView.Show();
                    LoggedUser = null;
      
                    // Close Login window
                    Window window = obj as Window;
                    window.Close();
          });
            }
        }

        private RelayCommand logout;
        public RelayCommand Logout
        {
            get
            {
                return logout ?? new RelayCommand(obj =>
                {
                    // logout user
                    ResultStr = "Wait ... logging out";

                    // if previousely was logged
                    if (LoggedUser != null)
                    {
                        LoggedUser.Active = false;
                        LoggedResultStr = DataWorker.SaveLoginUser(false);
                    }

                    LoginView loginWindow = new LoginView();
                    loginWindow.Show();
                    LoggedUser = null;
                    Window window = obj as Window;
                    window.Close();
                });
            }
        }

        public static string PasswordToReset = "passwordtoreset";
        private RelayCommand resetPass;
        public RelayCommand ResetPass
        {
            get
            {
                return resetPass ?? new RelayCommand(obj =>
                {
                    // logout user
                    ResultStr = "Reset password for: [" + PointedUser.UserName + "] ...";
                    PointedUser.Password = PasswordToReset;
                    PointedUser.ModifiedOn = DateTime.Now;
                    PointedUser.ModifiedBy = LoggedUser.Id;
                    LoggedResultStr = DataWorker.AllowUserResetPassword();
                });
            }
        }

        private RelayCommand suspendUser;
        public RelayCommand SuspendUser
        {
            get
            {
                return suspendUser ?? new RelayCommand(obj =>
                {
                    // logout user
                    ResultStr = "Try suspend: [" + PointedUser.UserName + "] ...";
                    PointedUser.ModifiedOn = DateTime.Now;
                    PointedUser.ModifiedBy = LoggedUser.Id;
                    PointedUser.Suspended = true;
                    LoggedResultStr = DataWorker.UserSuspend();
                    UsersRefresh();
                });
            }
        }

        public void UsersRefresh()
        {
            allUsers = DataWorker.GetAllUsers();
        }

        private RelayCommand changePass;
        public RelayCommand ChangePass
        {
            get
            {
                return changePass ?? new RelayCommand(obj =>
                {
                    // logout user
                    ResultStr = "Chaging password ...";
                    LoggedResultStr = "Password changed:" + DataWorker.SaveNewUserPass();
                    LoggedUser.Password = "";
                });
            }
        }
        #endregion LOGIN

        #region USER
        private RelayCommand newUser;
        public RelayCommand NewUser
        {
            get
            {
                return newUser ?? new RelayCommand(obj =>
                {
                    PointedUser = null;
                    OpenEditUserWindowMethod();
                });
            }
        }

        private RelayCommand editUser;
        public RelayCommand EditUser
        {
            get
            {
                return editUser ?? new RelayCommand(obj =>
                {
                    //если сотрудник
                    if (PointedUser != null)
                    {
                        PointedUserRole = PointedUser.RoleNavigation;
                        ResultStr = "Edit [" + PointedUser.UserName + "]";
                        OpenEditUserWindowMethod();
                    }
                    else
                    {
                        ResultStr = "No selection";
                    }
                });
            }
        }

        private RelayCommand deleteUser;
        public RelayCommand DeleteUser => deleteUser ?? new RelayCommand(obj =>
            {
                //если сотрудник
                if ((PointedUser != null) 
                    && (MessageBox.Show("Your are about to delete User[" + PointedUser.UserName + "]", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                                       
                {
                    ResultStr = "Deleting User[" + PointedUser.UserName + "] ...";
                    LoggedResultStr = DataWorker.DeleteUser(PointedUser);
                    UsersRefresh();
                }
                else
                {

                    ResultStr = "No selection";
                }
            }
         );

        private RelayCommand saveUser;
        public RelayCommand SaveUser
        {
            get
            {
                return saveUser ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    if (PointedUser != null)
                    {
                        ResultStr = "Wait ... saving user";
                        if (PointedUser.Id == 0) {
                            LoggedResultStr = DataWorker.SaveNewUser();
                        }
                        else
                        {
                            LoggedResultStr = DataWorker.SaveUser();
                        }
                        window.Close();
                        UsersRefresh();
                    }
                    else
                    {
                        ResultStr = "No user";
                    }
                }
             );
            }
        }

        private void OpenEditUserWindowMethod()
        {
            UserView editUserWindow = new UserView();
            SetCenterPositionAndOpen(editUserWindow);
            UsersRefresh();
        }
        #endregion USER
        
        // open as MODAL dialog
        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        #region EVENTS
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };

        static void OnGlobalPropertyChanged(Type type, string propertyName)
        {
            GlobalPropertyChanged(
                type,
                new PropertyChangedEventArgs(propertyName));
        }

        void HandleGlobalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ResultStr":
                    OnPropertyChanged("resultStr");
                    break;
                case "PointedUser":
                    OnPropertyChanged("pointedUser");
                    break;
            }
        }
        #endregion EVENTS
    }
}
