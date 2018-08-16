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
            GetLinksFromWebsite(model.URL, model, model.NumOfLevels);
            model.LinksTotal = model.Links.Count;

            return View(model);
        }

        public void GetLinksFromWebsite(string url, LinkFinderVM model, int numOfLevels)
        {
            bool error = false;
            numOfLevels--;
            if (!url.StartsWith("http")) url = $"http:{url}";
            using (WebClient client = new WebClient())
            {
                try
                {
                    model.Content = client.DownloadString(url);
                }
                catch (Exception ex)
                {
                    model.ErrorMessages.Add($"{ex.Source}: {ex.Message} URL: {url}");
                    error = true;
                }
            }

            if (!error)
            {
                List<string> realLinks = (model.KeepLinkStyles) ? GetLinksWithTag(model) : GetLinksNoTag(model);
                
                model.Links.AddRange(realLinks);

                if (numOfLevels > 0)
                {
                    foreach (var realLink in realLinks)
                    {
                        if(model.KeepLinkStyles)
                        {
                            GetLinksFromWebsite(GetAddressFromTag(realLink), model, numOfLevels);
                        }
                        else
                        {
                            GetLinksFromWebsite(realLink, model, numOfLevels);
                        }
                    }
                }
            }
        }

        public List<string> GetLinksNoTag(LinkFinderVM model)
        {
            List<string> realLinks = new List<string>();
            string[] links = model.Content.Split(new string[] { "href=\"", "url?q=" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var link in links)
            {
                if (realLinks.Count < model.NumOfLinks || model.AllLinksFromFirstPage)
                {
                    if (!(link.StartsWith("http") || link.StartsWith("//www"))) continue;
                    int index = link.IndexOf('"');
                    if (index != -1) realLinks.Add(link.Substring(0, index));
                }
                else
                {
                    break;
                }
            }
            model.AllLinksFromFirstPage = false;

            return realLinks;
        }

        public List<string> GetLinksWithTag(LinkFinderVM model)
        {
            List<string> realLinks = new List<string>();
            string[] links = model.Content.Split(new string[] { "<a" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var link in links)
            {
                if(realLinks.Count < model.NumOfLinks || model.AllLinksFromFirstPage)
                {
                    if (!link.Contains("href=\"")) continue;
                    int index = link.LastIndexOf("</a>");
                    if(index != 1)
                    {
                        string possibleLink = link.Substring(0, index + 4);
                        possibleLink = "<a" + possibleLink;
                        if (possibleLink.Contains("http")) realLinks.Add(possibleLink);
                    }
                }
                else
                {
                    break;
                }
            }
            model.AllLinksFromFirstPage = false;

            return realLinks;
        }

        public string GetAddressFromTag(string tag)
        {
            int index = tag.IndexOf("href=\"") + 6;
            string address = tag.Substring(index);
            index = address.IndexOf('"');
            address = address.Substring(0, index);

            return address;
        }
    }
}