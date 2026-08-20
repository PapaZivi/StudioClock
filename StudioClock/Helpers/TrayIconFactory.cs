using System.IO.Compression;
using Avalonia.Controls;

namespace StudioClock.Helpers;

public static class TrayIconFactory
{
    public static WindowIcon Create()
    {
        const int size = 32;
        var pixels = new byte[size * size * 4];
        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
        {
            var ring = Math.Abs(Math.Sqrt(Math.Pow(x - 15.5, 2) + Math.Pow(y - 15.5, 2)) - 13) < 1.4;
            var digit = Dot(x, y, 8, 11) || Dot(x, y, 13, 11) || Dot(x, y, 19, 11) || Dot(x, y, 24, 11)
                || Dot(x, y, 8, 16) || Dot(x, y, 13, 16) || Dot(x, y, 19, 16) || Dot(x, y, 24, 16)
                || Dot(x, y, 8, 21) || Dot(x, y, 13, 21) || Dot(x, y, 19, 21) || Dot(x, y, 24, 21);
            if (!ring && !digit) continue;
            var offset = (y * size + x) * 4; pixels[offset] = 0xCC; pixels[offset + 1] = 0x10; pixels[offset + 2] = 0x10; pixels[offset + 3] = 0xFF;
        }
        return new WindowIcon(new MemoryStream(EncodePng(pixels, size, size)));
    }

    private static bool Dot(int x, int y, int cx, int cy) => (x - cx) * (x - cx) + (y - cy) * (y - cy) <= 2;
    private static byte[] EncodePng(byte[] rgba, int width, int height)
    {
        using var output = new MemoryStream();
        output.Write([137, 80, 78, 71, 13, 10, 26, 10]);
        using var header = new MemoryStream(); WriteBig(header, (uint)width); WriteBig(header, (uint)height); header.Write([8, 6, 0, 0, 0]); WriteChunk(output, "IHDR", header.ToArray());
        using var raw = new MemoryStream(); for (var y = 0; y < height; y++) { raw.WriteByte(0); raw.Write(rgba, y * width * 4, width * 4); }
        using var compressed = new MemoryStream(); using (var zlib = new ZLibStream(compressed, CompressionLevel.SmallestSize, true)) raw.ToArray().AsSpan().CopyToStream(zlib);
        WriteChunk(output, "IDAT", compressed.ToArray()); WriteChunk(output, "IEND", []); return output.ToArray();
    }
    private static void CopyToStream(this ReadOnlySpan<byte> data, Stream stream) => stream.Write(data);
    private static void WriteChunk(Stream output, string type, byte[] data)
    {
        WriteBig(output, (uint)data.Length); var typeBytes = System.Text.Encoding.ASCII.GetBytes(type); output.Write(typeBytes); output.Write(data);
        var crcData = new byte[typeBytes.Length + data.Length]; typeBytes.CopyTo(crcData, 0); data.CopyTo(crcData, typeBytes.Length); WriteBig(output, Crc32(crcData));
    }
    private static void WriteBig(Stream stream, uint value) => stream.Write([(byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value]);
    private static uint Crc32(byte[] data) { var crc = 0xFFFFFFFFu; foreach (var b in data) { crc ^= b; for (var i = 0; i < 8; i++) crc = (crc >> 1) ^ (0xEDB88320u & (uint)-(int)(crc & 1)); } return ~crc; }
}
