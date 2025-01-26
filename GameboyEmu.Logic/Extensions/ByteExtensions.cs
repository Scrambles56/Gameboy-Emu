namespace GameboyEmu.Logic.Extensions;

public static class ByteExtensions
{
    public static bool IsBitSet(this byte b, int bit) => (b & (1 << bit)) != 0;
    
    public static byte SetBit(this byte b, int bit) => (byte)(b | (1 << bit));
    
    public static byte ClearBit(this byte b, int bit) => (byte)(b & ~(1 << bit));
    
    public static string ToBinaryString(this byte b) => Convert.ToString(b, 2).PadLeft(8, '0');
}