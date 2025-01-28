using System.Text;
using Microsoft.Extensions.Logging;

namespace GameboyEmu.Logic.Cartridge.Carts;

public class Cartridge
{
    private readonly ILogger _logger;
    private readonly string _fileName;

    public Cartridge(ILogger logger, string fileName)
    {
        _logger = logger;
        _fileName = fileName;
    }
    
    public async Task<LoadedCartridge> Load()
    {
        var data = await File.ReadAllBytesAsync(_fileName);
        
        var cartridgeType = new CartridgeType(data[0x147]);

        return cartridgeType.MbcType switch
        {
            MbcType.NoMbc => new LoadedCartridge(_logger, data),
            MbcType.Mbc1 => new Mbc1Cartridge(_logger, data),
            MbcType.Mbc3 => new Mbc3Cartridge(_logger, data),
            _ => throw new Exception($"Unsupported Cartridge Type {cartridgeType.MbcType.ToString()}")
        };
    }
}

public class LoadedCartridge
{
    protected readonly ILogger Logger;
    protected readonly byte[] Data;

    public LoadedCartridge(ILogger logger, byte[] data)
    {
        Logger = logger;
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
    
    protected ROMSize ROMSize => new(Data[0x148]);

    protected RAMSize RAMSize => new(Data[0x149]);
    
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
    
    public virtual byte Read(ushort address)
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