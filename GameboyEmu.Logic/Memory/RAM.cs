using System.Diagnostics;

namespace GameboyEmu.Logic.Memory;

public abstract class RAM : IMemoryAddressable
{
    private readonly ushort _lowerBound;
    protected readonly byte[] _data ;
    protected readonly bool[] _written;

    protected RAM(int size, ushort lowerBound)
    {
        _lowerBound = lowerBound;
        _data = new byte[size];
        _written = new bool[size];
        
        Init();
    }

    private void Init()
    {
        for (var i = 0; i < _data.Length; i++)
        {
            _data[i] = 0x00;
        }
    }

    public byte ReadByte(ushort address)
    {
        Debug.Assert(_written[address - _lowerBound]);
        
        return _data[address - _lowerBound];
    }
    
    public void WriteByte(ushort address, byte value)
    {
        _data[address - _lowerBound] = value;
        _written[address - _lowerBound] = true;
    }
}