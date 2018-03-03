namespace Yiwen.RMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class users
    {
        public users()
        {
            this.create_at = DateTime.Now;
        }
        public int id { get; set; }

        [StringLength(20)]
        public string uname { get; set; }

        [StringLength(100)]
        public string password { get; set; }

        [StringLength(20)]
        public string mobile { get; set; }

        public int? role { get; set; }

        public DateTime? create_at { get; set; }

        public int? create_user { get; set; }
    }
}
