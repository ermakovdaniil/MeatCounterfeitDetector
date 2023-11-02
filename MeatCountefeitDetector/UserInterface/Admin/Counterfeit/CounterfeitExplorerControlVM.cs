using DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ClientAPI;
using System.Threading.Tasks;
using ClientAPI.DTO.Counterfeit;
using Mapster;

namespace MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

public class CounterfeitExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public CounterfeitExplorerControlVM(CounterfeitClient counterfeitClient,
                                        CounterfeitPathClient counterfeitPathClient,
                                        DialogService dialogService,
                                        IMessageBoxService messageBoxService)
    {
        _counterfeitClient = counterfeitClient;
        _counterfeitPathClient = counterfeitPathClient;
        _messageBoxService = messageBoxService;
        _dialogService = dialogService;
        _counterfeitClient.CounterfeitGetAsync()
                          .ContinueWith(c => { Counterfeits = c.Result.ToList(); });
    }

    #endregion

    #endregion


    #region Properties

    private readonly DialogService _dialogService;
    private readonly CounterfeitClient _counterfeitClient;
    private readonly CounterfeitPathClient _counterfeitPathClient;
    private readonly IMessageBoxService _messageBoxService;
    public DataAccess.Models.Counterfeit SelectedCounterfeit { get; set; }

    public List<GetCounterfeitDTO> Counterfeits { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addCounterfeit;

    /// <summary>
    ///     Команда, открывающая окно создания нового фальсификата
    /// </summary>
    public RelayCommand AddCounterfeit
    {
        get
        {
            return _addCounterfeit ??= new RelayCommand(o =>
            {
                _dialogService.ShowDialog<CounterfeitEditControl>(new WindowParameters
                {
                    Height = 180,
                    Width = 300,
                    Title = "Добавление фальсификата",
                },
                data: new DataAccess.Models.Counterfeit());

                OnPropertyChanged(nameof(Counterfeits));
            });
        }
    }

    private RelayCommand _editCounterfeitObject;

    /// <summary>
    ///     Команда, открывающая окно редактирования нового фальсификата
    /// </summary>
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
                data: SelectedCounterfeit)) as DataAccess.Models.Counterfeit;

                if (result is null)
                {
                    return;
                }

                //var editingCounterfeitGetDTO = result.Adapt<GetCounterfeitDTO>();

                //if (!Counterfeits.Contains(editingCounterfeitGetDTO))
                //{
                //    //_context.Counterfeits.Add(EditingCounterfeit);               
                var editingCounterfeitCreateDTO = result.Adapt<CreateCounterfeitDTO>();
                await _counterfeitClient.CounterfeitPostAsync(editingCounterfeitCreateDTO);
                _messageBoxService.ShowMessage($"Объект добавлен!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //}

            //_context.SaveChanges();


            //});
            }, _ => SelectedCounterfeit is not null);
        }
    }

    private RelayCommand _deleteCounterfeit;

    /// <summary>
    ///     Команда, удаляющая фальсификат
    /// </summary>
    public RelayCommand DeleteCounterfeit
    {
        get
        {
            return _deleteCounterfeit ??= new RelayCommand(async o =>
            {
                if (_messageBoxService.ShowMessage($"Вы действительно хотите удалить фальсификат: \"{SelectedCounterfeit.Name}\"?", "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //_context.Counterfeits.Remove(SelectedCounterfeit);
                    //_context.SaveChanges();

                    //var analysisResultDTO = AnalysisResult.Adapt<CreateResultDTO>();


                    //Application.Current.Dispatcher.Invoke(async () =>
                    //{
                    await _counterfeitClient.CounterfeitDeleteAsync(SelectedCounterfeit.Id);
                    //});
                }
            }, c => SelectedCounterfeit is not null);
        }
    }

    #endregion
}

