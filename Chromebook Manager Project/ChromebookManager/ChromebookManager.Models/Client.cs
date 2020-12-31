using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public int StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int Grade { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime CSDateAdded { get; set; }
        public string CSAddedBy { get; set; }
        public string SchoolName { get; set; }
        public string CurrentSchool { get; set; }
        public int SchoolNumber { get; set; }
        public int SchoolId { get; set; }
        public bool DoesNotExist { get; set; }
        public bool NotEnrolled { get; set; }
        public List<School> Schools { get; set; }
        public string Barcode { get; set; }
    }
}
