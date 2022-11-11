using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Data.Entity;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;
using System.Windows.Controls;
using System;

namespace VKR.ViewModel
{
    public class LoginControlVM : ViewModelBase
    {
        #region Functions

        #region Constructors

        public LoginControlVM(MainWindowVM mainModel, UserDBContext context)
        {
            _context = context;
            _mainModel = mainModel;
            OpenUsersCommand = new RelayCommand(OpenUsers, CanOpenUsers);
        }

        private void OpenUsers(object _param)
        {
            LoginControlVM upmodel = new LoginControlVM(_mainModel, _context);
            LoginControl up = new LoginControl(upmodel);
            up.DataContext = upmodel;
            _mainModel.SetNewContent(up);
        }

        #endregion

        private bool CanOpenUsers(object _param)
        {
            return true;
        }

        private void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var user = _context.Users.First(u => u.Name == UserName && u.Password == Password);
                if (user.Type.Type == "Администратор")
                {
                    OnChangingRequest(new MainAdminControl());
                }

                if (user.Type.Type == "Исследователь")
                {
                    OnChangingRequest(new TechnologistControl());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Неверное имя пользователя или пароль! Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OnChangingRequest(new TechnologistControl());
        }

        #endregion


        #region Properties

        MainWindowVM _mainModel;

        private readonly UserDBContext _context;

        public string UserName;

        public string Password;
        public WindowState PreferedWindowState { get; set; } = WindowState.Normal;
        public string WindowTitle { get; set; } = "Авторизация";
        public double? PreferedHeight { get; set; } = 390;
        public double? PreferedWidth { get; set; } = 270;

        #endregion

        #region Commands
        // TODO: переписать нормально
        public RelayCommand OpenUsersCommand { get; private set; }

        public event IСhangeableControl.ChangingRequestHandler ChangingRequest;

        public void OnChangingRequest(UserControl newControl)
        {
            ChangingRequest?.Invoke(this, newControl);
        }

        #endregion
    }
}
