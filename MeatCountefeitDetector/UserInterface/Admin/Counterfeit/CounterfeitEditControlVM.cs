using System;
using System.Collections.Generic;
using System.Linq;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using ClientAPI;
using Mapster;
using MeatCounterfeitDetector.UserInterface.EntityVM;

namespace MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

public class CounterfeitEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public CounterfeitEditControlVM()
    {

    }

    #endregion

    #endregion


    #region Properties

    private object _data;
    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempCounterfeit = new CounterfeitVM
            {
                Id = EditingCounterfeit.Id,
                Name = EditingCounterfeit.Name,
            };

            OnPropertyChanged(nameof(TempCounterfeit));
        }
    }

    public Action FinishInteraction { get; set; }
    public object? Result { get; set; }

    public CounterfeitVM TempCounterfeit { get; set; }
    public CounterfeitVM EditingCounterfeit => (CounterfeitVM)Data;

    #endregion


    #region Commands

    private RelayCommand _saveCounterfeit;
    public RelayCommand SaveCounterfeit
    {
        get
        {
            return _saveCounterfeit ??= new RelayCommand(o =>
            {
                EditingCounterfeit.Id = TempCounterfeit.Id;
                EditingCounterfeit.Name = TempCounterfeit.Name;
                Result = EditingCounterfeit;
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

