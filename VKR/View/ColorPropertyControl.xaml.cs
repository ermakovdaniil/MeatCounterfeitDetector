using System.Windows.Controls;
using Autofac;

using VKR.ViewModel;

namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для ColorPropertyControl.xaml
    /// </summary>
    public partial class ColorPropertyControl : UserControl
    {
        public IContainer Container { get; set; }

        private ColorPropertyControlVM _viewModel;

        public ColorPropertyControl(ColorPropertyControlVM vm)
        {
            InitializeComponent();
            DataContext = vm;
            _viewModel = Container.Resolve<ColorPropertyControlVM>();
        }
    }
}