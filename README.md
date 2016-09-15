# ShouldBe
Beautiful object graph assertions for C#.

Granularity is important when testing in order to maximize coverage and provide precise feedback. But the cost of being precise is a often a lot of syntactical overhead that affects readability and productivity.

ShouldBe greatly enhances testing assertions' **readability** and **productivity**.

## Availability

ShouldBe is [available on NuGet](https://www.nuget.org/packages/ShouldBe/) as `ShouldBe`. Install it from NuGet Package Manager Console with:

    install-package ShouldBe
    
<img src="https://ci.appveyor.com/api/projects/status/32r7s2skrgm9ubva?svg=true" />

## Readability benefits
ShouldBe is a structured approach to constraint assertion and eliminates tests that look like this (e.g. in NUnit):

    var actual = OrderProvider.GetOrder();			
    Assert.That(actual.Id, Is.GreaterThan(0));
    Assert.That(actual.CustomerName, Is.EqualTo("The Great Coffee Sensationalist"));
    Assert.That(actual.Items, Is.Not.Null);
    Assert.That(actual.Items.Count, Is.EqualTo(2));
    Assert.That(actual.Items[0].Id, Is.GreaterThan(0));
    Assert.That(actual.Items[0].OrderId, Is.EqualTo(actual.Id));
    Assert.That(actual.Items[0].Name, Is.EqualTo("Donut"));
    Assert.That(actual.Items[0].Quantity, Is.EqualTo(7));
    Assert.That(actual.Items[1].Id, Is.GreaterThan(0));
    Assert.That(actual.Items[1].OrderId, Is.EqualTo(actual.Id));
    Assert.That(actual.Items[1].Name, Is.EqualTo("Crunch Muffin"));
    Assert.That(actual.Items[1].Quantity, Is.EqualTo(3));

And makes them look like this:

	OrderProvider.GetOrder().ShouldLookLike(new Order
	{
		Id = ShouldBe.GreaterThan(0).NameThis("OrderId"),
		CustomerName = "The Great Coffee Sensationalist",
		Items = new List<Item>
		{
			new Item
			{
				Id = ShouldBe.GreaterThan(0),
				OrderId = ShouldBe.SameAs<int>("OrderId"),
				Name = "Donut",
				Quantity = 7
			},
			new Item
			{
				Id = ShouldBe.GreaterThan(0),
				OrderId = ShouldBe.SameAs<int>("OrderId"),
				Name = "Crunch Muffin",
				Quantity = 3
			}
		}
	});
	
This syntax decreases the verbosity of the tests and clearly shows the structure of the object graph. It enforces type safety where appropriate, helping with code completion. It also clearly shows the constraints between related properties, such as Order.Id and Item.OrderId.

## Productivity benefits
Traditionally, tests stop execution on the first failed assertion, but since our test encapsulates a complete, immutable data structure we can run all the constraint comparisons at once and return failures in batch.
	
Instead of getting only the first constraint failure message like (e.g. in NUnit):
	
	Expected string length 31 but was 3. Strings differ at index 0.
	Expected: "The Great Coffee Sensationalist"
	But was:  "Bob"
	-----------^
	
We can get information on **all** failing constraints in one go:

	Begin Differences (6 differences):
	Types [Int32,Int32], Item Left.Id != Right.Id, Values (2,1143848580)
	Types [List`1,List`1], Item Left.Items.Count != Right.Items.Count, Values (1,2)
	Types [Int32,Int32], Item Left.Items[0].Id != Right.Items[0].Id, Values (0,-1203215033)
	Types [Int32,Int32], Item Left.Items[0].Quantity != Right.Items[0].Quantity, Values (0,7)
	Types [String,String], Item Left.CustomerName != Right.CustomerName, Values (Bob,The Great Coffee Sensationalist)
	Types [null,Int32], Item Left.Items[0].OrderId != Right.Items[0].OrderId, Values ((null),1143848580)
	End Differences (Maximum of 100 differences shown).

It's easy to underestimate how powerful this is - using ShouldBe we can save ourselves many cycles of fix/rebuild/rerun to fix the same set of errors.

# How it works

ShouldBe static methods return random 'placeholder' values in the same type that they're being assigned.
Each random value is linked to a corresponding constraint condition that is created at the same time as the random value.
When the assertion is being run, each property in the actual object is compared structurally to the expected property's constraint by looking up the constraint by it's (random placeholder) value. If no constraint is found, the comparison is made as usual.

# State of the project

The project is a functional prototype being used successfully with multi-threaded XUnit as the assertion platform in our production projects. There are many useful extension methods and constraints that could be added.

The project is currently dependent on Kellerman's CompareNetObjects, but ideally, the project could and should be dependency free.

# Acknowledgements

Inspired by Shouldly.
Leverages the awesome comparison power of Kellerman's CompareNetObjects.
Generates random & default placeholder values using techniques from Ploeh's AutoFixture.
