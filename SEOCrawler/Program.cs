using System;

namespace SEOCrawler
{
    class Program
    {
        static void Main()
        {
            // Run the analysis 
            var crawler = new RunCrawler();
            var report = crawler.AnalyseSite();

            if (report == null)
                throw new ApplicationException("Error Creating Crawler Report");

            // Log the analysis summary
            var logCrawlerReport = new CreateReportOutput();
            logCrawlerReport.CreateXmlLogSummary(report);
        }
    }
}
