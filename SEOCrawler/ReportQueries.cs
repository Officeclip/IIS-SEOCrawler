using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Web.Management.SEO.Crawler;

namespace SEOCrawler
{
    public class ReportQueries
    {
        public IEnumerable<IGrouping<HttpStatusCode, UrlInfo>> GetUrlsByStatusCode(CrawlerReport report)
        {
            return from url in report.GetUrls()
                   group url by url.StatusCode
                       into g
                       orderby g.Key
                       select g;
        }

        public IEnumerable<UrlInfo> GetBrokenLinks(CrawlerReport report)
        {
            return from url in report.GetUrls()
                   where url.StatusCode == HttpStatusCode.NotFound && !url.IsExternal
                   orderby url.Url.AbsoluteUri ascending
                   select url;
        }
    }
}
