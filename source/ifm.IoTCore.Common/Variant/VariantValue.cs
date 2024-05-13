namespace ifm.IoTCore.Common.Variant;

using System;
using Exceptions;

/// <summary>
/// Provides a Variant class for simple data types.
/// </summary>
public class VariantValue : Variant, IEquatable<VariantValue>
{
    /// <summary>
    /// The value types.
    /// </summary>
    public enum ValueType
    {
        Boolean,
        Character,
        Int8,
        UInt8,
        Int16,
        UInt16, 
        Int32,
        UInt32,
        Int64,
        UInt64,
        Float,
        Double,
        Decimal,
        String,
        DateTime,
        TimeSpan,
        Uri,
        Guid
    }

    /// <summary>
    /// Gets the value type.
    /// </summary>
    public ValueType Type { get; }

    /// <summary>
    /// Gets the value as object.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(VariantValue value)
    {
        Type = value.Type;
        Value = value.Value;
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(bool value)
    {
        Type = ValueType.Boolean;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(bool value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static explicit operator bool(VariantValue value) => value?.GetBool() ?? default;

    private bool GetBool()
    {
        if (Type == ValueType.Boolean) return (bool)Value;
        throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Boolean));
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(char value)
    {
        Type = ValueType.Character;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(char value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator char(VariantValue value) => value?.GetChar() ?? default;

    private char GetChar()
    {
        if (Type == ValueType.Character) return (char)Value;
        throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Character));
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(sbyte value)
    {
        Type = ValueType.Int8;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(sbyte value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator sbyte(VariantValue value) => value?.GetSByte() ?? default;

    private sbyte GetSByte()
    {
        if (Type == ValueType.Int8) return (sbyte)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Int8));
        try
        {
            return Convert.ToSByte(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(byte value)
    {
        Type = ValueType.UInt8;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(byte value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator byte(VariantValue value) => value?.GetByte() ?? default;

    private byte GetByte()
    {
        if (Type == ValueType.UInt8) return (byte)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.UInt8));
        try
        {
            return Convert.ToByte(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(short value)
    {
        Type = ValueType.Int16;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(short value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator short(VariantValue value) => value?.GetShort() ?? default;

    private short GetShort()
    {
        if (Type == ValueType.Int16) return (short)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Int16));
        try
        {
            return Convert.ToInt16(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(ushort value)
    {
        Type = ValueType.UInt16;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(ushort value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator ushort(VariantValue value) => value?.GetUShort() ?? default;

    private ushort GetUShort()
    {
        if (Type == ValueType.UInt16) return (ushort)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.UInt16));
        try
        {
            return Convert.ToUInt16(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(int value)
    {
        Type = ValueType.Int32;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(int value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator int(VariantValue value) => value?.GetInt() ?? default;

    private int GetInt()
    {
        if (Type == ValueType.Int32) return (int)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Int32));
        try
        {
            return Convert.ToInt32(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(uint value)
    {
        Type = ValueType.UInt32;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(uint value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator uint(VariantValue value) => value?.GetUInt() ?? default;

    private uint GetUInt()
    {
        if (Type == ValueType.UInt32) return (uint)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.UInt32));
        try
        {
            return Convert.ToUInt32(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(long value)
    {
        Type = ValueType.Int64;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(long value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator long(VariantValue value) => value?.GetLong() ?? default;

    private long GetLong()
    {
        if (Type == ValueType.Int64) return (long)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Int64));
        try
        {
            return Convert.ToInt64(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(ulong value)
    {
        Type = ValueType.UInt64;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(ulong value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator ulong(VariantValue value) => value?.GetULong() ?? default;

    private ulong GetULong()
    {
        if (Type == ValueType.UInt64) return (ulong)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.UInt64));
        try
        {
            return Convert.ToUInt64(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(float value)
    {
        Type = ValueType.Float;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(float value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator float(VariantValue value) => value?.GetFloat() ?? default;

    private float GetFloat()
    {
        if (Type == ValueType.Float) return (float)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Float));
        try
        {
            return Convert.ToSingle(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(double value)
    {
        Type = ValueType.Double;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(double value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator double(VariantValue value) => value?.GetDouble() ?? default;

    private double GetDouble()
    {
        if (Type == ValueType.Double) return (double)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Double));
        try
        {
            return Convert.ToDouble(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(decimal value)
    {
        Type = ValueType.Decimal;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(decimal value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator decimal(VariantValue value) => value?.GetDecimal() ?? default;

    private decimal GetDecimal()
    {
        if (Type == ValueType.Decimal) return (decimal)Value;
        if (!IsNumberType()) throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Decimal));
        try
        {
            return Convert.ToDecimal(Value);
        }
        catch (Exception e)
        {
            throw new DataOutOfRangeException(e.Message);
        }
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(string value)
    {
        Type = ValueType.String;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(string value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator string(VariantValue value) => value?.GetString();

    private string GetString()
    {
        if (Type == ValueType.String) return (string)Value;
        throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.String));
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(DateTime value)
    {
        Type = ValueType.DateTime;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(DateTime value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator DateTime(VariantValue value) => value?.GetDateTime() ?? default;

    private DateTime GetDateTime()
    {
        if (Type == ValueType.DateTime) return (DateTime)Value;
        throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.DateTime));
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(TimeSpan value)
    {
        Type = ValueType.TimeSpan;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(TimeSpan value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator TimeSpan(VariantValue value) => value?.GetTimeSpan() ?? default;

    private TimeSpan GetTimeSpan()
    {
        if (Type == ValueType.TimeSpan) return (TimeSpan)Value;
        throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.TimeSpan));
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(Uri value)
    {
        Type = ValueType.Uri;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(Uri value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator Uri(VariantValue value) => value?.GetUri();

    private Uri GetUri()
    {
        if (Type == ValueType.Uri) return (Uri)Value;
        throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Uri));
    }

    /// <summary>
    /// Initializes a new class instance with the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public VariantValue(Guid value)
    {
        Type = ValueType.Guid;
        Value = value;
    }

    /// <summary>
    /// Performs an implicit conversion of the provided value.
    /// </summary>
    /// <param name="value">The provided value.</param>
    public static implicit operator VariantValue(Guid value) => new(value);

    /// <summary>
    /// Performs an explicit conversion of the provided value.
    /// </summary>
    /// <returns>The value.</returns>
    public static explicit operator Guid(VariantValue value) => value?.GetGuid() ?? default;

    private Guid GetGuid()
    {
        if (Type == ValueType.Guid) return (Guid)Value;
        throw new DataInvalidException(string.Format(Resources.Resource1.TypeConversionFailed, Value.GetType(), ValueType.Guid));
    }

    /// <summary>
    /// Gets a string representation of the value.
    /// </summary>
    /// <returns>The string representation of the value.</returns>
    public override string ToString()
    {
        return Value?.ToString();
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">The object to compare with this object.</param>
    /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
    public override bool Equals(object other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return Equals((VariantValue)other);
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">The object to compare with this object.</param>
    /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
    public bool Equals(VariantValue other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        //if (Type != other.Type) return false;

        return Type switch
        {
            ValueType.Boolean => GetBool() == other.GetBool(),
            ValueType.Character => GetChar() == other.GetChar(),
            ValueType.Int8 => GetSByte() == other.GetSByte(),
            ValueType.UInt8 => GetByte() == other.GetByte(),
            ValueType.Int16 => GetShort() == other.GetShort(),
            ValueType.UInt16 => GetUShort() == other.GetUShort(),
            ValueType.Int32 => GetInt() == other.GetInt(),
            ValueType.UInt32 => GetUInt() == other.GetUInt(),
            ValueType.Int64 => GetLong() == other.GetLong(),
            ValueType.UInt64 => GetULong() == other.GetULong(),
            ValueType.Float => GetFloat().EqualsWithPrecision(other.GetFloat()),
            ValueType.Double => GetDouble().EqualsWithPrecision(other.GetDouble()),
            ValueType.Decimal => GetDecimal().EqualsWithPrecision(other.GetDecimal()),
            ValueType.String => GetString().Equals(other.GetString(), StringComparison.OrdinalIgnoreCase),
            ValueType.DateTime => GetDateTime().Equals(other.GetDateTime()),
            ValueType.TimeSpan => GetTimeSpan().Equals(other.GetTimeSpan()),
            ValueType.Uri => GetUri().Equals(other.GetUri()),
            ValueType.Guid => GetGuid().Equals(other.GetGuid()),
            _ => throw new ArgumentOutOfRangeException(nameof(Type))
        };
    }

    /// <summary>
    /// Gets a hash code for the current object.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return ((Value != null ? Value.GetHashCode() : 0) * 397) ^ (int)Type;
        }
    }

    private bool IsIntegerType()
    {
        return Type is 
            ValueType.Int8 or ValueType.UInt8 or
            ValueType.Int16 or ValueType.UInt16 or
            ValueType.Int32 or ValueType.UInt32 or
            ValueType.Int64 or ValueType.UInt64;
    }

    private bool IsFloatingPointType()
    {
        return Type is 
            ValueType.Float or ValueType.Double or 
            ValueType.Decimal;
    }

    private bool IsNumberType()
    {
        return Type is 
            ValueType.Int8 or ValueType.UInt8 or
            ValueType.Int16 or ValueType.UInt16 or
            ValueType.Int32 or ValueType.UInt32 or
            ValueType.Int64 or ValueType.UInt64 or
            ValueType.Float or ValueType.Double or 
            ValueType.Decimal;
    }
}