namespace SmartPick.Data.CbTote
{
    public class Selection
    {
        public int Id { get; set; }
        public int LegId { get; set; }
        public int Bin { get; set; }
        public decimal Probability { get; set; }
        public decimal? PlaceProbability { get; set; }
        //public virtual Leg Leg { get; set; }
    }
}
