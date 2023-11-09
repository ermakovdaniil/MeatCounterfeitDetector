using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class ResultImage : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid? OriginalImageId { get; set; }
        public string EncodedImage { get; set; } = null!;

        public virtual OriginalImage? OriginalImage { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}