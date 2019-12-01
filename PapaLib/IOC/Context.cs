using System;
using System.Collections.Generic;

namespace PapaLib.IOC
{
    public class Context
    {
        private readonly Dictionary<Type,Type> _unloadedSingletonDic;
        private readonly Dictionary<Type,object> _singletonObjectDic;
        private readonly Dictionary<Type,Type> _prototypeDic;
        public Context()
        {
            _unloadedSingletonDic = new Dictionary<Type, Type>();
            _singletonObjectDic = new Dictionary<Type, object>();
            _prototypeDic = new Dictionary<Type, Type>();
        }
        private bool IsSingletonRegistered(Type type)
        {
            return _singletonObjectDic.ContainsKey(type) || _unloadedSingletonDic.ContainsKey(type);
        }
        public void RegisterSingleton<T>(SingletonLoadMode loadMode = SingletonLoadMode.InTime) where T : class
        {
            var genericType = typeof(T);
            if (IsSingletonRegistered(genericType)) return;
            if (loadMode == SingletonLoadMode.InTime)
            {
                _singletonObjectDic.Add(genericType, Activator.CreateInstance(genericType));
            }
            else
            {
                _unloadedSingletonDic.Add(genericType, genericType);
            }
        }

        public void RegisterSingleton<TInterface, TInstance>(SingletonLoadMode loadMode = SingletonLoadMode.InTime) where TInstance : TInterface
        {
            var interfaceType = typeof(TInterface);
            if (_singletonObjectDic.ContainsKey(interfaceType)) return;
            var instanceType = typeof(TInstance);
            if (loadMode == SingletonLoadMode.InTime)
            {
                _singletonObjectDic.Add(interfaceType, Activator.CreateInstance(instanceType));
            }
            else
            {
                _unloadedSingletonDic.Add(interfaceType, instanceType);
            }
        }

        public void RegisterPrototype<T>() where T : class
        {
            var genericType = typeof(T);
            if (_prototypeDic.ContainsKey(genericType)) return;
            _prototypeDic.Add(genericType, genericType);
        }

        public void RegisterPrototype<TInterface, TInstance>() where TInstance : TInterface
        {
            var genericType = typeof(TInterface);
            if (_prototypeDic.ContainsKey(genericType)) return;
            var instanceType = typeof(TInstance);
            _prototypeDic.Add(genericType, instanceType);
        }

        public T GetInstance<T>() where T : class
        {
            var genericType = typeof(T);
            if (_unloadedSingletonDic.ContainsKey(genericType))
            {
                var instance = (T)Activator.CreateInstance(_unloadedSingletonDic[genericType]);
                _unloadedSingletonDic.Remove(genericType);
                _singletonObjectDic[genericType] = instance;
                return instance;
            }
            if (_singletonObjectDic.ContainsKey(genericType))
            {
                return (T)_singletonObjectDic[genericType];
            }
            if (_prototypeDic.ContainsKey(genericType))
            {
                return (T)Activator.CreateInstance(_prototypeDic[genericType]);
            }
            return default;
        }
    }
}