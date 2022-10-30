using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using HandyControl.Controls;

using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

using VKR.Data;
using VKR.ViewModel;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для MainAdminControl.xaml
    /// </summary>
    public partial class MainAdminControl : UserControl, IСhangeableControl
    {
        public MainAdminControl()
        {
            InitializeComponent();
            //DbContextSingleton.GetInstance().SavedChanges -= NotifyDbUpdated;
            //DbContextSingleton.GetInstance().SavedChanges += NotifyDbUpdated;
            //var vm = new MainAdminPanelVm();
            //DataContext = vm;
        }

        public WindowState PreferedWindowState { get; set; } = WindowState.Maximized;
        public string WindowTitle { get; set; } = "Панель администратора";
        public double? PreferedHeight { get; set; }
        public double? PreferedWidth { get; set; }
        public event IСhangeableControl.ChangingRequestHandler ChangingRequest;

        public void OnChangingRequest(UserControl newControl)
        {
            ChangingRequest.Invoke(this, newControl);
        }

        private static void NotifyDbUpdated(object? sender, SavedChangesEventArgs savedChangesEventArgs)
        {
            Growl.SuccessGlobal("Данные в базе обновлены");
        }

        private void ChangeUser(object sender, RoutedEventArgs e)
        {
            OnChangingRequest(new LoginControl());
        }

        private void SaveDB(object sender, RoutedEventArgs e)
        {
            //сохраняем на всякий случай все изменения
            //DbContextSingleton.GetInstance().SaveChanges();
            var sf = new SaveFileDialog();
            sf.Filter = "Файлы базы данных(*.db)|*.db|All files(*.*)|*.*";
            sf.FileName = $"DB_Backup_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}";

            if ((bool)sf.ShowDialog())
            {
                File.Copy($"{Environment.CurrentDirectory}/Membrane.db", sf.FileName, true);
                MessageBox.Show($@"Копия базы данных сохранена по пути {sf.FileName}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось сохранить базу данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDB(object sender, RoutedEventArgs e)
        {
            var of = new OpenFileDialog();
            of.Filter = "Файлы базы данных(*.db)|*.db|All files(*.*)|*.*";


            if ((bool)of.ShowDialog())
            {
                File.Copy(of.FileName, $"{Environment.CurrentDirectory}/Membrane.db", true);
                MessageBox.Show(@"Копия базы данных восстановлена, требуется приложение перезапустится после закрытия этого окна", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Не удалось загрузить базу данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }
    }
}
