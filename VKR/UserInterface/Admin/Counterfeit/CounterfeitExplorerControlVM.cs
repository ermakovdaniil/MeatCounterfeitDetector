using System.Collections.ObjectModel;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;

using MessageBox = HandyControl.Controls.MessageBox;

namespace VKR.ViewModel;

public class CounterfeitExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public CounterfeitExplorerControlVM(CounterfeitKBContext context)
    {
        _context = context;
        Counterfeits = _context.Counterfeits.Local.ToObservableCollection();
    }

    #endregion

    #endregion


    #region Properties

    private readonly CounterfeitKBContext _context;
    public Counterfeit SelectedCounterfeit { get; set; }
    public ObservableCollection<Counterfeit> Counterfeits { get; set; }

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
                //ShowChildWindow(new CounterfeitEditWindow(new Counterfeit()));
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
            return _editCounterfeitObject ??= new RelayCommand(o =>
            {
                //ShowChildWindow(new ColorPropertyEditWindow(SelectedCounterfeit));
            }, _ => SelectedCounterfeit != null);
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
            return _deleteCounterfeit ??= new RelayCommand(o =>
            {
                if (MessageBox.Show($"Вы действительно хотите удалить фальсификат: \"{SelectedCounterfeit.Name}\"?",
                                    "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Counterfeits.Remove(SelectedCounterfeit);
                    _context.SaveChanges();
                }
            }, c => SelectedCounterfeit != null);
        }
    }

    #endregion
}
