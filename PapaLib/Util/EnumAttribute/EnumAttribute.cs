using System;

namespace PapaLib.Util.EnumAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumAttribute : Attribute
    {
        protected EnumAttribute() {}
    }

    public interface IProperty<T>
    {
        T value {get;}
    }
}

