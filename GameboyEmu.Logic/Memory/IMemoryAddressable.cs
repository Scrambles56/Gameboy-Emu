using System.Diagnostics;

namespace GameboyEmu.Logic.Memory;

public interface IMemoryAddressable
{
    public byte ReadByte(ushort address);
    public void WriteByte(ushort address, byte value);
}