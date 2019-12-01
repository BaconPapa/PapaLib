using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PapaLib.IOC.Attributes;

namespace PapaLib.IOC
{
    public class Context
    {
        private readonly Dictionary<string,Type> _unloadedSingletonDic;
        private readonly Dictionary<string,object> _singletonObjectDic;
        private readonly Dictionary<string,Type> _prototypeDic;
        public Context()
        {
            _unloadedSingletonDic = new Dictionary<string, Type>();
            _singletonObjectDic = new Dictionary<string, object>();
            _prototypeDic = new Dictionary<string, Type>();
        }
        private bool IsSingletonRegistered(string memberName)
        {
            return _singletonObjectDic.ContainsKey(memberName) || _unloadedSingletonDic.ContainsKey(memberName);
        }
        public void RegisterSingleton<T>(SingletonLoadMode loadMode = SingletonLoadMode.InTime) where T : class
        {
            var genericType = typeof(T);
            var memberName = genericType.Name;
            if (IsSingletonRegistered(memberName)) return;
            if (loadMode == SingletonLoadMode.InTime)
            {
                _singletonObjectDic.Add(memberName, CreateInstance(genericType));
            }
            else
            {
                _unloadedSingletonDic.Add(memberName, genericType);
            }
        }

        public void RegisterSingleton<TInterface, TInstance>(SingletonLoadMode loadMode = SingletonLoadMode.InTime) where TInstance : TInterface
        {
            var interfaceName = typeof(TInterface).Name;
            if (_singletonObjectDic.ContainsKey(interfaceName)) return;
            var instanceType = typeof(TInstance);
            if (loadMode == SingletonLoadMode.InTime)
            {
                _singletonObjectDic.Add(interfaceName, Activator.CreateInstance(instanceType));
            }
            else
            {
                _unloadedSingletonDic.Add(interfaceName, instanceType);
            }
        }

        public void RegisterPrototype<T>() where T : class
        {
            var genericType = typeof(T);
            var memberName = genericType.Name;
            if (_prototypeDic.ContainsKey(memberName)) return;
            _prototypeDic.Add(memberName, genericType);
        }

        public void RegisterPrototype<TInterface, TInstance>() where TInstance : TInterface
        {
            var genericTypeName = typeof(TInterface).Name;
            if (_prototypeDic.ContainsKey(genericTypeName)) return;
            var instanceType = typeof(TInstance);
            _prototypeDic.Add(genericTypeName, instanceType);
        }

        public T GetInstance<T>() where T : class
        {
            var typeName = typeof(T).Name;
            return (T)FindInstance(typeName);
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
                string parameterName = default;
                if (Attribute.IsDefined(parameters[i], typeof(ReferenceAttribute)))
                {
                    var referenceAttribute = parameters[i].GetCustomAttribute<ReferenceAttribute>();
                    if (referenceAttribute.HasName)
                    {
                        parameterName = referenceAttribute.Name;
                    }
                }
                parameterName = parameterName ?? parameters[i].ParameterType.Name;
                parameterObjects[i] = FindInstance(parameterName) ?? throw new Exception();
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
                var referenceName = info.FieldType.Name;
                var referenceInstance = FindInstance(referenceName) ?? throw new Exception();
                info.SetValue(instance, referenceInstance);
            }

            var properties = instanceType.GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            var referencedProperties = properties
                .Where(fieldInfo => Attribute.IsDefined(fieldInfo, typeof(ReferenceAttribute)));
            foreach (var info in referencedProperties)
            {
                var referenceName = info.PropertyType.Name;
                var referenceInstance = FindInstance(referenceName) ?? throw new Exception();
                info.SetValue(instance, referenceInstance);
            }
            
        }

        private object FindInstance(string typeName)
        {
            if (_singletonObjectDic.TryGetValue(typeName, out var instance))
            {
                return instance;
            }

            if (_unloadedSingletonDic.TryGetValue(typeName, out var unInstantiateType))
            {
                var tempInstance = CreateInstance(unInstantiateType);
                _unloadedSingletonDic.Remove(typeName);
                _singletonObjectDic.Add(typeName, tempInstance);
                return tempInstance;
            }

            if (_prototypeDic.TryGetValue(typeName, out var proto))
            {
                return CreateInstance(proto);
            }

            return default;
        }
    }
}