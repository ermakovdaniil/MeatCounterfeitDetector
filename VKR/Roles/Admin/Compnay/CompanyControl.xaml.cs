using System.Windows.Controls;

using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    /// Логика взаимодействия для CompanyControl.xaml
    /// </summary>
    public partial class CompanyControl : UserControl
    {
        public CompanyControl()
        {
            InitializeComponent();
            DataContext = new CompanyControlVM();
        }
    }
}