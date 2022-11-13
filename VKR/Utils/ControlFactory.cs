using System.Windows.Controls;

using Autofac;


public interface IUserControlFactory
{
    UserControl CreateUserControl<T>(object param) where T : UserControl;
}


public class UserControlFactory : IUserControlFactory
{
    private readonly IComponentContext container;

    public UserControlFactory(IComponentContext container)
    {
        this.container = container;
    }

    public UserControl CreateUserControl<T>(object param) where T : UserControl
    {
        return container.Resolve<T>();
    }
}
