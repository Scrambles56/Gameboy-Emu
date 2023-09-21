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
    
    public abstract void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data);
    
    public FetchedData FetchData(GameboyEmu.Cpu.Cpu cpu) =>
        InstructionSize switch
        {
            InstructionSize.None => new(),
            InstructionSize.D8 => new(cpu.ReadNextByte()),
            InstructionSize.D16 => new(cpu.ReadNextByte(), cpu.ReadNextByte()),
            _ => throw new ArgumentOutOfRangeException()
        };

    public override string ToString() => $"{Opcode:X2}: {Mnemonic}";
}

public record FetchedData(byte? Byte1 = null, byte? Byte2 = null)
{
    public ushort ToUshort()
    {
        if (Byte1 is null || Byte2 is null)
        {
            throw new InvalidOperationException("Cannot convert to ushort when one of the bytes is null");
        }

        return (ushort)((Byte2 << 8) | Byte1);
    }
    
    public byte ToByte()
    {
        if (Byte1 is null)
        {
            throw new InvalidOperationException("Cannot convert to byte when one of the bytes is null");
        }

        return (byte)Byte1;
    }
    
    public override string ToString()
    {
        return Byte1 is null 
            ? "" 
            : Byte2 is null
                ? $"{Byte1:X2} ({Byte1})" 
                : $"{Byte1:X2} {Byte2:X2}, ({Byte1}) ({Byte2})";
    }
};