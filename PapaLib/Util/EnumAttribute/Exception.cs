using System;

namespace PapaLib.Util.EnumAttribute
{
    public class UnSupportAttributeException : ApplicationException
    {
        public UnSupportAttributeException(Type type) : base($"{type} is not a subclass of EnumAttribute") { }
    }

    public class UnSupportPropertyException<TValue> : ApplicationException
    {
        public UnSupportPropertyException(Type attributeType) : base($"{attributeType} is not an implementation of {typeof(IProperty<TValue>)}") { }
    }
}
