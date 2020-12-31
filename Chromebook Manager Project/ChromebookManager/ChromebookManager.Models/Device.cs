using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class Device
    {
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public string StorageCapacity { get; set; }
        public string ModelNumber { get; set; }
        public string BrandName { get; set; }
        public int ModelId { get; set; }
        public int SchoolId { get; set; }
        public int ClientDeviceId { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public string CurrentOwner { get; set; }
        public bool Loaner { get; set; }
        public string SchoolName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => _price = Math.Round((value / 100), 2);
        }
    }
}
