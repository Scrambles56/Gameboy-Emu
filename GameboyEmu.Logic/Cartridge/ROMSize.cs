namespace GameboyEmu.Logic.Cartridge;

public class ROMSize
{
    private readonly byte _value;

    public ROMSize(byte value)
    {
        _value = value;
    }
    
    public string ByteString() => _value.ToString("X2");
    
    public int Size => 32768 * (1 << _value);

    public override string ToString()
    {
        return _value switch
        {
            0x00 => "32KB",
            0x01 => "64KB",
            0x02 => "128KB",
            0x03 => "256KB",
            0x04 => "512KB",
            0x05 => "1MB",
            0x06 => "2MB",
            0x07 => "4MB",
            0x08 => "8MB",
            0x52 => "1.1MB",
            0x53 => "1.2MB",
            0x54 => "1.5MB",
            _ => "Unknown"
        };
    }
}