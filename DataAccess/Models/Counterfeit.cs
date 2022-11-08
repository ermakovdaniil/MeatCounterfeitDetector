using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Counterfeit
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public long ShapeId { get; set; }
        public long BotLineSize { get; set; }
        public long UpLineSize { get; set; }
        public long ColorId { get; set; }

        public virtual Color Color { get; set; } = null!;
        public virtual Shape Shape { get; set; } = null!;
    }
}
