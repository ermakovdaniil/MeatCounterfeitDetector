﻿using System;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ImageWorker.ProgressReporter;
using MeatCounterfeitDetector.Utils.EventAggregator;
using System.IO;
using System.Threading.Tasks;
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
        Contrast = 50;
    }

    public void PublishData()
    {
        _eventAggregator.Publish(new EventImageData(ResultImage));
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

    private int _brightness;
    public int Brightness
    {
        get => _brightness;
        set
        {
            if (value == _brightness) return;
            _brightness = value;
            AdjustBrightnessAndContrast.Execute(null);
            OnPropertyChanged();
        }
    }

    private int _contrast;
    public int Contrast
    {
        get => _contrast;
        set
        {
            if (value == _contrast) return;
            _contrast = value;
            AdjustBrightnessAndContrast.Execute(null);
            OnPropertyChanged();
        }
    }


    public BitmapSource OriginalImage { get; set; }
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

    private RelayCommand _adjustBrightnessAndContrast;
    public RelayCommand AdjustBrightnessAndContrast
    {
        get
        {
            return _adjustBrightnessAndContrast ??= new RelayCommand(_ =>
            {
                var state = new { brightness = _brightness, contrast = _contrast };
                Task.Delay(200).ContinueWith(_ =>
                {
                    if (state.brightness != _brightness || state.contrast != _contrast)
                    {
                        return;
                    }
                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        ResultImage = _editor.AdjustBrightnessAndContrast(OriginalImage, Contrast, Brightness);
                    });
                });
            });
        }
    }

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
                    OriginalImage = _bitmapService.LoadBitmapSource(path);
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
