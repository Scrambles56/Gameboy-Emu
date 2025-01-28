using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class SubtractInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new SubtractInstruction( 0x90, "SUB B", 4, InstructionSize.None, RegisterType.A, RegisterType.B),
        new SubtractInstruction( 0x91, "SUB C", 4, InstructionSize.None, RegisterType.A, RegisterType.C),
        new SubtractInstruction( 0x92, "SUB D", 4, InstructionSize.None, RegisterType.A, RegisterType.D),
        new SubtractInstruction( 0x93, "SUB E", 4, InstructionSize.None, RegisterType.A, RegisterType.E),
        new SubtractInstruction( 0x94, "SUB H", 4, InstructionSize.None, RegisterType.A, RegisterType.H),
        new SubtractInstruction( 0x95, "SUB L", 4, InstructionSize.None, RegisterType.A, RegisterType.L),
        new SubtractInstruction( 0x96, "SUB (HL)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL, loadSecondRegister: true),
        new SubtractInstruction( 0x97, "SUB A", 4, InstructionSize.None, RegisterType.A, RegisterType.A),
        new SubtractInstruction( 0x98, "SBC A,B", 4, InstructionSize.None, RegisterType.A, RegisterType.B, withCarry: true),
        new SubtractInstruction( 0x99, "SBC A,C", 4, InstructionSize.None, RegisterType.A, RegisterType.C, withCarry: true),
        new SubtractInstruction( 0x9A, "SBC A,D", 4, InstructionSize.None, RegisterType.A, RegisterType.D, withCarry: true),
        new SubtractInstruction( 0x9B, "SBC A,E", 4, InstructionSize.None, RegisterType.A, RegisterType.E, withCarry: true),
        new SubtractInstruction( 0x9C, "SBC A,H", 4, InstructionSize.None, RegisterType.A, RegisterType.H, withCarry: true),
        new SubtractInstruction( 0x9D, "SBC A,L", 4, InstructionSize.None, RegisterType.A, RegisterType.L, withCarry: true),
        new SubtractInstruction( 0x9E, "SBC A,(HL)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL, loadSecondRegister: true, withCarry: true),
        new SubtractInstruction( 0x9F, "SBC A,A", 4, InstructionSize.None, RegisterType.A, RegisterType.A, withCarry: true),
        new SubtractInstruction( 0xD6, "SUB d8", 8, InstructionSize.D8, RegisterType.A, RegisterType.None),
        new SubtractInstruction( 0xDE, "SBC A,d8", 8, InstructionSize.D8, RegisterType.A, RegisterType.None, withCarry: true)
    };
}

public class SubtractInstruction : Instruction
{
    private readonly bool _loadSecondRegister;
    private readonly bool _withCarry;

    public SubtractInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        InstructionSize instructionSize,
        RegisterType register1, 
        RegisterType register2,
        bool loadSecondRegister = false,
        bool withCarry = false) 
    : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        Debug.Assert(Register1 != RegisterType.None);
        Debug.Assert(
            InstructionSize == InstructionSize.None ?
                Register2 != RegisterType.None :
                Register2 == RegisterType.None
        );
        _loadSecondRegister = loadSecondRegister;
        _withCarry = withCarry;
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
        
        var carry = _withCarry && cpu.F.CarryFlag ? 1 : 0;
        var result = value1 - value2 - carry;
        
        cpu.WriteByteRegister(Register1, (byte)result);

        cpu.F.ZeroFlag = (byte)result == 0;
        cpu.F.SubtractFlag = true;
        cpu.F.HalfCarryFlag = (((value1 & 0x0F) - (value2 & 0x0F) - (carry & 0x0F)) & 0x10) > 0;
        cpu.F.CarryFlag = value1 < (value2 + carry);
        
        return Cycles;
    }
}