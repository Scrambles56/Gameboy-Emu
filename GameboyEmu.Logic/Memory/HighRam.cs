namespace GameboyEmu.Logic.Memory;

public class HighRam : RAM
{
    public HighRam() 
        : base(126, 0xFF80)
    {
    }
}