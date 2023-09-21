namespace GameboyEmu.Logic.Cpu.Instructions;

public static class IncrementInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new IncrementRegisterInstruction(0x04, "INC B", 4, InstructionSize.None, RegisterType.B),
        new IncrementRegisterInstruction(0x0C, "INC C", 4, InstructionSize.None, RegisterType.C),
        new IncrementRegisterInstruction(0x14, "INC D", 4, InstructionSize.None, RegisterType.D),
        new IncrementRegisterInstruction(0x1C, "INC E", 4, InstructionSize.None, RegisterType.E),
        new IncrementRegisterInstruction(0x24, "INC H", 4, InstructionSize.None, RegisterType.H),
        new IncrementRegisterInstruction(0x2C, "INC L", 4, InstructionSize.None, RegisterType.L),
        new IncrementRegisterInstruction(0x3C, "INC A", 4, InstructionSize.None, RegisterType.A),

        new IncrementAddressInstruction(0x34, "INC (HL)", 12, InstructionSize.None, RegisterType.HL),
        
        new IncrementUShortRegister(0x03, "INC BC", 8, InstructionSize.None, RegisterType.BC),
        new IncrementUShortRegister(0x13, "INC DE", 8, InstructionSize.None, RegisterType.DE),
        new IncrementUShortRegister(0x23, "INC HL", 8, InstructionSize.None, RegisterType.HL),
        new IncrementUShortRegister(0x33, "INC SP", 8, InstructionSize.None, RegisterType.SP),
    };
}

public class IncrementUShortRegister : Instruction
{
    public IncrementUShortRegister(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regVal = cpu.ReadUshortRegister(Register1);
        regVal++;
        cpu.WriteUshortRegister(Register1, regVal);
    }
}

public class IncrementRegisterInstruction : Instruction
{
    public IncrementRegisterInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regValue = cpu.ReadByteRegister(Register1);
        cpu.WriteByteRegister(Register1, --regValue);
        
        cpu.F.ZeroFlag = regValue == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = (regValue & 0x0F) == 0x0F;
    }
}

public class IncrementAddressInstruction : Instruction
{
    public IncrementAddressInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regVal = cpu.ReadByteRegister(Register1);
        regVal++;
        cpu.WriteByteRegister(Register1, regVal);
                    
        cpu.F.ZeroFlag = regVal == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = (regVal & 0x0F) == 0x0F;
    }
}