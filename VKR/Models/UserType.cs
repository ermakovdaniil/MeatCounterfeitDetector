using System.Collections.ObjectModel;

using PropertyChanged;


namespace VKR.Models
{
    [AddINotifyPropertyChangedInterface]
    public class UserType
    {
        public UserType()
        {
            Users = new ObservableCollection<User>();
        }

        public long UserTypeId { get; set; }
        public string UserTypeName { get; set; }

        public virtual ObservableCollection<User> Users { get; set; }
    }
}
