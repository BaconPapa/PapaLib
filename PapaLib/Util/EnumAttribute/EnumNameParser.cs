using System;
using System.Collections.Generic;

namespace PapaLib.Util.EnumAttribute
{
    public static class EnumAttributeExtension
    {
        private static Dictionary<Enum, Dictionary<Type, object>> enumPropertyBuffer = new Dictionary<Enum, Dictionary<Type, object>>();
        public static TValue AttributeProperty<TEnum, TAttribute, TValue>(this TEnum enumValue)
            where TEnum : Enum
            where TAttribute : EnumAttribute, IProperty<TValue>
        {
            if (!enumPropertyBuffer.ContainsKey(enumValue))
            {
                enumPropertyBuffer.Add(enumValue, new Dictionary<Type, object>());
            }
            var attributeType = typeof(TAttribute);
            var propertyBuffer = enumPropertyBuffer[enumValue];
            if (propertyBuffer.ContainsKey(attributeType)) return (TValue)propertyBuffer[attributeType];
            var enumType = typeof(TEnum);
            var enumFieldName = Enum.GetName(enumType, enumValue);
            var field = enumType.GetField(enumFieldName);
            TValue propertyValue = default;
            if (field.IsDefined(attributeType, true))
            {
                var attribute = (TAttribute)Attribute.GetCustomAttribute(field, attributeType);
                propertyValue = attribute.value;
            }
            propertyBuffer[attributeType] = propertyValue;
            return propertyValue;
        }

        private static Dictionary<Enum, Dictionary<Type, bool>> enumAttributeBuffer = new Dictionary<Enum, Dictionary<Type, bool>>();
        public static bool HasAttribute<TEnum, TAttribute>(this TEnum enumValue)
            where TEnum : Enum
            where TAttribute : EnumAttribute
        {
            if (!enumAttributeBuffer.ContainsKey(enumValue))
            {
                enumAttributeBuffer.Add(enumValue, new Dictionary<Type, bool>());
            }
            var typeBuffer = enumAttributeBuffer[enumValue];
            var attributeType = typeof(TAttribute);
            if (typeBuffer.ContainsKey(attributeType))
            {
                return typeBuffer[attributeType];
            }
            var enumType = typeof(TEnum);
            var enumFieldName = Enum.GetName(enumType, enumValue);
            var field = enumType.GetField(enumFieldName);
            var hasAttribute = field.IsDefined(attributeType, true);
            typeBuffer[attributeType] = hasAttribute;
            return hasAttribute;
        }
    }
}
