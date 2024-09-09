using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class XorInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new XorInstruction(
            0xA8,
            "XOR B",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.B
        ),
        new XorInstruction(
            0xA9,
            "XOR C",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.C
        ),
        new XorInstruction(
            0xAA,
            "XOR D",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.D
        ),
        new XorInstruction(
            0xAB,
            "XOR E",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.E
        ),
        new XorInstruction(
            0xAC,
            "XOR H",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.H
        ),
        new XorInstruction(
            0xAD,
            "XOR L",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.L
        ),
        new XorInstruction(
            0xAE,
            "XOR (HL)",
            8,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.HL,
            loadSecondRegister: true
        ),
        new XorInstruction(
            0xAF,
            "XOR A",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.A
        ),
        new XorInstruction(
            0xEE,
            "XOR d8",
            8,
            InstructionSize.D8,
            RegisterType.A,
            RegisterType.None
        )
    };
}

public class XorInstruction : Instruction
{
    private readonly bool _loadSecondRegister;

    public XorInstruction(
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
        Debug.Assert(
            InstructionSize == InstructionSize.None ?
                Register2 != RegisterType.None :
                Register2 == RegisterType.None
        );
        _loadSecondRegister = loadSecondRegister;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
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
        
        var result = (byte)(value1 ^ value2);

        cpu.WriteByteRegister(Register1, result);

        cpu.F.ZeroFlag = result == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = false;
    }
}