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
        private List<string> _duplicateCheck = new List<string>();
        private static LinkFinderVM _savedInfo = new LinkFinderVM();

        // GET: Home
        public ActionResult Index()
        {
            return View(_savedInfo);
        }

        [HttpPost]
        public ActionResult Index(LinkFinderVM model)
        {
            LinkInfo link = new LinkInfo { LinkContent = model.URL, LinkAddress = model.URL };
            GetLinksFromWebsite(link, model, model.NumOfLevels);
            model.LinksTotal = model.Links.Count;
            _savedInfo = model;
            _duplicateCheck.Clear();

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
                List<LinkInfo> childLinks = (model.KeepLinkStyles) ? GetLinksWithTag(model, link) : GetLinksNoTag(model, link);

                //model.Links.AddRange(realLinks);
                //link.ParentLink = url;
                link.ChildLinks = new List<LinkInfo>();
                link.ChildLinks.AddRange(childLinks);
                if (link.ParentLink?.LinkAddress == model.URL) model.Links.Add(link);

                if (numOfLevels > 0)
                {
                    foreach (var child in childLinks)
                    {
                        GetLinksFromWebsite(child, model, numOfLevels);
                    }
                }
            }
        }

        public List<LinkInfo> GetLinksNoTag(LinkFinderVM model, LinkInfo parent)
        {
            List<LinkInfo> realLinks = new List<LinkInfo>();
            string[] links = model.Content.Split(new string[] { "href=\"", "href=\'", "url?q=" }, StringSplitOptions.RemoveEmptyEntries);
            int linkNum = 0;
            foreach (var link in links)
            {
                linkNum++;
                if (linkNum < model.StartAtLinkNum && model.StartAtLinkNum < links.Length) continue;

                if (realLinks.Count < model.NumOfLinks || model.AllLinksFromFirstPage)
                {
                    if (!(link.StartsWith("http") || link.StartsWith("//www"))) continue;
                    int index = link.IndexOf('"');
                    if (index == -1) index = link.IndexOf('\'');
                    if (index != -1)
                    {
                        string address = link.Substring(0, index);
                        if (model.NoDuplicateSites)
                        {
                            if (!_duplicateCheck.Contains(address))
                            {
                                _duplicateCheck.Add(address);
                                realLinks.Add(new LinkInfo { LinkContent = address, LinkAddress = address, ParentLink = parent });
                            }
                        }
                        else
                        {
                            realLinks.Add(new LinkInfo { LinkContent = address, LinkAddress = address, ParentLink = parent });
                        }
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

        public List<LinkInfo> GetLinksWithTag(LinkFinderVM model, LinkInfo parent)
        {
            List<LinkInfo> realLinks = new List<LinkInfo>();
            string[] links = model.Content.Split(new string[] { "<a" }, StringSplitOptions.RemoveEmptyEntries);
            int linkNum = 0;
            foreach(var link in links)
            {
                linkNum++;
                if (linkNum < model.StartAtLinkNum && model.StartAtLinkNum < links.Length) continue;

                if(realLinks.Count < model.NumOfLinks || model.AllLinksFromFirstPage)
                {
                    if (!link.Contains("href=\"")) continue;
                    int index = link.LastIndexOf("</a>");
                    if(index != -1)
                    {
                        string possibleLink = link.Substring(0, index + 4);
                        possibleLink = "<a" + possibleLink;
                        if (!(possibleLink.Contains("http") || possibleLink.Contains("www.")))
                        {
                            int insertIndex = possibleLink.IndexOf("href=\"") + 6;

                            int domainLength = parent.LinkAddress.IndexOf('/', 8); //Start after the http(s)://
                            if (domainLength == -1) domainLength = parent.LinkAddress.Length;
                            if (domainLength != parent.LinkAddress.Length)
                            {
                                string domain = parent.LinkAddress.Substring(0, domainLength);
                                possibleLink = possibleLink.Insert(insertIndex, domain);
                            }
                            else
                            {
                                possibleLink = possibleLink.Insert(insertIndex, parent.LinkAddress);
                            }
                        }
                        if (model.NoDuplicateSites)
                        {
                            if (!_duplicateCheck.Contains(possibleLink))
                            {
                                _duplicateCheck.Add(possibleLink);
                                realLinks.Add(new LinkInfo { LinkContent = possibleLink, ParentLink = parent, LinkAddress = GetAddressFromTag(possibleLink) });
                            }
                        }
                        else
                        {
                            realLinks.Add(new LinkInfo { LinkContent = possibleLink, ParentLink = parent, LinkAddress = GetAddressFromTag(possibleLink) });
                        }

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
            if (index == -1) index = address.IndexOf('\'');
            address = address.Substring(0, index);

            return address;
        }
    }
}