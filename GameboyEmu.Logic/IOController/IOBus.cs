using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Gpu;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

public class IOBus(
    LcdControl lcdControl,
    InputControl inputControl,
    TimerController timerController,
    InterruptsController interruptsController
) : IMemoryAddressable
{
    private readonly ushort _lowerBound = 0xFF00;
    private readonly byte[] _data = new byte[128];
    
    public byte ReadByte(ushort address)
    {
        if (address == 0xFF00)
        {
            return inputControl.ReadByte(address);
        }
        
        if (address.IsBetween(0xFF04, 0xFF07))
        {
            return timerController.ReadByte(address);
        }

        if (address == 0xFF0F)
        {
            return interruptsController.ReadByte(address);
        }

        if (address.IsBetween(0xFF40, 0xFF4B))
        {
            return lcdControl.ReadByte(address);
        }

        return _data[address - _lowerBound];
    }

    public void WriteByte(ushort address, byte value)
    {
        if (address == 0xFF00)
        {
            inputControl.WriteByte(address, value);
            return;
        }
        
        if (address.IsBetween(0xFF04, 0xFF07))
        {
            timerController.WriteByte(address, value);
            return;
        }

        if (address == 0xFF0F)
        {
            interruptsController.WriteByte(address, value);
            return;
        }

        if (address.IsBetween(0xFF40, 0xFF4B))
        {
            lcdControl.WriteByte(address, value);
            return;
        }


        _data[address - _lowerBound] = value;
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