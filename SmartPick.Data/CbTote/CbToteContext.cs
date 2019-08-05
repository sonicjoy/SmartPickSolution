using Microsoft.EntityFrameworkCore;

namespace SmartPick.Data.CbTote
{
    public class CbToteContext : DbContext
    {
        public CbToteContext(DbContextOptions<CbToteContext> options) : base(options) { }

        public virtual DbSet<Pool> Pools { get; set; }
        public virtual DbSet<SmartPickPerm> SmartPickPerms { get; set; }
        public virtual DbSet<Leg> Legs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pool>().ToTable("pools", schema: "cbtote");
            modelBuilder.Entity<Pool>().Property(e => e.Id).HasColumnName("id");
            modelBuilder.Entity<Pool>().Property(e => e.SportCode).HasColumnName("sport_code");
            modelBuilder.Entity<Pool>().Property(e => e.TypeCode).HasColumnName("type_code");
            modelBuilder.Entity<Pool>().Property(e => e.LegNum).HasColumnName("leg_num");
            modelBuilder.Entity<Pool>().Property(e => e.HeadlinePrize).HasColumnName("headline_prize");
            modelBuilder.Entity<Pool>().Property(e => e.SmartPickTypeCode).HasColumnName("smart_pick_type_code");
            modelBuilder.Entity<Pool>().Property(e => e.Status).HasColumnName("status");

            modelBuilder.Entity<Leg>().ToTable("legs", "cbtote");
            modelBuilder.Entity<Leg>().Property(e => e.Id).HasColumnName("id");
            modelBuilder.Entity<Leg>().Property(e => e.LegOrder).HasColumnName("leg_order");
            modelBuilder.Entity<Leg>().Property(e => e.PoolId).HasColumnName("pool_id");

            modelBuilder.Entity<Selection>().ToTable("selections", "cbtote");
            modelBuilder.Entity<Selection>().Property(e => e.Id).HasColumnName("id");
            modelBuilder.Entity<Selection>().Property(e => e.LegId).HasColumnName("leg_id");
            modelBuilder.Entity<Selection>().Property(e => e.Bin).HasColumnName("bin");
            modelBuilder.Entity<Selection>().Property(e => e.Probability).HasColumnName("probability");
            modelBuilder.Entity<Selection>().Property(e => e.PlaceProbability).HasColumnName("place_probability");

            modelBuilder.Entity<SmartPickPerm>().ToTable("smart_pick_perms", "cbtote");
            modelBuilder.Entity<SmartPickPerm>().HasKey(e => new {e.TypeCode, e.NumberOfPerms});
            modelBuilder.Entity<SmartPickPerm>().Property(e => e.TypeCode).HasColumnName("type_code");
            modelBuilder.Entity<SmartPickPerm>().Property(e => e.NumberOfPerms).HasColumnName("number_of_perms");
            modelBuilder.Entity<SmartPickPerm>().Property(e => e.Pattern).HasColumnName("selections");
        }
    }
}
