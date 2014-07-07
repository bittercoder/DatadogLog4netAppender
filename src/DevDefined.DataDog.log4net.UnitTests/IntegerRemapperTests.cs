using DevDefined.Datadog.log4net;
using PowerAssert;
using Xunit;

namespace DevDefined.DataDog.log4net.UnitTests
{
	public class IntegerRemapperTests
	{
		readonly IntegerRemapper _mapper = new IntegerRemapper();

		[Fact]
		public void RemapNoNumbers_IsEquivalentString()
		{
			string text = "hello world";
			PAssert.IsTrue(() => text == _mapper.RemapNumbers(text));
		}

		[Fact]
		public void RemapSomeNumbers_ReturnsStringWithSameNumbersReplacedWithSameSentinels()
		{
			PAssert.IsTrue(() => "abc #0 #1 #0 xyz" == _mapper.RemapNumbers("abc #123 #456 #123 xyz"));
		}
	}
}