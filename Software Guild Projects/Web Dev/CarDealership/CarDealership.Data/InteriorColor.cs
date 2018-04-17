namespace CarDealership.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InteriorColor")]
    public partial class InteriorColor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InteriorColor()
        {
            Cars = new HashSet<Car>();
        }

        [Key]
        public int IntColorId { get; set; }

        [Required]
        [StringLength(25)]
        public string IntColorName { get; set; }

        [Required]
        [StringLength(10)]
        public string IntColorType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }
    }
}
