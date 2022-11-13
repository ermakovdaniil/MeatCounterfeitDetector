using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;


namespace VKR.ViewModel;

internal class UserEditWindowVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public UserEditWindowVM(User tempUser)
    {
        TempUser = new User
        {
            Id = tempUser.Id,
            Name = tempUser.Name,
            Password = tempUser.Password,
            Type = tempUser.Type,
        };

        EditingUser = tempUser;
        Db = new UserDBContext();
        UserTypes = Db.UserTypes.Local.ToObservableCollection();
    }

    #endregion

    #endregion


    #region Properties

    public ObservableCollection<UserType> UserTypes { get; set; }
    public User TempUser { get; set; }
    public User EditingUser { get; set; }

    private UserDBContext Db { get; }

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
                EditingUser.Id = TempUser.Id;
                EditingUser.Name = TempUser.Name;
                EditingUser.Password = TempUser.Password;
                EditingUser.Type = TempUser.Type;

                if (!Db.Users.Contains(EditingUser))
                {
                    Db.Users.Add(EditingUser);
                }

                Db.SaveChanges();

                //OnClosingRequest();
            });
        }
    }

    #endregion
}
