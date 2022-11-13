using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;


namespace VKR.ViewModel;

internal class CounterfeitEditWindowVM : ViewModelBase

{
#region Functions

#region Constructors

    public CounterfeitEditWindowVM(Counterfeit tempCounterfeit)
    {
        TempCounterfeit = new Counterfeit
        {
            Id = tempCounterfeit.Id,
            Name = tempCounterfeit.Name,
            ShapeId = tempCounterfeit.ShapeId,
            BotLineSize = tempCounterfeit.BotLineSize,
            UpLineSize = tempCounterfeit.UpLineSize,
            ColorId = tempCounterfeit.ColorId,
        };

        EditingCounterfeit = tempCounterfeit;
        Db = new CounterfeitKBContext();
        Colors = Db.Colors.Local.ToObservableCollection();
        Shapes = Db.Shapes.Local.ToObservableCollection();
    }

#endregion

#endregion


#region Properties

    public ObservableCollection<Color> Colors { get; set; }
    public ObservableCollection<Shape> Shapes { get; set; }
    public ObservableCollection<Counterfeit> Counterfeits { get; set; }
    public Counterfeit TempCounterfeit { get; set; }
    public Counterfeit EditingCounterfeit { get; set; }

    private CounterfeitKBContext Db { get; }

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

                if (!Db.Counterfeits.Contains(EditingCounterfeit))
                {
                    Db.Counterfeits.Add(EditingCounterfeit);
                }

                Db.SaveChanges();

                //OnClosingRequest();
            });
        }
    }

#endregion
}
