namespace GameboyEmu.Logic.Cpu.Instructions;

public static class SetBitInstructions
{
    public static List<Instruction> CbInstructions = new List<Instruction>
    {
        new SetRegisterBitInstruction(0xC0, "SET 0,B", 8, RegisterType.B, 0),
        new SetRegisterBitInstruction(0xC1, "SET 0,C", 8, RegisterType.C, 0),
        new SetRegisterBitInstruction(0xC2, "SET 0,D", 8, RegisterType.D, 0),
        new SetRegisterBitInstruction(0xC3, "SET 0,E", 8, RegisterType.E, 0),
        new SetRegisterBitInstruction(0xC4, "SET 0,H", 8, RegisterType.H, 0),
        new SetRegisterBitInstruction(0xC5, "SET 0,L", 8, RegisterType.L, 0),
        new SetRegisterBitInstruction(0xC7, "SET 0,A", 8, RegisterType.A, 0),
        new SetRegisterBitInstruction(0xC8, "SET 1,B", 8, RegisterType.B, 1),
        new SetRegisterBitInstruction(0xC9, "SET 1,C", 8, RegisterType.C, 1),
        new SetRegisterBitInstruction(0xCA, "SET 1,D", 8, RegisterType.D, 1),
        new SetRegisterBitInstruction(0xCB, "SET 1,E", 8, RegisterType.E, 1),
        new SetRegisterBitInstruction(0xCC, "SET 1,H", 8, RegisterType.H, 1),
        new SetRegisterBitInstruction(0xCD, "SET 1,L", 8, RegisterType.L, 1),
        new SetRegisterBitInstruction(0xCF, "SET 1,A", 8, RegisterType.A, 1),
        new SetRegisterBitInstruction(0xD0, "SET 2,B", 8, RegisterType.B, 2),
        new SetRegisterBitInstruction(0xD1, "SET 2,C", 8, RegisterType.C, 2),
        new SetRegisterBitInstruction(0xD2, "SET 2,D", 8, RegisterType.D, 2),
        new SetRegisterBitInstruction(0xD3, "SET 2,E", 8, RegisterType.E, 2),
        new SetRegisterBitInstruction(0xD4, "SET 2,H", 8, RegisterType.H, 2),
        new SetRegisterBitInstruction(0xD5, "SET 2,L", 8, RegisterType.L, 2),
        new SetRegisterBitInstruction(0xD7, "SET 2,A", 8, RegisterType.A, 2),
        new SetRegisterBitInstruction(0xD8, "SET 3,B", 8, RegisterType.B, 3),
        new SetRegisterBitInstruction(0xD9, "SET 3,C", 8, RegisterType.C, 3),
        new SetRegisterBitInstruction(0xDA, "SET 3,D", 8, RegisterType.D, 3),
        new SetRegisterBitInstruction(0xDB, "SET 3,E", 8, RegisterType.E, 3),
        new SetRegisterBitInstruction(0xDC, "SET 3,H", 8, RegisterType.H, 3),
        new SetRegisterBitInstruction(0xDD, "SET 3,L", 8, RegisterType.L, 3),
        new SetRegisterBitInstruction(0xDF, "SET 3,A", 8, RegisterType.A, 3),
        new SetRegisterBitInstruction(0xE0, "SET 4,B", 8, RegisterType.B, 4),
        new SetRegisterBitInstruction(0xE1, "SET 4,C", 8, RegisterType.C, 4),
        new SetRegisterBitInstruction(0xE2, "SET 4,D", 8, RegisterType.D, 4),
        new SetRegisterBitInstruction(0xE3, "SET 4,E", 8, RegisterType.E, 4),
        new SetRegisterBitInstruction(0xE4, "SET 4,H", 8, RegisterType.H, 4),
        new SetRegisterBitInstruction(0xE5, "SET 4,L", 8, RegisterType.L, 4),
        new SetRegisterBitInstruction(0xE7, "SET 4,A", 8, RegisterType.A, 4),
        new SetRegisterBitInstruction(0xE8, "SET 5,B", 8, RegisterType.B, 5),
        new SetRegisterBitInstruction(0xE9, "SET 5,C", 8, RegisterType.C, 5),
        new SetRegisterBitInstruction(0xEA, "SET 5,D", 8, RegisterType.D, 5),
        new SetRegisterBitInstruction(0xEB, "SET 5,E", 8, RegisterType.E, 5),
        new SetRegisterBitInstruction(0xEC, "SET 5,H", 8, RegisterType.H, 5),
        new SetRegisterBitInstruction(0xED, "SET 5,L", 8, RegisterType.L, 5),
        new SetRegisterBitInstruction(0xEF, "SET 5,A", 8, RegisterType.A, 5),
        new SetRegisterBitInstruction(0xF0, "SET 6,B", 8, RegisterType.B, 6),
        new SetRegisterBitInstruction(0xF1, "SET 6,C", 8, RegisterType.C, 6),
        new SetRegisterBitInstruction(0xF2, "SET 6,D", 8, RegisterType.D, 6),
        new SetRegisterBitInstruction(0xF3, "SET 6,E", 8, RegisterType.E, 6),
        new SetRegisterBitInstruction(0xF4, "SET 6,H", 8, RegisterType.H, 6),
        new SetRegisterBitInstruction(0xF5, "SET 6,L", 8, RegisterType.L, 6),
        new SetRegisterBitInstruction(0xF7, "SET 6,A", 8, RegisterType.A, 6),
        new SetRegisterBitInstruction(0xF8, "SET 7,B", 8, RegisterType.B, 7),
        new SetRegisterBitInstruction(0xF9, "SET 7,C", 8, RegisterType.C, 7),
        new SetRegisterBitInstruction(0xFA, "SET 7,D", 8, RegisterType.D, 7),
        new SetRegisterBitInstruction(0xFB, "SET 7,E", 8, RegisterType.E, 7),
        new SetRegisterBitInstruction(0xFC, "SET 7,H", 8, RegisterType.H, 7),
        new SetRegisterBitInstruction(0xFD, "SET 7,L", 8, RegisterType.L, 7),
        new SetRegisterBitInstruction(0xFF, "SET 7,A", 8, RegisterType.A, 7),

        new SetAddressBitInstruction(0xC6, "SET 0,(HL)", 16, RegisterType.HL, 0),
        new SetAddressBitInstruction(0xCE, "SET 1,(HL)", 16, RegisterType.HL, 1),
        new SetAddressBitInstruction(0xD6, "SET 2,(HL)", 16, RegisterType.HL, 2),
        new SetAddressBitInstruction(0xDE, "SET 3,(HL)", 16, RegisterType.HL, 3),
        new SetAddressBitInstruction(0xE6, "SET 4,(HL)", 16, RegisterType.HL, 4),
        new SetAddressBitInstruction(0xEE, "SET 5,(HL)", 16, RegisterType.HL, 5),
        new SetAddressBitInstruction(0xF6, "SET 6,(HL)", 16, RegisterType.HL, 6),
        new SetAddressBitInstruction(0xFE, "SET 7,(HL)", 16, RegisterType.HL, 7)
    };
}

public class SetRegisterBitInstruction : Instruction
{
    private readonly int _bitIndex;

    public SetRegisterBitInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,  
        RegisterType register1,
        int bitIndex
    ) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
        _bitIndex = bitIndex;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regValue = cpu.ReadByteRegister(Register1);
        var mask = (byte)(1 << _bitIndex);
        
        regValue |= mask;
        cpu.WriteByteRegister(Register1, regValue);
    }
}

public class SetAddressBitInstruction : Instruction
{
    private readonly int _bitIndex;

    public SetAddressBitInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        RegisterType register1,
        int bitIndex
    ) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
        _bitIndex = bitIndex;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadUshortRegister(Register1);
        var value = cpu.ReadByte(address);
        var mask = (byte)(1 << _bitIndex);
        
        value |= mask;
        cpu.WriteByte(address, value);
    }
}