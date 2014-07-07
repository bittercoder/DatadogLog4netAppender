using System;
using System.Security.Cryptography;
using System.Text;

namespace DevDefined.Datadog.log4net
{
	public class AggregationKeyCalculator
	{
		public string Calculate(string message, string loggerName)
		{
			var guidRemapper = new GuidRemapper();
			var intRemapper = new IntegerRemapper();

			string composedMessage = string.Format("{0}{1}", loggerName, message);
			composedMessage = guidRemapper.RemampGuids(composedMessage);
			composedMessage = intRemapper.RemapNumbers(composedMessage);

			byte[] bytes = Encoding.Unicode.GetBytes(composedMessage);
			var hashstring = new SHA256Managed();
			byte[] hash = hashstring.ComputeHash(bytes);
			string hashContents = Convert.ToBase64String(hash);

			int maxCharacters = Math.Min(message.Length, 100 - hashContents.Length);

			return message.Substring(0,maxCharacters) + hashContents;
		}
	}
}