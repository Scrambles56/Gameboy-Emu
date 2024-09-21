namespace GameboyEmu.Logic.Cpu.Instructions;

public static class RotateInstructions
{
    public static List<Instruction> Instructions => new()
    {
        new RotateInstruction(
            0x07,
            "RLCA",
            4,
            RegisterType.A
        ),
        new RotateInstruction(
            0x17,
            "RLA",
            4,
            RegisterType.A,
            Direction.Left,
            true
        ),
        new RotateInstruction(
            0x0F,
            "RRCA",
            4,
            RegisterType.A,
            Direction.Right
        ),
        new RotateInstruction(
            0x1F,
            "RRA",
            4,
            RegisterType.A,
            Direction.Right,
            true
        ),
    };

    public static List<Instruction> CbInstructions => new()
    {
        new RotateRegisterRightThroughCarry(
            0x18,
            "RR B",
            8,
            RegisterType.B
        ),
        new RotateRegisterRightThroughCarry(
            0x19,
            "RR C",
            8,
            RegisterType.C
        ),
        new RotateRegisterRightThroughCarry(
            0x1A,
            "RR D",
            8,
            RegisterType.D
        ),
        new RotateRegisterRightThroughCarry(
            0x1B,
            "RR E",
            8,
            RegisterType.E
        ),
        new RotateRegisterRightThroughCarry(
            0x1C,
            "RR H",
            8,
            RegisterType.H
        ),
        new RotateRegisterRightThroughCarry(
            0x1D,
            "RR L",
            8,
            RegisterType.L
        ),
        new RotateRegisterRightThroughCarry(
            0x1F,
            "RR A",
            8,
            RegisterType.A
        ),
        new RotateRegisterLeftThroughCarry(
            0x10,
            "RL B",
            8,
            RegisterType.B
        ),
        new RotateRegisterLeftThroughCarry(
            0x11,
            "RL C",
            8,
            RegisterType.C
        ),
        new RotateRegisterLeftThroughCarry(
            0x12,
            "RL D",
            8,
            RegisterType.D
        ),
        new RotateRegisterLeftThroughCarry(
            0x13,
            "RL E",
            8,
            RegisterType.E
        ),
        new RotateRegisterLeftThroughCarry(
            0x14,
            "RL H",
            8,
            RegisterType.H
        ),
        new RotateRegisterLeftThroughCarry(
            0x15,
            "RL L",
            8,
            RegisterType.L
        ),
        new RotateRegisterLeftThroughCarry(
            0x17,
            "RL A",
            8,
            RegisterType.A
        )
    };
}

public class RotateInstruction : Instruction
{
    private readonly Direction _direction;
    private readonly bool _useCarry;
    private readonly bool _allowWrap;

    public RotateInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1 = RegisterType.None,
        Direction direction = Direction.Left,
        bool useCarry = false,
        bool allowWrap = true
    ) : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
        _direction = direction;
        _useCarry = useCarry;
        _allowWrap = allowWrap;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var value = cpu.ReadByteRegister(Register1);
        var overflowBitNumber = _direction == Direction.Left ? 7 : 0;
        var overflowBit = value >> overflowBitNumber & 1;
        
        var carryBit = _useCarry ? (cpu.F.CarryFlag ? 1 : 0) : overflowBit;
        
        
        var newValue = _direction switch
        {
            Direction.Left => RotateLeft(value, carryBit),
            Direction.Right => RotateRight(value, carryBit),
            _ => throw new ArgumentOutOfRangeException()
        };

        cpu.WriteByteRegister(Register1, newValue);
        cpu.F.ZeroFlag = false;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = overflowBit == 1;
    }
    
    private byte RotateLeft(byte value, int carry)
    {
        return (byte)(value << 1 | (_allowWrap ? carry : 0));
    }
    
    private byte RotateRight(byte value, int carry)
    {
        return (byte)(value >> 1 | (_allowWrap ? carry << 7 : 0));
    }
}

public class RotateRegisterRightThroughCarry : Instruction
{
    public RotateRegisterRightThroughCarry(
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
        var carry = cpu.F.CarryFlag ? 1 : 0;
        var newValue = (byte)((value >> 1) | (carry << 7));
        
        cpu.WriteByteRegister(Register1, newValue);
        
        cpu.F.CarryFlag = (value & 0x01) != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}

public class RotateRegisterLeftThroughCarry : Instruction
{
    public RotateRegisterLeftThroughCarry(
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
        var carry = cpu.F.CarryFlag ? 1 : 0;
        
        var newValue = (byte)((value << 1) | carry);
        
        cpu.WriteByteRegister(Register1, newValue);
        
        cpu.F.CarryFlag = (value & 0x80) != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}