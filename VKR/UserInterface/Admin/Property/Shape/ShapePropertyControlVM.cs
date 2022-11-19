using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using Microsoft.EntityFrameworkCore;

using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel;

public class ShapePropertyControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ShapePropertyControlVM(CounterfeitKBContext context, DialogService ds)
    {
        _context = context;
        Shapes = _context.Shapes.Local.ToObservableCollection();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private DialogService _ds;

    private readonly CounterfeitKBContext _context;
    public Shape SelectedShape { get; set; }
    public ObservableCollection<Shape> Shapes { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addShape;

    /// <summary>
    ///     Команда, открывающая окно создания формы
    /// </summary>
    public RelayCommand AddShape
    {
        get
        {
            return _addShape ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<ShapePropertyControl>(
                windowParameters: new WindowParameters
                {
                    Height = 400,
                    Width = 300,
                    Title = "Добавление формы"
                },
                data: new Shape
                {

                });
            });
        }
    }

    private RelayCommand _editShape;

    /// <summary>
    ///     Команда, открывающая окно редактирования формы
    /// </summary>
    public RelayCommand EditShape
    {
        get
        {
            return _editShape ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<ShapePropertyControl>(
                windowParameters: new WindowParameters
                {
                    Height = 400,
                    Width = 300,
                    Title = "Добавление формы"
                },
                data: SelectedShape);
            }, _ => SelectedShape != null);
        }
    }

    private RelayCommand _deleteShape;

    /// <summary>
    ///     Команда, удаляющая форму
    /// </summary>
    public RelayCommand DeleteShape
    {
        get
        {
            return _deleteShape ??= new RelayCommand(o =>
            {
                if (MessageBox.Show($"Вы действительно хотите удалить форму: \"{SelectedShape.Name}\" и все фальсификаты связанные с ней?" +
                                    $"\nСвязанные фальсфикаты:\n{string.Join("\n", _context.Counterfeits.Where(c => c.ShapeId == SelectedShape.Id).Include(c => c.Shape).Select(c => c.Name))}",
                                    "Удаление формы", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    _context.Shapes.Remove(SelectedShape);
                    _context.SaveChanges();
                }
            }, _ => SelectedShape != null);
        }
    }

    #endregion
}
