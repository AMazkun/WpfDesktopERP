using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace WpfApp5.DBContext
{
    public static class DataWorker
    {
        #region USER.DB
        internal static List<Log> GetUserLog(User user, DateTime fromDate, DateTime toDate)
        {
            using (AppDbContext db = new AppDbContext())
            {
                List<Log> result = null;
                int id = user != null ? user.Id : 0;
                if (fromDate == DateTime.MinValue && toDate == DateTime.MinValue) result = db.Logs.Where(l =>  (id == 0) || (l.User == id)).ToList();
                if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue) result = db.Logs.Where(l => ((id == 0) || (l.User == id)) && l.Date >= fromDate).ToList();
                if (fromDate == DateTime.MinValue && toDate != DateTime.MinValue) result = db.Logs.Where(l => ((id == 0) || (l.User == id)) && l.Date <= toDate).ToList();
                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue) result = db.Logs.Where(l => ((id == 0) || (l.User == id)) && l.Date >= fromDate && l.Date <= toDate).ToList();
                return result;
            }
        }

        internal static List<User> GetAllUsers()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var result = db.Users.ToList();
                return result;
            }
        }

        internal static List<Setting> GetAllSettings()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var result = db.Settings.ToList();
                return result;
            }
        }

        //получение позиции по id позитиции
        internal static string GetUserNameById(int id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                User res = db.Users.FirstOrDefault(u => u.Id == id);
                return res == null ? "" : res.UserName;
            }
        }

        internal static User GeyUserById(int id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                User res = db.Users.FirstOrDefault(u => u.Id == id);
                return res;
            }
        }
        #endregion USER.DB

        #region LOGIN
        //check if user exists and allowed
        internal static User UserCredentals(string login, string pasword)
        {
            using (AppDbContext db = new AppDbContext())
            {
                User res = db.Users.FirstOrDefault(u => (u.UserName == login) && (u.Password == pasword) && (!u.Suspended));
                if (res == null) { return null; }
                // for security reasons
                res.Password = "";
                return res;
            }
        }

        // if admin reset pasword with secret then allow to change
        // suspended account not allow to change passwords
        internal static bool UserResetPassword(string userName, string password)
        {
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist
                User user = db.Users.FirstOrDefault(u => u.UserName == userName
                    && u.Password == DataManager.PasswordToReset && (!u.Suspended));
                if (user != null)
                {
                    user.Password = password;
                    DataManager.ErrorResultStr = dbSave(db);
                    if (!DataManager.IsError)
                    {
                        DataManager.LoggedUser = user;
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        //сохранение сотрудника
        public static string AllowUserResetPassword()
        {
            string result = "User do not exists";
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist or new
                User user = db.Users.FirstOrDefault(p => p.Id == DataManager.PointedUser.Id);

                if (user != null)
                {
                    user.Password = DataManager.PasswordToReset;
                    user.ModifiedBy = DataManager.PointedUser.ModifiedBy;
                    user.ModifiedOn = DataManager.PointedUser.ModifiedOn;
                    dbSaveUser(db, user);
                    result = "Password reset for User [" + user.UserName + "]";
                }
            }
            return result;
        }

        //сохранение сотрудника
        public static string UserSuspend()
        {
            string result = "User do not exists";
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist or new
                User user = db.Users.FirstOrDefault(p => p.Id == DataManager.PointedUser.Id);

                if (user != null)
                {
                    user.Suspended = DataManager.PointedUser.Suspended;
                    user.ModifiedBy = DataManager.PointedUser.ModifiedBy;
                    user.ModifiedOn = DataManager.PointedUser.ModifiedOn;
                    dbSaveUser(db, user);
                    result = "Suspended User [" + user.UserName + "]";
                }
            }
            return result;
        }

        //редактирование сотрудника
        public static string SaveLoginUser(bool state)
        {
            string result = "User do not exists";
            if (DataManager.LoggedUser == null)
            {
                return "Ups! Logged user gone";
            }
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist
                User user = db.Users.FirstOrDefault(p => p.Id == DataManager.LoggedUser.Id);
                if (user != null)
                {
                    user.Active = state;
                    user.LastLoginDate = DateTime.Now;
                    dbSaveUser(db, user);
                    result = ((state) ? "Login" : "Logout") + " User [" + user.UserName + "]";
                }
            }
            return result;
        }

        public static string SaveNewUserPass()
        {
            string result = "User do not exists";
            if (DataManager.LoggedUser == null)
            {
                return "Ups! Logged user gone";
            }
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist
                User user = db.Users.FirstOrDefault(p => p.Id == DataManager.LoggedUser.Id);
                if (user != null)
                {
                    user.Password = DataManager.LoggedUser.Password;
                    dbSaveUser(db, user);
                    result = "Password changed for User [" + user.UserName + "]"; 
                }
            }
            return result;
        }
        #endregion LOGIN


        internal static string dbSave(AppDbContext db)
        {
            string msg = "";
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException eve)
            {
                foreach (var vec in eve.EntityValidationErrors)
                    foreach (var ve in vec.ValidationErrors)
                    {
                        msg += String.Format("Property: \"{0}\", Error: \"{1}\". ", ve.PropertyName, ve.ErrorMessage);
                    }
                Debug.WriteLine(msg);
            }
            catch (DbUpdateException e)
            {
                //Add your code to inspect the inner exception and/or
                //e.Entries here.
                //Or just use the debugger.
                //Added this catch (after the comments below) to make it more obvious 
                //how this code might help this specific problem
                msg = String.Format("DbUpdateException: \"{0}\", Error: \"{1}\"", e.Message, e.InnerException.Message);
                Debug.WriteLine(msg);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return msg;
        }


        #region USER OPERATIONS

        internal static void dbSaveUser(AppDbContext db, User user)
        {
            DataManager.ErrorResultStr = dbSave(db);
            if (DataManager.IsError)
            {
                DataManager.PointedUser = user;
            }
        }

        //сохранение сотрудника
        public static string SaveUser()
        {
            string result = "User do not exists";
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist or new
                User user = db.Users.FirstOrDefault(p => p.Id == DataManager.PointedUser.Id);

                if (user != null)
                {
                    user.UserName = DataManager.PointedUser.UserName;
                    user.FirstName = DataManager.PointedUser.FirstName;
                    user.LastName = DataManager.PointedUser.LastName;
                    user.Role = DataManager.PointedUser.Role;
                    user.Suspended = DataManager.PointedUser.Suspended;

                    user.ModifiedBy = DataManager.LoggedUser.Id;
                    user.ModifiedOn = DateTime.Now;
                    dbSaveUser(db, user);
                    result = "Updated User [" + user.UserName + "]";
                }
            }
            return result;
        }

        //сохранение сотрудника
        public static string SaveNewUser()
        {
            string result = "User do not exists";
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist or new
                User user = (DataManager.PointedUser.Id == 0) ?
                    new User() : db.Users.FirstOrDefault(p => p.Id == DataManager.PointedUser.Id);

                if (user != null)
                {
                    user.UserName = DataManager.PointedUser.UserName;
                    user.FirstName = DataManager.PointedUser.FirstName;
                    user.LastName = DataManager.PointedUser.LastName;
                    user.Role = DataManager.PointedUser.Role;
                    user.Suspended = DataManager.PointedUser.Suspended;

                    // for new user
                    user.Id = db.Users.Max(u => u.Id) + 1;
                    user.Password = DataManager.PasswordToReset;
                    user.CreatedOn = DataManager.PointedUser.CreatedOn;
                    user.CreatedBy = DataManager.PointedUser.CreatedBy;
                    db.Users.Add(user);
                    dbSaveUser(db, user);
                    result = "New User [" + user.UserName + "]";

                }
            }
            return result;
        }

        internal static string DeleteUser(User selectedUser)
        {
            string result = "User do not exists";
            using (AppDbContext db = new AppDbContext())
            {
                //check user is exist
                User user = db.Users.FirstOrDefault(p => p.Id == selectedUser.Id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    dbSaveUser(db, user);
                    result = "Deleted User [" + user.UserName + "]";
                    DataManager.PointedUser = null;
                }
            }
            return result;
        }

        #endregion USER OPERATIONS

        #region ROLES
        internal static List<Role> GetAllRoles()
        {
            using (AppDbContext db = new AppDbContext())
            {
                var res = db.Roles.ToList();
                return res;
            }
        }

        internal static Role GetRoleById(int id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                Role res = db.Roles.FirstOrDefault(r => r.Id == id);
                return res ?? new Role();
            }
        }

        internal static string GetRoleNameById(int id)
        {
            Role res = GetRoleById(id);
            return res == null ? "" : res.Name;
        }
        #endregion ROLES

        #region LOG

        internal static void Log(string value, bool err = false)
        {
            using (AppDbContext db = new AppDbContext())
            {
                int userId = DataManager.LoggedUser != null ? DataManager.LoggedUser.Id : 0;
                Log log = new Log
                {
                    Id = (db.Logs.Count() > 0) ? db.Logs.Max(l => l.Id) + 1 : 1,
                    Date = DateTime.Now,
                    Error = false,
                    User = userId,
                    Address = App.LocalIP,
                    Message = value
                };

                db.Logs.Add(log);
                DataManager.ErrorResultStr = dbSave(db);
            }
        }

        internal static void LogError(string value)
        {
            Log(value, true);
        }

        #endregion LOG
    }
}