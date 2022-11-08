using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class OriginalPath
    {
        public OriginalPath()
        {
            Results = new ObservableCollection<Result>();
        }

        public long Id { get; set; }
        public string Path { get; set; } = null!;

        public virtual ObservableCollection<Result> Results { get; set; }
    }
}
