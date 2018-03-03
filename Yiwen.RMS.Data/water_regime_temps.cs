namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class water_regime_temps
    {
        public int id { get; set; }

        [StringLength(50)]
        public string project { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [StringLength(50)]
        public string item { get; set; }
    }
}
