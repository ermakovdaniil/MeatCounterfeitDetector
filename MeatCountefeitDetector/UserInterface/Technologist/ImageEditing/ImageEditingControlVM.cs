using System;
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
using System.Diagnostics.Contracts;

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

        CompareVisibility = Visibility.Hidden;
        ProgressVisibility = Visibility.Hidden;

        Brightness = 50;
        Contrast = 50;
        Noise = 0;
        Sharpness = 50;
        Glare = 0;
        FocalLengthX = 300;
        FocalLengthY = 200;
        Width = 1;
        Height = 1;
        Rotation = 0;
    }

    public void PublishData()
    {
        _eventAggregator.Publish(new EventImageData(ResultImage));
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
            AdjustFilter.Execute(null);
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
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private int _noise;
    public int Noise
    {
        get => _noise;
        set
        {
            if (value == _noise) return;
            _noise = value;
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private int _sharpness;
    public int Sharpness
    {
        get => _sharpness;
        set
        {
            if (value == _sharpness) return;
            _sharpness = value;
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private int _glare;
    public int Glare
    {
        get => _glare;
        set
        {
            if (value == _glare) return;
            _glare = value;
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private int _focalLengthX;
    public int FocalLengthX
    {
        get => _focalLengthX;
        set
        {
            if (value == _focalLengthX) return;
            _focalLengthX = value;
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private int _focalLengthY;
    public int FocalLengthY
    {
        get => _focalLengthY;
        set
        {
            if (value == _focalLengthY) return;
            _focalLengthY = value;
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private double _height;
    public double Height
    {
        get => _height;
        set
        {
            if (value == _height) return;
            _height = value;
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private double _width;
    public double Width
    {
        get => _width;
        set
        {
            if (value == _width) return;
            _width = value;
            AdjustFilter.Execute(null);
            OnPropertyChanged();
        }
    }

    private int _rotation;
    public int Rotation
    {
        get => _rotation;
        set
        {
            if (value == _rotation) return;
            _rotation = value;
            AdjustFilter.Execute(null);
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
    public Visibility CompareVisibility { get; set; }
    public Visibility ProgressVisibility { get; set; } 

    #endregion


    #region Commands

    private RelayCommand _adjustFilter;
    public RelayCommand AdjustFilter
    {
        get
        {
            return _adjustFilter ??= new RelayCommand(_ =>
            {
                var state = new { brightness = _brightness,
                                  contrast = _contrast, 
                                  noise = _noise,
                                  sharpness = _sharpness, 
                                  glare = _glare,
                                  focalLengthX = _focalLengthX, 
                                  focalLengthY = _focalLengthY,
                                  width = _width,
                                  height = _height, 
                                  rotation = _rotation };
                
                Task.Delay(200).ContinueWith(_ =>
                {
                    if (state.brightness != _brightness || 
                        state.contrast != _contrast || 
                        state.noise != _noise || 
                        state.sharpness != _sharpness || 
                        state.glare != _glare ||
                        state.focalLengthX != _focalLengthX || 
                        state.focalLengthY != _focalLengthY ||
                        state.width != _width ||
                        state.height != _height || 
                        state.rotation != _rotation)
                    {
                        return;
                    }
                    ProgressVisibility = Visibility.Visible;
                    Application.Current.Dispatcher.Invoke(async () =>
                    {
                        ResultImage = _editor.AdjustFilter(OriginalImage, Brightness, Contrast, Noise, Sharpness, Glare, FocalLengthX, FocalLengthY, Width, Height, Rotation);
                    });
                    ProgressVisibility = Visibility.Hidden;
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

                    Brightness = 50;
                    Contrast = 50;
                    Noise = 0;
                    Sharpness = 50;
                    Glare = 0;
                    FocalLengthX = 300;
                    FocalLengthY = 200;
                    Width = 1;
                    Height = 1;
                    Rotation = 0;
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
