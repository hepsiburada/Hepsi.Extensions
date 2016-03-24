using System.Linq;
using Hepsi.Extensions.Exceptions;
using Hepsi.Extensions.IQueryableExtensions;
using NUnit.Framework;

namespace Hepsi.Extensions.UnitTests.IQueryableExtensions.When_filtering_an_iqueryable
{
    [TestFixture]
    public class with_a_filter_whose_field_doesnt_exist
    {
        [Test]
        [ExpectedException(typeof(PropertyNotFoundException), ExpectedMessage = "Field not_existing_property couldnt be found in document")]
        public void should_throw_property_doesnt_exist_exception()
        {
            var source = new[]
            {
                new DummyDocument
                {
                    Id = 1,
                    Name = "First",
                    Count = 2
                }
            }
            .AsQueryable();

            source.DynamicFiltering("not_existing_property", "3");
        }
    }
}
