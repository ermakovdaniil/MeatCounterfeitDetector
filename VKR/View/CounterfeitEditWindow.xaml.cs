using VKR.Models;
using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для MaterialEditWindow.xaml
    /// </summary>
    public partial class CounterfeitEditWindow
    {
        public CounterfeitEditWindow(MembraneObject material)
        {
            InitializeComponent();
            var vm = new CounterfeitEditWindowVM(material);
            DataContext = vm;
            vm.ClosingRequest += (sender, e) => Close();
        }
    }
}