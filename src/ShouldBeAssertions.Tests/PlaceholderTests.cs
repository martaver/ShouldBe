using System;
using NUnit.Framework;
using ShouldBeAssertions.Tools;

namespace ShouldBeAssertions.Tests
{
	[TestFixture(TypeArgs= new [] {typeof(byte)})]
	[TestFixture(TypeArgs= new [] {typeof(int)})]
	[TestFixture(TypeArgs= new [] {typeof(short)})]
	[TestFixture(TypeArgs= new [] {typeof(long)})]
	[TestFixture(TypeArgs= new [] {typeof(float)})]
	[TestFixture(TypeArgs= new [] {typeof(double)})]
	[TestFixture(TypeArgs= new [] {typeof(decimal)})]
	[TestFixture(TypeArgs= new [] {typeof(DateTime)})]
	[TestFixture(TypeArgs= new [] {typeof(DateTimeOffset)})]
	[TestFixture(TypeArgs= new [] {typeof(TimeSpan)})]
	[TestFixture(TypeArgs= new [] {typeof(char)})]
	[TestFixture(TypeArgs= new [] {typeof(string)})]
	[TestFixture(TypeArgs = new[] { typeof(byte?) })]
	[TestFixture(TypeArgs = new[] { typeof(int?) })]
	[TestFixture(TypeArgs = new[] { typeof(short?) })]
	[TestFixture(TypeArgs = new[] { typeof(long?) })]
	[TestFixture(TypeArgs = new[] { typeof(float?) })]
	[TestFixture(TypeArgs = new[] { typeof(double?) })]
	[TestFixture(TypeArgs = new[] { typeof(decimal?) })]
	[TestFixture(TypeArgs = new[] { typeof(DateTime?) })]
	[TestFixture(TypeArgs = new[] { typeof(DateTimeOffset?) })]
	[TestFixture(TypeArgs = new[] { typeof(TimeSpan?) })]
	[TestFixture(TypeArgs = new[] { typeof(char?) })]	
	public class PlaceholderTests<T>
    {
	    [Test]		
	    public void CreateReturnsDifferentValuesEachTime()
	    {
		    var first = Placeholder.Create<T>();
		    var second = Placeholder.Create<T>();
		    Assert.That(first, Is.Not.EqualTo(second));
	    }
    }
}
