namespace DataAccess.Models
{
    public partial class Result
    {
        public Guid Id { get; set; }
        public string Date { get; set; } = null!;
        public Guid UserId { get; set; }
        public string AnRes { get; set; } = null!;
        public double Time { get; set; }
        public double PercentOfSimilarity { get; set; }
        public Guid OrigPathId { get; set; }
        public Guid? ResPathId { get; set; }

        public virtual OriginalPath OrigPath { get; set; } = null!;
        public virtual ResultPath? ResPath { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
