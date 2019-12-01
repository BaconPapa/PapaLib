using System;

namespace PapaLib.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ReferenceAttribute : Attribute
    {
        public string Name { get; }
        public bool HasName => !string.IsNullOrEmpty(Name);
        public ReferenceAttribute() {}

        public ReferenceAttribute(string name)
        {
            this.Name = name;
        }
    }
}