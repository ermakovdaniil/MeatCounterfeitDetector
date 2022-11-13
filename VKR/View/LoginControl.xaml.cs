using System.Windows.Controls;
using Autofac;

using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для ColorPropertyControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private LoginControlVM _viewModel;

        public LoginControl(LoginControlVM vm)
        {
            InitializeComponent();
            DataContext = vm;
            _viewModel = vm;
        }
    }
}

//using System.Windows.Controls;

//using DataAccess.Data;

//using VKR.ViewModel;

//namespace VKR.View
//{
//    /// <summary>
//    ///     Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class LoginControl : UserControl
//    {
//        public LoginControl()
//        {
//            InitializeComponent();
//            // TODO: Вызвать autofac
//            //DataContext = new LoginControlVM(MainWindowVM, UserDBContext);
//        }

//        //public WindowState PreferedWindowState { get; set; } = WindowState.Normal;
//        //public string WindowTitle { get; set; } = "Авторизация";
//        //public double? PreferedHeight { get; set; } = 390;
//        //public double? PreferedWidth { get; set; } = 270;
//        //public event IСhangeableControl.ChangingRequestHandler ChangingRequest;

//        //public void OnChangingRequest(UserControl newControl)
//        //{
//        //    ChangingRequest?.Invoke(this, newControl);
//        //}


//        //private void EnterButtonClick(object sender, RoutedEventArgs e)
//        //{
//        //    var con = DbContextSingleton.GetInstance();
//        //    var userName = UserNameTextbox.Text;
//        //    var password = PasswordTextBox.Password;

//        //    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
//        //    {
//        //        MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//        //        return;
//        //    }

//        //    try
//        //    {
//        //        var user = con.Users.First(u => u.UserName == userName && u.UserPassword == password);
//        //        if (user.UserType.UserTypeName == "Администратор")
//        //        {
//        //            OnChangingRequest(new MainAdminControl());
//        //        }

//        //        if (user.UserType.UserTypeName == "Исследователь")
//        //        {
//        //            OnChangingRequest(new TechnologistControl());
//        //        }
//        //    }
//        //    catch (Exception exception)
//        //    {

//        //        MessageBox.Show("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

//        //    }
//        //}

//        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
//        //{
//        //    OnChangingRequest(new TechnologistControl());
//        //}
//    }
//}