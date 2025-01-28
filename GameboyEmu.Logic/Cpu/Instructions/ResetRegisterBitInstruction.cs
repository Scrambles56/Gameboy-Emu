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
        new ResetRegisterBitInstruction(0x87, "RES 0,A", 8, 0, RegisterType.A),
        new ResetRegisterBitInstruction(0x88, "RES 1,B", 8, 1, RegisterType.B),
        new ResetRegisterBitInstruction(0x89, "RES 1,C", 8, 1, RegisterType.C),
        new ResetRegisterBitInstruction(0x8A, "RES 1,D", 8, 1, RegisterType.D),
        new ResetRegisterBitInstruction(0x8B, "RES 1,E", 8, 1, RegisterType.E),
        new ResetRegisterBitInstruction(0x8C, "RES 1,H", 8, 1, RegisterType.H),
        new ResetRegisterBitInstruction(0x8D, "RES 1,L", 8, 1, RegisterType.L),
        new ResetRegisterBitInstruction(0x8F, "RES 1,A", 8, 1, RegisterType.A),
        new ResetRegisterBitInstruction(0x90, "RES 2,B", 8, 2, RegisterType.B),
        new ResetRegisterBitInstruction(0x91, "RES 2,C", 8, 2, RegisterType.C),
        new ResetRegisterBitInstruction(0x92, "RES 2,D", 8, 2, RegisterType.D),
        new ResetRegisterBitInstruction(0x93, "RES 2,E", 8, 2, RegisterType.E),
        new ResetRegisterBitInstruction(0x94, "RES 2,H", 8, 2, RegisterType.H),
        new ResetRegisterBitInstruction(0x95, "RES 2,L", 8, 2, RegisterType.L),
        new ResetRegisterBitInstruction(0x97, "RES 2,A", 8, 2, RegisterType.A),
        new ResetRegisterBitInstruction(0x98, "RES 3,B", 8, 3, RegisterType.B),
        new ResetRegisterBitInstruction(0x99, "RES 3,C", 8, 3, RegisterType.C),
        new ResetRegisterBitInstruction(0x9A, "RES 3,D", 8, 3, RegisterType.D),
        new ResetRegisterBitInstruction(0x9B, "RES 3,E", 8, 3, RegisterType.E),
        new ResetRegisterBitInstruction(0x9C, "RES 3,H", 8, 3, RegisterType.H),
        new ResetRegisterBitInstruction(0x9D, "RES 3,L", 8, 3, RegisterType.L),
        new ResetRegisterBitInstruction(0x9F, "RES 3,A", 8, 3, RegisterType.A),
        new ResetRegisterBitInstruction(0xA0, "RES 4,B", 8, 4, RegisterType.B),
        new ResetRegisterBitInstruction(0xA1, "RES 4,C", 8, 4, RegisterType.C),
        new ResetRegisterBitInstruction(0xA2, "RES 4,D", 8, 4, RegisterType.D),
        new ResetRegisterBitInstruction(0xA3, "RES 4,E", 8, 4, RegisterType.E),
        new ResetRegisterBitInstruction(0xA4, "RES 4,H", 8, 4, RegisterType.H),
        new ResetRegisterBitInstruction(0xA5, "RES 4,L", 8, 4, RegisterType.L),
        new ResetRegisterBitInstruction(0xA7, "RES 4,A", 8, 4, RegisterType.A),
        new ResetRegisterBitInstruction(0xA8, "RES 5,B", 8, 5, RegisterType.B),
        new ResetRegisterBitInstruction(0xA9, "RES 5,C", 8, 5, RegisterType.C),
        new ResetRegisterBitInstruction(0xAA, "RES 5,D", 8, 5, RegisterType.D),
        new ResetRegisterBitInstruction(0xAB, "RES 5,E", 8, 5, RegisterType.E),
        new ResetRegisterBitInstruction(0xAC, "RES 5,H", 8, 5, RegisterType.H),
        new ResetRegisterBitInstruction(0xAD, "RES 5,L", 8, 5, RegisterType.L),
        new ResetRegisterBitInstruction(0xAF, "RES 5,A", 8, 5, RegisterType.A),
        new ResetRegisterBitInstruction(0xB0, "RES 6,B", 8, 6, RegisterType.B),
        new ResetRegisterBitInstruction(0xB1, "RES 6,C", 8, 6, RegisterType.C),
        new ResetRegisterBitInstruction(0xB2, "RES 6,D", 8, 6, RegisterType.D),
        new ResetRegisterBitInstruction(0xB3, "RES 6,E", 8, 6, RegisterType.E),
        new ResetRegisterBitInstruction(0xB4, "RES 6,H", 8, 6, RegisterType.H),
        new ResetRegisterBitInstruction(0xB5, "RES 6,L", 8, 6, RegisterType.L),
        new ResetRegisterBitInstruction(0xB7, "RES 6,A", 8, 6, RegisterType.A),
        new ResetRegisterBitInstruction(0xB8, "RES 7,B", 8, 7, RegisterType.B),
        new ResetRegisterBitInstruction(0xB9, "RES 7,C", 8, 7, RegisterType.C),
        new ResetRegisterBitInstruction(0xBA, "RES 7,D", 8, 7, RegisterType.D),
        new ResetRegisterBitInstruction(0xBB, "RES 7,E", 8, 7, RegisterType.E),
        new ResetRegisterBitInstruction(0xBC, "RES 7,H", 8, 7, RegisterType.H),
        new ResetRegisterBitInstruction(0xBD, "RES 7,L", 8, 7, RegisterType.L),
        new ResetRegisterBitInstruction(0xBF, "RES 7,A", 8, 7, RegisterType.A),
        
        new ResetAddressBitInstruction(0x86, "RES 0,(HL)", 16, 0, RegisterType.HL),
        new ResetAddressBitInstruction(0x8E, "RES 1,(HL)", 16, 1, RegisterType.HL),
        new ResetAddressBitInstruction(0x96, "RES 2,(HL)", 16, 2, RegisterType.HL),
        new ResetAddressBitInstruction(0x9E, "RES 3,(HL)", 16, 3, RegisterType.HL),
        new ResetAddressBitInstruction(0xA6, "RES 4,(HL)", 16, 4, RegisterType.HL),
        new ResetAddressBitInstruction(0xAE, "RES 5,(HL)", 16, 5, RegisterType.HL),
        new ResetAddressBitInstruction(0xB6, "RES 6,(HL)", 16, 6, RegisterType.HL),
        new ResetAddressBitInstruction(0xBE, "RES 7,(HL)", 16, 7, RegisterType.HL)
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

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regValue = cpu.ReadByteRegister(Register1);
        
        var mask = (byte)~(1 << _bitIndex);
        regValue &= mask;
        
        cpu.WriteByteRegister(Register1, regValue);
        
        return Cycles;
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

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadUshortRegister(Register1);
        var value = cpu.ReadByte(address);
        
        var mask = (byte)~(1 << _bitIndex);
        value &= mask;
        
        cpu.WriteByte(address, value);
        
        return Cycles;
    }
}