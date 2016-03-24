using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using Hepsi.Extensions.IQueryableExtensions.Ordering;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_ordering_an_iqueryable
{
    [TestFixture]
    public class and_property_does_not_exist
    {
        [Test]
        public void iquerable_should_be_sorted_correctly()
        {
            var source = new[]
            {
                new DummyDocument
                {
                    Name = "Middle"
                },
                new DummyDocument
                {
                    Name = "Last"
                },
                new DummyDocument
                {
                    Name = "First"}
            }
            .AsQueryable();

            var sortedData = source.DynamicOrderBy(new OrderByRequest("NON_EXISTENT_PROPERTY_NAME", OrderByDirection.Ascending));
            var expectedSortedData = source;

            sortedData.SequenceEqual(expectedSortedData).Should().BeTrue();
        }
    }
}
