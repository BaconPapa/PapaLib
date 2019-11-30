using System;
using System.Collections.Generic;

namespace PapaLib.IOC
{
    public class Context
    {
        public enum SingletonLoadMode
        {
            InTime,
            Lazy
        }
        private Dictionary<Type,Type> unloadedSingletonDic;
        private Dictionary<Type,object> singletonObjectDic;
        private Dictionary<Type,Type> prototypeDic;
        public Context()
        {
            unloadedSingletonDic = new Dictionary<Type, Type>();
            singletonObjectDic = new Dictionary<Type, object>();
            prototypeDic = new Dictionary<Type, Type>();
        }
        private bool IsSingletonRegistered(Type type)
        {
            return singletonObjectDic.ContainsKey(type) || unloadedSingletonDic.ContainsKey(type);
        }
        public void RegisterSingleton<T>(SingletonLoadMode loadMode = SingletonLoadMode.InTime) where T : class
        {
            var genericType = typeof(T);
            if (IsSingletonRegistered(genericType)) return;
            if (loadMode == SingletonLoadMode.InTime)
            {
                singletonObjectDic.Add(genericType, Activator.CreateInstance(genericType));
            }
            else
            {
                unloadedSingletonDic.Add(genericType, genericType);
            }
        }

        public void RegisterSingleton<TInterface, TInstance>(SingletonLoadMode loadMode = SingletonLoadMode.InTime) where TInstance : TInterface
        {
            var interfaceType = typeof(TInterface);
            if (singletonObjectDic.ContainsKey(interfaceType)) return;
            var instanceType = typeof(TInstance);
            if (loadMode == SingletonLoadMode.InTime)
            {
                singletonObjectDic.Add(interfaceType, Activator.CreateInstance(instanceType));
            }
            else
            {
                unloadedSingletonDic.Add(interfaceType, instanceType);
            }
        }

        public void RegisterPrototype<T>() where T : class
        {
            var genericType = typeof(T);
            if (prototypeDic.ContainsKey(genericType)) return;
            prototypeDic.Add(genericType, genericType);
        }

        public void RegisterPrototype<TInterface, TInstance>() where TInstance : TInterface
        {
            var genericType = typeof(TInterface);
            if (prototypeDic.ContainsKey(genericType)) return;
            var instanceType = typeof(TInstance);
            prototypeDic.Add(genericType, instanceType);
        }

        public T GetInstance<T>() where T : class
        {
            var genericType = typeof(T);
            if (unloadedSingletonDic.ContainsKey(genericType))
            {
                var instance = (T)Activator.CreateInstance(unloadedSingletonDic[genericType]);
                unloadedSingletonDic.Remove(genericType);
                singletonObjectDic[genericType] = instance;
                return instance;
            }
            if (singletonObjectDic.ContainsKey(genericType))
            {
                return (T)singletonObjectDic[genericType];
            }
            if (prototypeDic.ContainsKey(genericType))
            {
                return (T)Activator.CreateInstance(prototypeDic[genericType]);
            }
            return default;
        }
    }
}