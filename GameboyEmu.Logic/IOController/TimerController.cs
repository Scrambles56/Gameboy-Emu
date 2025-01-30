using GameboyEmu.Logic.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

public class TimerController(InterruptsController interruptsController)
    : IMemoryAddressable
{
    private int TickCount = 0;
    public byte DividerRegister { get; set; }
    
    public byte TimerCounter { get; set; }
    
    public byte TimerModulo { get; set; }
    
    public byte TimerControl { get; set; }

    public void Tick()
    {
        TickCount++;
        DividerRegister++;

        var timerEnabled = TimerControl.IsBitSet(2);
        if (!timerEnabled)
        {
            return;
        }
        
        var timerFrequency = GetTimerFrequency();
        if (TickCount % timerFrequency == 0 && ++TimerCounter == 0)
        {
            TimerCounter = TimerModulo;
            interruptsController.RequestInterrupt(Interrupt.Timer);
        }
        
    }

    public byte ReadByte(ushort address)
    {
        switch (address)
        {
            case 0xFF04:
                return DividerRegister;
            case 0xFF05:
                return TimerCounter;
            case 0xFF06:
                return TimerModulo;
            case 0xFF07:
                return TimerControl;
            default:
                throw new InvalidOperationException("Invalid address");
        }
    }

    public void WriteByte(ushort address, byte value)
    {
        switch (address)
        {
            case 0xFF04:
                DividerRegister = 0;
                break;
            case 0xFF05:
                TimerCounter = value;
                break;
            case 0xFF06:
                TimerModulo = value;
                break;
            case 0xFF07:
                TimerControl = value;
                break;
            default:
                throw new InvalidOperationException("Invalid address");
        }
    }
    
    private int GetTimerFrequency() =>
        (TimerControl & 0b11) switch
        {
            0b00 => 1024,
            0b01 => 16,
            0b10 => 64,
            0b11 => 256,
            _ => throw new InvalidOperationException("Invalid timer control")
        };
}