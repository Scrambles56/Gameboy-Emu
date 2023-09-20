namespace GameboyEmu.Logic.Cartridge;

public class RAMSize
{
    private readonly byte _value;

    public RAMSize(byte value)
    {
        _value = value;
    }
    
    public string ByteString() => _value.ToString("X2");
    
    public int Size => _value switch
    {
        0x00 => 0,
        0x02 => 8192,
        0x03 => 32768,
        0x04 => 131072,
        0x05 => 65536,
        _ => throw new Exception("Invalid RAM size"),
    };
    
    public override string ToString()
    {
        return _value switch
        {
            0x00 => "None",
            0x02 => "8KB",
            0x03 => "32KB",
            0x04 => "128KB",
            0x05 => "64KB",
            _ => "Unknown"
        };
    }
}