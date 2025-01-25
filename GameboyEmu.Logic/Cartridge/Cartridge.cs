using System.Diagnostics;
using System.Text;
using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Cartridge;

public class Cartridge
{
    private readonly string _fileName;

    public Cartridge(string fileName)
    {
        _fileName = fileName;
    }
    
    public async Task<LoadedCartridge> Load()
    {
        var data = await File.ReadAllBytesAsync(_fileName);
        
        var cartridgeType = new CartridgeType(data[0x147]);

        return cartridgeType.MbcType switch
        {
            MbcType.NoMbc => new LoadedCartridge(data),
            MbcType.Mbc3 => new Mbc3Cartridge(data),
            _ => new LoadedCartridge(data)
        };
    }
}

public class LoadedCartridge
{
    protected readonly byte[] Data;

    public LoadedCartridge(byte[] data)
    {
        Data = data;
    }
    
    private byte[] EntryPointBytes => Data[0x100..0x104];

    private byte[] NintendoLogo => Data[0x104..0x134];
    
    private string CartridgeName => Encoding.ASCII.GetString(Data[0x134..0x144]);
    private byte[] CartridgeNameBytes => Data[0x134..0x144];
    
    private string ManufacturerCode => Encoding.ASCII.GetString(Data[0x13F..0x144]);
    private byte[] ManufacturerCodeBytes => Data[0x13F..0x144];
    
    private CGBFlag CGBFlag => new(Data[0x143]);

    private NewLicenceeCode NewLicenseeCode => new(Data[0x144..0x146], OldLicenceeCode.UseNewLicenseeCode);
    
    private SGBFlag SGBFlag => new(Data[0x146]);
    
    private CartridgeType CartridgeType => new(Data[0x147]);
    
    private ROMSize ROMSize => new(Data[0x148]);

    private RAMSize RAMSize => new(Data[0x149]);
    
    public DestinationCode DestinationCode => new(Data[0x14A]);
    public OldLicenceeCode OldLicenceeCode => new(Data[0x14B]);
    
    private MaskROMVersion MaskROMVersion => new(Data[0x14C]);
    
    private byte HeaderChecksum => Data[0x14D];
    
    private byte CalculateHeaderChecksum()
    {
        var checksum = 0;
        for (var i = 0x134; i <= 0x14C; i++)
        {
            checksum = checksum - Data[i] - 0x01;
        }

        return (byte)checksum;
    }
    
    private bool HeaderChecksumIsValid => HeaderChecksum == CalculateHeaderChecksum();
    
    public byte Read(ushort address)
    {
        if (address < 0x8000)
        {
            return Data[address];
        }

        throw new NotImplementedException();
    }
    
    public virtual void Write(ushort address, byte value)
    {
        // if (address.IsBetween(0x0000, 0x3FFF))
        // {
        //     Debug.Assert(true, "Writing to ROM Bank 00 is not implemented");
        // }
        //
        // else if (address.IsBetween(0x4000, 0x7FFF))
        // {
        //     Debug.Assert(true, "Writing to ROM Bank 01 - 7F is not implemented");
        // }
        //
        // throw new NotImplementedException();
        
        // Console.WriteLine($"Writing to Cartridge is not implemented, Address: {address:X4}, Value: {value:X2}");
    }

    public override string? ToString()
    {
        return $$"""
Cartridge
    Name: {{CartridgeName}} ({{string.Join(",", CartridgeNameBytes)}})
    Manufacturer: {{ManufacturerCode}} ({{string.Join(",", ManufacturerCodeBytes)}})
    CGBFlag: {{CGBFlag}} ({{CGBFlag.ByteString()}})
    Old Licencee Code: {{OldLicenceeCode}} ({{OldLicenceeCode.ByteString()}})
    New Licencee Code: {{NewLicenseeCode}} ({{NewLicenseeCode.ByteString()}})
    SGBFlag: {{SGBFlag}} ({{SGBFlag.ByteString()}})
    CartridgeType: {{CartridgeType}} ({{CartridgeType.ByteString()}})
    ROMSize: {{ROMSize}} ({{ROMSize.ByteString()}})
    RAMSize: {{RAMSize}} ({{RAMSize.ByteString()}})
    DestinationCode: {{DestinationCode}} ({{DestinationCode.ByteString()}})
    MaskROMVersion: {{MaskROMVersion}} ({{MaskROMVersion.ByteString()}})

    HeaderChecksum: {{HeaderChecksum}}
    CalculatedChecksum: {{CalculateHeaderChecksum()}}
    HeaderChecksumIsValid: {{HeaderChecksumIsValid}}

    EntryPoint: {{string.Join(",", EntryPointBytes.Select(b => b.ToString("X2")))}}
""";
    }
}

public class Mbc3Cartridge : LoadedCartridge
{
    public ROM RomBank00 { get; set; }
    public RAM RamBank00 { get; set; }
    public RAM? RamBank01 { get; set; }
    public RAM? RamBank02 { get; set; }
    public RAM? RamBank03 { get; set; }

    public bool EnableRamAndTimer { get; set; }
    
    public Mbc3Cartridge(byte[] data) : base(data)
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
        
        throw new NotImplementedException($"Writing to MB3 Cartridge is not implemented, Address: {address:X4}, Value: {value:X2}");
    }
}