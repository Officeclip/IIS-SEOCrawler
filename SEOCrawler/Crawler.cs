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
            var siteToScan = GetSiteToCrawl();

            // Create a URI class 
            var startUrl = new Uri(siteToScan);

            // Generate a unique name 
            var settings = new CrawlerSettings(startUrl)
            {
                ExternalLinkCriteria = ExternalLinkCriteria.SameFolderAndDeeper,
                Name = startUrl.Host + " " + DateTime.Now.ToString("yy-MM-dd hh-mm-ss")
            };
            

            // Use the same directory as the default used by the UI 
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "IIS SEO Reports");

            settings.DirectoryCache = Path.Combine(path, settings.Name);

            // Create a new crawler and start running 
            var crawler = new WebCrawler(settings);
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
                crawler.Report.Save(path);

                Console.WriteLine("Crawling complete!!!");
                return crawler.Report;
            }
            catch (Exception ex)
            {
                //logging needs added here
                throw new ApplicationException(string.Format("Error Crawling {0}", siteToScan));
            }
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
