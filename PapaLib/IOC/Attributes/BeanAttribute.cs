using System;

namespace PapaLib.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class BeanAttributes : Attribute {}
}