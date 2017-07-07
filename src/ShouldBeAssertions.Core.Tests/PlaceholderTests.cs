using System;
using ShouldBeAssertions.Core.Tools;
using Shouldly;
using Xunit;

namespace ShouldBeAssertions.Core.Tests
{		
	public class PlaceholderTests
    {
	    public class SomeObject
	    {
		    public SomeObject(int someParam, string someOtherParam)
		    {
			    
		    }
	    }
	    
	    [Fact]		
	    public void CreateReturnsDifferentValuesEachTime()
	    {
		    this.TestDifferentReturnValues<byte>();
		    this.TestDifferentReturnValues<int>();
		    this.TestDifferentReturnValues<short>();
		    this.TestDifferentReturnValues<long>();
		    this.TestDifferentReturnValues<float>();
		    this.TestDifferentReturnValues<double>();
		    this.TestDifferentReturnValues<decimal>();
		    this.TestDifferentReturnValues<DateTime>();
		    this.TestDifferentReturnValues<DateTimeOffset>();
		    this.TestDifferentReturnValues<TimeSpan>();
		    this.TestDifferentReturnValues<char>();
		    this.TestDifferentReturnValues<string>();
		    this.TestDifferentReturnValues<byte?>();
		    this.TestDifferentReturnValues<int?>();
		    this.TestDifferentReturnValues<short?>();
		    this.TestDifferentReturnValues<long?>();
		    this.TestDifferentReturnValues<float?>();
		    this.TestDifferentReturnValues<double?>();
		    this.TestDifferentReturnValues<decimal?>();
		    this.TestDifferentReturnValues<DateTime?>();
		    this.TestDifferentReturnValues<DateTimeOffset?>();
		    this.TestDifferentReturnValues<TimeSpan?>();
		    this.TestDifferentReturnValues<char?>();
		    this.TestDifferentReturnValues<SomeObject>();
	    }

	    public void TestDifferentReturnValues<T>()
	    {
		    var first = Placeholder.Create<T>();
		    var second = Placeholder.Create<T>();
		    first.ShouldNotBe(second);
	    }
    }
}
