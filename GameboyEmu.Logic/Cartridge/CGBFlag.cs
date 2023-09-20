namespace GameboyEmu.Logic.Cartridge;

public class CGBFlag
{
    private readonly byte _value;

    public CGBFlag(byte value)
    {
        _value = value;
    }
    
    public string ByteString() => _value.ToString("X2");
    
    public bool IsCGB => _value == 0x80 || _value == 0xC0;
    
    public override string ToString()
    {
        return _value switch
        {
            0x80 => "CGB supported",
            0xC0 => "CGB only",
            _ => "No CGB support",
        };
    }
}