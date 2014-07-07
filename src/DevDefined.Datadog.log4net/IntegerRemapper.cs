using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DevDefined.Datadog.log4net
{
	public class IntegerRemapper
	{
		static readonly Regex NUMBERREGEX = new Regex("(\\d+)", RegexOptions.Compiled);
		readonly Dictionary<string, string> numbermap = new Dictionary<string, string>();
		int nextNumber = 0;

		public string RemapNumbers(string input)
		{
			if (input == null) return null;
			string result = NUMBERREGEX.Replace(input, match => NextNumber(match.Value));
			return result;
		}

		string NextNumber(string value)
		{
			if (!numbermap.ContainsKey(value))
			{
				numbermap[value] = nextNumber.ToString();
				nextNumber++;
			}	
				
			return numbermap[value];
		}
	}
}