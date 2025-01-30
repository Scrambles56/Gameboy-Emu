using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class JumpInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new JumpRelativeInstruction(0x20, "JR NZ,r8", 12, InstructionSize.D8, condition: Condition.NZ),
        new JumpRelativeInstruction(0x30, "JR NC,r8", 12, InstructionSize.D8, condition: Condition.NC),
        new JumpRelativeInstruction(0x18, "JR r8", 12, InstructionSize.D8),
        new JumpRelativeInstruction(0x28, "JR Z, n", 12, InstructionSize.D8, condition: Condition.Z),
        new JumpRelativeInstruction(0x38, "JR C,r8", 12, InstructionSize.D8, condition: Condition.C),
        
        new JumpAbsoluteInstruction(0xC2, "JP NZ,a16", 16, InstructionSize.D16, condition: Condition.NZ),
        new JumpAbsoluteInstruction(0xD2, "JP NC,a16", 16, InstructionSize.D16, condition: Condition.NC),
        new JumpAbsoluteInstruction(0xC3, "JP nn", 16, InstructionSize.D16),
        new JumpAbsoluteInstruction(0xCA, "JP Z,a16", 16, InstructionSize.D16, condition: Condition.Z),
        new JumpAbsoluteInstruction(0xDA, "JP C,a16", 16, InstructionSize.D16, condition: Condition.C),
        
        new JumpHLInstruction()
    };
}

public class JumpAbsoluteInstruction : Instruction
{
    private Condition Condition { get; }

    public JumpAbsoluteInstruction(
            byte opcode, 
            string mnemonic, 
            int cycles, 
            InstructionSize instructionSize = InstructionSize.None, 
            RegisterType register1 = RegisterType.None, 
            RegisterType register2 = RegisterType.None, 
            Condition condition = Condition.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        Condition = condition;
        Debug.Assert(InstructionSize != InstructionSize.D8);
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        if (!cpu.CheckCondition(Condition))
        {
            return 12;
        }
        
        cpu.PC.SetValue(data.ToUshort());
        return Cycles;
    }
}

public class JumpRelativeInstruction : Instruction
{
    public Condition Condition { get; }

    public JumpRelativeInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None, 
        Condition condition = Condition.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        Condition = condition;
        
        Debug.Assert(InstructionSize != InstructionSize.D16);
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        if (!cpu.CheckCondition(Condition))
        {
            return 8;
        }

        var offset = (sbyte)data.ToByte();
        
        var pcVal = (ushort)(cpu.PC + offset);
        cpu.PC = pcVal;
        return Cycles;
    }
}

public class JumpHLInstruction : Instruction
{
    public JumpHLInstruction() 
        : base(0xE9, "jp HL", 4)
    {
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        cpu.PC = cpu.HL;
        
        return Cycles;
    }
}