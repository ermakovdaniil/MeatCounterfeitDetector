using DataAccess.Models;
using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для CounterfeitEditWindow.xaml
    /// </summary>
    public partial class CounterfeitEditWindow
    {
        public CounterfeitEditWindow(Counterfeit counterfeit)
        {
            InitializeComponent();
            var vm = new CounterfeitEditWindowVM(counterfeit);
            DataContext = vm;
            vm.ClosingRequest += (sender, e) => Close();
        }
    }
}