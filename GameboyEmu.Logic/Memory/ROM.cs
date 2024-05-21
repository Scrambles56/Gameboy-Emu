using System.Diagnostics;

namespace GameboyEmu.Logic.Memory;

public class ROM : IMemoryReadable
{
    private readonly ushort _lowerBound;
    private readonly byte[] _data;

    public ROM(
        int size,
        ushort lowerBound,
        byte[] data)
    {
        Debug.Assert(data.Length <= size, "Data is too large for ROM");
        _lowerBound = lowerBound;

        _data = new byte[size];
        data.CopyTo(_data, 0);
        
        
    }
    
    public byte ReadByte(ushort address)
    {
        return _data[address - _lowerBound];
    }
}