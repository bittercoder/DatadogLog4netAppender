using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DevDefined.Datadog.log4net
{	
	public class GuidRemapper
	{
		static readonly Regex GUIDREGEX = new Regex("[\\da-fA-F]{8}\\-[\\da-fA-F]{4}-[\\da-fA-F]{4}-[\\da-fA-F]{4}-[\\da-fA-F]{12}", RegexOptions.Compiled);
		readonly Dictionary<string, string> guidmap = new Dictionary<string, string>();
		Guid nextguid = Guid.Empty;

		public string RemampGuids(string input)
		{
			if (input == null) return null;
			string result = GUIDREGEX.Replace(input, match => NextGuid(match.Value));
			return result;
		}

		string NextGuid(string value)
		{
			if (guidmap.ContainsKey(value))
				return guidmap[value];

			byte[] x = nextguid.ToByteArray();
			x[0] = (byte) (x[0] + 1);

			nextguid = new Guid(x);

			guidmap[value] = nextguid.ToString();

			return guidmap[value];
		}
	}
}