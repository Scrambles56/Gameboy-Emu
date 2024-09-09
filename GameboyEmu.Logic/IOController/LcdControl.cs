using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

public class LcdControl : RAM
{
    public LcdControl() : base(0x000B, 0xFF40)
    {
    }
    
    public byte ReadByte(ushort address)
    {
        return Data[address - LowerBound];
    }

    public void WriteByte(ushort address, byte value)
    {
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