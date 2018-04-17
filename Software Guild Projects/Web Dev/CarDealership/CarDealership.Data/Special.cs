namespace CarDealership.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Special")]
    public partial class Special
    {
        public int SpecialId { get; set; }

        [Required]
        [StringLength(50)]
        public string SpecialName { get; set; }

        public DateTime SpecialStartDate { get; set; }

        public DateTime SpecialEndDate { get; set; }

        [Required]
        [StringLength(500)]
        public string SpecialDescription { get; set; }

        [StringLength(200)]
        public string SpecialJTronImage { get; set; }
    }
}
