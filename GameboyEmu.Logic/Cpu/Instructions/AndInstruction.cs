using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class AndInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new AndInstruction(0xA0, "AND B", 4, InstructionSize.None, RegisterType.A, RegisterType.B),
        new AndInstruction(0xA1, "AND C", 4, InstructionSize.None, RegisterType.A, RegisterType.C),
        new AndInstruction(0xA2, "AND D", 4, InstructionSize.None, RegisterType.A, RegisterType.D),
        new AndInstruction(0xA3, "AND E", 4, InstructionSize.None, RegisterType.A, RegisterType.E),
        new AndInstruction(0xA4, "AND H", 4, InstructionSize.None, RegisterType.A, RegisterType.H),
        new AndInstruction(0xA5, "AND L", 4, InstructionSize.None, RegisterType.A, RegisterType.L),
        new AndInstruction(0xA6, "AND (HL)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL, loadSecondRegister: true),
        new AndInstruction(0xA7, "AND A", 4, InstructionSize.None, RegisterType.A, RegisterType.A),
        new AndInstruction(0xE6, "AND d8", 8, InstructionSize.D8, RegisterType.A, RegisterType.None)
    };
}

public class AndInstruction : Instruction
{
    private readonly bool _loadSecondRegister;

    public AndInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        InstructionSize size,
        RegisterType register1, 
        RegisterType register2,
        bool loadSecondRegister = false) 
    : base(opcode, mnemonic, cycles, size, register1, register2)
    {
        Debug.Assert(Register1 != RegisterType.None);
        Debug.Assert(InstructionSize == InstructionSize.None ? Register2 != RegisterType.None : Register2 == RegisterType.None);
        _loadSecondRegister = loadSecondRegister;
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var value1 = cpu.ReadByteRegister(Register1);
        byte value2;
        if (InstructionSize == InstructionSize.D8)
        {
            value2 = data.ToByte();
        }
        else if (_loadSecondRegister)
        {
            var address = cpu.ReadUshortRegister(Register2);
            value2 = cpu.ReadByte(address);
        }
        else
        {
            value2 = cpu.ReadByteRegister(Register2);
        }
        
        var result = (byte)(value1 & value2);
        
        cpu.WriteByteRegister(Register1, result);
        
        cpu.F.ZeroFlag = result == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = true;
        cpu.F.CarryFlag = false;

        return Cycles;
    }
}