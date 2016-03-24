using System.Linq;
using FluentAssertions;
using Hepsi.Extensions.IQueryableExtensions;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_filtering_an_iqueryable
{
    [TestFixture]
    public class and_no_result_exits
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

            var sortedData = source.DynamicFiltering("name", "NON_EXISTENT_NAME");

            sortedData.Any().Should().BeFalse();
        }
    }
}
