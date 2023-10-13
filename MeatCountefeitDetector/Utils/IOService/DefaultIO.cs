using Microsoft.Win32;


namespace MeatCounterfeitDetector.Utils.IOService;

public class FileDialogService : IFileDialogService
{
    public string OpenFileDialog(string initialDirectory = "", string filter = "", string ext = "")
    {
        var d = new OpenFileDialog
        {
            InitialDirectory = initialDirectory,
            Filter = filter,
            DefaultExt = ext,
        };

        if (d.ShowDialog() == true)
        {
            return d.FileName;
        }

        return string.Empty;
    }

    public string SaveFileDialog(string fileName = "", string initialDirectory = "", string filter = "", string ext = "")
    {
        var d = new SaveFileDialog
        {
            FileName = fileName,
            InitialDirectory = initialDirectory,
            Filter = filter,
            DefaultExt = ext,
        };

        if (d.ShowDialog() == true)
        {
            return d.FileName;
        }

        return string.Empty;
    }
}

