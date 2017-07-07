using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using Xunit;

namespace ShouldBeAssertions.Core.Tests.Samples
{	
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

		[Fact]
		public void UsingNUnitAssertions()
		{
			var actual = this.OrderProvider.GetOrder();			
			actual.Id.ShouldBeGreaterThan(0);
			actual.CustomerName.ShouldBe("The Great Coffee Sensationalist");
			actual.Items.ShouldNotBeNull();
			actual.Items.Count.ShouldBe(2);
			actual.Items[0].Id.ShouldBeGreaterThan(0);
			actual.Items[0].OrderId.ShouldBe(actual.Id);
			actual.Items[0].Name.ShouldBe("Donut");
			actual.Items[0].Quantity.ShouldBe(7);
			actual.Items[1].Id.ShouldBeGreaterThan(0);
			actual.Items[1].OrderId.ShouldBe(actual.Id);
			actual.Items[1].Name.ShouldBe("Crunch Muffin");
			actual.Items[1].Quantity.ShouldBe(3);

			//Type safety...?
			//Is the Item.OrderId constraint with Order.Id obvious?			
		}

		[Fact]
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