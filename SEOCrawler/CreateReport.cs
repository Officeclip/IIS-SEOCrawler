using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Web.Management.SEO.Crawler;

namespace SEOCrawler
{
    public class CreateReport
    {
        private CrawlerQueryManager _queryBuilder;

        public string LogSummary(CrawlerReport report)
        {
            var logSummary = new StringBuilder();

            Console.WriteLine("----------------------------");
            Console.WriteLine("Overview");
            Console.WriteLine("----------------------------");
            LogToConsoleAndStringBuilder(logSummary, string.Format("Start URL:  {0}", report.Settings.StartUrl));
            LogToConsoleAndStringBuilder(logSummary, string.Format("Start Time: {0}", report.Settings.StartTime));
            LogToConsoleAndStringBuilder(logSummary, string.Format("End Time:   {0}", report.Settings.EndTime));
            LogToConsoleAndStringBuilder(logSummary, string.Format("URLs:       {0}", report.GetUrlCount()));
            LogToConsoleAndStringBuilder(logSummary, string.Format("Links:      {0}", report.Settings.LinkCount));
            LogToConsoleAndStringBuilder(logSummary, string.Format("Violations: {0}", report.Settings.ViolationCount));

            return logSummary.ToString();
        }

        public string LogBrokenLinks(CrawlerReport report)
        {
            var brokenLinksSummary = new StringBuilder();

            Console.WriteLine("----------------------------");
            Console.WriteLine("Broken Links");
            Console.WriteLine("----------------------------");

            _queryBuilder = new CrawlerQueryManager(report);
            var urls = _queryBuilder.GetBrokenLinks();
            foreach (var item in urls)
            {
                LogToConsoleAndStringBuilder(brokenLinksSummary, item.Url.AbsoluteUri);
            }

            return brokenLinksSummary.ToString();
        }

        public string LogStatusCodeSummary(CrawlerReport report)
        {
            var statusCodeSummary = new StringBuilder();

            Console.WriteLine("----------------------------");
            Console.WriteLine("Status Code Summary");
            Console.WriteLine("----------------------------");

            _queryBuilder = new CrawlerQueryManager(report);
            var statusCodeUrls = _queryBuilder.GetUrlsByStatusCode();
            foreach (var item in statusCodeUrls)
            {
                LogToConsoleAndStringBuilder(statusCodeSummary, string.Format("{0,20} - {1,5:N0}", item.Key, item.Count()));
            }

            return statusCodeSummary.ToString();
        }

        private static void LogToConsoleAndStringBuilder(StringBuilder summary, string message)
        {
            Console.WriteLine(message);
            summary.AppendLine("<p>" + message + "</p>");
        }

        public void CreateXmlLogSummary(CrawlerReport report)
        {
            var html = CreateHtmlString(report);

            var streamWriter = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["LogPath"], "SEOReport.html"));
            streamWriter.Write(html);
            streamWriter.Flush();
            streamWriter.Close();
        }

        private string CreateHtmlString(CrawlerReport report)
        {
            var html = new StringBuilder();
            html.Append("<html>");

            html.Append("<head>");
            html.Append("<title>");
            html.Append(string.Format("SEO Report for {0}", ConfigurationManager.AppSettings["siteName"]));
            html.Append("</title>");
            html.Append("</head>");

            html.Append("<body>");
            html.Append(string.Format("<p>Report of {0} created on {1}</p>", ConfigurationManager.AppSettings["siteName"], DateTime.Today.ToShortDateString()));

            html.Append("<h2>Summary</h2>");
            html.Append(LogSummary(report));
            html.Append("<br />");
            html.Append("<h2>Status Codes</h2>");
            html.Append(LogStatusCodeSummary(report));
            html.Append("<br />");
            html.Append("<h2>Broken Links</h2>");
            html.Append(LogBrokenLinks(report));
            html.Append("</body>");
            html.Append("</html>");

            return html.ToString();
        }
    }
}
