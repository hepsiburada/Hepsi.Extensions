using System;

namespace Hepsi.Extensions.UnitTests
{
    public class DummyDocument
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public DummyPropertyDocument ComplexType { get; set; }
    }
}
