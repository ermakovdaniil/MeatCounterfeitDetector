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
        private UserControl _control;

        public MainWindow()
        {
            InitializeComponent();
            ChangeContent(null, new LoginControl());
        }

        private void ChangeContent(object sender, UserControl control)
        {
            _control = control;
            ((IСhangeableControl)_control).ChangingRequest -= ChangeContent;
            ((IСhangeableControl)_control).ChangingRequest += ChangeContent;
            content.Content = _control;
            WindowState = ((IСhangeableControl)_control).PreferedWindowState;

            if (WindowState != WindowState.Maximized)
            {
                Height = (double)((IСhangeableControl)_control).PreferedHeight;
                Width = (double)((IСhangeableControl)_control).PreferedWidth;
            }

            Title = ((IСhangeableControl)_control).WindowTitle;

            MenuControl.Content = control.TryFindResource("Menu") as Menu;
        }
    }
}
