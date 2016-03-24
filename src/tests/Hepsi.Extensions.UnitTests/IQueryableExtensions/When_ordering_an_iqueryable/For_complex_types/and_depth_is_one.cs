using System;
using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using Hepsi.Extensions.IQueryableExtensions.Ordering;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_ordering_an_iqueryable.For_complex_types
{
    [TestFixture]
    class and_depth_is_one
    {
        private IQueryable<DummyDocument> source;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var now = DateTime.Now;

            source = new[]
            {
                new DummyDocument
                {
                    Name = "Middle",
                    CreatedAt = now,
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyInteger = 5, DummyString = "f"
                    }
                },
                new DummyDocument
                {
                    Name = "Last",
                    CreatedAt = now.AddHours(1),
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyInteger = 3, DummyString = "z"
                    }
                },
                new DummyDocument
                {
                    Name = "First",
                    CreatedAt = now.AddHours(-1),
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyInteger = 8, DummyString = "a"
                    }
                }
            }
            .AsQueryable();
        }

        [Test]
        public void iquerable_should_be_sorted_correctly()
        {
            var sortedData = source.DynamicOrderBy(new OrderByRequest("complexType.DummyInteger", OrderByDirection.Ascending));
            var expectedSortedData = source.OrderBy(document => document.ComplexType.DummyInteger);

            sortedData.SequenceEqual(expectedSortedData).Should().BeTrue();
        }
    }
}
