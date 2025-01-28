namespace GameboyEmu.Logic.Cpu.Instructions;

public static class DecrementInstructions
{
    public static List<Instruction> Instructions = new()
    {

        new DecrementRegisterInstruction(0x05, "DEC B", 4, InstructionSize.None, RegisterType.B),
        new DecrementRegisterInstruction(0x0D, "DEC C", 4, InstructionSize.None, RegisterType.C),
        new DecrementRegisterInstruction(0x15, "DEC D", 4, InstructionSize.None, RegisterType.D),
        new DecrementRegisterInstruction(0x1D, "DEC E", 4, InstructionSize.None, RegisterType.E),
        new DecrementRegisterInstruction(0x25, "DEC H", 4, InstructionSize.None, RegisterType.H),
        new DecrementRegisterInstruction(0x2D, "DEC L", 4, InstructionSize.None, RegisterType.L),
        new DecrementRegisterInstruction(0x3D, "DEC A", 4, InstructionSize.None, RegisterType.A),
        
        new DecrementUShortRegister(0x0B, "DEC BC", 8, InstructionSize.None, RegisterType.BC),
        new DecrementUShortRegister(0x1B, "DEC DE", 8, InstructionSize.None, RegisterType.DE),
        new DecrementUShortRegister(0x2B, "DEC HL", 8, InstructionSize.None, RegisterType.HL),
        new DecrementUShortRegister(0x3B, "DEC SP", 8, InstructionSize.None, RegisterType.SP),
        
        new GenericInstruction(
            0x35,
            "DEC (HL)",
            12,
            InstructionSize.None,
            RegisterType.HL,
            RegisterType.None,
            action: (instruction, cpu, data) =>
            {
                var address = cpu.ReadUshortRegister(instruction.Register1);
                var value = cpu.ReadByte(address);
                
                cpu.WriteByte(address, --value);
                
                cpu.F.ZeroFlag = value == 0;
                cpu.F.SubtractFlag = true;
                cpu.F.HalfCarryFlag = (value & 0x0F) == 0x0F;
                
                return instruction.Cycles;
            }
        )
    };
}

public class DecrementUShortRegister : Instruction
{
    public DecrementUShortRegister(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regVal = cpu.ReadUshortRegister(Register1);
        regVal--;
        cpu.WriteUshortRegister(Register1, regVal);
        
        return Cycles;
    }
}

public class DecrementRegisterInstruction : Instruction
{
    public DecrementRegisterInstruction(
            byte opcode, 
            string mnemonic, 
            int cycles, 
            InstructionSize instructionSize = InstructionSize.None, 
            RegisterType register1 = RegisterType.None, 
            RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regVal = cpu.ReadByteRegister(Register1);
        regVal--;
        cpu.WriteByteRegister(Register1, regVal);
                    
        cpu.F.ZeroFlag = regVal == 0;
        cpu.F.SubtractFlag = true;
        cpu.F.HalfCarryFlag = (regVal & 0x0F) == 0x0F;

        return Cycles;
    }
}