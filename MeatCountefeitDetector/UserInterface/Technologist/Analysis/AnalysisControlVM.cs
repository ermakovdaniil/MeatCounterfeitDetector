﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ClientAPI;
using ClientAPI.DTO.CounterfeitImage;
using ClientAPI.DTO.Result;
using Mapster;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MainWindowControlChanger;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using ImageWorker.ProgressReporter;
using MeatCounterfeitDetector.Utils.EventAggregator;
using System.Windows.Media.Imaging;
using ImageWorker.BitmapService;
using ImageWorker.ImageAnalyzis;
using System.Collections.ObjectModel;
using MeatCounterfeitDetector.UserInterface.EntityVM;
using ClientAPI.DTO.OriginalImage;
using ClientAPI.DTO.ResultImage;
using MeatCounterfeitDetector.Utils.Dialog;
using ImageWorker.Enums;
using MeatCountefeitDetector.Utils.ImageLoader;
using ImageWorker.ImageEditing;
using System.Threading.Tasks;

namespace MeatCounterfeitDetector.UserInterface.Technologist.Analysis;

public class AnalysisControlVM : ViewModelBase
{
    private readonly IImageAnalyzer _analyzer;
    private readonly IImageEditor _editor;
    private readonly IFileDialogService _fileDialogService;
    private readonly DialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly CounterfeitClient _counterfeitClient;
    private readonly CounterfeitImageClient _counterfeitImageClient;
    private readonly OriginalImageClient _originalImageClient;
    private readonly ResultImageClient _resultImageClient;
    private readonly ResultClient _resultClient;
    private readonly UserClient _userClient;
    private readonly NavigationManager _navigationManager;
    private readonly IUserService _userService;
    private readonly IProgressReporter _progressReporter;
    private readonly IEventAggregator _eventAggregator;
    private readonly IBitmapService _bitmapService;
    private readonly IImageLoader _imageLoader;

    #region Functions

    public AnalysisControlVM(CounterfeitClient counterfeitClient,
                                 CounterfeitImageClient counterfeitImageClient,
                                 OriginalImageClient originalImageClient,
                                 ResultImageClient resultImageClient,
                                 ResultClient resultClient,
                                 UserClient userClient,
                                 NavigationManager navigationManager,
                                 IImageAnalyzer analyzer,
                                 IFileDialogService fileDialogService,
                                 DialogService dialogService,
                                 IMessageBoxService messageBoxService,
                                 IUserService userService,
                                 IProgressReporter progressReporter,
                                 IEventAggregator eventAggregator,
                                 IBitmapService bitmapService,
                                 IImageLoader imageLoader,
                                 IImageEditor editor)
    {
        _counterfeitClient = counterfeitClient;
        _counterfeitImageClient = counterfeitImageClient;
        _originalImageClient = originalImageClient;
        _resultImageClient = resultImageClient;
        _resultClient = resultClient;
        _navigationManager = navigationManager;
        _analyzer = analyzer;
        _editor = editor;
        _fileDialogService = fileDialogService;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        _userClient = userClient;
        _userService = userService;
        _progressReporter = progressReporter;
        _eventAggregator = eventAggregator;
        _bitmapService = bitmapService;
        _imageLoader = imageLoader;
        _eventAggregator.Subscribe<EventImageData>(OnDataReceived);

        _progressReporter.Subscribe<ProgressData>(OnProgressReceived);

        _counterfeitClient.CounterfeitGetAsync()
                                 .ContinueWith(c => { CounterfeitVMs = c.Result.ToList().Adapt<ObservableCollection<CounterfeitVM>>(); });

        SelectedAlgorithm = Algorithms.SIFT;
        SelectedPriority = Priorities.Accuracy;
        PercentOfSimilarity = 70.0;
    }

    private void OnDataReceived(EventImageData eventData)
    {
        DisplayedImage = eventData.ImageBitmapSource;
    }

    public void PublishData()
    {
        _eventAggregator.Publish(new Event());
    }

