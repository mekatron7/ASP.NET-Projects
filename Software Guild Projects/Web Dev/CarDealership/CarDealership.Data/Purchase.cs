namespace CarDealership.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Purchase")]
    public partial class Purchase
    {
        public int PurchaseId { get; set; }

        [Column("VIN#")]
        [Required]
        [StringLength(17)]
        public string VIN_ { get; set; }

        [Required]
        [StringLength(50)]
        public string PurchaseName { get; set; }

        [StringLength(12)]
        public string PurchasePhone { get; set; }

        [StringLength(50)]
        public string PurchaseEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string Street1 { get; set; }

        [StringLength(50)]
        public string Street2 { get; set; }

        [Required]
        [StringLength(35)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string PState { get; set; }

        public int ZipCode { get; set; }

        public decimal PurchasePrice { get; set; }

        [Required]
        [StringLength(15)]
        public string PurchaseType { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string PurchaseUser { get; set; }

        public virtual Car Car { get; set; }
    }
}
