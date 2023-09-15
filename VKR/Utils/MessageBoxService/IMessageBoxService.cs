using System.Windows;


namespace MeatCountefeitDetector.Utils.MessageBoxService;

public interface IMessageBoxService
{
    public MessageBoxResult ShowMessage(string messageBoxText,
                                        string caption = null,
                                        MessageBoxButton button = MessageBoxButton.OK,
                                        MessageBoxImage icon = MessageBoxImage.None,
                                        MessageBoxResult defaultResult = MessageBoxResult.None);
}

