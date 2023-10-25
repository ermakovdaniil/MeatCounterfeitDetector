using ImageAnalyzis;
using System;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ImageAnalyzer.ProgressReporter;
using MeatCountefeitDetector.Utils.EventAggregator;
using Emgu.CV;
using MeatCountefeitDetector.Utils;

namespace MeatCounterfeitDetector.UserInterface.Technologist.Edit;

public class ImageEditingControlVM : ViewModelBase
{
    private readonly IImageAnalyzer _analyzer;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IProgressReporter _progressReporter;
    private readonly IEventAggregator _eventAggregator;

    #region Functions

    public ImageEditingControlVM(IImageAnalyzer analyzer,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService,
                                 IProgressReporter progressReporter,
                                 IEventAggregator eventAggregator)
    {
        _analyzer = analyzer;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        _progressReporter = progressReporter;
        _eventAggregator = eventAggregator;

    }

    #endregion

    // Method where you want to publish data
    public void PublishData()
    {
        // ... some logic to get the data
        var data = new Mat(); // MAT или сразу передавать обрабанное 
        _eventAggregator.Publish(new DataEvent(data));
    }

    #region Properties

    public int Brightness { get; set; } = 50;
    public int Progress { get; set; }
    public string DisplayedImagePath { get; set; }
    public string ResultImagePath { get; set; }

    #endregion


    #region Commands

    private RelayCommand _changePathImage;

    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                var path = _dialogService.OpenFileDialog(filter: "Pictures (*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.gif;*.png", ext: ".jpg");

                if (path != "")
                {
                    DisplayedImagePath = path;
                    ResultImagePath = "";
                }
            });
        }
    }

    private RelayCommand _createFile;

    public RelayCommand CreateFile
    {
        get
        {
            return _createFile ??= new RelayCommand(_ =>
            {
                if (ResultImagePath is null)
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var filename = "АНАЛИЗ_" + DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
                    var filePath = _dialogService.SaveFileDialog(filename, ext: ".pdf");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        //FileSystem.ExportPdf(filePath, AnalysisResult, _userService.User);
                    }
                }
            });
        }
    }

    #endregion
}
