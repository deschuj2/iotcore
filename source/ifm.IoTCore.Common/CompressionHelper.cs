namespace ifm.IoTCore.Common;

using System;
using System.IO;
using System.IO.Compression;

/// <summary>
/// Provides methods for compressing and decompressing data
/// </summary>
public static class CompressionHelper
{
    // Zip Local file header signature that indicates a Zip file
    private const uint ZipHeader = 0x04034b50;

    // GZip header signature that indicates a GZip file
    private const ushort GZipHeader = 0x8b1f;

    // Some other not possible but not implemented compression formats.
    //private const ushort TarHeader = 0x9d1f;
    //private const ushort LzhHeader = 0xa01f;
    //private const uint Bzip2Header = 0xff685a42;
    //private const uint LZipHeader = 0x50495a4c;
    
    /// <summary>
    /// Checks the first 4 bytes if data is a possible zip file stream.
    /// The first 4 bytes needed to check if the rest of the
    /// array is a zip compressed format.
    /// </summary>
    /// <param name="data">An array of min. 4 bytes.</param>
    /// <returns>true if it is a Zip format otherwise false.</returns>
    /// <exception cref="ArgumentException">In case of an invalid array length. 4 bytes needed.</exception>
    public static bool IsZipFormat(byte[] data)
    {
        if (data.Length < 4)
        {
            throw new ArgumentException($"Invalid length of argument '{nameof(data)}'. The array length must be >= 4.");
        }

        return BitConverter.ToUInt32(data, 0) == ZipHeader;
    }

    /// <summary>
    /// Checks the first 2 bytes if data is a possible gzip file stream.
    /// The first 2 bytes needed to check if the rest of the
    /// array is a gzip compressed format.
    /// </summary>
    /// <param name="data">An array of min. 2 bytes.</param>
    /// <returns>true if it is a GZip format otherwise false.</returns>
    /// <exception cref="ArgumentException">In case of an invalid array length. 2 bytes needed.</exception>
    public static bool IsGZipFormat(byte[] data)
    {
        if (data.Length < 2)
        {
            throw new ArgumentException($"Invalid length of argument '{nameof(data)}'. The array length must be >= 2.");
        }

        return BitConverter.ToUInt16(data, 0) == GZipHeader;
    }

    /// <summary>
    /// Compresses the provided data.
    /// </summary>
    /// <param name="data">The data to compress.</param>
    /// <param name="compressionLevel">The compression level. Default: optimal</param>
    /// <returns>The compressed data.</returns>
    public static byte[] GZipCompress(byte[] data, CompressionLevel compressionLevel)
    {
        return GZipCompress(data, compressionLevel.ToString().ToLower());
    }

    /// <summary>
    /// Compresses the provided data.
    /// </summary>
    /// <param name="data">The data to compress.</param>
    /// <param name="compressionLevel">The compression level. Default: optimal</param>
    /// <returns>The compressed data.</returns>
    public static byte[] GZipCompress(byte[] data, string compressionLevel)
    {
        var level = CompressionLevel.Optimal;
        foreach (var value in Enum.GetValues(typeof(CompressionLevel)))
        {
            if (value.ToString().ToLower().Equals(compressionLevel.ToLower()))
            {
                level = (CompressionLevel)value;
            }
        }

        MemoryStream stream;
        using (var gZipStream = new GZipStream(new MemoryStream(), level))
        {
            gZipStream.Write(data, 0, data.Length);
            stream = (MemoryStream)gZipStream.BaseStream;
        }

        return stream.ToArray();
    }

    /// <summary>
    /// Decompresses the provided data.
    /// </summary>
    /// <param name="data">The data to decompress.</param>
    /// <returns>The decompressed data.</returns>
    public static byte[] GZipDeCompress(byte[] data)
    {
        if (!IsGZipFormat(data))
        {
            return data;
        }

        using (var gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
        {
            int len;
            var decompressedBytes = new byte[data.Length];
            var stream = new MemoryStream();

            while ((len = gZipStream.Read(decompressedBytes, 0, data.Length)) > 0)
            {
                stream.Write(decompressedBytes, 0, len);
            }

            return stream.ToArray();
        }
    }
}