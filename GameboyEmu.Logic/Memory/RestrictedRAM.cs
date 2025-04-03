namespace GameboyEmu.Logic.Memory;

using Cpu;

public abstract class RestrictedRam
{
    public AccessSource AllowedSources { get; set; } = AccessSource.Cpu | AccessSource.Gpu;
    
    protected readonly ushort LowerBound;
    protected readonly byte[] Data;
    protected readonly bool[] Written;

    public RestrictedRam(int size, ushort lowerBound)
    {
        LowerBound = lowerBound;
        Data = new byte[size];
        Written = new bool[size];
        
        Init();
    }

    private void Init()
    {
        for (var i = 0; i < Data.Length; i++)
        {
            Data[i] = 0x00;
        }
    }

    public virtual byte ReadByte(ushort address, AccessSource accessSource)
    {
        /*if ((AllowedSources & accessSource) == 0)
        {
            throw new InvalidOperationException("Access not allowed");
        }*/
        
        var indexAddress = address - LowerBound;
        return Data[indexAddress];
    }
    
    public virtual void WriteByte(ushort address, byte value, AccessSource accessSource)
    {
        // if ((AllowedSources & accessSource) == 0)
        // {
        //     throw new InvalidOperationException("Access not allowed");
        // }
        
        Data[address - LowerBound] = value;
        Written[address - LowerBound] = true;
    }
}