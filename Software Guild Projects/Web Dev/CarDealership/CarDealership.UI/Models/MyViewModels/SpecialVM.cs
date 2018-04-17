using CarDealership.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.MyViewModels
{
    public class SpecialVM
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

        public HttpPostedFileBase SpecialJTronImage { get; set; }

        public string SpecialPicture { get; set; }

        public List<Special> AllSpecials { get; set; }
    }
}