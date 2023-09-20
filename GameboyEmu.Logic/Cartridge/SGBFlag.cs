namespace GameboyEmu.Logic.Cartridge;

public class SGBFlag
{
    private readonly byte _value;

    public SGBFlag(byte value)
    {
        _value = value;
    }
    
    public string ByteString() => _value.ToString("X2");
    
    public bool HasSGBFunctions => _value == 0x03;
    
    public override string ToString()
    {
        return _value switch
        {
            0x03 => "SGB functions",
            _ => "No SGB functions",
        };
    }
}