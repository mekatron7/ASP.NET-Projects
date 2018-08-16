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
            LinkInfo link = new LinkInfo { LinkContent = model.URL, LinkAddress = model.URL };
            GetLinksFromWebsite(link, model, model.NumOfLevels);
            model.LinksTotal = model.Links.Count;

            return View(model);
        }

        public void GetLinksFromWebsite(LinkInfo link, LinkFinderVM model, int numOfLevels)
        {
            string url = link.LinkAddress;
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
                List<LinkInfo> childLinks = (model.KeepLinkStyles) ? GetLinksWithTag(model, url) : GetLinksNoTag(model, url);

                //model.Links.AddRange(realLinks);
                //link.ParentLink = url;
                link.ChildLinks = new List<LinkInfo>();
                link.ChildLinks.AddRange(childLinks);
                if (link.ParentLink == model.URL) model.Links.Add(link);

                if (numOfLevels > 0)
                {
                    foreach (var child in childLinks)
                    {
                        GetLinksFromWebsite(child, model, numOfLevels);
                    }
                }
            }
        }

        public List<LinkInfo> GetLinksNoTag(LinkFinderVM model, string url)
        {
            List<LinkInfo> realLinks = new List<LinkInfo>();
            string[] links = model.Content.Split(new string[] { "href=\"", "url?q=" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var link in links)
            {
                if (realLinks.Count < model.NumOfLinks || model.AllLinksFromFirstPage)
                {
                    if (!(link.StartsWith("http") || link.StartsWith("//www"))) continue;
                    int index = link.IndexOf('"');
                    if (index != -1)
                    {
                        LinkInfo newLink = new LinkInfo();
                        newLink.LinkContent = link.Substring(0, index);
                        newLink.LinkAddress = newLink.LinkContent;
                        newLink.ParentLink = url;
                        realLinks.Add(newLink);
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

        public List<LinkInfo> GetLinksWithTag(LinkFinderVM model, string url)
        {
            List<LinkInfo> realLinks = new List<LinkInfo>();
            string[] links = model.Content.Split(new string[] { "<a" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var link in links)
            {
                if(realLinks.Count < model.NumOfLinks || model.AllLinksFromFirstPage)
                {
                    if (!link.Contains("href=\"")) continue;
                    int index = link.LastIndexOf("</a>");
                    if(index != -1)
                    {
                        string possibleLink = link.Substring(0, index + 4);
                        possibleLink = "<a" + possibleLink;
                        if (!possibleLink.Contains("http"))
                        {
                            int insertIndex = possibleLink.IndexOf("href=\"") + 6;

                            int domainLength = url.IndexOf('/', 8); //Start after the http(s)://
                            if (domainLength != url.Length)
                            {
                                string domain = url.Substring(0, domainLength);
                                possibleLink = possibleLink.Insert(insertIndex, domain);
                            }
                            else
                            {
                                possibleLink = possibleLink.Insert(insertIndex, url);
                            }
                        }

                        realLinks.Add(new LinkInfo { LinkContent = possibleLink, ParentLink = url, LinkAddress = GetAddressFromTag(possibleLink) });
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