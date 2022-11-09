using System.Windows.Controls;

using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    /// Логика взаимодействия для CompaniesControl.xaml
    /// </summary>
    public partial class CompaniesControl : UserControl
    {
        public CompaniesControl()
        {
            InitializeComponent();
            DataContext = new CompanyControlVM();
        }
    }
}