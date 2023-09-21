using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

public class IOBus : IMemoryAddressable
{
    private readonly ushort _lowerBound = 0xFF00;
    private readonly byte[] _data = new byte[127];
    
    public IOBus()
    {
    }


    public byte ReadByte(ushort address)
    {
        return _data[address - _lowerBound];
    }

    public void WriteByte(ushort address, byte value)
    {
        _data[address - _lowerBound] = value;
    }
}