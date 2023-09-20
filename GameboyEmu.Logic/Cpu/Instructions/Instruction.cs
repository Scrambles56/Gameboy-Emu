namespace GameboyEmu.Logic.Cpu.Instructions;

public enum Instr
{
    Noop,
    Jump,
    Xor,
    Ld
}

public enum AddressingMode
{
    Impl,
    D16,
    Register,
    R_D16,
    D8
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

public class Instruction
{
    private readonly Action<GameboyEmu.Cpu.Cpu, ushort>? _action;

    public Instruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        Instr instr, 
        AddressingMode addressingMode = AddressingMode.Impl, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None, 
        Condition condition = Condition.None,
        Action<GameboyEmu.Cpu.Cpu, ushort>? action = null
    )
    {
        _action = action;
        Opcode = opcode;
        Mnemonic = mnemonic;
        Cycles = cycles;
        Instr = instr;
        AddressingMode = addressingMode;
        Register1 = register1;
        Register2 = register2;
        Condition = condition;
    }

    public byte Opcode { get; }
    public string Mnemonic { get; }
    public int Cycles { get; }
    
    public Instr Instr { get; }
    public AddressingMode AddressingMode { get; }
    public RegisterType Register1 { get; }
    public RegisterType Register2 { get; }
    public Condition Condition { get; }

    public void Execute(GameboyEmu.Cpu.Cpu cpu, ushort data) => _action?.Invoke(cpu, data);
    
    public override string ToString()
    {
        return $"{Opcode:X2}: {Mnemonic}";
    }
}