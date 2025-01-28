using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class OrInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new OrInstruction(0xB0, "OR B", 4, InstructionSize.None, RegisterType.A, RegisterType.B),
        new OrInstruction(0xB1, "OR C", 4, InstructionSize.None, RegisterType.A, RegisterType.C),
        new OrInstruction(0xB2, "OR D", 4, InstructionSize.None, RegisterType.A, RegisterType.D),
        new OrInstruction(0xB3, "OR E", 4, InstructionSize.None, RegisterType.A, RegisterType.E),
        new OrInstruction(0xB4, "OR H", 4, InstructionSize.None, RegisterType.A, RegisterType.H),
        new OrInstruction(0xB5, "OR L", 4, InstructionSize.None, RegisterType.A, RegisterType.L),
        new OrInstruction(0xB6, "OR (HL)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL, loadSecondRegister: true),
        new OrInstruction(0xB7, "OR A", 4, InstructionSize.None, RegisterType.A, RegisterType.A),
        new OrInstruction(0xF6, "OR d8", 8, InstructionSize.D8, RegisterType.A)
    };
}

public class OrInstruction : Instruction
{
    private readonly bool _loadSecondRegister;

    public OrInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        InstructionSize size = InstructionSize.None,
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None,
        bool loadSecondRegister = false) 
    : base(opcode, mnemonic, cycles, size, register1, register2)
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
        
        var result = (byte)(value1 | value2);

        cpu.WriteByteRegister(Register1, result);

        cpu.F.ZeroFlag = result == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = false;
        
        return Cycles;
    }
}