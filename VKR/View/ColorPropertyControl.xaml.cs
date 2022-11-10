using System.Windows.Controls;

using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    /// Логика взаимодействия для ColorPropertyControl.xaml
    /// </summary>
    public partial class ColorPropertyControl : UserControl
    {
        public ColorPropertyControl()
        {
            InitializeComponent();
            DataContext = new ColorPropertyControlVM();
        }
    }
}