    private void OnProgressReceived(ProgressData progressData)
    {
        Progress = progressData.Progress;
    }

    private string CreateSearchResult(CreateResultDTO AnalysisResult)
    {
        string searchResult;
        searchResult = AnalysisResult.AnalysisResult + "\n" +
                       "Дата проведения анализа: " + AnalysisResult.Date + "\n" +
                       "Время проведения: " + AnalysisResult.Time + " с\n" +
                       "Процент сходства: " + AnalysisResult.PercentOfSimilarity + "%";
        return searchResult;
    }

    private void ChooseAlgorithm(double brightness, double noise, double blur, double glare)
    {
        switch (SelectedPriority)
        {
            case Priorities.Accuracy: // AKAZE SIFT DAISY
                if (brightness < 50.0)
                {
                    SelectedAlgorithm = Algorithms.AKAZE;
                }
                else if (noise > 50.0 || blur > 50.0)
                {
                    SelectedAlgorithm = Algorithms.SIFT;
                }
                else if (glare > 50.0)
                {
                    SelectedAlgorithm = Algorithms.ORB;
                }
                else
                {
                    SelectedAlgorithm = Algorithms.SIFT;
                }
                break;

            case Priorities.Performance: // ORB BRISK FREAK LATCH
                if (brightness < 50.0 || glare > 50.0)
                {
                    SelectedAlgorithm = Algorithms.LATCH; 
                }
                else if (noise > 50.0)
                {
                    SelectedAlgorithm = Algorithms.BRISK;
                }
                else if (blur > 50.0)
                {
                    SelectedAlgorithm = Algorithms.ORB;
                }
                else
                {
                    SelectedAlgorithm = Algorithms.FREAK;
                }
                break;

            default:
                break;
        }
    }

    #endregion


    #region Properties

    public ObservableCollection<CounterfeitVM> CounterfeitVMs { get; set; }
    public CounterfeitVM SelectedCounterfeit { get; set; }

    public Algorithms SelectedAlgorithm { get; set; }
    public List<Algorithms> EnumAlgorithms
    {
        get { return Enum.GetValues(typeof(Algorithms)).Cast<Algorithms>().ToList(); }
    }

    private Priorities _selectedPriority;
    public Priorities SelectedPriority
    {
        get => _selectedPriority;
        set
        {
            _selectedPriority = value;

            if (DisplayedImage is not null)
            {
                ChooseAlgorithm(_brightness, _contrast, _noise, _glare);
            }
            OnPropertyChanged();
        }
    }
    public List<Priorities> EnumPriorities
    {
        get { return Enum.GetValues(typeof(Priorities)).Cast<Priorities>().ToList(); }
    }

    public double PercentOfSimilarity { get; set; }

    public BitmapSource DisplayedImage { get; set; }
    public string ResultImage { get; set; }
    public string SearchResult { get; set; }
    public CreateResultDTO AnalysisResult { get; set; }
    public int Progress { get; set; }
    private string _fileName { get; set; }
    private string _pathToInitialImage { get; set; }
    private Guid noId { get; set; } = Guid.Parse("00000000-0000-0000-0000-000000000000");

    private double _brightness;
    private double _contrast;
    private double _noise;
    private double _glare;

    #endregion


    #region Commands

    private RelayCommand _changePathImage;
    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                var path = _fileDialogService.OpenFileDialog(filter: "Pictures (*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.gif;*.png", ext: ".jpg");

