using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Gpu;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

public class IOBus : IMemoryAddressable
{
    private readonly LcdControl _lcdControl;
    private readonly ushort _lowerBound = 0xFF00;
    private readonly byte[] _data = new byte[127];
    
    public IOBus(LcdControl lcdControl)
    {
        _lcdControl = lcdControl;
    }


    public byte ReadByte(ushort address)
    {
        if (address.IsBetween(0xFF40, 0xFF4B))
        {
            return _lcdControl.ReadByte(address);
        }
        
        return _data[address - _lowerBound];
    }

    public void WriteByte(ushort address, byte value)
    {
        _data[address - _lowerBound] = value;
    }
}