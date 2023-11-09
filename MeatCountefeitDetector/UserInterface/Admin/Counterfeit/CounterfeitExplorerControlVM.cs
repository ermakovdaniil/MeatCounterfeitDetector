using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ClientAPI;
using ClientAPI.DTO.Counterfeit;
using Mapster;
using System.Collections.ObjectModel;
using MeatCounterfeitDetector.UserInterface.EntityVM;

namespace MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

public class CounterfeitExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public CounterfeitExplorerControlVM(CounterfeitClient counterfeitClient,
                                        DialogService dialogService,
                                        IMessageBoxService messageBoxService)
    {
        _counterfeitClient = counterfeitClient;
        _messageBoxService = messageBoxService;
        _dialogService = dialogService;

        _counterfeitClient.CounterfeitGetAsync()
                          .ContinueWith(c => { CounterfeitVMs = c.Result.ToList().Adapt<ObservableCollection<CounterfeitVM>>(); });
    }

    #endregion

    #endregion


    #region Properties

    private readonly CounterfeitClient _counterfeitClient;
    private readonly IMessageBoxService _messageBoxService;
    private readonly DialogService _dialogService;

    public ObservableCollection<CounterfeitVM> CounterfeitVMs { get; set; }
    public CounterfeitVM SelectedCounterfeit { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addCounterfeit;
    public RelayCommand AddCounterfeit
    {
        get
        {
            return _addCounterfeit ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{
                var result = (await _dialogService.ShowDialog<CounterfeitEditControl>(new WindowParameters
                {
                    Height = 180,
                    Width = 300,
                    Title = "Добавление фальсификата",
                },
                data: new CounterfeitVM())) as CounterfeitVM;

                if (result is null)
                {
                    return;
                }

                if (!CounterfeitVMs.Any(rec => rec.Name == result.Name))
                {
                    var addingCounterfeitCreateDTO = result.Adapt<CreateCounterfeitDTO>();
                    var id = await _counterfeitClient.CounterfeitPostAsync(addingCounterfeitCreateDTO);
                    result.Id = id;
                    CounterfeitVMs.Add(result);

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

    private RelayCommand _editCounterfeitObject;
    public RelayCommand EditCounterfeit
    {
        get
        {
            return _editCounterfeitObject ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{
                var result = (await _dialogService.ShowDialog<CounterfeitEditControl>(new WindowParameters
                {
                    Height = 180,
                    Width = 300,
                    Title = "Добавление фальсификата",
                },
                data: SelectedCounterfeit)) as CounterfeitVM;

                if (result is null)
                {
                    return;
                }

                if (!CounterfeitVMs.Any(rec => rec.Name == result.Name))
                {
                    var editingCounterfeitUpdateDTO = result.Adapt<UpdateCounterfeitDTO>();
                    await _counterfeitClient.CounterfeitPutAsync(editingCounterfeitUpdateDTO);
                    result.Adapt(CounterfeitVMs.FirstOrDefault(x => x.Id == result.Id));

                    _messageBoxService.ShowMessage($"Объект успешно обновлён!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _messageBoxService.ShowMessage($"Такая запись уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //});
            }, _ => SelectedCounterfeit is not null);
        }
    }

    private RelayCommand _deleteCounterfeit;
    public RelayCommand DeleteCounterfeit
    {
        get
        {
            return _deleteCounterfeit ??= new RelayCommand(async o =>
            {
                if (_messageBoxService.ShowMessage($"Вы действительно хотите удалить фальсификат: \"{SelectedCounterfeit.Name}\"?", "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //var analysisResultDTO = AnalysisResult.Adapt<CreateResultDTO>();
                    //Application.Current.Dispatcher.Invoke(async () =>
                    //{
                    await _counterfeitClient.CounterfeitDeleteAsync(SelectedCounterfeit.Id);
                    CounterfeitVMs.Remove(SelectedCounterfeit);
                    //});
                }
            }, c => SelectedCounterfeit is not null);
        }
    }

    #endregion
}

