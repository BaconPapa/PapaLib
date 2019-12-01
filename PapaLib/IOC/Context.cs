using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PapaLib.IOC.Attributes;

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
                _singletonObjectDic.Add(genericType, CreateInstance(genericType));
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
            return (T)FindInstance(genericType);
        }
        
        private object CreateInstance(Type instanceType)
        {
            var instance = InstantiateWithFirstConstructor(instanceType);
            ResolveReference(instanceType, instance);
            return instance;
        }

        private object InstantiateWithFirstConstructor(Type instanceType)
        {
            var constructor = instanceType.GetConstructors()[0];
            var parameters = constructor.GetParameters();
            var parameterObjects = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;
                parameterObjects[i] = FindInstance(parameterType) ?? throw new Exception();
            }
            return constructor.Invoke(parameterObjects);
        }
        
        private void ResolveReference(IReflect instanceType, object instance)
        {
            var fields = instanceType.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            var referencedFields = fields
                .Where(fieldInfo => Attribute.IsDefined(fieldInfo, typeof(ReferenceAttribute)));
            foreach (var info in referencedFields)
            {
                var referenceType = info.FieldType;
                var referenceInstance = FindInstance(referenceType) ?? throw new Exception();
                info.SetValue(instance, referenceInstance);
            }

            var properties = instanceType.GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            var referencedProperties = properties
                .Where(fieldInfo => Attribute.IsDefined(fieldInfo, typeof(ReferenceAttribute)));
            foreach (var info in referencedProperties)
            {
                var referenceType = info.PropertyType;
                var referenceInstance = FindInstance(referenceType) ?? throw new Exception();
                info.SetValue(instance, referenceInstance);
            }
            
        }

        private object FindInstance(Type instanceType)
        {
            if (_singletonObjectDic.TryGetValue(instanceType, out var instance))
            {
                return instance;
            }

            if (_unloadedSingletonDic.TryGetValue(instanceType, out var unInstantiateType))
            {
                var tempInstance = CreateInstance(unInstantiateType);
                _unloadedSingletonDic.Remove(instanceType);
                _singletonObjectDic.Add(instanceType, tempInstance);
                return tempInstance;
            }

            if (_prototypeDic.TryGetValue(instanceType, out var proto))
            {
                return CreateInstance(proto);
            }

            return default;
        }
    }
}