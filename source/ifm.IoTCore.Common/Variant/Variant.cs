namespace ifm.IoTCore.Common.Variant;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Exceptions;

/// <summary>
/// Provides an abstract base class for general purpose container classes used for holding and transporting data.
/// </summary>
public abstract class Variant
{
    /// <summary>
    /// Converts an object to a variant.
    /// </summary>
    /// <param name="data">The object to convert.</param>
    /// <returns>The converted variant.</returns>
    public static Variant FromObject(object data)
    {
        return VariantFromObject(data);
    }

    private static Variant VariantFromObject(object data)
    {
        if (data == null)
        {
            return null;
        }

        var type = data.GetType();

        if (IsVariant(type))
        {
            return (Variant)data;
        }
        if (IsSimple(type))
        {
            return VariantFromSimple(data, type);
        }
        if (IsArray(type))
        {
            return VariantFromEnumerable(data, new VariantArray());
        }
        if (IsGenericDictionary(type))
        {
            return VariantFromDictionary(data, new VariantObject());
        }
        if (IsGenericList(type) || IsGenericEnumerable(type))
        {
            return VariantFromEnumerable(data, new VariantArray());
        }
        if (!IsEnumerable(type) && IsComplex(type))
        {
            return VariantFromComplex(data, type, new VariantObject());
        }

        throw new Exception($"Unsupported type '{type.FullName}'");
    }

    private static VariantValue VariantFromSimple(object data, Type type)
    {
        if (type == typeof(bool))
        {
            return new VariantValue((bool)data);
        }
        if (type == typeof(char))
        {
            return new VariantValue((char)data);
        }
        if (type == typeof(sbyte))
        {
            return new VariantValue((sbyte)data);
        }
        if (type == typeof(byte))
        {
            return new VariantValue((byte)data);
        }
        if (type == typeof(short))
        {
            return new VariantValue((short)data);
        }
        if (type == typeof(ushort))
        {
            return new VariantValue((ushort)data);
        }
        if (type == typeof(int))
        {
            return new VariantValue((int)data);
        }
        if (type == typeof(uint))
        {
            return new VariantValue((uint)data);
        }
        if (type == typeof(long))
        {
            return new VariantValue((long)data);
        }
        if (type == typeof(ulong))
        {
            return new VariantValue((ulong)data);
        }
        if (type == typeof(float))
        {
            return new VariantValue((float)data);
        }
        if (type == typeof(double))
        {
            return new VariantValue((double)data);
        }
        if (type == typeof(decimal))
        {
            return new VariantValue((decimal)data);
        }
        if (type == typeof(string))
        {
            return new VariantValue((string)data);
        }
        if (type == typeof(DateTime))
        {
            return new VariantValue((DateTime)data);
        }
        if (type == typeof(TimeSpan))
        {
            return new VariantValue((TimeSpan)data);
        }
        if (type == typeof(Uri))
        {
            return new VariantValue((Uri)data);
        }
        if (type == typeof(Guid))
        {
            return new VariantValue((Guid)data);
        }
        if (type.IsEnum)
        {
            return new VariantValue((int)data);
        }

        throw new Exception($"Unsupported type '{type.FullName}'");
    }

    private static VariantArray VariantFromEnumerable(object data, VariantArray vArray)
    {
        foreach (var item in (IEnumerable)data)
        {
            vArray.Add(VariantFromObject(item));
        }
        return vArray;
    }

    private static VariantObject VariantFromDictionary(object data, VariantObject vObject)
    {
        foreach (var item in (IEnumerable)data)
        {
            var key = item.GetType().GetProperty("Key")?.GetValue(item);
            var value = item.GetType().GetProperty("Value")?.GetValue(item);
            vObject.Add(VariantFromObject(key), VariantFromObject(value));
        }
        return vObject;
    }

    private static VariantObject VariantFromComplex(object data, Type type, VariantObject vObject)
    {
        foreach (var property in type.GetProperties())
        {
            var propertyIgnored = false;
            var propertyIgnoredIfNull = false;
            var propertyName = property.Name;
            var attribute = GetVariantPropertyAttribute(property);
            if (attribute != null)
            {
                propertyIgnored = attribute.Ignored;
                propertyIgnoredIfNull = attribute.IgnoredIfNull;
                propertyName = attribute.Name;
            }
            if (propertyIgnored)
            {
                continue;
            }
            var value = property.GetValue(data);
            if (value == null && propertyIgnoredIfNull)
            {
                continue;
            }
            vObject.Add(propertyName, VariantFromObject(value));
        }
        return vObject;
    }

    /// <summary>
    /// Converts a variant to a class instance of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to create.</typeparam>
    /// <param name="data">The variant to convert.</param>
    /// <returns>The converted object.</returns>
    public static T ToObject<T>(Variant data)
    {
        if (data == null) throw new IoTCoreException(ResponseCodes.DataInvalid, "The provided data for the service is invalid.");

        return (T)VariantToObject(data, typeof(T));
    }

