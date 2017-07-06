using System.Collections.Generic;
using NUnit.Framework;

namespace ShouldBeAssertions.Tests.Samples
{
	[TestFixture]
	public class Samples
	{
		public class Order
		{
			public int Id { get; set; }
			public List<Item> Items { get; set; }
			public string CustomerName { get; set; }
		}

		public class Item
		{
			public int Id { get; set; }
			public int? OrderId { get; set; }
			public string Name { get; set; }
			public int Quantity { get; set; }
		}


		public class Provider
		{
			public Order GetOrder()
			{
				return new Order
				{
					Id = 2,
					CustomerName = "Bob",
					Items = new List<Item>
					{
						new Item
						{
							Name = "Donut"
						}
					}
				};
			}
		}

		private Provider OrderProvider { get; set; } = new Provider();

		[Test]
		public void UsingNUnitAssertions()
		{
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

			//Type safety...?
			//Is the Item.OrderId constraint with Order.Id obvious?			
		}

		[Test]
		public void UsingShouldBeAssertions()
		{
			this.OrderProvider.GetOrder().ShouldLookLike(new Order
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

		}
	}
}