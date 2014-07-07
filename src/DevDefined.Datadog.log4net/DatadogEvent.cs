using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevDefined.Datadog.log4net
{
	public class DatadogEvent
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("priority")]
		public string Priority { get; set; }

		[JsonProperty("tags")]
		public List<string> Tags { get; set; }

		[JsonProperty("alert_type")]
		public string AlertyType { get; set; }

		[JsonProperty("aggregation_key")]
		public string AggregationKey { get; set; }
	}
}