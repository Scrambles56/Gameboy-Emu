namespace GameboyEmu.Logic.Cpu.Instructions;

public enum InstructionSize
{
    None,
    D8,
    D16
}   

public enum RegisterType
{
    None,
    A,
    B,
    C,
    D,
    E,
    H,
    L,
    AF,
    BC,
    DE,
    HL,
    SP,
    PC
}

public enum Condition
{
    None,
    C,
    NC,
    Z,
    NZ
}

public abstract class Instruction
{
    protected Instruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None
    )
    {
        Opcode = opcode;
        Mnemonic = mnemonic;
        Cycles = cycles;
        InstructionSize = instructionSize;
        Register1 = register1;
        Register2 = register2;
    }

    public byte Opcode { get; }
    public string Mnemonic { get; }
    public int Cycles { get; }
    public InstructionSize InstructionSize { get; }
    public RegisterType Register1 { get; }
    public RegisterType Register2 { get; }
    
    // Returns the number of cycles the instruction took
    public abstract int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data);
    
    private readonly FetchedData _fetchedData = new();
    public virtual FetchedData FetchData(GameboyEmu.Cpu.Cpu cpu)
    {
        _fetchedData.SetValues(null, null);

        switch (InstructionSize)
        {
            case InstructionSize.None:
                break;
            case InstructionSize.D8:
                _fetchedData.SetValues(cpu.ReadNextByte(), null);
                break;
            case InstructionSize.D16:
                _fetchedData.SetValues(cpu.ReadNextByte(), cpu.ReadNextByte());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return _fetchedData;
    }

    public override string ToString() => $"{Opcode:X2}: {Mnemonic}";
}

public class FetchedData(byte? byte1 = null, byte? byte2 = null)
{
    private byte? _byte1 = byte1;
    private byte? _byte2 = byte2;

    public void SetValues(byte? b1, byte? b2)
    {
        _byte1 = b1;
        _byte2 = b2;
    }
    
    public ushort ToUshort()
    {
        if (_byte1 is null || _byte2 is null)
        {
            throw new InvalidOperationException("Cannot convert to ushort when one of the bytes is null");
        }

        return (ushort)((_byte2 << 8) | _byte1);
    }
    
    public byte ToByte()
    {
        if (_byte1 is null)
        {
            throw new InvalidOperationException("Cannot convert to byte when one of the bytes is null");
        }

        return (byte)_byte1;
    }
    
    public override string ToString()
    {
        return _byte1 is null 
            ? "" 
            : _byte2 is null
                ? $"{_byte1:X2} ({_byte1})" 
                : $"{_byte1:X2} {_byte2:X2}, ({_byte1}) ({_byte2})";
    }
};