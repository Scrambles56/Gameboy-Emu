namespace GameboyEmu.Logic.Memory;

public class WorkRAM : RAM
{
    
    public WorkRAM(ushort lowerBound)
        : base(4 * 1024, lowerBound)
    {
    }
}