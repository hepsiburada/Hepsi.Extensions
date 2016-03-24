using System.Collections.Generic;
using System.Linq;

namespace Hepsi.Extensions.IQueryableExtensions.Ordering
{
    public class OrderByRequest
    {
        private readonly Dictionary<string, OrderByDirection> thenByRequests = new Dictionary<string, OrderByDirection>();

        public string PropertyName { get; private set; }

        public OrderByDirection Direction { get; private set; }

        public bool IsOrderBySpecified
        {
            get
            {
                return !string.IsNullOrEmpty(PropertyName);
            }
        }

        public IEnumerable<KeyValuePair<string, OrderByDirection>> ThenByRequests
        {
            get
            {
                return thenByRequests.AsEnumerable();
            }
        }

        public OrderByRequest(string propertyName, OrderByDirection direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }

        public void AddThenByRequest(string propertyName, OrderByDirection direction)
        {
            thenByRequests.Add(propertyName, direction);
        }
    }
}
