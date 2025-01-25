using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Gpu;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

public class IOBus : IMemoryAddressable
{
    private readonly LcdControl _lcdControl;
    private readonly ushort _lowerBound = 0xFF00;
    private readonly byte[] _data = new byte[128];
    
    public IOBus(LcdControl lcdControl)
    {
        _lcdControl = lcdControl;
    }

    public byte ReadByte(ushort address)
    {
        if (address == 0xFF00)
        {
            return 0xFF;
        }
        
        if (address.IsBetween(0xFF40, 0xFF4B))
        {
            return _lcdControl.ReadByte(address);
        }
        
        return _data[address - _lowerBound];
    }

    public void WriteByte(ushort address, byte value)
    {
        if (address.IsBetween(0xFF40, 0xFF4B))
        {
            _lcdControl.WriteByte(address, value);
            return;
        }
        
        
        _data[address - _lowerBound] = value;
    }
    
    public void RequestInterrupt(Interrupt interrupt)
    {
        var requestedInterrupts = ReadByte(0xFF0F);
        requestedInterrupts |= (byte)interrupt;
        WriteByte(0xFF0F, requestedInterrupts);
    }
    
    public void ClearInterrupt(Interrupt interrupt)
    {
        var requestedInterrupts = ReadByte(0xFF0F);
        requestedInterrupts &= (byte)~interrupt;
        WriteByte(0xFF0F, requestedInterrupts);
    }
    
}

public enum Interrupt
{
    VBlank = 0b1,
    LcdStat = 0b10,
    Timer = 0b100,
    Serial = 0b1000,
    Joypad = 0b10000
}