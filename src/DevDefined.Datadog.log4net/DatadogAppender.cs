using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Hosting;
using log4net.Appender;
using log4net.Core;

namespace DevDefined.Datadog.log4net
{
	public class DatadogAppender : AppenderSkeleton
    {
        readonly HttpClient client = new HttpClient();
        readonly JsonMediaTypeFormatter formatter;

        public DatadogAppender()
        {
            client.BaseAddress = new Uri("https://app.datadoghq.com/api/");
            formatter = new JsonMediaTypeFormatter();
        }

        public string ApiKey { get; set; }
        public string Tags { get; set; }

        IEnumerable<string> EnvironmentalTags
        {
            get
            {
                if (HostingEnvironment.ApplicationHost != null)
                {
                    string siteName = HostingEnvironment.ApplicationHost.GetSiteName();
                    if (siteName != null)
                    {
                        yield return "iis_site_" + siteName.Replace(" ", "_").ToLower();
                    }
                }

                if (Environment.MachineName != null)
                {
                    yield return "host_" + Environment.MachineName.Replace(" ", "_").ToLower();
                }
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (string.IsNullOrEmpty(ApiKey)) return;

            List<string> tags = (Tags ?? "").Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList();

            tags = tags.Concat(EnvironmentalTags).OrderBy(x => x).Distinct().ToList();

            string text = RenderLoggingEvent(loggingEvent);

            string title = BuildTitle(loggingEvent, text);

            string priority = (loggingEvent.Level >= Level.Error) ? "normal" : "low";

            string alertType = GetAlertType(loggingEvent);

	        string aggregateKey = new AggregationKeyCalculator().Calculate(title, loggingEvent.LoggerName);

            var datadogEvent = new DatadogEvent
            {
                Tags = tags,
                AlertyType = alertType,
                Priority = priority,
                Text = text,
                Title = title,
				AggregationKey = aggregateKey
            };

            PostEvent(datadogEvent).Wait();
        }

        string BuildTitle(LoggingEvent loggingEvent, string renderedText)
        {
            string title = null;

            if (loggingEvent.MessageObject != null)
            {
                title = loggingEvent.MessageObject.ToString();
            }
            else if (loggingEvent.ExceptionObject != null)
            {
                title = loggingEvent.ExceptionObject.Message;
            }
            else
            {
                title = renderedText;
            }

            if (title.Length > 100) title = title.Substring(0, 100);

            return title;
        }

        string GetAlertType(LoggingEvent loggingEvent)
        {
            if (loggingEvent.Level == Level.Debug || loggingEvent.Level == Level.Info)
            {
                return "info";
            }
            if (loggingEvent.Level == Level.Warn)
            {
                return "warn";
            }
            if (loggingEvent.ExceptionObject == null)
            {
                return "success";
            }

            return "error";
        }

        async Task PostEvent(DatadogEvent datadogEvent)
        {
            var content = new ObjectContent(typeof (DatadogEvent), datadogEvent, formatter, "application/json");
            HttpResponseMessage response = await client.PostAsync(string.Format("v1/events?api_key={0}", ApiKey), content);
            response.EnsureSuccessStatusCode();
        }
    }
}