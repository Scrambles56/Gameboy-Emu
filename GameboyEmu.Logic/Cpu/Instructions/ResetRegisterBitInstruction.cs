namespace GameboyEmu.Logic.Cpu.Instructions;

public static class ResetInstructions
{
    public static List<Instruction> CbInstructions = new()
    {
        new ResetRegisterBitInstruction(0x80, "RES 0,B", 8, 0, RegisterType.B),
        new ResetRegisterBitInstruction(0x81, "RES 0,C", 8, 0, RegisterType.C),
        new ResetRegisterBitInstruction(0x82, "RES 0,D", 8, 0, RegisterType.D),
        new ResetRegisterBitInstruction(0x83, "RES 0,E", 8, 0, RegisterType.E),
        new ResetRegisterBitInstruction(0x84, "RES 0,H", 8, 0, RegisterType.H),
        new ResetRegisterBitInstruction(0x85, "RES 0,L", 8, 0, RegisterType.L),
        new ResetAddressBitInstruction(0x86, "RES 0,(HL)", 16, 0, RegisterType.HL),
        new ResetRegisterBitInstruction(0x87, "RES 0,A", 8, 0, RegisterType.A),
    };
} 

public class ResetRegisterBitInstruction : Instruction
{
    private readonly int _bitIndex;

    public ResetRegisterBitInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        int bitIndex,
        RegisterType register1 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, InstructionSize.None, register1, RegisterType.None)
    {
        _bitIndex = bitIndex;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regValue = cpu.ReadByteRegister(Register1);
        
        var mask = (byte)~(1 << _bitIndex);
        regValue &= mask;
        
        cpu.WriteByteRegister(Register1, regValue);
    }
}

public class ResetAddressBitInstruction : Instruction
{
    private readonly int _bitIndex;
    

    public ResetAddressBitInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        int bitIndex,
        RegisterType register1 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
        _bitIndex = bitIndex;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadByteRegister(Register1);
        var value = cpu.ReadByte(address);
        
        var mask = (byte)~(1 << _bitIndex);
        value &= mask;
        
        cpu.WriteByte(address, value);
    }
}