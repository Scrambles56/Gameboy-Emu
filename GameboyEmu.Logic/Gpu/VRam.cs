using System.Diagnostics;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Gpu;

public class VRam : RAM
{
    public bool IsAccessible { get; set; } = true;
    
    public VRam()
        : base(8192, 0x8000)
    {
    }

    public override byte ReadByte(ushort address)
    {
        if (!IsAccessible)
        {
            throw new InvalidOperationException("VRam is not accessible");
        }
        // Debug.Assert(!address.IsBetween(0x8000, 0x9FFF));

        return Data[address - LowerBound];
    }
    
    
    public override void WriteByte(ushort address, byte value)
    {
        if (!IsAccessible)
        {
            throw new InvalidOperationException("VRam is not accessible");
        }
        
        Data[address - LowerBound] = value;
        Written[address - LowerBound] = true;
    }
}