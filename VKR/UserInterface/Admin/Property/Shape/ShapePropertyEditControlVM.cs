using System;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.Dialog.Abstract;

namespace VKR.ViewModel;

public class ShapePropertyEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware

{
    #region Functions

    #region Constructors

    public ShapePropertyEditControlVM(CounterfeitKBContext context)
    {
        _context = context;
    }

    #endregion

    #endregion


    #region Properties

    private Shape _tempShape;

    public Shape TempShape
    {
        get
        {
            return _tempShape;
        }
        set
        {
            _tempShape = value;
            OnPropertyChanged();
        }
    }

    public Shape EditingShape => (Shape)Data;

    private readonly CounterfeitKBContext _context;

    #endregion


    #region Commands

    private RelayCommand _saveShape;

    /// <summary>
    ///     Команда сохраняющая изменение данных о форме в базе данных
    /// </summary>
    public RelayCommand SaveShape
    {
        get
        {
            return _saveShape ??= new RelayCommand(o =>
            {
                EditingShape.Id = TempShape.Id;
                EditingShape.Name = TempShape.Name;
                EditingShape.Formula = TempShape.Formula;

                if (!_context.Shapes.Contains(EditingShape))
                {
                    _context.Shapes.Add(EditingShape);
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
            TempShape = new Shape()
            {
                Id = EditingShape.Id,
                Name = EditingShape.Name,
                Formula = EditingShape.Formula,
            };
            OnPropertyChanged(nameof(TempShape));
        }
    }

    public object? Result { get; }
    public Action FinishInteraction { get; set; }
}
