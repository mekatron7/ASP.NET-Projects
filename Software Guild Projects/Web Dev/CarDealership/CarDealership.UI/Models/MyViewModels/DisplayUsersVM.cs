using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealership.UI.Models.MyViewModels
{
    public class DisplayUsersVM
    {
        public List<UserVM> AllUsers { get; set; }
        public List<SelectListItem> AllRoles { get; set; }
        public string SelectedRole { get; set; }
    }
}