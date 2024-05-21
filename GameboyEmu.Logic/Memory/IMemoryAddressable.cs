namespace GameboyEmu.Logic.Memory;

public interface IMemoryAddressable : IMemoryReadable
{
    public void WriteByte(ushort address, byte value);
}