namespace ifm.IoTCore.Common.UnitTests;

using System.IO.Compression;
using NUnit.Framework;

[TestFixture]
public class CompressionHelperTests
{
    private static readonly byte[] UnCompressed = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
    
    private static readonly byte[] GZipOptimalCompression =
    {
        0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0a, 0x63, 0x64, 0x62, 0x66, 0x61, 0x65, 0x03, 0x00,
        0x24, 0x77, 0xf6, 0x81, 0x06, 0x00, 0x00, 0x00
    };

    private static readonly byte[] GZipFastestCompression =
    {
        0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x0a, 0x63, 0x64, 0x62, 0x66, 0x61, 0x65, 0x03, 0x00,
        0x24, 0x77, 0xf6, 0x81, 0x06, 0x00, 0x00, 0x00
    };

    private static readonly byte[] GZipNoCompression =
    {
        0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x0a, 0x01, 0x06, 0x00, 0xf9, 0xff, 0x01, 0x02, 0x03,
        0x04, 0x05, 0x06, 0x24, 0x77, 0xf6, 0x81, 0x06, 0x00, 0x00, 0x00
    };
    
    private static readonly byte[] GZipSmallestSizeCompression =
    {
        0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x0a, 0x63, 0x64, 0x62, 0x66, 0x61, 0x65, 0x03, 0x00,
        0x24, 0x77, 0xf6, 0x81, 0x06, 0x00, 0x00, 0x00
    };

    private static readonly byte[] ZipCompression =
    {
        0x50, 0x4b, 0x03, 0x04, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2f, 0x77, 0x54, 0x56, 0x24, 0x77, 0xf6, 0x81,
        0x06, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x62, 0x6c, 0x75, 0x62, 0x62, 0x65,
        0x72, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x50, 0x4b, 0x01, 0x02, 0x3f, 0x00, 0x0a, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x2f, 0x77, 0x54, 0x56, 0x24, 0x77, 0xf6, 0x81, 0x06, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x07,
        0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x62,
        0x6c, 0x75, 0x62, 0x62, 0x65, 0x72, 0x0a, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x18, 0x00,
        0x91, 0x23, 0x04, 0x45, 0x33, 0x45, 0xd9, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x50, 0x4b, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00,
        0x59, 0x00, 0x00, 0x00, 0x2b, 0x00, 0x00, 0x00, 0x00, 0x00
    };

    [Test]
    public void InValidGZipFileLength()
    {
        System.IO.File.WriteAllBytes("UnCompressed.bin", UnCompressed);
        Assert.That(() =>
        {
            CompressionHelper.IsGZipFormat(new byte[] { 0x1f });
        }, Throws.InstanceOf<System.ArgumentException>());
    }

    [Test]
    public void InValidZipFileLength()
    {
        Assert.That(() =>
        {
            CompressionHelper.IsZipFormat(new byte[] { 0x50, 0x4b, 0x03 });
        }, Throws.InstanceOf<System.ArgumentException>());
    }

    [Test]
    public void IsValidGZipFormat()
    {
        if (!CompressionHelper.IsGZipFormat(GZipOptimalCompression))
        {
            Assert.Fail("The given value is not a valid GZip format.");
        }
    }

    [Test]
    public void IsInValidGZipFormat_String()
    {
        if (CompressionHelper.IsGZipFormat(UnCompressed))
        {
            Assert.Fail("The given data is not an invalid GZip format.");
        }
    }

    [Test]
    public void IsInValidGZipFormat_Zip()
    {
        if (CompressionHelper.IsGZipFormat(ZipCompression))
        {
            Assert.Fail("The given data is not an invalid GZip format.");
        }
    }

    [Test]
    public void IsValidZipFormat()
    {
        if (!CompressionHelper.IsZipFormat(ZipCompression))
        {
            Assert.Fail("The given value is not a valid Zip format.");
        }
    }

    [Test]
    public void IsInValidZipFormat_GZip()
    {
        if (CompressionHelper.IsZipFormat(GZipOptimalCompression))
        {
            Assert.Fail("The given data is not an invalid Zip format.");
        }
    }

    [Test]
    public void IsInValidZipFormat_String()
    {
        if (CompressionHelper.IsZipFormat(UnCompressed))
        {
            Assert.Fail("The given data is not an invalid Zip format.");
        }
    }

    [Test]
    public void GZipDeCompress_InvalidData()
    {
        var decompressed = CompressionHelper.GZipDeCompress(new byte[] { 0x53, 0x21, 0x53 });
        Assert.That(new byte[] { 0x53, 0x21, 0x53 }, Is.EqualTo(decompressed));
    }
    
    [Test]
    [TestCase(CompressionLevel.Optimal)]
    [TestCase(CompressionLevel.Fastest)]
    [TestCase(CompressionLevel.SmallestSize)]
    [TestCase(CompressionLevel.NoCompression)]
    public void GZipDeCompress(CompressionLevel level)
    {
        switch (level)
        {
            case CompressionLevel.Optimal:
            {
                var decompressed = CompressionHelper.GZipDeCompress(GZipOptimalCompression);
                Assert.That(UnCompressed, Is.EqualTo(decompressed));
                break;
            }
            case CompressionLevel.Fastest:
            {
                var decompressed = CompressionHelper.GZipDeCompress(GZipFastestCompression);
                Assert.That(UnCompressed, Is.EqualTo(decompressed));
                break;
            }
            case CompressionLevel.SmallestSize:
            {
                var decompressed = CompressionHelper.GZipDeCompress(GZipSmallestSizeCompression);
                Assert.That(UnCompressed, Is.EqualTo(decompressed));
                break;
            }
            case CompressionLevel.NoCompression:
            {
                var decompressed = CompressionHelper.GZipDeCompress(GZipNoCompression);
                Assert.That(UnCompressed, Is.EqualTo(decompressed));
                break;
            }
            default:
            {
                Assert.Fail("InValid compression level");
                break;
            }
        }
    }

    [Test]
    [TestCase(CompressionLevel.Optimal)]
    [TestCase(CompressionLevel.Fastest)]
    [TestCase(CompressionLevel.SmallestSize)]
    [TestCase(CompressionLevel.NoCompression)]
    public void GZipCompress(CompressionLevel level)
    {
        switch (level)
        {
            case CompressionLevel.Optimal:
            {
                var compressed = CompressionHelper.GZipCompress(UnCompressed, "optional");
                var uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                compressed = CompressionHelper.GZipCompress(UnCompressed, CompressionLevel.Optimal);
                uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                break;
            }
            case CompressionLevel.Fastest:
            {
                var compressed = CompressionHelper.GZipCompress(UnCompressed, "fastest");
                var uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                compressed = CompressionHelper.GZipCompress(UnCompressed, CompressionLevel.Fastest);
                uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                break;
            }
            case CompressionLevel.SmallestSize:
            {
                var compressed = CompressionHelper.GZipCompress(UnCompressed, "smallestsize");
                var uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                compressed = CompressionHelper.GZipCompress(UnCompressed, CompressionLevel.SmallestSize);
                uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                break;
            }
            case CompressionLevel.NoCompression:
            {
                var compressed = CompressionHelper.GZipCompress(UnCompressed, "nocompression");
                var uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                compressed = CompressionHelper.GZipCompress(UnCompressed, CompressionLevel.NoCompression);
                uncompressed = CompressionHelper.GZipDeCompress(compressed);
                Assert.That(UnCompressed, Is.EqualTo(uncompressed));

                break;
            }
            default:
                Assert.Fail("InValid compression level");
                break;
        }
    }
}