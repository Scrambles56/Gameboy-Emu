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
        new RotateRegisterRightThroughCarryInstruction(
            0x18,
            "RR B",
            8,
            RegisterType.B
        ),
        new RotateRegisterRightThroughCarryInstruction(
            0x19,
            "RR C",
            8,
            RegisterType.C
        ),
        new RotateRegisterRightThroughCarryInstruction(
            0x1A,
            "RR D",
            8,
            RegisterType.D
        ),
        new RotateRegisterRightThroughCarryInstruction(
            0x1B,
            "RR E",
            8,
            RegisterType.E
        ),
        new RotateRegisterRightThroughCarryInstruction(
            0x1C,
            "RR H",
            8,
            RegisterType.H
        ),
        new RotateRegisterRightThroughCarryInstruction(
            0x1D,
            "RR L",
            8,
            RegisterType.L
        ),
        new RotateRegisterRightThroughCarryInstruction(
            0x1F,
            "RR A",
            8,
            RegisterType.A
        ),
        new RotateRegisterLeftThroughCarryInstruction(
            0x10,
            "RL B",
            8,
            RegisterType.B
        ),
        new RotateRegisterLeftThroughCarryInstruction(
            0x11,
            "RL C",
            8,
            RegisterType.C
        ),
        new RotateRegisterLeftThroughCarryInstruction(
            0x12,
            "RL D",
            8,
            RegisterType.D
        ),
        new RotateRegisterLeftThroughCarryInstruction(
            0x13,
            "RL E",
            8,
            RegisterType.E
        ),
        new RotateRegisterLeftThroughCarryInstruction(
            0x14,
            "RL H",
            8,
            RegisterType.H
        ),
        new RotateRegisterLeftThroughCarryInstruction(
            0x15,
            "RL L",
            8,
            RegisterType.L
        ),
        new RotateRegisterLeftThroughCarryInstruction(
            0x17,
            "RL A",
            8,
            RegisterType.A
        ),
        new RotateAddressRightThroughCarryInstruction(
            0x1E,
            "RR (HL)",
            16,
            RegisterType.HL
        ),
        new RotateAddressLeftThroughCarryInstruction(
            0x16,
            "RL (HL)",
            16,
            RegisterType.HL
        ),
        
        new RotateRegisterLeftInstruction(
            0x00,
            "RLC B",
            8,
            RegisterType.B
        ),
        new RotateRegisterLeftInstruction(
            0x01,
            "RLC C",
            8,
            RegisterType.C
        ),
        new RotateRegisterLeftInstruction(
            0x02,
            "RLC D",
            8,
            RegisterType.D
        ),
        new RotateRegisterLeftInstruction(
            0x03,
            "RLC E",
            8,
            RegisterType.E
        ),
        new RotateRegisterLeftInstruction(
            0x04,
            "RLC H",
            8,
            RegisterType.H
        ),
        new RotateRegisterLeftInstruction(
            0x05,
            "RLC L",
            8,
            RegisterType.L
        ),
        new RotateRegisterLeftInstruction(
            0x07,
            "RLC A",
            8,
            RegisterType.A
        ),
        new RotateAddressLeftInstruction(
            0x06,
            "RLC (HL)",
            16,
            RegisterType.HL
        ),
        
        new RotateRegisterRightInstruction(
            0x08,
            "RRC B",
            8,
            RegisterType.B
        ),
        new RotateRegisterRightInstruction(
            0x09,
            "RRC C",
            8,
            RegisterType.C
        ),
        new RotateRegisterRightInstruction(
            0x0A,
            "RRC D",
            8,
            RegisterType.D
        ),
        new RotateRegisterRightInstruction(
            0x0B,
            "RRC E",
            8,
            RegisterType.E
        ),
        new RotateRegisterRightInstruction(
            0x0C,
            "RRC H",
            8,
            RegisterType.H
        ),
        new RotateRegisterRightInstruction(
            0x0D,
            "RRC L",
            8,
            RegisterType.L
        ),
        new RotateRegisterRightInstruction(
            0x0F,
            "RRC A",
            8,
            RegisterType.A
        ),
        new RotateAddressRightInstruction(
            0x0E,
            "RRC (HL)",
            16,
            RegisterType.HL
        ),
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

public class RotateRegisterRightThroughCarryInstruction : Instruction
{
    public RotateRegisterRightThroughCarryInstruction(
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

public class RotateAddressRightThroughCarryInstruction : Instruction
{
    public RotateAddressRightThroughCarryInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        RegisterType register1 = RegisterType.None) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadByteRegister(Register1);
        var value = cpu.ReadByte(address);
        var carry = cpu.F.CarryFlag ? 1 : 0;
        var newValue = (byte)((value >> 1) | (carry << 7));
        
        cpu.WriteByte(address, newValue);
        
        cpu.F.CarryFlag = (value & 0x01) != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}

public class RotateRegisterLeftThroughCarryInstruction : Instruction
{
    public RotateRegisterLeftThroughCarryInstruction(
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

public class RotateAddressLeftThroughCarryInstruction : Instruction
{
    public RotateAddressLeftThroughCarryInstruction(
        byte opcode, 
        string mnemonic,
        int cycles, 
        RegisterType register1 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadByteRegister(Register1);
        var value = cpu.ReadByte(address);
        var carry = cpu.F.CarryFlag ? 1 : 0;
        
        var newValue = (byte)((value << 1) | carry);
        
        cpu.WriteByte(address, newValue);
        
        cpu.F.CarryFlag = (value & 0x80) != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}

public class RotateRegisterLeftInstruction : Instruction
{
    public RotateRegisterLeftInstruction(
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
        var topBit = (value & 0x80) >> 7;
        var newValue = (byte)((value << 1) | topBit);
        
        cpu.WriteByteRegister(Register1, newValue);
        
        cpu.F.CarryFlag = topBit != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}

public class RotateAddressLeftInstruction : Instruction
{
    public RotateAddressLeftInstruction(
        byte opcode, 
        string mnemonic,
        int cycles, 
        RegisterType register1 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadByteRegister(Register1);
        var value = cpu.ReadByte(address);
        
        var topBit = (value & 0x80) >> 7;
        var newValue = (byte)((value << 1) | topBit);
        
        cpu.WriteByte(address, newValue);
        
        cpu.F.CarryFlag = topBit != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}

public class RotateRegisterRightInstruction : Instruction
{
    public RotateRegisterRightInstruction(
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
        var bottomBit = value & 0x01;
        var newValue = (byte)((value >> 1) | (bottomBit << 7));
        
        cpu.WriteByteRegister(Register1, newValue);
        
        cpu.F.CarryFlag = bottomBit != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}

public class RotateAddressRightInstruction : Instruction
{
    public RotateAddressRightInstruction(
        byte opcode, 
        string mnemonic,
        int cycles, 
        RegisterType register1 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadByteRegister(Register1);
        var value = cpu.ReadByte(address);
        
        var bottomBit = value & 0x01;
        var newValue = (byte)((value >> 1) | (bottomBit << 7));
        
        cpu.WriteByte(address, newValue);
        
        cpu.F.CarryFlag = bottomBit != 0;
        cpu.F.ZeroFlag = newValue == 0;
    }
}