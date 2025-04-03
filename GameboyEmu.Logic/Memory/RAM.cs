namespace GameboyEmu.Logic.Memory;

public class RAM : IMemoryAddressable
{
    protected readonly ushort LowerBound;
    protected readonly byte[] Data;
    protected readonly bool[] Written;

    public RAM(int size, ushort lowerBound)
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

    public virtual byte ReadByte(ushort address)
    {
        var indexAddress = address - LowerBound;
        return Data[indexAddress];
    }
    
    public virtual void WriteByte(ushort address, byte value)
    {
        Data[address - LowerBound] = value;
        Written[address - LowerBound] = true;
    }
}