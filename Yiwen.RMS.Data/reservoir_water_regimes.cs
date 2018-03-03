namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class reservoir_water_regimes
    {
        public reservoir_water_regimes()
        {
            this.create_at = DateTime.Now.Date;
        }
        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? create_at { get; set; }

        [StringLength(50)]
        public string item { get; set; }

        public decimal? water_level { get; set; }

        public decimal? water_storage { get; set; }

        [StringLength(50)]
        public string dispatch { get; set; }

        [StringLength(50)]
        public string flood { get; set; }

        public decimal? normal_level { get; set; }
    }
}
