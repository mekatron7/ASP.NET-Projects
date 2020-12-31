using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChromebookManager.Models.Admin
{
    public class SysDataVM
    {
        public List<Brand> Brands { get; set; }
        public List<Model> Models { get; set; }
        public List<Part> Parts { get; set; }
        public List<ModelPart> ModelParts { get; set; }
        public List<IssueType> IssueTypes { get; set; }
        public Alert Alert { get; set; }
        public string ModelNumber { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }
        public int IssueId { get; set; }
        public int ModelPartId { get; set; }
        public decimal Cost { get; set; }
        public List<SelectListItem> BrandSelectList { get; set; }
        public List<SelectListItem> ModelSelectList { get; set; }
        public List<SelectListItem> PartSelectList { get; set; }
        public long PONumber { get; set; }
        public long SelectedPONumber { get; set; }
        public List<SelectListItem> PurchaseOrders { get; set; }
        public string Notes { get; set; }
        public int Qty { get; set; }
        public int MPCostId { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}