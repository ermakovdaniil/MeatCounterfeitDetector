using VKR.Models;
using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow
    {
        public UserEditWindow(User user)
        {
            InitializeComponent();
            var vm = new UserEditVM(user);
            DataContext = vm;
            vm.ClosingRequest += (sender, e) => Close();
        }
    }
}
