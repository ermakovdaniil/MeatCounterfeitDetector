using System.Windows;
using System.Windows.Controls;

using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindowVM VM;
        public MainWindow(MainWindowVM vm)
        {
            InitializeComponent();
            VM = vm;
            DataContext = vm;
        }
    }
}
