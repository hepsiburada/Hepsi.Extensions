using System;
using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using Hepsi.Extensions.IQueryableExtensions.Ordering;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_ordering_an_iqueryable.For_complex_types
{
    [TestFixture]
    public class descendingly_and_then_by_descendingly
    {
        [Test]
        public void iquerable_should_be_sorted_correctly()
        {
            var now = DateTime.Now;

            var source = new[]
            {
                new DummyDocument
                {
                    Name = "Middle-2",
                    CreatedAt = now,
                    Count = 1,
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyString = "b"
                    } 
                },
                new DummyDocument
                {
                    Name = "Middle",
                    CreatedAt = now,
                    Count = 10,
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyString = "d"
                    } 
                },
                new DummyDocument
                {
                    Name = "Last",
                    CreatedAt = now.AddHours(1),
                    Count = 20,
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyString = "a"
                    } 
                },
                new DummyDocument
                {
                    Name = "First",
                    CreatedAt = now.AddHours(-1),
                    Count= 0,
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyString = "f"
                    } 
                }
            }
            .AsQueryable();

            var orderByRequest = new OrderByRequest("ComplexType.dummyString", OrderByDirection.Descending);
            orderByRequest.AddThenByRequest("count", OrderByDirection.Descending);

            var sortedData = source.DynamicOrderBy(
                orderByRequest);
            var expectedSortedData = source.OrderByDescending(document => document.ComplexType.DummyString).ThenByDescending(document => document.Count);

            sortedData.SequenceEqual(expectedSortedData).Should().BeTrue();
        }
    }
}
