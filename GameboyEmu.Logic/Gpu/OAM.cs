using System.Diagnostics;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Gpu;

public class OAM : RAM
{
    public bool IsAccessible { get; set; } = true;
    
    public OAM()
        : base(160, 0xFE00)
    {
    }

    public override byte ReadByte(ushort address)
    {
        Console.WriteLine("OAM ReadByte");
        
        if (!IsAccessible)
        {
            throw new InvalidOperationException("OAM is not accessible");
        }
        // Debug.Assert(!address.IsBetween(0x8000, 0x9FFF));

        return Data[address - LowerBound];
    }
    
    public override void WriteByte(ushort address, byte value)
    {
        Console.WriteLine("OAM WriteByte");
        
        if (!IsAccessible)
        {
            throw new InvalidOperationException("OAM is not accessible");
        }
        
        Data[address - LowerBound] = value;
        Written[address - LowerBound] = true;
    }
}