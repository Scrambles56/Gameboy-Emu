namespace GameboyEmu.Logic.Cpu.Extensions;

public static class Extensions
{
    public static (byte high, byte low) ToBytes(this ushort value) => ((byte)(value >> 8), (byte)(value & 0xFF));

    public static bool IsBetween(this ushort value, ushort min, ushort max)
        => min <= value && value <= max;
}