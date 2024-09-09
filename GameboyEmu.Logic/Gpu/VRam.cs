using System.Diagnostics;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Gpu;

public class VRam : RAM
{
    public VRam()
        : base(8192, 0x8000)
    {
    }

    public override byte ReadByte(ushort address)
    {
        Debug.Assert(!address.IsBetween(0x8000, 0x97FF));

        return Data[address - LowerBound];
    }
}