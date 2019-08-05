using System.Collections.Generic;

namespace SmartPick.Data.CbTote
{
    public class Pool
    {
        public int Id { get; set; }
        public string SportCode { get; set; }
        public string TypeCode { get; set; }
        public int LegNum { get; set; }
        public string Status { get; set; }
        public string SmartPickTypeCode { get; set; }
        public decimal HeadlinePrize { get; set; }
        public virtual ICollection<Leg> Legs { get; set; }
    }
}
