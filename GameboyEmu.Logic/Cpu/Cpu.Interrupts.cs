namespace GameboyEmu.Cpu;

using Logic.Cpu.Extensions;
using Logic.IOController;
using Microsoft.Extensions.Logging;

public partial class Cpu
{
    public void SetInterruptMasterFlag(bool state)
    {
        if (state)
        {
            _addressBus.SetInterruptMasterEnableFlag = true;
        }
        else
        {
            _addressBus.InterruptMasterEnabledFlag = state;
        }
    }
    
    public bool HandleInterrupts()
    {
        var req = _addressBus.GetRequestedInterrupt();
        if (req is not {} interrupt)
        {
            return false;
        }

        if (!DocMode)
        {
            _logger.LogInformation("Handling interrupt {InterruptType}", interrupt.ToString());
        }

        _addressBus.ClearInterrupt(interrupt);
        _addressBus.InterruptMasterEnabledFlag = false;
        
        PushPCToStack();
        JumpToHandler(interrupt);

        return true;
    }

    private void JumpToHandler(Interrupt interrupt)
    {
        ushort address = interrupt switch
        {
            Interrupt.VBlank => 0x40,
            Interrupt.LcdStat => 0x48,
            Interrupt.Timer => 0x50,
            Interrupt.Serial => 0x58,
            Interrupt.Joypad => 0x60,
            _ => throw new ArgumentOutOfRangeException(nameof(interrupt), interrupt, null)
        };
        
        PC.SetValue(address);
    }

    private void PushPCToStack()
    {
        var (high, low) = PC.GetValue().ToBytes();
        SP--;
        WriteByte(SP, high);
        SP--;
        WriteByte(SP, low);
        
    }
}