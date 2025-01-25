namespace GameboyEmu.Logic.Memory;

public class HighRam : RAM
{
    public HighRam() 
        : base(127, 0xFF80)
    {
    }
}