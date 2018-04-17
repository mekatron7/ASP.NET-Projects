namespace CarDealership.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public int ContactId { get; set; }

        public DateTime ContactDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactName { get; set; }

        [StringLength(50)]
        public string ContactEmail { get; set; }

        [StringLength(12)]
        public string ContactPhone { get; set; }

        [Required]
        [StringLength(1000)]
        public string ContactMessage { get; set; }

        [Column("VIN#")]
        [StringLength(17)]
        public string VIN_ { get; set; }

        public virtual Car Car { get; set; }
    }
}
