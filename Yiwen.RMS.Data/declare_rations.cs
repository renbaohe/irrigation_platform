namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class declare_rations
    {
        public declare_rations()
        {
            this.create_at = DateTime.Now;
        }
        public int id { get; set; }

 
        public DateTime? create_at { get; set; }

        public int? create_user { get; set; }

        [NotMapped()]
        public string create_username { get; set; }

        public int? status { get; set; }

        public DateTime? ration_at { get; set; }

        public DateTime? report_at { get; set; }

        [NotMapped()]
        public string datas { get; set; }
    }
}
