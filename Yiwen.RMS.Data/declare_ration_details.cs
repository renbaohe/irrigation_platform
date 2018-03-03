namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class declare_ration_details
    {
        public int id { get; set; }

        public int dr_id { get; set; }

        [StringLength(50)]
        public string site { get; set; }

        [StringLength(50)]
        public string subsite { get; set; }

        [StringLength(50)]
        public string channel { get; set; }

        public decimal? declare_water { get; set; }

        public decimal? ration_water { get; set; }

        public decimal? at8_water { get; set; }

        public decimal? yesterday_water { get; set; }
    }
}
