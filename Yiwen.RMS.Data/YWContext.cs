namespace Yiwen.RMS.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class YWContext :System.Data.Entity.EFContext
    {
        public YWContext()
            : base("name=YWContext")
        {
        }

        public virtual DbSet<at8_water_regimes> at8_water_regimes { get; set; }
        public virtual DbSet<declare_ration_details> declare_ration_details { get; set; }
        public virtual DbSet<declare_rations> declare_rations { get; set; }
        public virtual DbSet<reservoir_water_regimes> reservoir_water_regimes { get; set; }
        public virtual DbSet<reservoir_water_situations> reservoir_water_situations { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<water_logs> water_logs { get; set; }
        public virtual DbSet<water_regime_temps> water_regime_temps { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<at8_water_regimes>()
                .Property(e => e.remark)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.uname)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.mobile)
                .IsUnicode(false);
        }
    }
}
