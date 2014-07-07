using DevDefined.Datadog.log4net;
using PowerAssert;
using Xunit;

namespace DevDefined.DataDog.log4net.UnitTests
{
	public class GuidRemapperTests
	{
		readonly GuidRemapper _mapper = new GuidRemapper();

		[Fact]
		public void RemapNoGuids_IsEquivalentString()
		{
			string text = "hello world";
			PAssert.IsTrue(() => text == _mapper.RemampGuids(text));
		}

		[Fact]
		public void RemapSomeGuids_WhenGuidsUppercase_ReturnsStringWithSameGuidsReplacedWithSameSentinels()
		{
			PAssert.IsTrue(() => "abc 00000001-0000-0000-0000-000000000000 00000002-0000-0000-0000-000000000000 00000001-0000-0000-0000-000000000000 xyz" == _mapper.RemampGuids("abc 47E26F75-9BC2-4996-9D37-8A981766FDC2 9BCD406E-2CEA-49DF-9EDC-38EB0009DC9F 47E26F75-9BC2-4996-9D37-8A981766FDC2 xyz"));			
		}

		[Fact]
		public void RemapSomeGuids_WhenGuidsLowercase_ReturnsStringWithSameGuidsReplacedWithSameSentinels()
		{
			PAssert.IsTrue(() => "abc 00000001-0000-0000-0000-000000000000 00000002-0000-0000-0000-000000000000 00000001-0000-0000-0000-000000000000 xyz" == _mapper.RemampGuids("abc 47e26f75-9bc2-4996-9d37-8a981766fdc2 9bcd406e-2cea-49df-9edc-38eb0009dc9f 47e26f75-9bc2-4996-9d37-8a981766fdc2 xyz"));
		}
	}
}