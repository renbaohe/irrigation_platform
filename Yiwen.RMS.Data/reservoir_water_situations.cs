namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class reservoir_water_situations
    {
        public reservoir_water_situations()
        {
            this.create_at = DateTime.Now.Date;
        }
        public int id { get; set; }

        [StringLength(50)]
        public string reservoir { get; set; }

        [StringLength(50)]
        public string subject { get; set; }

        public decimal? at8 { get; set; }

        public decimal? at20 { get; set; }

        [StringLength(50)]
        public string statis { get; set; }

        public decimal? data { get; set; }

        [Column(TypeName = "date")]
        public DateTime? create_at { get; set; }
    }
}
