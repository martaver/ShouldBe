# ShouldBe
Beautiful object graph assertions for C#.

Being fine-grained and precise in our tests is important in order to maximize coverage and ensure that future changes don't break our specifications. But the cost of being precise is a load of syntactical overhead that affects readability.

ShouldBe greatly enhances testing assertions in two areas: **readability** and **productivity**.

## Readability benefits
ShouldBe eliminates tests that look like this:

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
	
This syntax, apart from being clearer and more readable, also enforces type safety where appropriate, helping with code completion.

## Productivity benefits
The second biggest benefit of using ShouldBe is the boost to productivity. Traditionally, tests failstop on the first failed assertion, but since we are focusing our test on a complete, immutable data structure we can now run all the constraint comparisons at once.
	
Instead of getting only the first constraint failure message like:
	
	Expected string length 31 but was 3. Strings differ at index 0.
	Expected: "The Great Coffee Sensationalist"
	But was:  "Bob"
	-----------^
	
And then having to fix, rebuild and re-run to reveal the next, we can get information on **all** failing constraints in one go:

	Begin Differences (6 differences):
	Types [Int32,Int32], Item Left.Id != Right.Id, Values (2,1143848580)
	Types [List`1,List`1], Item Left.Items.Count != Right.Items.Count, Values (1,2)
	Types [Int32,Int32], Item Left.Items[0].Id != Right.Items[0].Id, Values (0,-1203215033)
	Types [Int32,Int32], Item Left.Items[0].Quantity != Right.Items[0].Quantity, Values (0,7)
	Types [String,String], Item Left.CustomerName != Right.CustomerName, Values (Bob,The Great Coffee Sensationalist)
	Types [null,Int32], Item Left.Items[0].OrderId != Right.Items[0].OrderId, Values ((null),1143848580)
	End Differences (Maximum of 100 differences shown).

It's easy to underestimate how powerful this is - using ShouldBe we can save ourselves 5 cycles of fix/rebuild/rerun to fix the same set of errors.

# How it works

ShouldBe static methods return random 'placeholder' values in the same type that they're being assigned.
Each random value is linked to a corresponding constraint condition that is created at the same time as the random value.
When the assertion is being run, each property in the actual object is compared structurally to the expected property's constraint by looking up the constraint by it's (random placeholder) value. If no constraint is found, the comparison is made as usual.

# State of the project

The project, although messy, is being used successfully with XUnit as the assertion platform in our production projects. There are many useful extension methods that could be added. It is even working fine XUnit running multi-threaded.

The project is currently dependent on Kellerman's CompareNetObjects and Ploeh's AutoFixture - although these were mainly used for convenience - ideally, the project would be dependency free.

# Acknowledgements

Inspired by Shouldly.
Leverages the awesome comparison power of Kellerman's CompareNetObjects.
Generates random & default values using Ploeh's AutoFixture.
