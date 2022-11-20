using System;
using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.Dialog.Abstract;


namespace VKR.ViewModel;

public class CounterfeitEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public CounterfeitEditControlVM(CounterfeitKBContext context)
    {
        _context = context;
        Colors = new ObservableCollection<Color>(_context.Colors.ToList());
        Shapes = new ObservableCollection<Shape>(_context.Shapes.ToList());
        Counterfeits = new ObservableCollection<Counterfeit>(_context.Counterfeits.ToList());
    }

    #endregion

    #endregion


    #region Properties

    private Counterfeit _tempCounterfeit;

    public Counterfeit TempCounterfeit
    {
        get
        {
            return _tempCounterfeit;
        }
        set
        {
            _tempCounterfeit = value;
            OnPropertyChanged();
        }
    }

    public Counterfeit EditingCounterfeit => (Counterfeit)Data;

    private readonly CounterfeitKBContext _context;

    public ObservableCollection<Color> Colors { get; set; }
    public ObservableCollection<Shape> Shapes { get; set; }
    public ObservableCollection<Counterfeit> Counterfeits { get; set; }

    #endregion


    #region Commands

    private RelayCommand _saveCounterfeit;

    /// <summary>
    ///     Команда сохраняющая изменение данных о цвете в базе данных
    /// </summary>
    public RelayCommand SaveCounterfeit
    {
        get
        {
            return _saveCounterfeit ??= new RelayCommand(o =>
            {
                EditingCounterfeit.Id = TempCounterfeit.Id;
                EditingCounterfeit.Name = TempCounterfeit.Name;
                EditingCounterfeit.ShapeId = TempCounterfeit.ShapeId;
                EditingCounterfeit.BotLineSize = TempCounterfeit.BotLineSize;
                EditingCounterfeit.UpLineSize = TempCounterfeit.UpLineSize;
                EditingCounterfeit.ColorId = TempCounterfeit.ColorId;

                if (!_context.Counterfeits.Contains(EditingCounterfeit))
                {
                    _context.Counterfeits.Add(EditingCounterfeit);
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

    private object _data;
    public object Data
    {
        get => _data;
        set
        {
            _data = value;
            TempCounterfeit = new Counterfeit()
            {
                Id = EditingCounterfeit.Id,
                Name = EditingCounterfeit.Name,
                ShapeId = EditingCounterfeit.ShapeId,
                BotLineSize = EditingCounterfeit.BotLineSize,
                UpLineSize = EditingCounterfeit.UpLineSize,
                ColorId = EditingCounterfeit.ColorId,
            };
            OnPropertyChanged(nameof(TempCounterfeit));
        }
    }

    public object? Result { get; }
    public Action FinishInteraction { get; set; }
}
