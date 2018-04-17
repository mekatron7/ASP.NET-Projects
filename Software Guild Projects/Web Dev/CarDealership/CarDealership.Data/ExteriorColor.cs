namespace CarDealership.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExteriorColor")]
    public partial class ExteriorColor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExteriorColor()
        {
            Cars = new HashSet<Car>();
        }

        [Key]
        public int ExtColorId { get; set; }

        [Required]
        [StringLength(25)]
        public string ExtColorName { get; set; }

        [Required]
        [StringLength(10)]
        public string ExtColorType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }
    }
}
