using System;
using System.Collections.Generic;

namespace PapaLib.IOC
{
    public class CodeContext
    {
        private Dictionary<Type,object> singletonObjectDic;
        private Dictionary<Type,Type> prototypeDic;
        public CodeContext()
        {
            singletonObjectDic = new Dictionary<Type, object>();
            prototypeDic = new Dictionary<Type, Type>();
        }
        public void RegisterSingleton<T>() where T : class
        {
            var genericType = typeof(T);
            if (singletonObjectDic.ContainsKey(genericType)) return;
            var instance = Activator.CreateInstance(genericType);
            singletonObjectDic.Add(genericType, instance);
        }

        public void RegisterPrototype<T>() where T : class
        {
            var genericType = typeof(T);
            if (prototypeDic.ContainsKey(genericType)) return;
            prototypeDic.Add(genericType, genericType);
        }

        public T GetInstance<T>() where T : class
        {
            var genericType = typeof(T);
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