using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_filtering_an_iqueryable
{
    [TestFixture]
    public class and_at_least_a_result_exists
    {
        [Test]
        public void correct_iquerable_should_be_returned()
        {
            var source = new[]
            {
                new DummyDocument
                {
                    Name = "First",
                },
                new DummyDocument
                {
                    Name = "Middle",
                },
                new DummyDocument
                {
                    Name = "Last",
                }
            }
            .AsQueryable();

            var sortedData = source.DynamicFiltering("name", "Last");

            var expectedSortedData = source.Where(document => document.Name == "Last");

            sortedData.SequenceEqual(expectedSortedData).Should().BeTrue();
        }

        public IQueryable<DummyDocument> FilterDocuments(IQueryable<DummyDocument> documents, string propertyName, string value)
        {
            return documents.DynamicFiltering(propertyName, value);
        }
    }
}
