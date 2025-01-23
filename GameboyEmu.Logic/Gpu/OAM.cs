using System.Diagnostics;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Gpu;

using Microsoft.Extensions.Logging;

public class OAM(ILogger logger) : RAM(160, 0xFE00)
{
    public bool IsAccessible { get; set; } = true;
    

    public override byte ReadByte(ushort address)
    {
        logger.LogInformation("OAM ReadByte");
        
        if (!IsAccessible)
        {
            throw new InvalidOperationException("OAM is not accessible");
        }
        // Debug.Assert(!address.IsBetween(0x8000, 0x9FFF));

        return Data[address - LowerBound];
    }
    
    public override void WriteByte(ushort address, byte value)
    {
        logger.LogInformation("OAM WriteByte");
        
        if (!IsAccessible)
        {
            throw new InvalidOperationException("OAM is not accessible");
        }
        
        Data[address - LowerBound] = value;
        Written[address - LowerBound] = true;
    }
}