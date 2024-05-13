namespace ifm.IoTCore.Common.UnitTests;

using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class HexStringEncoderTests
{
    [Test]
    public void ConvertByteArrayToHexStringNull_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => HexStringEncoder.ByteArrayToHexString(null));
    }

    [Test]
    public void ConvertHexStringToByteArrayNull_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => HexStringEncoder.HexStringToByteArray(null));
    }

    [Test]
    public void ConvertByteArrayToHexString_Success()
    {
        var str = HexStringEncoder.ByteArrayToHexString(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });

        Assert.That(str == "000102030405060708090A0B0C0D0E0F1011121314");
    }

    [Test]
    public void ConvertHexStringToByteArray_Success()
    {
        const string str = "000102030405060708090A0B0C0D0E0F1011121314";
        var bytes = HexStringEncoder.HexStringToByteArray(str);

        Assert.That(bytes.SequenceEqual(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }));
    }

    [Test]
    public void ConvertHexStringToByteArrayOddCount_LastDigitDropped()
    {
        const string str = "00012";
        var bytes = HexStringEncoder.HexStringToByteArray(str);

        Assert.That(bytes.SequenceEqual(new byte[] { 0, 1 }));
    }

    [Test]
    public void ConvertDecimalStringToHexStringNull_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => HexStringEncoder.StringToHexString(null));
    }

    [Test]
    public void ConvertHexStringToDecimalStringNull_Fail()
    {
        Assert.Throws<ArgumentNullException>(() => HexStringEncoder.HexStringToString(null));
    }

    [Test]
    public void ConvertStringToHexString_Success()
    {
        var str = HexStringEncoder.StringToHexString("ABCDEF");

        Assert.That(str == "414243444546");
    }

    [Test]
    public void ConvertHexStringToString_Success()
    {
        var str = HexStringEncoder.HexStringToString("414243444546");

        Assert.That(str == "ABCDEF");
    }
}