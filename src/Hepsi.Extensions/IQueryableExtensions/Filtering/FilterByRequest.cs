namespace Hepsi.Extensions.IQueryableExtensions.Filtering
{
    public class FilterByRequest
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public FilterByRequest(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
