using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;
using Microsoft.Extensions.Logging;

namespace GameboyEmu.Logic.Cartridge.Carts;

public class Mbc3Cartridge : LoadedCartridge
{
    public ROM RomBank00 { get; set; }
    public RAM RamBank00 { get; set; }
    public RAM? RamBank01 { get; set; }
    public RAM? RamBank02 { get; set; }
    public RAM? RamBank03 { get; set; }

    public bool EnableRamAndTimer { get; set; }
    
    public Mbc3Cartridge(ILogger logger, byte[] data) : base(logger, data)
    {
        RomBank00 = new(16384, 0x0000, data[..0x4000]);
        RamBank00 = new(8192, 0xA000);
    }
    
    public override void Write(ushort address, byte value)
    {
        if (address.IsBetween(0xA000, 0xBFFF))
        {
            if (!EnableRamAndTimer)
            {
                throw new("RAM and Timer is not enabled");
            }
            
            RamBank00.WriteByte(address, value);
            return;
        }
        
        if (address.IsBetween(0x0000, 0x1FFF))
        {
            EnableRamAndTimer = value == 0x0A;
            return;
        }
        
        // throw new NotImplementedException($"Writing to MB3 Cartridge is not implemented, Address: {address:X4}, Value: {value:X2}");
    }
}