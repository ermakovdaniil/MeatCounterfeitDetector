using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

using VKR.Utils;
using VKR.ViewModel;

using MessageBox = HandyControl.Controls.MessageBox;



namespace VKR.View
{
    /// <summary>
    ///     Логика взаимодействия для ResearcherControl.xaml
    /// </summary>
    public partial class TechnologistControl : UserControl, IСhangeableControl
    {
        public TechnologistControl()
        {
            InitializeComponent();
            var vm = new TechnologistControl();
            DataContext = vm;
            //vm.ClosingRequest += (sender, e) => Close();
        }

        public WindowState PreferedWindowState { get; set; } = WindowState.Maximized;
        public string WindowTitle { get; set; } = "Программный комплекс для исследования неизотермического течения аномально-вязких материалов";
        public double? PreferedHeight { get; set; }
        public double? PreferedWidth { get; set; }
        public event IСhangeableControl.ChangingRequestHandler ChangingRequest;

        public void OnChangingRequest(UserControl newControl)
        {
            ChangingRequest?.Invoke(this, newControl);
        }

        //private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var dlg = new SaveFileDialog
        //    {
        //        DefaultExt = ".pdf",
        //        FileName = "АНАЛИЗ_" + DateTime.Now.ToString().Replace(':', '_'),
        //    };

        //    var res = dlg.ShowDialog();

        //    if (res == true)
        //    {
        //        if ((DataContext as ResearcherControlVM).IsCalculated)
        //        {
        //            var tempChartBitMap = ChartToBitmap(tempChart);

        //            var nChartBitMap = ChartToBitmap(nChart);


        //            FileSystem.ExportPdf(dlg.FileName, tempChartBitMap, nChartBitMap, (DataContext as ResearcherControlVM).MathClass);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Нет данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

        private void Button_Click(object sender, RoutedEventArgs e) // нарушение mvvm
        {
            if (!IsValid(MainGrid))
            {
                MessageBox.Show("Невозможно произвести расчёт, есть ошибки ввода данных", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // (DataContext as TechnologistControlVM).CalcCommand.Execute(null); TODO: СДЕЛАТЬ
            }
        }

        private bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
                   LogicalTreeHelper.GetChildren(obj)
                                    .OfType<DependencyObject>()
                                    .All(IsValid);
        }

        private void Validation_OnError(object? sender, ValidationErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ChangeUser(object sender, RoutedEventArgs e)
        {
            //OnChangingRequest(new LoginControl());
        }
    }
}