    /// <summary>
    /// Converts this variant to a class instance of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to create.</typeparam>
    /// <returns>The converted object.</returns>
    public T ToObject<T>()
    {
        return (T)VariantToObject(this, typeof(T));
    }

    private static object VariantToObject(Variant data, Type type)
    {
        var contractResolver = type.GetCustomAttribute<VariantContractResolverAttribute>();
        if (contractResolver != null)
        {
            var contractResolverInstance = (IVariantContractResolver)Activator.CreateInstance(contractResolver.ResolverType);
            return contractResolverInstance.CreateInstance(data);
        }

        if (data == null)
        {
            return null;
        }

        if (IsVariant(type))
        {
            return data;
        }
        if (IsSimple(type))
        {
            return VariantToSimple((VariantValue)data, type);
        }
        if (IsArray(type))
        {
            return VariantToArray((VariantArray)data, type);
        }
        if (IsGenericDictionary(type))
        {
            return VariantToGenericDictionary((VariantObject)data, type);
        }
        if (IsGenericList(type) || IsGenericEnumerable(type))
        {
            return VariantToGenericEnumerable((VariantArray)data, type);
        }
        if (!IsEnumerable(type) && IsComplex(type))
        {
            return VariantToComplex((VariantObject)data, type);
        }

        throw new Exception($"Unsupported type '{type.FullName}'");
    }

    private static object VariantToSimple(VariantValue data, Type type)
    {
        if (type == typeof(bool))
        {
            return (bool)data;
        }
        if (type == typeof(bool?))
        {
            return (bool)data;
        }
        if (type == typeof(char))
        {
            return (char)data;
        }
        if (type == typeof(char?))
        {
            return (char)data;
        }
        if (type == typeof(sbyte))
        {
            return (sbyte)data;
        }
        if (type == typeof(sbyte?))
        {
            return (sbyte)data;
        }
        if (type == typeof(byte))
        {
            return (byte)data;
        }
        if (type == typeof(byte?))
        {
            return (byte)data;
        }
        if (type == typeof(short))
        {
            return (short)data;
        }
        if (type == typeof(short?))
        {
            return (short)data;
        }
        if (type == typeof(ushort))
        {
            return (ushort)data;
        }
        if (type == typeof(ushort?))
        {
            return (ushort)data;
        }
        if (type == typeof(int))
        {
            return (int)data;
        }
        if (type == typeof(int?))
        {
            return (int)data;
        }
        if (type == typeof(uint))
        {
            return (uint)data;
        }
        if (type == typeof(uint?))
        {
            return (uint)data;
        }
        if (type == typeof(long))
        {
            return (long)data;
        }
        if (type == typeof(long?))
        {
            return (long)data;
        }
        if (type == typeof(ulong))
        {
            return (ulong)data;
        }
        if (type == typeof(ulong?))
        {
            return (ulong)data;
        }
        if (type == typeof(float))
        {
            return (float)data;
        }
        if (type == typeof(float?))
        {
            return (float)data;
        }
        if (type == typeof(double))
        {
            return (double)data;
        }
        if (type == typeof(double?))
        {
            return (double)data;
        }
        if (type == typeof(decimal))
        {
            return (decimal)data;
        }
        if (type == typeof(decimal?))
        {
            return (decimal)data;
        }
        if (type == typeof(string))
        {
            return (string)data;
        }
        if (type == typeof(DateTime))
        {
            return (DateTime)data;
        }
        if (type == typeof(DateTime?))
        {
            return (DateTime)data;
        }
        if (type == typeof(TimeSpan))
        {
            return (TimeSpan)data;
        }
        if (type == typeof(TimeSpan?))
        {
            return (TimeSpan)data;
        }
        if (type == typeof(Uri))
        {
            return (Uri)data;
        }
        if (type == typeof(Guid))
        {
            return (Guid)data;
        }
        if (type == typeof(Guid?))
        {
            return (Guid)data;
        }
        if (type.IsEnum)
        {
            return (int)data;
        }

        throw new Exception($"Unsupported type '{type.FullName}'");
    }

    private static object VariantToArray(VariantArray data, Type type)
    {
        var objectToFill = (IList)Activator.CreateInstance(type, data.Count);
        var elementType = type.GetElementType();
        for (var index = 0; index < data.Count; index++)
        {
            objectToFill[index] = VariantToObject(data[index], elementType);
        }
        return objectToFill;
    }

    private static object VariantToGenericEnumerable(VariantArray data, Type type)
    {
        var objectToFill = Activator.CreateInstance(type /*, data.Count*/);
        var elementType = GetGenericCollectionUnderlyingType(type);
        var addMethod = type.GetMethod("Add", new[] { elementType }) ?? throw new Exception($"Requested type '{type.FullName}' does not provide an Add-method");
        foreach (var item in data)
        {
            addMethod.Invoke(objectToFill, new[] { VariantToObject(item, elementType) });
        }
        return objectToFill;
    }

