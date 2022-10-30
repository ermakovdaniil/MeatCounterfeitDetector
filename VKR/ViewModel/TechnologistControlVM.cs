using Microsoft.Win32;
using System.Windows;
using VKR.Utils;

namespace VKR.ViewModel
{
    public class TechnologistControlVM : ViewModelBase
    {
        #region Functions
        public TechnologistControlVM() { }

        #endregion

        #region Properties
        private string _displayedImagePath;
        public string DisplayedImagePath 
        {
            get
            {
                return _displayedImagePath;
            }
            set
            {
                _displayedImagePath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        private RelayCommand _changeUser;

        public RelayCommand ChangeUser
        {
            get
            {
                return _changeUser ??= new RelayCommand(_ =>
                {

                });
            }
        }

        private RelayCommand _changePathImage;

        public RelayCommand ChangePathImageCommand
        {
            get
            {
                return _changePathImage ??= new RelayCommand(_ =>
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.DefaultExt = (".png");
                    open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

                    if (open.ShowDialog() == true)
                        DisplayedImagePath = open.FileName;
                });
            }
        }

        private RelayCommand _showInfo;

        // TODO: НАПИСАТЬ
        public RelayCommand ShowInfo
        {
            get
            {
                return _showInfo ??= new RelayCommand(c =>
                {
                    HandyControl.Controls.MessageBox.Show("Вариант 5\n" +
                                    "Объектом оптимизации является химический реактор, в котором происходит образование целевого компонента.\n" +
                                    "Реактор  оборудован  мешалкой и  двумя теплообменными устройствами: змеевиком и рубашкой.\n" +
                                    "Необходимо определить температурные условия технологического процесса, обеспечивающие минимальную себестоимость целевого компонента.\n" +
                                    "Согласно эмпирической математической модели, количество  получаемого целевого компонента  S(кг) связано с параметрами процесса следующим образом:\n" +
                                    "S = α * (G * µ * ((T2 - T1) ^ N + (β * A - T1) ^ N)),\n" +
                                    "где  α, β, µ, -нормирующие множители, равные 1;\n" +
                                    "       G - расход реакционной массы ( 2 кг/ч );\n" +
                                    "       А - давление в реакторе ( 1 Кпа );\n" +
                                    "       N - скорость вращения мешалки ( 2 об/c );\n" +
                                    "       Т1, Т2 - температуры в теплообменных устройствах (°C).\n" +
                                    "Регламентом установлено, что температура в змеевике может изменяться в диапазоне от -3 до 3 °C,\n" +
                                    "в рубашке -от - 2 до 6 °C. Кроме того, должно выполняться условие:\n" +
                                    "       T1 + 0.5 * T2 ≤ 1.\n" +
                                    "Себестоимость 1 кг целевого компонента \n" +
                                    "составляет 100 у.е. Точность решения – 0,01 °C \n" +
                                    "Автор:  Ермаков Даниил Игоревич\n" +
                                    "Группа: 494\n" +
                                    "Учебное заведение: СПбГТИ (ТУ)", "Условие",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        #endregion
    }
}