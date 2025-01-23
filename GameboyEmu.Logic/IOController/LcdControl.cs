using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

using System.Diagnostics;

public class LcdControl : RAM
{
    public LcdControl() : base(0x000B, 0xFF40)
    {
    }
    
    public override byte ReadByte(ushort address)
    {
        // Gameboy Doctor
        if (false && address == 0xFF44)
        {
            return 0x90;
        }
        
        return Data[address - LowerBound];
    }

    public override void WriteByte(ushort address, byte value)
    {
        if (address == 0xFF40)
        {
            var oldState = new LcdFlags(
                LcdAndPpuEnabled,
                WindowTileMapDisplaySelect,
                WindowDisplayEnable,
                BackgroundAndWindowTileDataSelect,
                BackgroundTileMapDisplaySelect,
                ObjectSize,
                ObjectEnable,
                BackgroundAndWindowEnable
            );
            
            Data[address - LowerBound] = value;
            
            var newState = new LcdFlags(
                LcdAndPpuEnabled,
                WindowTileMapDisplaySelect,
                WindowDisplayEnable,
                BackgroundAndWindowTileDataSelect,
                BackgroundTileMapDisplaySelect,
                ObjectSize,
                ObjectEnable,
                BackgroundAndWindowEnable
            );
            
            
            newState.LogChangedFlags(oldState);
        }

        Data[address - LowerBound] = value;
    }

    private bool IsBitSet(byte value, int bit) => (value & (1 << bit)) != 0;

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

file record LcdFlags(
    bool LcdAndPpuEnabled,
    bool WindowTileMapDisplaySelect,
    bool WindowDisplayEnable,
    bool BackgroundAndWindowTileDataSelect,
    bool BackgroundTileMapDisplaySelect,
    bool ObjectSize,
    bool ObjectEnable,
    bool BackgroundAndWindowEnable
)
{
    public void LogChangedFlags(LcdFlags oldState)
    {
        if (LcdAndPpuEnabled != oldState.LcdAndPpuEnabled)
        {
            Console.WriteLine($"LcdAndPpuEnabled changed from {oldState.LcdAndPpuEnabled} to {LcdAndPpuEnabled}");
        }
        
        if (WindowTileMapDisplaySelect != oldState.WindowTileMapDisplaySelect)
        {
            Console.WriteLine($"WindowTileMapDisplaySelect changed from {oldState.WindowTileMapDisplaySelect} to {WindowTileMapDisplaySelect}");
        }
        
        if (WindowDisplayEnable != oldState.WindowDisplayEnable)
        {
            Console.WriteLine($"WindowDisplayEnable changed from {oldState.WindowDisplayEnable} to {WindowDisplayEnable}");
        }
        
        if (BackgroundAndWindowTileDataSelect != oldState.BackgroundAndWindowTileDataSelect)
        {
            Console.WriteLine($"BackgroundAndWindowTileDataSelect changed from {oldState.BackgroundAndWindowTileDataSelect} to {BackgroundAndWindowTileDataSelect}");
        }
        
        if (BackgroundTileMapDisplaySelect != oldState.BackgroundTileMapDisplaySelect)
        {
            Console.WriteLine($"BackgroundTileMapDisplaySelect changed from {oldState.BackgroundTileMapDisplaySelect} to {BackgroundTileMapDisplaySelect}");
        }
        
        if (ObjectSize != oldState.ObjectSize)
        {
            Console.WriteLine($"ObjectSize changed from {oldState.ObjectSize} to {ObjectSize}");
        }
        
        if (ObjectEnable != oldState.ObjectEnable)
        {
            Console.WriteLine($"ObjectEnable changed from {oldState.ObjectEnable} to {ObjectEnable}");
        }
        
        if (BackgroundAndWindowEnable != oldState.BackgroundAndWindowEnable)
        {
            Console.WriteLine($"BackgroundAndWindowEnable changed from {oldState.BackgroundAndWindowEnable} to {BackgroundAndWindowEnable}");
        }
    }
};