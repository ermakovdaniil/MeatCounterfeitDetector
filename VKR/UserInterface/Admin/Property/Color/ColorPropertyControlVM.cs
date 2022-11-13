using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel;

public class ColorPropertyControlVM : ViewModelBase
{
#region Functions

#region Constructors

    public ColorPropertyControlVM(CounterfeitKBContext context)
    {
        _context = context;
        Colors = _context.Colors.Local.ToObservableCollection();

        EditColor = new RelayCommand(obj =>
        {
        }, obj => false);
    }

#endregion

#endregion


#region Properties

    private readonly CounterfeitKBContext _context;
    public Color SelectedColor { get; set; }
    public ObservableCollection<Color> Colors { get; set; }

#endregion


#region Commands

    private RelayCommand _addColor;

    /// <summary>
    ///     Команда, открывающая окно создания цвета
    /// </summary>
    public RelayCommand AddColor
    {
        get
        {
            return _addColor ??= new RelayCommand(o =>
            {
                //ShowChildWindow(new ColorPropertyEditWindow(new Color()));
            });
        }
    }

    private RelayCommand _editColor;

    /// <summary>
    ///     Команда, открывающая окно редактирования цвета
    /// </summary>
    public RelayCommand EditColor { get; set; }

    //{
    //    get
    //    {
    //        return _editColor ??= new RelayCommand(o =>
    //        {
    //            ShowChildWindow(new ColorPropertyEditWindow(SelectedColor));
    //        }, _ => SelectedColor != null);
    //    }
    //}

    private RelayCommand _deleteColor;

    /// <summary>
    ///     Команда, удаляющая цвет
    /// </summary>
    public RelayCommand DeleteColor
    {
        get
        {
            return _deleteColor ??= new RelayCommand(o =>
            {
                if (MessageBox.Show($"Вы действительно хотите удалить цвет: \"{SelectedColor.Name}\" и все фальсификаты связанные с ним?" +
                                    $"\nСвязанные фальсфикаты:\n{string.Join("\n", _context.Counterfeits.Where(c => c.ColorId == SelectedColor.Id).Include(c => c.Color).Select(c => c.Name))}",
                                    "Удаление цвета", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    _context.Colors.Remove(SelectedColor);
                    _context.SaveChanges();
                }
            }, _ => SelectedColor != null);
        }
    }

#endregion
}
