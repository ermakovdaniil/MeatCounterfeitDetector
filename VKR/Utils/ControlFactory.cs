using System.Windows.Controls;

using Autofac;
using Autofac.Core;


public interface IUserControlFactory
{
    UserControl CreateUserControl<T>(object param)where T: UserControl;
}

public class UserControlFactory : IUserControlFactory
{
    private IComponentContext container;

    public UserControlFactory(IComponentContext  container)
    {
        this.container = container;
    }

    public UserControl CreateUserControl<T>(object param) where T : UserControl
    {
        return container.Resolve<T>();
    }
}
