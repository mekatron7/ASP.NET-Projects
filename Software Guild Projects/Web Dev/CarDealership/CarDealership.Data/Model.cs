namespace CarDealership.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Model")]
    public partial class Model
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Model()
        {
            Cars = new HashSet<Car>();
        }

        public int ModelId { get; set; }

        [Required]
        [StringLength(25)]
        public string ModelName { get; set; }

        [Required]
        [StringLength(10)]
        public string ModelEdition { get; set; }

        public int MakeId { get; set; }

        public string ModelUser { get; set; }

        public DateTime DateAdded { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }

        public virtual Make Make { get; set; }
    }
}
