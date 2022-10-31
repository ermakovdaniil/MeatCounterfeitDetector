using System.Windows;
using System.Windows.Controls;

using VKR.Models;

using VKR.ViewModel;


namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для MaterialExplorerControl.xaml
    /// </summary>
    public partial class CounterfeitExplorerControl : UserControl
    {
        public CounterfeitExplorerControl()
        {
            InitializeComponent();
            DataContext = new CounterfeitExplorerControl();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new CounterfeitEditWindow((DataContext as CounterfeitExplorerControlVM).SelectedMemObject);
            win.ShowDialog();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new CounterfeitEditWindow(new MembraneObject());
            win.ShowDialog();
        }
    }
}
