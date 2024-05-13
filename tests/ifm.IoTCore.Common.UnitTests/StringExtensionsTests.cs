namespace ifm.IoTCore.Common.UnitTests;

using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class StringExtensionsTests
{
    [Test]
    public void EndsWith_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.EndsWith(null, '\0'));

        Assert.Throws<ArgumentNullException>(() => StringExtensions.EndsWith(string.Empty, '\0'));
    }

    [Test]
    public void EndsWith_Success()
    {
        var s = "HuHu";
        Assert.That(StringExtensions.EndsWith(s, 'u'));
        Assert.That(StringExtensions.EndsWith(s, 'z'), Is.False);
    }


    [Test]
    public void StartsWith_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.StartsWith(null, '\0'));

        Assert.Throws<ArgumentNullException>(() => StringExtensions.StartsWith(string.Empty, '\0'));
    }

    [Test]
    public void StartsWith_Success()
    {
        var s = "HuHu";
        Assert.That(StringExtensions.StartsWith(s, 'H'));
        Assert.That(StringExtensions.StartsWith(s, 'h'), Is.False);
    }

    [Test]
    public void LeftWithChar_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.Left(null, '\0'));

        Assert.Throws<ArgumentOutOfRangeException>(() => StringExtensions.Left("1,2,3,4,5", '\0', 10));
    }

    [Test]
    public void LeftWithChar_Success()
    {
        var s = "1,2,3,4,5";
        var left = s.Left(',');
        Assert.That(left == "1");

        left = s.Left(',', 0, true);
        Assert.That(left == "1,");

        left = s.Left(',', 4);
        Assert.That(left == "1,2,3");

        left = s.Left(',', 4, true);
        Assert.That(left == "1,2,3,");

        left = s.Left(',', 8);
        Assert.That(left == null);

        left = s.Left(',', 8, true);
        Assert.That(left == null);
    }


    [Test]
    public void LeftWithString_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.Left(null, ""));

        Assert.Throws<ArgumentOutOfRangeException>(() => StringExtensions.Left("1,2,3,4,5", "", 10));
    }

    [Test]
    public void LeftWithString_Success()
    {
        var s = "1,2,3,4,5";
        var left = s.Left(",");
        Assert.That(left == "1");

        left = s.Left(",", 0, true);
        Assert.That(left == "1,");

        left = s.Left(",", 4);
        Assert.That(left == "1,2,3");

        left = s.Left(",", 4, true);
        Assert.That(left == "1,2,3,");

        left = s.Left(",", 8);
        Assert.That(left == null);

        left = s.Left(",", 8, true);
        Assert.That(left == null);
    }

    [Test]
    public void RightWithChar_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.Right(null, '\0'));

        Assert.Throws<ArgumentOutOfRangeException>(() => StringExtensions.Right("1,2,3,4,5", '\0', 10));
    }

    [Test]
    public void RightWithChar_Success()
    {
        var s = "1,2,3,4,5";
        var right = s.Right(',');
        Assert.That(right == "2,3,4,5");

        right = s.Right(',', 0, true);
        Assert.That(right == ",2,3,4,5");

        right = s.Right(',', 4);
        Assert.That(right == "4,5");

        right = s.Right(',', 4, true);
        Assert.That(right == ",4,5");

        right = s.Right(',', 8);
        Assert.That(right == null);

        right = s.Right(',', 8, true);
        Assert.That(right == null);
    }


    [Test]
    public void RightWithString_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.Right(null, ""));

        Assert.Throws<ArgumentOutOfRangeException>(() => StringExtensions.Right("1,2,3,4,5", "", 10));
    }

    [Test]
    public void RightWithString_Success()
    {
        var s = "1,2,3,4,5";
        var right = s.Right(",");
        Assert.That(right == "2,3,4,5");

        right = s.Right(",", 0, true);
        Assert.That(right == ",2,3,4,5");

        right = s.Right(",", 4);
        Assert.That(right == "4,5");

        right = s.Right(",", 4, true);
        Assert.That(right == ",4,5");

        right = s.Right(",", 8);
        Assert.That(right == null);

        right = s.Right(",", 8, true);
        Assert.That(right == null);
    }

    [Test]
    public void GetFirstToken_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.GetFirstToken(null, '\0'));

        Assert.Throws<ArgumentOutOfRangeException>(() => StringExtensions.GetFirstToken("1,2,3,4,5", ',', 10));
    }

    [Test]
    public void GetFirstToken_Success()
    {
        var s = "1,2,3,4,5";
        var token = s.GetFirstToken(',');
        Assert.That(token == "1");

        token = s.GetFirstToken(',', 0, true);
        Assert.That(token == "1,");

        token = s.GetFirstToken(',', 4, true);
        Assert.That(token == "3,");

        token = s.GetFirstToken(',', 8);
        Assert.That(token == "1,2,3,4,5");

        token = s.GetFirstToken('/');
        Assert.That(token == "1,2,3,4,5");
    }

    [Test]
    public void GetLastToken_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.GetLastToken(null, '\0'));

        Assert.Throws<ArgumentOutOfRangeException>(() => StringExtensions.GetLastToken("1,2,3,4,5", ',', 10));
    }


    [Test]
    public void GetLastToken_Success()
    {
        var s = "1,2,3,4,5";
        var token = s.GetLastToken(',');
        Assert.That(token == "1,2,3,4,5");

        token = s.GetLastToken(',', 4, true);
        Assert.That(token == ",3,4,5");

        token = s.GetLastToken(',', 8);
        Assert.That(token == "5");

        token = s.GetLastToken(',', 8, true);
        Assert.That(token == ",5");

        token = s.GetLastToken('/');
        Assert.That(token == "1,2,3,4,5");
    }

    [Test]
    public void RemoveFirstToken_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.RemoveFirstToken(null, '\0'));
    }

    [Test]
    public void RemoveFirstToken_Success()
    {
        var s = "1,2,3,4,5";

        var remainder = s.RemoveFirstToken(',');
        Assert.That(remainder == "2,3,4,5");

        remainder = s.RemoveFirstToken('/');
        Assert.That(remainder == "1,2,3,4,5");
    }

    [Test]
    public void RemoveLastToken_InvalidParameter_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.RemoveLastToken(null, '\0'));
    }

    [Test]
    public void RemoveLastToken_Success()
    {
        var s = "1,2,3,4,5";

        var remainder = s.RemoveLastToken(',');
        Assert.That(remainder == "1,2,3,4");

        remainder = s.RemoveLastToken('/');
        Assert.That(remainder == "1,2,3,4,5");
    }

    [Test]
    public void GetUtf8String_Success()
    {
        var bytes = new byte[] {65, 66, 67, 195, 182, 195, 188, 88, 89, 90};

        var s = bytes.GetUtf8String();

        Assert.That(s == "ABCöüXYZ");
    }


    [Test]
    public void GetUtf8Bytes_Success()
    {
        var s = "ABCöüXYZ";

        var bytes = s.GetUtf8Bytes();

        Assert.That(bytes.SequenceEqual(new byte[] { 65, 66, 67, 195, 182, 195, 188, 88, 89, 90 }));
    }
}