using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class Color
    {
        public Color()
        {
            Counterfeits = new ObservableCollection<Counterfeit>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string BotLine { get; set; } = null!;
        public string UpLine { get; set; } = null!;

        public virtual ObservableCollection<Counterfeit> Counterfeits { get; set; }
    }
}
