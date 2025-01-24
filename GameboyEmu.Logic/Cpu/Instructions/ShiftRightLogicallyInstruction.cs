namespace GameboyEmu.Logic.Cpu.Instructions;

public static class ShiftRightLogicallyInstructions
{
    public static List<Instruction> CbInstructions = new()
    {
        new ShiftRightLogicallyInstruction(
            0x38,
            "SRL B",
            8,
            InstructionSize.None,
            RegisterType.B
        ),
        new ShiftRightLogicallyInstruction(
            0x39,
            "SRL C",
            8,
            InstructionSize.None,
            RegisterType.C
        ),
        new ShiftRightLogicallyInstruction(
            0x3A,
            "SRL D",
            8,
            InstructionSize.None,
            RegisterType.D
        ),
        new ShiftRightLogicallyInstruction(
            0x3B,
            "SRL E",
            8,
            InstructionSize.None,
            RegisterType.E
        ),
        new ShiftRightLogicallyInstruction(
            0x3C,
            "SRL H",
            8,
            InstructionSize.None,
            RegisterType.H
        ),
        new ShiftRightLogicallyInstruction(
            0x3D,
            "SRL L",
            8,
            InstructionSize.None,
            RegisterType.L
        ),
        new ShiftRightLogicallyInstruction(
            0x3E,
            "SRL (HL)",
            16,
            InstructionSize.None,
            RegisterType.HL,
            asAddress: true
        ),
        new ShiftRightLogicallyInstruction(
            0x3F,
            "SRL A",
            8,
            InstructionSize.None,
            RegisterType.A
        )
    };
}

public class ShiftRightLogicallyInstruction : Instruction
{
    private readonly bool _asAddress;

    public ShiftRightLogicallyInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        InstructionSize instructionSize, 
        RegisterType register1,
        bool asAddress = false) 
    : base(opcode, mnemonic, cycles, instructionSize, register1)
    {
        _asAddress = asAddress;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        byte value;

        int overflowBit;
        byte newValue;
        if (_asAddress)
        {
            var address = cpu.ReadUshortRegister(Register1);
            value = cpu.ReadByte(address);
            
            overflowBit = value & 1;
            newValue = (byte)(value >> 1);
            
            cpu.WriteByte(address, newValue);
        }
        else
        {
            value = cpu.ReadByteRegister(Register1);
            
            overflowBit = value & 1;
            newValue = (byte)(value >> 1);
            
            cpu.WriteByteRegister(Register1, newValue);
        }
        
        
        cpu.F.ZeroFlag = newValue == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = overflowBit == 1;
    }
}