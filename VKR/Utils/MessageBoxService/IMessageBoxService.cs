using System.Windows;


namespace VKR.Utils.MessageBoxService;

public interface IMessageBoxService
{
    MessageBoxResult ShowMessage(string messageBoxText,
                                 string caption = null,
                                 MessageBoxButton button = MessageBoxButton.OK,
                                 MessageBoxImage icon = MessageBoxImage.None,
                                 MessageBoxResult defaultResult = MessageBoxResult.None);
}
