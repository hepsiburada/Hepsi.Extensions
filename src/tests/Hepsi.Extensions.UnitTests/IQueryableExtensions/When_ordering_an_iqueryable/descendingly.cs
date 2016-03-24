using System;
using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using Hepsi.Extensions.IQueryableExtensions.Ordering;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_ordering_an_iqueryable
{
    [TestFixture]
    public class descendingly
    {
        [Test]
        public void iquerable_should_be_sorted_correctly()
        {
            var now = DateTime.Now;

            var source = new[]
            {
                new DummyDocument
                {
                    Name = "Middle",
                    CreatedAt = now
                },
                new DummyDocument
                {
                    Name = "Last",
                    CreatedAt = now.AddHours(1)
                },
                new DummyDocument
                {
                    Name = "First",
                    CreatedAt = now.AddHours(-1)
                }
            }
            .AsQueryable();

            var sortedData = source.DynamicOrderBy(new OrderByRequest("createdAt", OrderByDirection.Descending));
            var expectedSortedData = source.OrderByDescending(document => document.CreatedAt);

            sortedData.SequenceEqual(expectedSortedData).Should().BeTrue();
        }
    }
}
