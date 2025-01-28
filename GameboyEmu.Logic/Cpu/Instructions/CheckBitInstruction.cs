namespace GameboyEmu.Logic.Cpu.Instructions;

public static class CheckBitInstructions
{
    public static List<Instruction> CbInstructions = new List<Instruction>
    {
        new CheckRegisterBitInstruction(0x40, "BIT 0, B", 8, RegisterType.B, 0),
        new CheckRegisterBitInstruction(0x41, "BIT 0, C", 8, RegisterType.C, 0),
        new CheckRegisterBitInstruction(0x42, "BIT 0, D", 8, RegisterType.D, 0),
        new CheckRegisterBitInstruction(0x43, "BIT 0, E", 8, RegisterType.E, 0),
        new CheckRegisterBitInstruction(0x44, "BIT 0, H", 8, RegisterType.H, 0),
        new CheckRegisterBitInstruction(0x45, "BIT 0, L", 8, RegisterType.L, 0),
        new CheckRegisterBitInstruction(0x47, "BIT 0, A", 8, RegisterType.A, 0),
        new CheckRegisterBitInstruction(0x48, "BIT 1, B", 8, RegisterType.B, 1),
        new CheckRegisterBitInstruction(0x49, "BIT 1, C", 8, RegisterType.C, 1),
        new CheckRegisterBitInstruction(0x4A, "BIT 1, D", 8, RegisterType.D, 1),
        new CheckRegisterBitInstruction(0x4B, "BIT 1, E", 8, RegisterType.E, 1),
        new CheckRegisterBitInstruction(0x4C, "BIT 1, H", 8, RegisterType.H, 1),
        new CheckRegisterBitInstruction(0x4D, "BIT 1, L", 8, RegisterType.L, 1),
        new CheckRegisterBitInstruction(0x4F, "BIT 1, A", 8, RegisterType.A, 1),
        new CheckRegisterBitInstruction(0x50, "BIT 2, B", 8, RegisterType.B, 2),
        new CheckRegisterBitInstruction(0x51, "BIT 2, C", 8, RegisterType.C, 2),
        new CheckRegisterBitInstruction(0x52, "BIT 2, D", 8, RegisterType.D, 2),
        new CheckRegisterBitInstruction(0x53, "BIT 2, E", 8, RegisterType.E, 2),
        new CheckRegisterBitInstruction(0x54, "BIT 2, H", 8, RegisterType.H, 2),
        new CheckRegisterBitInstruction(0x55, "BIT 2, L", 8, RegisterType.L, 2),
        new CheckRegisterBitInstruction(0x57, "BIT 2, A", 8, RegisterType.A, 2),
        new CheckRegisterBitInstruction(0x58, "BIT 3, B", 8, RegisterType.B, 3),
        new CheckRegisterBitInstruction(0x59, "BIT 3, C", 8, RegisterType.C, 3),
        new CheckRegisterBitInstruction(0x5A, "BIT 3, D", 8, RegisterType.D, 3),
        new CheckRegisterBitInstruction(0x5B, "BIT 3, E", 8, RegisterType.E, 3),
        new CheckRegisterBitInstruction(0x5C, "BIT 3, H", 8, RegisterType.H, 3),
        new CheckRegisterBitInstruction(0x5D, "BIT 3, L", 8, RegisterType.L, 3),
        new CheckRegisterBitInstruction(0x5F, "BIT 3, A", 8, RegisterType.A, 3),
        new CheckRegisterBitInstruction(0x60, "BIT 4, B", 8, RegisterType.B, 4),
        new CheckRegisterBitInstruction(0x61, "BIT 4, C", 8, RegisterType.C, 4),
        new CheckRegisterBitInstruction(0x62, "BIT 4, D", 8, RegisterType.D, 4),
        new CheckRegisterBitInstruction(0x63, "BIT 4, E", 8, RegisterType.E, 4),
        new CheckRegisterBitInstruction(0x64, "BIT 4, H", 8, RegisterType.H, 4),
        new CheckRegisterBitInstruction(0x65, "BIT 4, L", 8, RegisterType.L, 4),
        new CheckRegisterBitInstruction(0x67, "BIT 4, A", 8, RegisterType.A, 4),
        new CheckRegisterBitInstruction(0x68, "BIT 5, B", 8, RegisterType.B, 5),
        new CheckRegisterBitInstruction(0x69, "BIT 5, C", 8, RegisterType.C, 5),
        new CheckRegisterBitInstruction(0x6A, "BIT 5, D", 8, RegisterType.D, 5),
        new CheckRegisterBitInstruction(0x6B, "BIT 5, E", 8, RegisterType.E, 5),
        new CheckRegisterBitInstruction(0x6C, "BIT 5, H", 8, RegisterType.H, 5),
        new CheckRegisterBitInstruction(0x6D, "BIT 5, L", 8, RegisterType.L, 5),
        new CheckRegisterBitInstruction(0x6F, "BIT 5, A", 8, RegisterType.A, 5),
        new CheckRegisterBitInstruction(0x70, "BIT 6, B", 8, RegisterType.B, 6),
        new CheckRegisterBitInstruction(0x71, "BIT 6, C", 8, RegisterType.C, 6),
        new CheckRegisterBitInstruction(0x72, "BIT 6, D", 8, RegisterType.D, 6),
        new CheckRegisterBitInstruction(0x73, "BIT 6, E", 8, RegisterType.E, 6),
        new CheckRegisterBitInstruction(0x74, "BIT 6, H", 8, RegisterType.H, 6),
        new CheckRegisterBitInstruction(0x75, "BIT 6, L", 8, RegisterType.L, 6),
        new CheckRegisterBitInstruction(0x77, "BIT 6, A", 8, RegisterType.A, 6),
        new CheckRegisterBitInstruction(0x78, "BIT 7, B", 8, RegisterType.B, 7),
        new CheckRegisterBitInstruction(0x79, "BIT 7, C", 8, RegisterType.C, 7),
        new CheckRegisterBitInstruction(0x7A, "BIT 7, D", 8, RegisterType.D, 7),
        new CheckRegisterBitInstruction(0x7B, "BIT 7, E", 8, RegisterType.E, 7),
        new CheckRegisterBitInstruction(0x7C, "BIT 7, H", 8, RegisterType.H, 7),
        new CheckRegisterBitInstruction(0x7D, "BIT 7, L", 8, RegisterType.L, 7),
        new CheckRegisterBitInstruction(0x7F, "BIT 7, A", 8, RegisterType.A, 7),
        
        new CheckAddressBitInstruction(0x46, "BIT 0, (HL)", 12, RegisterType.HL, 0),
        new CheckAddressBitInstruction(0x4E, "BIT 1, (HL)", 12, RegisterType.HL, 1),
        new CheckAddressBitInstruction(0x56, "BIT 2, (HL)", 12, RegisterType.HL, 2),
        new CheckAddressBitInstruction(0x5E, "BIT 3, (HL)", 12, RegisterType.HL, 3),
        new CheckAddressBitInstruction(0x66, "BIT 4, (HL)", 12, RegisterType.HL, 4),
        new CheckAddressBitInstruction(0x6E, "BIT 5, (HL)", 12, RegisterType.HL, 5),
        new CheckAddressBitInstruction(0x76, "BIT 6, (HL)", 12, RegisterType.HL, 6),
        new CheckAddressBitInstruction(0x7E, "BIT 7, (HL)", 12, RegisterType.HL, 7)
    };
}

public class CheckRegisterBitInstruction : Instruction
{
    private readonly int _bitIndex;

    public CheckRegisterBitInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        RegisterType register1,
        int bitIndex) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
        _bitIndex = bitIndex;
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var mask = 1 << _bitIndex;
        var value = cpu.ReadByteRegister(Register1);
        var result = (value & mask) == 0;
        
        cpu.F.ZeroFlag = result;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = true;
        
        return Cycles;
    }
}

public class CheckAddressBitInstruction : Instruction
{
    private readonly int _bitIndex;

    public CheckAddressBitInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1,
        int bitIndex) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
        _bitIndex = bitIndex;
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var mask = 1 << _bitIndex;
        var address = cpu.ReadUshortRegister(Register1);
        var value = cpu.ReadByte(address);
        var result = (value & mask) == 0;
        
        cpu.F.ZeroFlag = result;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = true;
        
        return Cycles;
    }
}