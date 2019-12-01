using System;

namespace PapaLib.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReferenceAttribute : Attribute {}
}