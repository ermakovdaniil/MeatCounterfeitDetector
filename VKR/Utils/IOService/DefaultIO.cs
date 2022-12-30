using System;
using System.Text;

using Microsoft.Win32;


namespace VKR.Services.IOService;

public class FileDialogService : IFileDialogService
{
    public string OpenFileDialog(string initialDirectory="", string filter="", string ext = "")
    {
        var d = new OpenFileDialog();
        d.InitialDirectory = initialDirectory;
        d.Filter = filter;
        d.DefaultExt = ext;

        if (d.ShowDialog()==true)
        {
            return d.FileName;
        }

        return string.Empty;
    }
    public string SaveFileDialog(string fileName="",string initialDirectory="", string filter="", string ext = "")
    {
        var d = new SaveFileDialog();
        d.FileName = fileName;
        d.InitialDirectory = initialDirectory;
        d.Filter = filter;
        d.DefaultExt = ext;

        if (d.ShowDialog()==true)
        {
            return d.FileName;
        }

        return string.Empty;
    }
    
}
