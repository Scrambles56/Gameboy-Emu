namespace GameboyEmu.Logic.Cartridge;

public class DestinationCode
{
    private readonly byte _value;

    public DestinationCode(byte value)
    {
        _value = value;
    }
    
    public string ByteString() => _value.ToString("X2");
    
    public override string ToString()
    {
        return _value switch
        {
            0x00 => "Japanese",
            0x01 => "Non-Japanese",
            _ => "Unknown",
        };
    }
}