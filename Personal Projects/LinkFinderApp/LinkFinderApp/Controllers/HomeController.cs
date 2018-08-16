using LinkFinderApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LinkFinderApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            LinkFinderVM model = new LinkFinderVM();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LinkFinderVM model)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    model.Content = client.DownloadString(model.URL);
                }
                catch (Exception ex)
                {
                    model.ErrorMessage = $"{ex.Source}: {ex.Message}";
                }
            }

            if (string.IsNullOrEmpty(model.ErrorMessage))
            {
                string[] links = model.Content.Split(new string[] { "href=\"", "url?q=" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var link in links)
                {
                    if (link.StartsWith("<")) continue;
                    int index = link.IndexOf('"');
                    if (!link.StartsWith("/")) model.Links.Add(link.Substring(0, index));
                }

                model.NumOfLinks = model.Links.Count();
            }


            return View(model);
        }
    }
}