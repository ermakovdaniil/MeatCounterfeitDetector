using DataAccess.Data;

using VKR.Utils;
using VKR.View;


namespace VKR.ViewModel;

public class LoginControlVM : ViewModelBase
{
    private readonly UserDBContext _context;

    #region Functions

    #region Constructors

    public LoginControlVM(UserDBContext context)
    {
        _context = context;
    }

    #endregion

    #endregion


    #region Properties

    #endregion


    #region Commands

    private RelayCommand _openColorProperty;

    public RelayCommand OpenColorProperty
    {
        get
        {
            return _openColorProperty ??= new RelayCommand(o =>
            {
                //todo invoke navigation manager
                //changeControl<ColorPropertyControl>(null);
            });
        }
    }

    #endregion
}

//private void EnterButtonClick(object sender, RoutedEventArgs e)
//{
//    var con = DbContextSingleton.GetInstance();
//    var userName = UserNameTextbox.Text;
//    var password = PasswordTextBox.Password;

//    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
//    {
//        MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//        return;
//    }

//    try
//    {
//        var user = con.Users.First(u => u.UserName == userName && u.UserPassword == password);
//        if (user.UserType.UserTypeName == "Администратор")
//        {
//            OnChangingRequest(new MainAdminControl());
//        }

//        if (user.UserType.UserTypeName == "Исследователь")
//        {
//            OnChangingRequest(new TechnologistControl());
//        }
//    }
//    catch (Exception exception)
//    {

//        MessageBox.Show("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

//    }
//}