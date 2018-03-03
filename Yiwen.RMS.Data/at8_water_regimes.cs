namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class at8_water_regimes
    {
        public at8_water_regimes()
        {
            this.create_at = DateTime.Now.Date;
        }
        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? create_at { get; set; }

        [StringLength(50)]
        public string project { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [StringLength(50)]
        public string item { get; set; }

        public decimal? value { get; set; }

        [Column(TypeName = "text")]
        public string remark { get; set; }
    }
}
