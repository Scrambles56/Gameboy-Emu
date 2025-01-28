using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;
using Microsoft.Extensions.Logging;

namespace GameboyEmu.Logic.Cartridge.Carts;

public class Mbc1Cartridge : LoadedCartridge
{
    public ROM[] RomBanks { get; set; }
    public ROM SelectedRomBank { get; set; }
    public int SelectedRomBankIndex { get; set; }
    
    public RAM RamBank00 { get; set; }
    public RAM? RamBank01 { get; set; }
    public RAM? RamBank02 { get; set; }
    public RAM? RamBank03 { get; set; }

    public bool EnableRam { get; set; }
    public bool EnableRamBankingMode { get; set; }

    public Mbc1Cartridge(ILogger logger, byte[] data) : base(logger, data)
    {
        RomBanks = new ROM[ROMSize.Banks];
        RomBanks[0] = new ROM(0x4000, 0, data[..0x4000]);
        for (var i = 1; i < ROMSize.Banks; i++)
        {
            RomBanks[i] = new ROM(0x4000, 0x4000, data[(i * 0x4000)..((i + 1) * 0x4000)]);
        }
        
        SelectedRomBank = RomBanks[0];
        
        RamBank00 = new(8192, 0xA000);
    }

    public override void Write(ushort address, byte value)
    {
        if (address.IsBetween(0x0000, 0x1FFF))
        {
            EnableRam = (value & 0x0F) == 0x0A;
        }
        else if (address.IsBetween(0x2000, 0x3FFF))
        {
            var bank = value & ROMSize.BankSelectMask;
            SelectedRomBankIndex = bank;
            SelectedRomBank = RomBanks[SelectedRomBankIndex];
        }
        else if (address.IsBetween(0x4000, 0x5FFF))
        {
            if (EnableRamBankingMode)
            {
                var bank = value & 0x03;
                switch (bank)
                {
                    case 0x00:
                        RamBank01 = null;
                        break;
                    case 0x01:
                        RamBank01 = new(8192, 0xA000);
                        break;
                    case 0x02:
                        RamBank02 = new(8192, 0xA000);
                        break;
                    case 0x03:
                        RamBank03 = new(8192, 0xA000);
                        break;
                }
            }
            else
            {
                var bank = value & ROMSize.BankSelectMask;
                SelectedRomBankIndex = bank;
                SelectedRomBank = RomBanks[SelectedRomBankIndex];
            }
        }
        else if (address.IsBetween(0x6000, 0x7FFF))
        {
            EnableRamBankingMode = (value & 0x01) == 0x01;
        }
        else if (address.IsBetween(0xA000, 0xBFFF))
        {
            if (EnableRam)
            {
                if (RamBank01 != null)
                {
                    RamBank01.WriteByte((ushort)(address - 0xA000), value);
                }
                else if (RamBank02 != null)
                {
                    RamBank02.WriteByte((ushort)(address - 0xA000), value);
                }
                else if (RamBank03 != null)
                {
                    RamBank03.WriteByte((ushort)(address - 0xA000), value);
                }
            }
        }
    }
    
    public override byte Read(ushort address)
    {
        if (address.IsBetween(0x0000, 0x3FFF))
        {
            var romBank = RomBanks[0];
            return romBank.ReadByte(address);
        }
        if (address.IsBetween(0x4000, 0x7FFF))
        {
            try
            {
                return SelectedRomBank.ReadByte(address);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error reading from selected ROM bank, {SelectedRomBank}. {Address}", SelectedRomBankIndex, address);
                throw;
            }
        }
        if (address.IsBetween(0xA000, 0xBFFF))
        {
            if (EnableRam)
            {
                if (RamBank01 != null)
                {
                    return RamBank01.ReadByte((ushort)(address - 0xA000));
                }
                if (RamBank02 != null)
                {
                    return RamBank02.ReadByte((ushort)(address - 0xA000));
                }
                if (RamBank03 != null)
                {
                    return RamBank03.ReadByte((ushort)(address - 0xA000));
                }
            }
        }
        return 0xFF;
    }
}