namespace VKR.Utils.IOService;

public interface IFileDialogService
{
    public string OpenFileDialog(string initialDirectory = "", string filter = "", string ext = "");
    public string SaveFileDialog(string fileName = "", string initialDirectory = "", string filter = "", string ext = "");
}

