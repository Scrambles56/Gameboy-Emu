using System.Diagnostics;
using GameboyEmu.Logic.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

public class InterruptsController  : IMemoryAddressable
{
    public bool InterruptMasterEnabledFlag { get; set; }
    public bool SetInterruptMasterEnabledFlag { get; set; }
    
    private byte _enabledInterrupts = 0x00;
    
    private byte _requestedInterrupts = 0x00;
    
    private string _enabledInterruptsString => _enabledInterrupts.ToBinaryString();
    private string _requestedInterruptsString => _requestedInterrupts.ToBinaryString();
    

    public byte ReadByte(ushort address)
    {
        Debug.Assert(address is 0xFFFF or 0xFF0F, "Invalid address for interrupts controller");

        switch (address)
        {
            case 0xFFFF:
            {
                var mask = (byte)(InterruptMasterEnabledFlag ? 0xFF : 0x00);
                return (byte)(_enabledInterrupts & mask);
            }
            case 0xFF0F:
                return _requestedInterrupts;
            default:
                throw new ArgumentOutOfRangeException(nameof(address), address,
                    "Invalid address for interrupts controller");
        }
    }

    public void WriteByte(ushort address, byte value)
    {
        Debug.Assert(address is 0xFFFF or 0xFF0F, "Invalid address for interrupts controller");
        
        switch (address)
        {
            case 0xFFFF:
                _enabledInterrupts = value;
                return;
            case 0xFF0F:
                _requestedInterrupts = value;
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(address), address, "Invalid address for interrupts controller");
        }
    }
    

    public Interrupt? GetRequestedInterrupt()
    {
        var requested = _requestedInterrupts & _enabledInterrupts;

        // Return first enabled interrupt, from smallest bit

        if (requested == 0)
        {
            return null;
        }

        if ((requested & (byte)Interrupt.VBlank) != 0)
        {
            return Interrupt.VBlank;
        }

        if ((requested & (byte)Interrupt.LcdStat) != 0)
        {
            return Interrupt.LcdStat;
        }

        if ((requested & (byte)Interrupt.Timer) != 0)
        {
            return Interrupt.Timer;
        }

        if ((requested & (byte)Interrupt.Serial) != 0)
        {
            return Interrupt.Serial;
        }

        if ((requested & (byte)Interrupt.Joypad) != 0)
        {
            return Interrupt.Joypad;
        }

        Debug.Assert(true, "Unexpected interrupt state");
        return null;
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