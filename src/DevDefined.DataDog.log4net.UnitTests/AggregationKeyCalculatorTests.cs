using DevDefined.Datadog.log4net;
using PowerAssert;
using Xunit;

namespace DevDefined.DataDog.log4net.UnitTests
{
	public class AggregationKeyCalculatorTests
	{
		readonly AggregationKeyCalculator _calculator = new AggregationKeyCalculator();

		[Fact]
		public void RestrictsLengthTo100Characters()
		{
			string correlation = _calculator.Calculate("This is a long message This is a long message This is a long message This is a long message This is a long message This is a long message This is a long message This is a long message This is a long message This is a long message", "Sample.Logger");
			PAssert.IsTrue(() => correlation.Length == 100);
		}

		[Fact]
		public void WorksOnShortStrings()
		{
			string correlation = _calculator.Calculate("This is a short message", "Sample.Logger");

			PAssert.IsTrue(() => correlation == "This is a short message8aK+NN32Jg3N0qNk5wu4CBvFTZTe099QP4YEJ+XXcSM=");
		}
	}
}