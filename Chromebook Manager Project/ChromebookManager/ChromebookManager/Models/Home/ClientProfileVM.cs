using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChromebookManager.Models.Home
{
    public class ClientProfileVM
    {
        public Client ClientInfo { get; set; }
        public List<RepairLog> RepairLogs { get; set; }
        public List<PartUsed> PartsUsed { get; set; }
        public List<Device> Devices { get; set; }
    }
}