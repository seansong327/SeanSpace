using HtmlAgilityPack;
using SeanSpace.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;

namespace SeanSpace.Controllers
{
    public class SearchController : Controller
    {
        private string blogPlaceHolder = "79912D68-883E-431D-8087-384DCF01EDCD";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Search(string q, string start)
        {
            string finalUrl = string.Format(Const.SearchUrl, HttpUtility.UrlEncode(q), (int.Parse(start) - 1) * 10);

            string htmlScr = null;
            using (WebClient client = new WebClient())
            {
                htmlScr = client.DownloadString(finalUrl);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlScr);
            HtmlNode node = doc.GetElementbyId("ires");

            foreach (var aNode in node.Descendants("a"))
            {
                var href = aNode.GetAttributeValue("href", "");
                href = href.Replace("/url?q=", string.Empty);

                int endIndex = href.IndexOf("&amp;sa=U");
                if (endIndex >= 0)
                {
                    href = href.Remove(endIndex);
                }

                aNode.SetAttributeValue("href", href);
                aNode.SetAttributeValue("target", "_blank");
            }

            return node.InnerHtml;
        }

        [HttpGet]
        public ActionResult GetPage()
        {
            return View();
        }

        [HttpPost]
        public string GetPage(string url, string encoding)
        {
            Uri uri = new Uri(url);
            string htmlScr = null;
            using (WebResponse response = WebRequest.Create(uri).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(encoding)))
                    {
                        htmlScr = sr.ReadToEnd();
                    }
                }
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlScr);
            HtmlNode bodyNode = doc.DocumentNode.Descendants("body").First();
            IEnumerable<HtmlNode> links = doc.DocumentNode.Descendants("head").First().Descendants("link");

            // Append style.
            foreach (HtmlNode link in links)
            {
                if (link.GetAttributeValue("type", "") == "text/css")
                {
                    var href = link.GetAttributeValue("href", "");
                    if (!href.StartsWith("//") && href.StartsWith("/"))
                    {
                        href = uri.Scheme + "://" + uri.Host + href;
                    }

                    link.SetAttributeValue("href", href);

                    bodyNode.AppendChild(link);
                }
            }

            // Replace url to target.
            foreach (var aNode in bodyNode.Descendants("a"))
            {
                var href = aNode.GetAttributeValue("href", "");
                if (!href.StartsWith("//") && href.StartsWith("/"))
                {
                    href = uri.Scheme + "://" + uri.Host + href;
                }

                aNode.SetAttributeValue("href", href);
                aNode.SetAttributeValue("target", "_blank");
            }

            // Process picture
            foreach (var imageNode in bodyNode.Descendants("img"))
            {
                string src = imageNode.GetAttributeValue("src", "");

                if (!src.StartsWith("data:image"))
                {
                    if (src.StartsWith("//"))
                    {
                        src = src.TrimStart('/');
                        src = src.Remove(0, src.IndexOf('/'));
                        src = uri.Scheme + "://" + uri.Host + src;
                    }
                    else if (src.StartsWith("/"))
                    {
                        src = uri.Scheme + "://" + uri.Host + src;
                    }
                    src = src.Replace("blog", blogPlaceHolder);
                    src = "/search/getpic?url=" + src;
                }

                imageNode.SetAttributeValue("src", src);
            }

            return bodyNode.InnerHtml;
        }

        public ActionResult GetPic(string url)
        {
            Uri uri = new Uri(url.Replace(blogPlaceHolder, "blog"));
            if (uri.Host != Request.Url.Host)
            {
                byte[] byteDate = null;
                using (WebClient client = new WebClient())
                {
                    byteDate = client.DownloadData(uri);
                }

                string imageType = "jpg";
                string[] strs = url.Split('/');
                strs = strs[strs.Length - 1].Split('.');
                if (strs.Length > 1)
                {
                    imageType = strs[strs.Length - 1];
                }

                return File(byteDate, "image/" + imageType);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
