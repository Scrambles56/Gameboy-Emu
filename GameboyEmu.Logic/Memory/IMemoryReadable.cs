namespace GameboyEmu.Logic.Memory;

public interface IMemoryReadable
{
    byte ReadByte(ushort address);
}