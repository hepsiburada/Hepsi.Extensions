using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using Hepsi.Extensions.IQueryableExtensions.Filtering;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_filtering_an_iqueryable
{
    [TestFixture]
    public class with_a_list_of_filters
    {
        [Test]
        public void correct_iquerable_should_be_returned()
        {
            var source = new[]
            {
                new DummyDocument
                {
                    Id = 1,
                    Name = "First",
                    Count = 2
                },
                new DummyDocument
                {
                    Id = 2,
                    Name = "Middle",
                    Count = 3
                },
                new DummyDocument
                {
                    Id = 3,
                    Name = "Last",
                    Count = 3
                },
                new DummyDocument
                {
                    Id = 4,
                    Name = "Last",
                    Count = 4
                }
            }
            .AsQueryable();

            var filterList = new List<FilterByRequest>()
            {
                new FilterByRequest("name", "Last"),
                new FilterByRequest("count", "3"),
            };

            var filteredData = source.DynamicFiltering(filterList);
            var expectedFilteredData = source.Where(document => document.Id == 3);
            filteredData.SequenceEqual(expectedFilteredData).Should().BeTrue();
        }
    }
}
