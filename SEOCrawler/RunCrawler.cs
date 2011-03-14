using System;
using System.Configuration;
using System.IO;
using System.Threading;
using Microsoft.Web.Management.SEO.Crawler;

namespace SEOCrawler
{
    public class RunCrawler
    {
        public CrawlerReport AnalyseSite()
        {
            var startUrl = new Uri(GetSiteToCrawl());
            var crawlerSettings = BuildCrawlerSettings(startUrl);

            var crawler = new WebCrawler(crawlerSettings);
            try
            {
                crawler.Start();
                while (crawler.IsRunning)
                {
                    Thread.Sleep(500);
                }

                crawler.Report.Save(BuildReportPath());
            }
            catch (Exception ex)
            {
                //logging needs added here
                throw new ApplicationException(string.Format("Error Crawling {0}", startUrl));
            }

            return crawler.Report;
        }

        private static CrawlerSettings BuildCrawlerSettings(Uri startUrl)
        {
            var seoReportPath = BuildReportPath();
            var reportName = BuildReportName(startUrl);

            return new CrawlerSettings(startUrl)
                       {
                           ExternalLinkCriteria = ExternalLinkCriteria.SameFolderAndDeeper,
                           Name = reportName,
                           DirectoryCache = Path.Combine(seoReportPath, reportName)
                       };
        }

        private static string BuildReportName(Uri startUrl)
        {
            return startUrl.Host + " " + DateTime.Now.ToString("yy-MM-dd hh-mm-ss");
        }

        private static string BuildReportPath()
        {
            //this is the default path used by IIS UI when i creates a report.
            //saving it to this location as well means you can view the report in the UI

            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "IIS SEO Reports");
        }

        private static string GetSiteToCrawl()
        {
            var siteToScan = ConfigurationManager.AppSettings.Get("siteName");

            if (string.IsNullOrWhiteSpace(siteToScan))
                throw new NullReferenceException("Please specify a site URL.");

            return siteToScan;
        }
    }
}
