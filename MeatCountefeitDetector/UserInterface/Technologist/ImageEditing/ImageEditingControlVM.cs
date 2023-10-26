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
using ImageAnalyzer.ImageAnalyzis.KeypointAlgorithms;
using System.IO;
using System.Windows.Media.Imaging;
using Emgu.CV.Reg;
using MeatCountefeitDetector.Utils.BitmapService;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MeatCounterfeitDetector.UserInterface.Technologist.Edit;

public class ImageEditingControlVM : ViewModelBase
{
    private readonly IImageAnalyzer _analyzer;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IProgressReporter _progressReporter;
    private readonly IEventAggregator _eventAggregator;
    private readonly IBitmapService _bitmapService;

    #region Functions

    public ImageEditingControlVM(IImageAnalyzer analyzer,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService,
                                 IProgressReporter progressReporter,
                                 IEventAggregator eventAggregator,
                                 IBitmapService bitmapService)
    {
        _analyzer = analyzer;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        _progressReporter = progressReporter;
        _eventAggregator = eventAggregator;
        _bitmapService = bitmapService;
    }




    public void PublishData()
    {
        var data = ResultImageMat;
        _eventAggregator.Publish(new DataEvent(data));
    }

    #endregion


    #region Properties

    public int Brightness { get; set; } = 50;
    public int Progress { get; set; }
    private Mat ResultImageMat { get; set; }
    public BitmapSource ResultImage { get; set; }
    public string OriginalImagePath { get; set; }


    private bool _compareIsChecked;
    public bool CompareIsChecked
    {
        get { return _compareIsChecked; }
        set
        {
            if (_compareIsChecked != value)
            {
                _compareIsChecked = value;
                CompareVisibility = _compareIsChecked ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
    public Visibility CompareVisibility { get; set; }


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
                    OriginalImagePath = path;

                    ResultImage = _bitmapService.LoadBitmap(path);
                }
            });
        }
    }

    private RelayCommand _brightnessChangedCommand;
    public RelayCommand BrightnessChangedCommand
    {
        get
        {
            return _brightnessChangedCommand ??= new RelayCommand(_ =>
            {
                ResultImage = _bitmapService.ConvertMatToBitmapSource(ResultImageMat);
            });
        }
    }


    private RelayCommand _transferImage;
    public RelayCommand TransferImage
    {
        get
        {
            return _transferImage ??= new RelayCommand(_ =>
            {
                PublishData();
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
                if (ResultImage is null)
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var filename = "ИЗОБРАЖЕНИЕ_" + DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
                    var filePath = _dialogService.SaveFileDialog(filter: "Pictures (*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.gif;*.png", ext: ".jpg");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        CvInvoke.Imwrite(filePath, ResultImageMat);
                    }
                }
            });
        }
    }

    #endregion
}
