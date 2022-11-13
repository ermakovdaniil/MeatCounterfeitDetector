using System.Windows;

using DataAccess.Data;
using DataAccess.Models;
using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для CompanyEditWindow.xaml
    /// </summary>
    public partial class CompanyEditWindow
    {
        public CompanyEditWindow(Company company)
        {
            InitializeComponent();
            var vm = new CompanyEditWindowVM(company);
            DataContext = vm;
            //vm.ClosingRequest += (sender, e) => Close();
        }
    }
}