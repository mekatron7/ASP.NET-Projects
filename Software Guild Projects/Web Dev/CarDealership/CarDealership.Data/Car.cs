namespace CarDealership.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Car")]
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            Contacts = new HashSet<Contact>();
            Purchases = new HashSet<Purchase>();
        }

        [Key]
        [Column("VIN#")]
        [StringLength(17)]
        public string VIN_ { get; set; }

        public int ModelId { get; set; }

        public int ExtColorId { get; set; }

        public int IntColorId { get; set; }

        public int BSId { get; set; }

        public int TransmissionId { get; set; }

        [Required]
        [StringLength(5)]
        public string CarType { get; set; }

        [Required]
        [StringLength(1)]
        public string Purchased { get; set; }

        [Required]
        [StringLength(1)]
        public string Featured { get; set; }

        public int CarYear { get; set; }

        public int Mileage { get; set; }

        public decimal MSRP { get; set; }

        public decimal SalePrice { get; set; }

        [Required]
        [StringLength(1000)]
        public string CarDescription { get; set; }

        [StringLength(200)]
        public string CarPicture { get; set; }

        public virtual BodyStyle BodyStyle { get; set; }

        public virtual ExteriorColor ExteriorColor { get; set; }

        public virtual InteriorColor InteriorColor { get; set; }

        public virtual Model Model { get; set; }

        public virtual Transmission Transmission { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
