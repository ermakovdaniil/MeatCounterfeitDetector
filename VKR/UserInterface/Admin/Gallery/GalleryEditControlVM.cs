using System;
using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.Dialog.Abstract;


namespace VKR.UserInterface.Admin.Gallery;

public class GalleryEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private object _data;


#region Functions

#region Constructors

    public GalleryEditControlVM(CounterfeitKBContext context)
    {
        _context = context;
        Counterfeits = new ObservableCollection<DataAccess.Models.Counterfeit>(_context.Counterfeits.ToList());
    }

#endregion

#endregion


    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempCounterfeitPath = new CounterfeitPath
            {
                Id = EditingCounterfeitPath.Id,
                Counterfeit = EditingCounterfeitPath.Counterfeit,
                ImagePath = EditingCounterfeitPath.ImagePath,
            };

            OnPropertyChanged(nameof(TempCounterfeitPath));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


#region Properties

    private CounterfeitPath _tempCounterfeitPath;

    public CounterfeitPath TempCounterfeitPath
    {
        get => _tempCounterfeitPath;
        set
        {
            _tempCounterfeitPath = value;
            OnPropertyChanged();
        }
    }

    public CounterfeitPath EditingCounterfeitPath => (CounterfeitPath) Data;

    private readonly CounterfeitKBContext _context;

    public ObservableCollection<DataAccess.Models.Counterfeit> Counterfeits { get; set; }

#endregion


#region Commands

    private RelayCommand _saveCounterfeitPath;

    /// <summary>
    ///     Команда сохраняющая изменение данных о пути к изображению фальсификата в базе данных
    /// </summary>
    public RelayCommand SaveCounterfeitPath
    {
        get
        {
            return _saveCounterfeitPath ??= new RelayCommand(o =>
            {
                EditingCounterfeitPath.Id = TempCounterfeitPath.Id;
                EditingCounterfeitPath.Counterfeit = TempCounterfeitPath.Counterfeit;
                EditingCounterfeitPath.ImagePath = TempCounterfeitPath.ImagePath;

                if (!_context.CounterfeitPaths.Contains(EditingCounterfeitPath))
                {
                    _context.CounterfeitPaths.Add(EditingCounterfeitPath);
                }

                _context.SaveChanges();
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

