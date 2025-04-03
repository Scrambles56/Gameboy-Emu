namespace GameboyEmu.Cpu;

using Logic.Cpu.Extensions;
using Logic.IOController;

public partial class Cpu
{
    public void SetInterruptMasterFlag(bool state)
    {
        if (state)
        {
            _interruptsController.SetInterruptMasterEnabledFlag = true;
        }
        else
        {
            _interruptsController.InterruptMasterEnabledFlag = state;
        }
    }
    
    public void SetInterruptMasterFlagImmediate(bool state)
    {
        _interruptsController.InterruptMasterEnabledFlag = state;
    }
    
    public bool HandleInterrupts()
    {
        if (!_interruptsController.InterruptMasterEnabledFlag && Halted)
        {
            return false;
        }
        
        var req = _interruptsController.GetRequestedInterrupt();
        if (req is not {} interrupt)
        {
            return false;
        }

        Halted = false;
        
        if (!_interruptsController.InterruptMasterEnabledFlag)
        {
            return false;
        }

        _interruptsController.ClearInterrupt(interrupt);
        _interruptsController.InterruptMasterEnabledFlag = false;
        
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