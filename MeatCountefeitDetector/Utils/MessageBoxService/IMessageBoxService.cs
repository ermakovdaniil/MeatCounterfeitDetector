using System.Windows;


namespace MeatCounterfeitDetector.Utils.MessageBoxService;

public interface IMessageBoxService
{
    public MessageBoxResult ShowMessage(string messageBoxText,
                                        string caption = null,
                                        MessageBoxButton button = MessageBoxButton.OK,
                                        MessageBoxImage icon = MessageBoxImage.None,
                                        MessageBoxResult defaultResult = MessageBoxResult.None);
}

