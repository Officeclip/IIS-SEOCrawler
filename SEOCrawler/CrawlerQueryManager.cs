using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Web.Management.SEO.Crawler;

namespace SEOCrawler
{
    public class CrawlerQueryManager
    {
        private readonly CrawlerReport _report;

        public CrawlerQueryManager(CrawlerReport report)
        {
            _report = report;
        }

        public IEnumerable<IGrouping<HttpStatusCode, UrlInfo>> GetUrlsByStatusCode()
        {
            return from url in _report.GetUrls()
                   group url by url.StatusCode
                       into g
                       orderby g.Key
                       select g;
        }

        public IEnumerable<UrlInfo> GetBrokenLinks()
        {
            return from url in _report.GetUrls()
                   where url.StatusCode == HttpStatusCode.NotFound && !url.IsExternal
                   orderby url.Url.AbsoluteUri ascending
                   select url;
        }
    }
}
