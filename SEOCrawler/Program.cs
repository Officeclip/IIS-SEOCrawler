using System;

namespace SEOCrawler
{
    class Program
    {
        static void Main()
        {
            // Run the analysis 
            var crawler = new Crawler();
            var report = crawler.RunAnalysis();

            if (report == null)
                throw new ApplicationException("Error Creating Crawler Report");

            // Log the analysis summary
            var logCrawlerReport = new CreateReport();
            logCrawlerReport.CreateXmlLogSummary(report);
        }
    }
}
