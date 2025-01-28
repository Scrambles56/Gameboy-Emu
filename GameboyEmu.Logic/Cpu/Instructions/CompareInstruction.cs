using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class CompareInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new CompareInstruction(0xB8, "CP B", 4, InstructionSize.None, RegisterType.A, RegisterType.B),
        new CompareInstruction(0xB9, "CP C", 4, InstructionSize.None, RegisterType.A, RegisterType.C),
        new CompareInstruction(0xBA, "CP D", 4, InstructionSize.None, RegisterType.A, RegisterType.D),
        new CompareInstruction(0xBB, "CP E", 4, InstructionSize.None, RegisterType.A, RegisterType.E),
        new CompareInstruction(0xBC, "CP H", 4, InstructionSize.None, RegisterType.A, RegisterType.H),
        new CompareInstruction(0xBD, "CP L", 4, InstructionSize.None, RegisterType.A, RegisterType.L),
        new CompareInstruction(0xBE, "CP (HL)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL, loadSecondRegister: true),
        new CompareInstruction(0xBF, "CP A", 4, InstructionSize.None, RegisterType.A, RegisterType.A),
        new CompareInstruction(0xFE, "CP d8", 8, InstructionSize.D8, RegisterType.A)
    };
}

public class CompareInstruction : Instruction
{
    private readonly bool _loadSecondRegister;

    public CompareInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None,
        bool loadSecondRegister = false
        ) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        Debug.Assert(Register1 != RegisterType.None);
        Debug.Assert(
            InstructionSize == InstructionSize.None ?
                Register2 != RegisterType.None :
                Register2 == RegisterType.None
        );
        
        _loadSecondRegister = loadSecondRegister;
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var registerValue = cpu.ReadByteRegister(Register1);
        byte compValue;
        if (InstructionSize == InstructionSize.D8)
        {
            compValue = data.ToByte();
        }
        else if (_loadSecondRegister)
        {
            var address = cpu.ReadUshortRegister(Register2);
            compValue = cpu.ReadByte(address);
        }
        else
        {
            compValue = cpu.ReadByteRegister(Register2);
        }
        
        cpu.F.ZeroFlag = registerValue == compValue;
        cpu.F.SubtractFlag = true;
        cpu.F.HalfCarryFlag = (registerValue & 0x0F) < (compValue & 0x0F);
        cpu.F.CarryFlag = registerValue < compValue;
        
        return Cycles;
    }
}