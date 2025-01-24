namespace GameboyEmu.Logic.Cpu.Instructions;

public static class ShiftInstructions
{
    public static List<Instruction> CbInstructions = new List<Instruction>()
    {
        new ShiftRegisterLeftInstruction(0x20, "SLA B", 8, RegisterType.B),
        new ShiftRegisterLeftInstruction(0x21, "SLA C", 8, RegisterType.C),
        new ShiftRegisterLeftInstruction(0x22, "SLA D", 8, RegisterType.D),
        new ShiftRegisterLeftInstruction(0x23, "SLA E", 8, RegisterType.E),
        new ShiftRegisterLeftInstruction(0x24, "SLA H", 8, RegisterType.H),
        new ShiftRegisterLeftInstruction(0x25, "SLA L", 8, RegisterType.L),
        new ShiftAddressLeftInstruction(0x26, "SLA (HL)", 16, RegisterType.HL),
        new ShiftRegisterLeftInstruction(0x27, "SLA A", 8, RegisterType.A),

        new ShiftRegisterRightInstruction(0x28, "SRA B", 8, RegisterType.B),
        new ShiftRegisterRightInstruction(0x29, "SRA C", 8, RegisterType.C),
        new ShiftRegisterRightInstruction(0x2A, "SRA D", 8, RegisterType.D),
        new ShiftRegisterRightInstruction(0x2B, "SRA E", 8, RegisterType.E),
        new ShiftRegisterRightInstruction(0x2C, "SRA H", 8, RegisterType.H),
        new ShiftRegisterRightInstruction(0x2D, "SRA L", 8, RegisterType.L),
        new ShiftAddressRightInstruction(0x2E, "SRA (HL)", 16, RegisterType.HL),
        new ShiftRegisterRightInstruction(0x2F, "SRA A", 8, RegisterType.A)
    };
}

public class ShiftRegisterLeftInstruction : Instruction
{
    public ShiftRegisterLeftInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1 = RegisterType.None) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var value = cpu.ReadByteRegister(Register1);
        var topBit = (value & 0x80) >> 7;
        value = (byte)(value << 1);
        
        cpu.WriteByteRegister(Register1, value);
        
        cpu.F.ZeroFlag = value == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = topBit == 1;
    }
}

public class ShiftAddressLeftInstruction : Instruction
{
    public ShiftAddressLeftInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1 = RegisterType.None) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadUshortRegister(Register1);
        var value = cpu.ReadByte(address);
        var topBit = (value & 0x80) >> 7;
        value = (byte)(value << 1);
        
        cpu.WriteByte(address, value);
        cpu.F.ZeroFlag = value == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = topBit == 1;
    }
}

public class ShiftRegisterRightInstruction : Instruction
{
    public ShiftRegisterRightInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1 = RegisterType.None) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var value = cpu.ReadByteRegister(Register1);
        var bottomBit = value & 1;
        var topBit = value & 0x80;
        value = (byte)((value >> 1) | topBit);
        
        cpu.WriteByteRegister(Register1, value);
        
        cpu.F.ZeroFlag = value == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = bottomBit == 1;
    }
}

public class ShiftAddressRightInstruction : Instruction
{
    public ShiftAddressRightInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1 = RegisterType.None) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadUshortRegister(Register1);
        var value = cpu.ReadByte(address);
        var bottomBit = value & 1;
        var topBit = value & 0x80;
        value = (byte)((value >> 1) | topBit);
        
        cpu.WriteByte(address, value);
        cpu.F.ZeroFlag = value == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = bottomBit == 1;
    }
}