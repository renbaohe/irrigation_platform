namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class water_logs
    {
        public water_logs()
        {
            this.create_at = DateTime.Now.Date;
        }
        public int id { get; set; }

        [StringLength(50)]
        public string site { get; set; }

        [StringLength(50)]
        public string observ_station { get; set; }

        [StringLength(50)]
        public string observ_item { get; set; }

        public decimal? at2 { get; set; }

        public decimal? at4 { get; set; }

        public decimal? at6 { get; set; }

        public decimal? at8 { get; set; }

        public decimal? at10 { get; set; }

        public decimal? at12 { get; set; }

        public decimal? at14 { get; set; }

        public decimal? at16 { get; set; }

        public decimal? at18 { get; set; }

        public decimal? at20 { get; set; }

        public decimal? at22 { get; set; }

        public decimal? at24 { get; set; }

        [StringLength(50)]
        public string statis_item { get; set; }

        public decimal? daily_water { get; set; }

        public decimal? process_water { get; set; }

        [Column(TypeName = "date")]
        public DateTime? create_at { get; set; }
    }
}
