using System.Text;

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
        return new LoadedCartridge(data);
    }
}

public class LoadedCartridge
{
    private readonly byte[] _data;

    public LoadedCartridge(byte[] data)
    {
        _data = data;
    }
    
    private byte[] EntryPointBytes => _data[0x100..0x104];

    private byte[] NintendoLogo => _data[0x104..0x134];
    
    private string CartridgeName => Encoding.ASCII.GetString(_data[0x134..0x144]);
    private byte[] CartridgeNameBytes => _data[0x134..0x144];
    
    private string ManufacturerCode => Encoding.ASCII.GetString(_data[0x13F..0x144]);
    private byte[] ManufacturerCodeBytes => _data[0x13F..0x144];
    
    private CGBFlag CGBFlag => new(_data[0x143]);

    private NewLicenceeCode NewLicenseeCode => new(_data[0x144..0x146], OldLicenceeCode.UseNewLicenseeCode);
    
    private SGBFlag SGBFlag => new(_data[0x146]);
    
    private CartridgeType CartridgeType => new(_data[0x147]);
    
    private ROMSize ROMSize => new(_data[0x148]);

    private RAMSize RAMSize => new(_data[0x149]);
    
    public DestinationCode DestinationCode => new(_data[0x14A]);
    public OldLicenceeCode OldLicenceeCode => new(_data[0x14B]);
    
    private MaskROMVersion MaskROMVersion => new(_data[0x14C]);
    
    private byte HeaderChecksum => _data[0x14D];
    
    private byte CalculateHeaderChecksum()
    {
        var checksum = 0;
        for (var i = 0x134; i <= 0x14C; i++)
        {
            checksum = checksum - _data[i] - 0x01;
        }

        return (byte)checksum;
    }
    
    private bool HeaderChecksumIsValid => HeaderChecksum == CalculateHeaderChecksum();
    
    public byte Read(ushort address)
    {
        if (address < 0x8000)
        {
            return _data[address];
        }

        throw new NotImplementedException();
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

    EntryPoint: {{string.Join(",", EntryPointBytes)}}
""";
    }
}