    private static object VariantToGenericDictionary(VariantObject data, Type type)
    {
        var objectToFill = Activator.CreateInstance(type /*, data.Count*/);
        var elementType = GetGenericCollectionUnderlyingType(type);
        var keyType = elementType.GetProperty("Key")?.PropertyType;
        var valueType = elementType.GetProperty("Value")?.PropertyType;
        var addMethod = type.GetMethod("Add", new[] { keyType, valueType }) ?? throw new Exception($"Requested type '{type.FullName}' does not provide an Add-method");
        foreach (var item in data)
        {
            var key = VariantToObject(item.Key, keyType);
            var value = VariantToObject(item.Value, valueType);
            addMethod.Invoke(objectToFill, new[] { key, value });
        }
        return objectToFill;
    }

    private static object VariantToComplex(VariantObject data, Type type)
    {
        // How to support complex types (structs and classes) with no parameterless constructor and no public settable properties:
        // The type must have a constructor with VariantConstructor attribute and the constructor parameters must have the
        // VariantProperty attribute. Then search for the constructor with the VariantConstructor attribute.
        // From the parameter list find the corresponding parameter for each value by VariantProperty attribute and build
        // the parameter array for CreateInstance accordingly. Then call CreateInstance with the parameter list and return the object.

        var objectToFill = Activator.CreateInstance(type);
        foreach (var property in type.GetProperties())
        {
            var propertyName = property.Name;
            string[] propertyAlternativeNames = null;
            var propertyRequired = false;
            var propertyIgnored = false;

            var attribute = GetVariantPropertyAttribute(property);
            if (attribute != null)
            {
                propertyName = attribute.Name;
                propertyAlternativeNames = attribute.AlternativeNames;
                propertyRequired = attribute.Required;
                propertyIgnored = attribute.Ignored;
            }

            if (propertyIgnored) continue;
            var propertyFound = false;
            if (data.TryGetValue(propertyName, out var item))
            {
                propertyFound = true;
            }
            else
            {
                if (propertyAlternativeNames != null)
                {
                    if (data.TryGetValue(propertyAlternativeNames, out item))
                    {
                        propertyFound = true;
                    }
                }
            }

            if (propertyFound)
            {
                if (property.SetMethod != null)
                {
                    var value = VariantToObject(item, property.PropertyType);
                    property.SetValue(objectToFill, value);
                }
                else
                {
                    throw new Exception($"The property '{propertyName}' does not provide a Set-method");
                }
            }
            else
            {
                if (propertyRequired)
                {
                    throw new Exception($"Required property '{propertyName}' not found in variant");
                }
            }
        }
        return objectToFill;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsVariant(Type type)
    {
        return type == typeof(Variant) || type.IsSubclassOf(typeof(Variant));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSimple(Type type)
    {
        if (IsNullable(type))
        {
            type = GetNullableUnderlyingType(type);
        }
        return type.IsPrimitive ||
               type.IsEnum ||
               type == typeof(decimal) ||
               type == typeof(string) ||
               type == typeof(DateTime) ||
               type == typeof(TimeSpan) ||
               type == typeof(Uri) ||
               type == typeof(Guid);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsArray(Type type)
    {
        return type.IsArray;
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //private static bool IsList(Type type)
    //{
    //    return !type.IsGenericType && typeof(IList).IsAssignableFrom(type);
    //}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsGenericList(Type type)
    {
        return type.GetInterfaces().Any(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IList<>));
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //private static bool IsDictionary(Type type)
    //{
    //    return !type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type);
    //}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsGenericDictionary(Type type)
    {
        return type.GetInterfaces().Any(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IDictionary<,>));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsEnumerable(Type type)
    {
        return !type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsGenericEnumerable(Type type)
    {
        return type.GetInterfaces().Any(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsComplex(Type type)
    {
        return type.IsClass ||
               // If it is a value type, but not a primitive type and not an enum it is a struct
               type.IsValueType && !type.IsPrimitive && !type.IsEnum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsNullable(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Type GetNullableUnderlyingType(Type type)
    {
        return type.GetGenericArguments()[0];
    }

    private static Type GetGenericCollectionUnderlyingType(Type type)
    {
        return (from item in type.GetInterfaces()
                where
            item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                select
            item.GetGenericArguments()[0]).FirstOrDefault();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static VariantPropertyAttribute GetVariantPropertyAttribute(ICustomAttributeProvider property)
    {
        var attributes = property.GetCustomAttributes(false);
        return attributes.Where(attribute => attribute.GetType() == typeof(VariantPropertyAttribute)).Cast<VariantPropertyAttribute>().FirstOrDefault();
    }
}