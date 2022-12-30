using System.Windows;


namespace VKR.Utils.Dialog;

public record WindowParameters
{
    public int Height { get; init; } = 200;
    public int Width { get; init; } = 300;
    public WindowStartupLocation StartupLocation { get; init; } = WindowStartupLocation.CenterScreen;
    public WindowState WindowState { get; set; } = WindowState.Normal;
    public string Title { get; init; } = "";
}

