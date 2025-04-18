﻿using System.Diagnostics;
using GameboyEmu.Logic.Extensions;
using Microsoft.Extensions.Logging;

namespace GameboyEmu.Logic.IOController;

public class InputControl(
    InterruptsController interruptsController,
    ILogger logger
)
{
    private byte _inputState = 0b0011_0000;
    
    private readonly Dictionary<GbButton, bool> _buttonStates = new()
    {
        { GbButton.Up, false },
        { GbButton.Down, false },
        { GbButton.Left, false },
        { GbButton.Right, false },
        { GbButton.A, false },
        { GbButton.B, false },
        { GbButton.Start, false },
        { GbButton.Select, false }
    };

    public void PressButton(GbButton button)
    {
        _buttonStates[button] = true;
    }
    
    public void ReleaseButton(GbButton button)
    {
        _buttonStates[button] = false;
    }
    
    public byte ReadByte(ushort address)
    {
        Debug.Assert(address == 0xFF00, "Invalid address for InputControl");
        byte result = 0x3F;
        
        var isDpadSelect = !_inputState.IsBitSet(4);
        var isButtonSelect = !_inputState.IsBitSet(5);

        if (isDpadSelect)
        {
            result = _buttonStates[GbButton.Down] ? result.ClearBit(3) : result;
            result = _buttonStates[GbButton.Up] ? result.ClearBit(2) : result;
            result = _buttonStates[GbButton.Left] ? result.ClearBit(1) : result;
            result = _buttonStates[GbButton.Right] ? result.ClearBit(0) : result;
        }
        
        if (isButtonSelect)
        {
            result = _buttonStates[GbButton.Start] ? result.ClearBit(3) : result;
            result = _buttonStates[GbButton.Select] ? result.ClearBit(2) : result;
            result = _buttonStates[GbButton.B] ? result.ClearBit(1) : result;
            result = _buttonStates[GbButton.A] ? result.ClearBit(0) : result;
        }
        
        result = isDpadSelect ? result.ClearBit(4) : result;
        result = isButtonSelect ? result.ClearBit(5) : result;

        return result;
    }
    
    public void WriteByte(ushort address, byte value)
    {
        Debug.Assert(address == 0xFF00, "Invalid address for InputControl");
        
        _inputState = (byte)(value & 0b0011_0000);
    }
}

public enum GbButton
{
    Up,
    Down,
    Left,
    Right,
    A,
    B,
    Start,
    Select
}