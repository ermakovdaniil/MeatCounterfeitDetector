using System;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ImageWorker.ProgressReporter;
using MeatCountefeitDetector.Utils.EventAggregator;
using MeatCountefeitDetector.Utils;
using System.IO;
using System.Windows.Media.Imaging;
using ImageWorker.BitmapService;
using ImageWorker.ImageEditing;

namespace MeatCounterfeitDetector.UserInterface.Technologist.Edit;

public class ImageEditingControlVM : ViewModelBase
{
    private readonly IImageEditor _editor;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IProgressReporter _progressReporter;
    private readonly IEventAggregator _eventAggregator;
    private readonly IBitmapService _bitmapService;

    #region Functions

    public ImageEditingControlVM(IImageEditor editor,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService,
                                 IProgressReporter progressReporter,
                                 IEventAggregator eventAggregator,
                                 IBitmapService bitmapService)
    {
        _editor = editor;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        _progressReporter = progressReporter;
        _eventAggregator = eventAggregator;
        _bitmapService = bitmapService;
    }

    public void PublishData()
    {
        _eventAggregator.Publish(new DataEvent(ResultImage));
    }

    public void GetImageData(BitmapSource source)
    {
        Brightness = _editor.GetBrightness(source);
        originalBrightness = _editor.GetBrightness(source);
    }

    private void SaveBitmapSourceAsImage(BitmapSource source, string filePath)
    {
        BitmapEncoder encoder = new JpegBitmapEncoder();

        encoder.Frames.Add(BitmapFrame.Create(source));

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            encoder.Save(fileStream);
        }
    }

    #endregion


    #region Properties

    public int Brightness { get; set; }
    public int Contrast { get; set; }



    public BitmapSource OrigianlImage { get; set; }
    public BitmapSource ResultImage { get; set; }
    public string OriginalImagePath { get; set; }

    public int Progress { get; set; }


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
    public Visibility CompareVisibility { get; set; } = Visibility.Hidden;

    private int originalBrightness { get; set; }

    #endregion


    #region Commands


    private RelayCommand _brightnessChangedCommand;
    public RelayCommand BrightnessChangedCommand
    {
        get
        {
            return _brightnessChangedCommand ??= new RelayCommand(_ =>
            {
                if(OrigianlImage is not null)
                {
                    ResultImage = _editor.AdjustBrightnessAndContrast(OrigianlImage, Contrast, Brightness);
                }
                else
                {
                    _messageBoxService.ShowMessage("Нет изображения для редактирования.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
        }
    }

    //private RelayCommand _contrastChangedCommand;
    //public RelayCommand ContrastChangedCommand
    //{
    //    get
    //    {
    //        return _contrastChangedCommand ??= new RelayCommand(_ =>
    //        {
    //            ResultImage = _editor.AdjustContrast(OrigianlImage, originalBrightness, Brightness);
    //        });
    //    }
    //}

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
                    OrigianlImage = _bitmapService.LoadBitmapSource(path);
                    ResultImage = _bitmapService.LoadBitmapSource(path);

                    GetImageData(ResultImage);
                }
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
                if (ResultImage is null)
                {
                    ResultImage = _bitmapService.LoadBitmapSource(OriginalImagePath);
                }
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
                        SaveBitmapSourceAsImage(ResultImage, filePath);
                    }
                }
            });
        }
    }

    #endregion
}
