using System;
using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using Hepsi.Extensions.IQueryableExtensions.Ordering;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_ordering_an_iqueryable.For_complex_types
{
    [TestFixture]
    class and_depth_is_more_than_one
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
                    CreatedAt = now,
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyInteger = 5, 
                        DummyString = "f",
                        InnerComplexType = new DummyInnerPropertyDocument
                        {
                            DummyInnerInteger = 2
                        }
                    }
                },
                new DummyDocument
                {
                    Name = "Last",
                    CreatedAt = now.AddHours(1),
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyInteger = 3,
                        DummyString = "z",
                        InnerComplexType = new DummyInnerPropertyDocument
                        {
                            DummyInnerInteger = 3
                        }
                    }
                },
                new DummyDocument
                {
                    Name = "First",
                    CreatedAt = now.AddHours(-1),
                    ComplexType = new DummyPropertyDocument
                    {
                        DummyInteger = 8, 
                        DummyString = "a", 
                        InnerComplexType = new DummyInnerPropertyDocument
                        {
                            DummyInnerInteger = 1
                        }
                    }
                }
            }
            .AsQueryable();

            var sortedData = source.DynamicOrderBy(new OrderByRequest("complexType.innerComplexType.DummyInnerInteger", OrderByDirection.Ascending));
            var expectedSortedData = source.OrderBy(document => document.ComplexType.InnerComplexType.DummyInnerInteger);

            sortedData.SequenceEqual(expectedSortedData).Should().BeTrue();
        }
    }
}