                if (path != "")
                {
                    DisplayedImage = _bitmapService.LoadBitmapSource(path);
                    ResultImage = "";

                    _fileName = _imageLoader.GetFileName(Path.GetFileName(path), @"..\..\..\resources\origImages\", path);
                    _pathToInitialImage = path;

                    _editor.EstimateImageCharacteristics(DisplayedImage, out _brightness, out _contrast, out _noise, out _glare);
                    ChooseAlgorithm(_brightness, _contrast, _noise, _glare);
                }
            });
        }
    }

    private RelayCommand _scanImage;
    public RelayCommand ScanImage
    {
        get
        {
            return _scanImage ??= new RelayCommand(async _ =>
            {
                if (DisplayedImage is null)
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для произведения анализа", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    try
                    {
                        List<GetCounterfeitImageDTO> counterfeitImagesDTOs = new List<GetCounterfeitImageDTO>();
                        if (SelectedCounterfeit is null)
                        {
                            counterfeitImagesDTOs = (await _counterfeitImageClient.CounterfeitImageGetAsync()).ToList();
                        }
                        else
                        {
                            counterfeitImagesDTOs = (await _counterfeitImageClient.GetAllByCounterfeitIdAsync(SelectedCounterfeit.Id)).ToList();
                        }
                        // https://dottutorials.net/wpf-loading-animation-in-net-core-3/

                        //await Application.Current.Dispatcher.Invoke(async () =>
                        //{
                            AnalysisResult = (await _analyzer.RunAnalysisAsync(DisplayedImage, counterfeitImagesDTOs, PercentOfSimilarity, _userService.CurrentUserId, SelectedAlgorithm, _fileName, _pathToInitialImage));
                        //});

                        SearchResult = CreateSearchResult(AnalysisResult);

                        if (AnalysisResult.ResultImagePath is not null)
                        {
                            string pathToBase = Directory.GetCurrentDirectory();
                            string pathToResults = @"..\..\..\resources\resImages\";
                            string combinedPath = Path.Combine(pathToBase, AnalysisResult.ResultImagePath);
                            ResultImage = combinedPath;
                        }

                        Guid originalId = (await _originalImageClient.GetIdByNameAsync(AnalysisResult.OriginalImagePath));

                        if (originalId == noId)
                        {
                            var originalImageDTO = new CreateOriginalImageDTO
                            {
                                ImagePath = AnalysisResult.OriginalImagePath
                            };

                            originalId = await _originalImageClient.OriginalImagePostAsync(originalImageDTO);
                        }

                        if (AnalysisResult.ResultImagePath != "")
                        {
                            var resultImageDTO = new CreateResultImageDTO
                            {
                                OriginalImageId = originalId,
                                ImagePath = AnalysisResult.ResultImagePath
                            };

                            var resultId = await _resultImageClient.ResultImagePostAsync(resultImageDTO);
                            AnalysisResult.ResultImageId = resultId;
                        }

                        AnalysisResult.OriginalImageId = originalId;                       

                        var res = await _resultClient.ResultPostAsync(AnalysisResult);
                    }
                    catch (ArgumentException)
                    {
                        _messageBoxService.ShowMessage("Данные в базе фальсификатов были удалены или повреждены.\nПеред запуском анализа устраните проблему.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Emgu.CV.Util.CvException)
                    {
                        _messageBoxService.ShowMessage("Данное изображение не удаётся обработать. Попробуйте изменить разрешение изображения.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }
    }

    private RelayCommand _createFile;
    public RelayCommand CreateFile
    {
        get
        {
            return _createFile ??= new RelayCommand(async _ =>
            {
                if (AnalysisResult is null)
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var filename = "АНАЛИЗ_" + DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
                    var filePath = _fileDialogService.SaveFileDialog(filename, ext: ".pdf");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var user = await _userClient.UserGetAsync(_userService.CurrentUserId);
                        FileSystem.ExportPdf(filePath, AnalysisResult, user);
                    }
                }
            });
        }
    }

    private RelayCommand _showAlgInfo;
    public RelayCommand ShowAlgInfo
    {
        get
        {
            return _showAlgInfo ??= new RelayCommand(async o =>
            {
                var result = (await _dialogService.ShowDialog<AlgInfoControl>(new WindowParameters
                {
                    Height = 600,
                    Width = 800,
                    Title = "Информация об алгоритмах поиска",
                }));
            });
        }
    }

    #endregion
}
