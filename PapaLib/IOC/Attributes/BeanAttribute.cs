using System;
using PapaLib.IOC.Enums;

namespace PapaLib.IOC.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class BeanAttribute : Attribute
    {
        public InstantiateMod InstantiateMod { get; }
        public SingletonLoadMod SingletonLoadMod { get; }

        public BeanAttribute()
        {
            InstantiateMod = InstantiateMod.Singleton;
            SingletonLoadMod = SingletonLoadMod.InTime;
        }

        public BeanAttribute(InstantiateMod instantiateMod)
        {
            InstantiateMod = instantiateMod;
            if (instantiateMod == InstantiateMod.Singleton)
            {
                SingletonLoadMod = SingletonLoadMod.InTime;
            }
        }

        public BeanAttribute(SingletonLoadMod singletonLoadMod)
        {
            InstantiateMod = InstantiateMod.Singleton;
            SingletonLoadMod = singletonLoadMod;
        }
    }
}