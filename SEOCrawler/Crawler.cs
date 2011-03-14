using System;
using System.Configuration;
using System.IO;
using System.Threading;
using Microsoft.Web.Management.SEO.Crawler;

namespace SEOCrawler
{
    public class Crawler
    {
        public CrawlerReport RunAnalysis()
        {
            var startUrl = new Uri(GetSiteToCrawl());

            var seoReportPath = BuildReportPath();
            var reportName = BuildReportName(startUrl);
            var crawlerSettings = BuildCrawlerSettings(startUrl, reportName, seoReportPath);

            // Create a new crawler and start running 
            var crawler = new WebCrawler(crawlerSettings);
            try
            {
                crawler.Start();

                Console.WriteLine("Processed - Remaining - Download Size");
                while (crawler.IsRunning)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("{0,9:N0} - {1,9:N0} - {2,9:N2} MB",
                        crawler.Report.GetUrlCount(),
                        crawler.RemainingUrls,
                        crawler.BytesDownloaded / 1048576.0f);
                }

                // Save the report 
                crawler.Report.Save(seoReportPath);

                Console.WriteLine("Crawling complete!!!");
                return crawler.Report;
            }
            catch (Exception ex)
            {
                //logging needs added here
                throw new ApplicationException(string.Format("Error Crawling {0}", startUrl));
            }
        }

        private static CrawlerSettings BuildCrawlerSettings(Uri startUrl, string reportName, string seoReportPath)
        {
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
