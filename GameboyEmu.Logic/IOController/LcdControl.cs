using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

using System.Diagnostics;
using Extensions;
using Microsoft.Extensions.Logging;

public class LcdControl(ILogger logger, InterruptsController interruptsController, bool docMode) : RAM(12, 0xFF40)
{
    public override byte ReadByte(ushort address)
    {
        // Gameboy Doctor
        if (docMode && address == 0xFF44)
        {
            return 0x90;
        }
        
        if (address == 0xFF41 && !LcdAndPpuEnabled)
        {
            return Data[address - LowerBound].ClearBit(0).ClearBit(1);
        }

        return Data[address - LowerBound];
    }

    public override void WriteByte(ushort address, byte value)
    {
        var oldValue = Data[address - LowerBound];
        Data[address - LowerBound] = value;
        
        if (address == 0xFF41)
        {
            if (oldValue == value)
            {
                return;
            }

            var oldLycEq = oldValue.IsBitSet((int)LcdStatusFlag.LycEq);
            var newLycEq = value.IsBitSet((int)LcdStatusFlag.LycEq);
        
            if (oldLycEq != newLycEq && value.IsBitSet((int)LcdStatusFlag.LycSelect))
            {
                interruptsController.RequestInterrupt(Interrupt.LcdStat);
            }
        }
    }

    public void WriteLcdStatusFlag(LcdStatusFlag flag, bool state)
    {
        var currentValue = ReadByte(0xFF41);
        
        var bitIndex = (int)flag;
        var newValue = state 
            ? currentValue.SetBit(bitIndex)
            : currentValue.ClearBit(bitIndex);

        WriteByte(0xFF41, newValue);
    }

    public static bool IsBitSet(byte value, int bit) => (value & (1 << bit)) != 0;

    private byte LcdControlByte => ReadByte(0xFF40);

    public bool LcdAndPpuEnabled => IsBitSet(LcdControlByte, 7);
    public bool WindowTileMapDisplaySelect => IsBitSet(LcdControlByte, 6);
    public bool WindowDisplayEnable => IsBitSet(LcdControlByte, 5);
    public bool BackgroundAndWindowTileDataSelect => IsBitSet(LcdControlByte, 4);
    public bool BackgroundTileMapDisplaySelect => IsBitSet(LcdControlByte, 3);
    public bool ObjectSize => IsBitSet(LcdControlByte, 2);
    public bool ObjectEnable => IsBitSet(LcdControlByte, 1);
    public bool BackgroundAndWindowEnable => IsBitSet(LcdControlByte, 0);
}

public enum LcdStatusFlag
{
    PPUModeLowBit = 0,
    PPUModeHighBit = 1,
    LycEq = 2,
    Mode0Select = 3,
    Mode1Select = 4,
    Mode2Select = 5,
    LycSelect = 6
}

file class LcdStatusHandler(InterruptsController interruptsController, byte oldState, byte newState)
{
    public void HandleLcdStatusChange()
    {
        if (oldState == newState)
        {
            return;
        }

        var oldLycEq = oldState.IsBitSet((int)LcdStatusFlag.LycEq);
        var newLycEq = newState.IsBitSet((int)LcdStatusFlag.LycEq);
        
        if (oldLycEq != newLycEq && newState.IsBitSet((int)LcdStatusFlag.LycSelect))
        {
            interruptsController.RequestInterrupt(Interrupt.LcdStat);
        }
    }
}