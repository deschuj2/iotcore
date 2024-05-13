namespace ifm.IoTCore.ElementManager.Elements;

using System;
using Contracts.Elements.Formats;
using Contracts.Elements.Valuations;

internal static class DataElementHelpers
{
    public static Format CreateFormat(Type type)
    {
        Format format = null;

        if (type == typeof(bool))
        {
            format = new BooleanFormat();
        }
        else if (type == typeof(char) || type == typeof(sbyte))
        {
            format = new Int8Format();
        }
        else if (type == typeof(byte))
        {
            format = new UInt8Format();
        }
        else if (type == typeof(short))
        {
            format = new Int16Format();
        }
        else if (type == typeof(ushort))
        {
            format = new UInt16Format();
        }
        else if (type == typeof(int))
        {
            format = new Int32Format();
        }
        else if (type == typeof(uint))
        {
            format = new UInt32Format();
        }
        else if (type == typeof(long))
        {
            format = new Int64Format();
        }
        else if (type == typeof(ulong))
        {
            format = new UInt64Format();
        }
        else if (type == typeof(float))
        {
            format = new FloatFormat();
        }
        else if (type == typeof(double))
        {
            format = new DoubleFormat();
        }
        else if (type == typeof(string))
        {
            format = new StringFormat();
        }
        else if (type == typeof(bool[]))
        {
            format = new ArrayFormat(new ArrayValuation(new BooleanFormat()));
        }
        else if (type == typeof(char[]) || type == typeof(sbyte[]))
        {
            format = new ArrayFormat(new ArrayValuation(new Int8Format()));
        }
        else if (type == typeof(byte[]))
        {
            format = new ArrayFormat(new ArrayValuation(new UInt8Format()));
        }
        else if (type == typeof(short[]))
        {
            format = new ArrayFormat(new ArrayValuation(new Int16Format()));
        }
        else if (type == typeof(ushort[]))
        {
            format = new ArrayFormat(new ArrayValuation(new UInt16Format()));
        }
        else if (type == typeof(int[]))
        {
            format = new ArrayFormat(new ArrayValuation(new Int32Format()));
        }
        else if (type == typeof(uint[]))
        {
            format = new ArrayFormat(new ArrayValuation(new UInt32Format()));
        }
        else if (type == typeof(long[]))
        {
            format = new ArrayFormat(new ArrayValuation(new Int64Format()));
        }
        else if (type == typeof(ulong[]))
        {
            format = new ArrayFormat(new ArrayValuation(new UInt64Format()));
        }
        else if (type == typeof(float[]))
        {
            format = new ArrayFormat(new ArrayValuation(new FloatFormat()));
        }
        else if (type == typeof(double[]))
        {
            format = new ArrayFormat(new ArrayValuation(new DoubleFormat()));
        }
        else if (type == typeof(string[]))
        {
            format = new ArrayFormat(new ArrayValuation(new StringFormat(null)));
        }

        return format;
    }

    //private static void ValidateData(Variant newValue, Format format)
    //{
    //    switch (format)
    //    {
    //        case IntegerFormat { Valuation: { } } integerFormat:
    //            {
    //                if (newValue.AsVariantValue() == null || !newValue.AsVariantValue().TryGetInt(out var integerValue))
    //                {
    //                    throw new DataInvalidException();
    //                }
    //                if (integerFormat.Valuation.Min.HasValue && integerValue < integerFormat.Valuation.Min ||
    //                    integerFormat.Valuation.Max.HasValue && integerValue > integerFormat.Valuation.Max)
    //                {
    //                    throw new DataOutOfRangeException();
    //                }
    //                break;
    //            }
    //        case FloatFormat { Valuation: { } } floatFormat:
    //            {
    //                if (newValue.AsVariantValue() == null || !newValue.AsVariantValue().TryGetFloat(out var floatValue))
    //                {
    //                    throw new DataInvalidException();
    //                }
    //                if (floatFormat.Valuation.Min.HasValue && floatValue < floatFormat.Valuation.Min ||
    //                    floatFormat.Valuation.Max.HasValue && floatValue > floatFormat.Valuation.Max)
    //                {
    //                    throw new DataOutOfRangeException();
    //                }
    //                break;
    //            }
    //        case StringFormat { Valuation: { } } stringFormat:
    //            {
    //                var stringValue = (string)newValue.AsVariantValue() ?? throw new DataInvalidException();
    //                if (stringFormat.Valuation.MinLength.HasValue && stringValue.Length < stringFormat.Valuation.MinLength ||
    //                    stringFormat.Valuation.MaxLength.HasValue && stringValue.Length > stringFormat.Valuation.MaxLength ||
    //                    !string.IsNullOrEmpty(stringFormat.Valuation.Pattern) && !Regex.IsMatch(stringValue, stringFormat.Valuation.Pattern))
    //                {
    //                    throw new DataOutOfRangeException();
    //                }
    //                break;
    //            }
    //        case IntegerEnumFormat { Valuation: { } } integerEnumFormat:
    //            {
    //                var enumValue = (string)newValue.AsVariantValue() ?? throw new DataInvalidException();
    //                if (integerEnumFormat.Valuation.Values != null && !integerEnumFormat.Valuation.Values.ContainsKey(enumValue))
    //                {
    //                    throw new DataOutOfRangeException();
    //                }
    //                break;
    //            }
    //        case ArrayFormat { Valuation: { } } arrayFormat:
    //            {
    //                var arrayValue = newValue.AsVariantArray() ?? throw new DataInvalidException();
    //                if (arrayFormat.Valuation.Length.HasValue && arrayValue.Count != arrayFormat.Valuation.Length)
    //                {
    //                    throw new DataOutOfRangeException();
    //                }
    //                foreach (var arrayItem in arrayValue)
    //                {
    //                    ValidateData(arrayItem, arrayFormat.Valuation.Format);
    //                }
    //                break;
    //            }
    //        case ObjectFormat { Valuation: { } } objectFormat:
    //            {
    //                var objectValue = newValue.AsVariantObject() ?? throw new DataInvalidException();
    //                foreach (var objectItem in objectValue)
    //                {
    //                    var field = objectFormat.Valuation.Fields.Find(x => x.Name == (string)(VariantValue)objectItem.Key);
    //                    if (field != null)
    //                    {
    //                        ValidateData(objectItem.Value, field.Format);
    //                    }
    //                }
    //                break;
    //            }
    //    }
    //}
}
