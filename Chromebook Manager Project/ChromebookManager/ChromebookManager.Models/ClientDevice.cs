using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookManager.Models
{
    public class ClientDevice
    {
        public int ClientDeviceId { get; set; }
        public Client Client { get; set; }
        public School School { get; set; }
        public Device Device { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
