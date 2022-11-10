using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using VKR.Utils;

namespace VKR.ViewModel
{
    /// <summary>
    ///     Абстрактный класс для VM
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        private RelayCommand _closeWindow;

        // TODO: убрать
        /// <summary>
        ///     Команда, закрывающая текущее окно
        /// </summary>
        public RelayCommand CloseWindow
        {
            get
            {
                return _closeWindow ??= new RelayCommand(o =>
                {
                    OnClosingRequest();
                });
            }
        }

        // TODO: убрать
        /// <summary>
        ///     Команда, открывающая новое окно
        /// </summary>
        /// <param name="window"></param>
        public static void ShowChildWindow(Window window)
        {
            window.Show();
        }

        public event EventHandler ClosingRequest;

        // TODO: убрать
        /// <summary>
        ///     Функция, закрывающая текущее окно
        /// </summary>
        protected void OnClosingRequest()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Обработчик изменения свойств
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
