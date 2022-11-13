using System.Windows;
using System.Windows.Controls;

using DataAccess.Models;

using VKR.ViewModel;


namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для CounterfeitExplorerControl.xaml
    /// </summary>
    public partial class CounterfeitExplorerControl : UserControl
    {
        public CounterfeitExplorerControl()
        {
            InitializeComponent();
            DataContext = new CounterfeitExplorerControl();
        }
    }
}
