using CarDealership.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealership.UI.Models.MyViewModels
{
    public class HomePageVM
    {
        public List<Special> Specials { get; set; }
        public List<Car> FeaturedCars { get; set; }
    }
}