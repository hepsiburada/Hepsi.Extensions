using System;

namespace Hepsi.Extensions.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        private readonly string propertyName;

        public PropertyNotFoundException(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public override string Message
        {
            get { return string.Format("Field {0} couldnt be found in document", propertyName); }
        }
    }
}
