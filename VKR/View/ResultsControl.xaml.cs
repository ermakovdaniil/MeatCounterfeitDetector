using System.Windows.Controls;

using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    /// Логика взаимодействия для ColorPropertyControl.xaml
    /// </summary>
    public partial class ResultsControl : UserControl
    {
        public ResultsControl()
        {
            InitializeComponent();
            DataContext = new ResultsControlVM();
        }
    }
}

