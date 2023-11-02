using DataAccess.Data;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using ClientAPI;
using ClientAPI.DTO.Counterfeit;
using Mapster;
using System.Threading.Tasks;

namespace MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

public class CounterfeitEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public CounterfeitEditControlVM(CounterfeitClient counterfeitClient,)
    {
        _counterfeitClient = counterfeitClient;
        Task.Run(async () =>
        {
            Counterfeits = (await _counterfeitClient.CounterfeitGetAsync()).ToList();
        });
    }

    #endregion

    #endregion

    private readonly CounterfeitClient _counterfeitClient;

    private object _data;

    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempCounterfeit = new DataAccess.Models.Counterfeit
            {
                Id = EditingCounterfeit.Id,
                Name = EditingCounterfeit.Name,
            };

            OnPropertyChanged(nameof(TempCounterfeit));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


    #region Properties

    public List<GetCounterfeitDTO> Counterfeits { get; set; }

    public DataAccess.Models.Counterfeit TempCounterfeit { get; set; }
    public DataAccess.Models.Counterfeit EditingCounterfeit => (DataAccess.Models.Counterfeit)Data;

    #endregion


    #region Commands

    private RelayCommand _saveCounterfeit;

    /// <summary>
    ///     Команда сохраняющая изменение данных о фальсификате в базе данных
    /// </summary>
    public RelayCommand SaveCounterfeit
    {
        get
        {
            return _saveCounterfeit ??= new RelayCommand(o =>
            {
                EditingCounterfeit.Id = TempCounterfeit.Id;
                EditingCounterfeit.Name = TempCounterfeit.Name;

                var editingCounterfeitGetDTO = EditingCounterfeit.Adapt<GetCounterfeitDTO>();

                if (!Counterfeits.Contains(editingCounterfeitGetDTO))
                {
                    //_context.Counterfeits.Add(EditingCounterfeit);               
                    var editingCounterfeitCreateDTO = EditingCounterfeit.Adapt<CreateCounterfeitDTO>();
                    _counterfeitClient.CounterfeitPostAsync(editingCounterfeitCreateDTO);
                }

                //_context.SaveChanges();
                FinishInteraction();
            });
        }
    }

    private RelayCommand _closeCommand;

    public RelayCommand CloseCommand
    {
        get
        {
            return _closeCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }

    #endregion
}

