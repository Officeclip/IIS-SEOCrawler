using System;
using System.Dynamic;
using System.Net;

namespace SEOCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args==null)
                throw new ApplicationException("args[0] relates to a site URL and should not be empty");

            // Run the analysis 
            dynamic site = new ExpandoObject();
            site.Url = args[0];

            var crawler = new RunCrawler();
            var report = crawler.AnalyseSite(site);

            if (report == null)
                throw new ApplicationException("Error Creating Crawler Report");

            // Log the analysis summary
            var logCrawlerReport = new CreateReportOutput();
            logCrawlerReport.CreateXmlLogSummary(site, report);
        }
    }
}
