using DataAccess.Data;

using VKR.Utils;
using VKR.View;


namespace VKR.ViewModel;

public class LoginControlVM : ControlViewModel
{
    private readonly UserDBContext _context;
    private readonly MainWindowVM _mainModel;


#region Functions

#region Constructors

    public LoginControlVM(MainWindowVM mainModel, UserDBContext context, IUserControlFactory fac) : base(fac, mainModel)
    {
        _context = context;
        _mainModel = mainModel;
    }

#endregion

#endregion


#region Properties

#endregion


#region Commands

    private RelayCommand _openColorProperty;

    public RelayCommand OpenColorProperty
    {
        get
        {
            return _openColorProperty ??= new RelayCommand(o =>
            {
                changeControl<ColorPropertyControl>(null);
            });
        }
    }

#endregion
}
