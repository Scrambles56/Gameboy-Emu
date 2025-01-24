using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class AddInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new AddInstruction(
            0x80,
            "ADD A,B",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.B
        ),
        new AddInstruction(
            0x81,
            "ADD A,C",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.C
        ),
        new AddInstruction(
            0x82,
            "ADD A,D",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.D
        ),
        new AddInstruction(
            0x83,
            "ADD A,E",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.E
        ),
        new AddInstruction(
            0x84,
            "ADD A,H",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.H
        ),
        new AddInstruction(
            0x85,
            "ADD A,L",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.L
        ),
        new AddInstruction(
            0x86,
            "ADD A,(HL)",
            8,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.HL,
            loadSecondRegister: true
        ),
        new AddInstruction(
            0x87,
            "ADD A,A",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.A
        ),
        new AddInstruction(
            0x88,
            "ADC A,B",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.B,
            withCarry: true
        ),
        new AddInstruction(
            0x89,
            "ADC A,C",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.C,
            withCarry: true
        ),
        new AddInstruction(
            0x8A,
            "ADC A,D",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.D,
            withCarry: true
        ),
        new AddInstruction(
            0x8B,
            "ADC A,E",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.E,
            withCarry: true
        ),
        new AddInstruction(
            0x8C,
            "ADC A,H",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.H,
            withCarry: true
        ),
        new AddInstruction(
            0x8D,
            "ADC A,L",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.L,
            withCarry: true
        ),
        new AddInstruction(
            0x8E,
            "ADC A,(HL)",
            8,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.HL,
            loadSecondRegister: true,
            withCarry: true
        ),
        new AddInstruction(
            0x8F,
            "ADC A,A",
            4,
            InstructionSize.None,
            RegisterType.A,
            RegisterType.A,
            withCarry: true
        ),
        new AddInstruction(
            0xC6,
            "ADD A,d8",
            8,
            InstructionSize.D8,
            RegisterType.A,
            RegisterType.None
        ),
        new AddInstruction(
            0xCE,
            "ADC A,d8",
            8,
            InstructionSize.D8,
            RegisterType.A,
            RegisterType.None,
            withCarry: true
        ),
        new AddRegisterToHlInstruction(
            0x09,
            "ADD HL,BC",
            8,
            RegisterType.BC
        ),
        new AddRegisterToHlInstruction(
            0x19,
            "ADD HL,DE",
            8,
            RegisterType.DE
        ),
        new AddRegisterToHlInstruction(
            0x29,
            "ADD HL,HL",
            8,
            RegisterType.HL
        ),
        new AddRegisterToHlInstruction(
            0x39,
            "ADD HL,SP",
            8,
            RegisterType.SP
        ),
        new GenericInstruction(
            0xE8,
            "ADD SP,e8",
            16,
            InstructionSize.D8,
            action: (_, cpu, data) =>
            {
                var value = data.ToByte();
                var result = cpu.SP + (sbyte)value;
                
                cpu.F.ZeroFlag = false;
                cpu.F.SubtractFlag = false;
                cpu.F.HalfCarryFlag = (cpu.SP & 0x0F) + (value & 0x0F) > 0x0F;
                cpu.F.CarryFlag = (cpu.SP & 0xFF) + value > 0xFF;
                
                cpu.SP.SetValue((ushort)result);
            }
        )
    };
}


public class AddInstruction : Instruction
{
    private readonly bool _loadSecondRegister;
    private readonly bool _withCarry;

    public AddInstruction(
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
        Debug.Assert(InstructionSize == InstructionSize.None ? Register2 != RegisterType.None : Register2 == RegisterType.None);
        
        _loadSecondRegister = loadSecondRegister;
        _withCarry = withCarry;
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
        
        var carry = _withCarry && cpu.F.CarryFlag ? 1 : 0;
        var result = value1 + value2 + carry;

        cpu.WriteByteRegister(Register1, (byte)result);

        cpu.F.ZeroFlag = (byte)result == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = (value1 & 0x0F) + (value2 & 0x0F) + (carry & 0x0F) > 0x0F;
        cpu.F.CarryFlag = result > 0xFF;
    }
}

public class AddRegisterToHlInstruction : Instruction
{
    public AddRegisterToHlInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        RegisterType register1) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1, RegisterType.HL)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var value1 = cpu.ReadUshortRegister(Register1);
        var value2 = cpu.ReadUshortRegister(Register2);
        var result = value1 + value2;

        cpu.WriteUshortRegister(Register2, (ushort)result);

        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = (value1 & 0x0FFF) + (value2 & 0x0FFF) > 0x0FFF;
        cpu.F.CarryFlag = result > 0xFFFF;
    }
}