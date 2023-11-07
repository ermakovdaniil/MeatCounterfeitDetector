using System.IO;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ClientAPI;
using ClientAPI.DTO.CounterfeitPath;
using Mapster;
using System.Collections.ObjectModel;

namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public class GalleryControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public GalleryControlVM(CounterfeitPathClient counterfeitPathClient,
                            DialogService dialogService,
                            IMessageBoxService messageBoxService)
    {
        _counterfeitPathClient = counterfeitPathClient;
        _messageBoxService = messageBoxService;
        _dialogService = dialogService;

        _counterfeitPathClient.CounterfeitPathGetAsync()
                              .ContinueWith(c => { CounterfeitPathVMs = c.Result.ToList().Adapt<ObservableCollection<CounterfeitPathVM>>(); });
    }

    #endregion

    #endregion

    #region Properties

    private readonly DialogService _dialogService;
    private readonly CounterfeitPathClient _counterfeitPathClient;
    private readonly IMessageBoxService _messageBoxService;

    //public CounterfeitPath SelectedCounterfeitPath { get; set; }
    //public List<CounterfeitPath> CounterfeitPaths => _context.CounterfeitPaths.ToList();
    //public List<DataAccess.Models.Counterfeit> Counterfeits => _context.Counterfeits.ToList();

    public ObservableCollection<CounterfeitPathVM> CounterfeitPathVMs { get; set; }
    public CounterfeitPathVM SelectedCounterfeitPath { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addCounterfeitPath;
    public RelayCommand AddCounterfeitPath
    {
        get
        {
            return _addCounterfeitPath ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{
                var result = (await _dialogService.ShowDialog<GalleryEditControl>(new WindowParameters
                {
                    Height = 550,
                    Width = 350,
                    Title = "Добавление изображения фальсификата",
                },
                data: new CounterfeitPathVM())) as CounterfeitPathVM;

                if (result is null)
                {
                    return;
                }

                if (!CounterfeitPathVMs.Any(rec => rec.CounterfeitId == result.CounterfeitId && rec.ImagePath == result.ImagePath))
                {
                    var addingCounterfeitPathCreateDTO = result.Adapt<CreateCounterfeitPathDTO>();
                    var id = await _counterfeitPathClient.CounterfeitPathPostAsync(addingCounterfeitPathCreateDTO);
                    result.Id = id;
                    CounterfeitPathVMs.Add(result);

                    if (!CounterfeitPathVMs.Any(rec => rec.ImagePath == result.ImagePath))
                    {
                        File.Copy(result.ImagePath, @"..\..\..\resources\counterfeits\" + Path.GetFileName(result.ImagePath), true);
                    }
                    _messageBoxService.ShowMessage($"Объект успешно добавлен!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _messageBoxService.ShowMessage($"Такая запись уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //});
            });
        }
    }

    private RelayCommand _editCounterfeitPath;
    public RelayCommand EditCounterfeitPath
    {
        get
        {
            return _editCounterfeitPath ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{

                var path = SelectedCounterfeitPath.ImagePath;

                var result = (await _dialogService.ShowDialog<GalleryEditControl>(new WindowParameters
                {
                    Height = 550,
                    Width = 350,
                    Title = "Редактирование изображения фальсификата",
                },
                data: SelectedCounterfeitPath)) as CounterfeitPathVM;

                if (result is null)
                {
                    return;
                }

                if (!CounterfeitPathVMs.Any(rec => rec.CounterfeitId == result.CounterfeitId && rec.ImagePath == result.ImagePath))
                {
                    var editingCounterfeitPathUpdateDTO = result.Adapt<UpdateCounterfeitPathDTO>();
                    await _counterfeitPathClient.CounterfeitPathPutAsync(editingCounterfeitPathUpdateDTO)
                                                .ContinueWith(c => { CounterfeitPathVMs.Add(result); });

                    if(path != result.ImagePath)
                    {
                        File.Copy(result.ImagePath, @"..\..\..\resources\counterfeits\" + Path.GetFileName(result.ImagePath), true);
                    }
                    _messageBoxService.ShowMessage($"Объект успешно добавлен!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _messageBoxService.ShowMessage($"Такая запись уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //});

            }, _ => SelectedCounterfeitPath is not null);
        }
    }

    private RelayCommand _deleteCounterfeitPath;
    public RelayCommand DeleteCounterfeitPath
    {
        get
        {
            return _deleteCounterfeitPath ??= new RelayCommand(async o =>
            {
                if (_messageBoxService.ShowMessage("Вы действительно хотите удалить изображение?", "Удаление изображения", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //string pathToBase = Directory.GetCurrentDirectory();
                    //string combinedPath = Path.Combine(pathToBase, SelectedCounterfeitPath.ImagePath);
                    //File.Delete(combinedPath);

                    //_context.CounterfeitPaths.Remove(SelectedCounterfeitPath);
                    //_context.SaveChanges();

                    //Application.Current.Dispatcher.Invoke(async () =>
                    //{
                    await _counterfeitPathClient.CounterfeitPathDeleteAsync(SelectedCounterfeitPath.Id)
                                                .ContinueWith(c => { CounterfeitPathVMs.Remove(SelectedCounterfeitPath); });
                    //});
                }
            }, c => SelectedCounterfeitPath is not null);
        }
    }

    #endregion
}