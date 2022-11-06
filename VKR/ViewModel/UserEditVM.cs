using System.Collections.ObjectModel;
using System.Linq;

using VKR.Data;
using VKR.Models;

using VKR.Utils;


namespace VKR.ViewModel
{
    internal class UserEditVM : ViewModelBase

    {
        #region Functions

        #region Constructors

        public UserEditVM(User tempUser)
        {
            TempUser = new User
            {
                UserId = tempUser.UserId,
                UserName = tempUser.UserName,
                UserPassword = tempUser.UserPassword,
                UserType = tempUser.UserType,
            };

            EditingUser = tempUser;
            Db = DbContextSingleton.GetInstance();
            UserTypes = Db.UserTypes.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        public ObservableCollection<UserType> UserTypes { get; set; }
        public User TempUser { get; set; }
        public User EditingUser { get; set; }

        private MembraneContext Db { get; }

        #endregion


        #region Commands

        private RelayCommand _saveUser;

        /// <summary>
        ///     Команда сохраняющая изменение данных о пользователе в базе данных
        /// </summary>
        public RelayCommand SaveUser
        {
            get
            {
                return _saveUser ??= new RelayCommand(o =>
                {
                    EditingUser.UserId = TempUser.UserId;
                    EditingUser.UserName = TempUser.UserName;
                    EditingUser.UserPassword = TempUser.UserPassword;
                    EditingUser.UserType = TempUser.UserType;

                    if (!Db.Users.Contains(EditingUser))
                    {
                        Db.Users.Add(EditingUser);
                    }

                    Db.SaveChanges();
                    OnClosingRequest();
                });
            }
        }

        #endregion
    }
}