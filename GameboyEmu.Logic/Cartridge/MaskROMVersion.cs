namespace GameboyEmu.Logic.Cartridge;

public class MaskROMVersion
{
    private readonly byte _value;

    public MaskROMVersion(byte value)
    {
        _value = value;
    }
    
    public string ByteString() => _value.ToString("X2");
    
    public override string ToString() => ByteString();
}