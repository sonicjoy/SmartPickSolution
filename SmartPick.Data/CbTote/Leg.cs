using System.Collections.Generic;

namespace SmartPick.Data.CbTote
{
    public class Leg
    {
        public int Id { get; set; }
        public int LegOrder { get; set; }
        public int PoolId { get; set; }
        public virtual ICollection<Selection> Selections { get; set; }
        //public virtual Pool Pool { get; set; }
    }
}
