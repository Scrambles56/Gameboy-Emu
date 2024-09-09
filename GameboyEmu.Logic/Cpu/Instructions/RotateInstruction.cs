namespace GameboyEmu.Logic.Cpu.Instructions;

public class RotateInstruction : Instruction
{
    private readonly Direction _direction;
    private readonly bool _useCarry;

    public RotateInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1 = RegisterType.None,
        Direction direction = Direction.Left,
        bool useCarry = false
    ) : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
        _direction = direction;
        _useCarry = useCarry;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var value = cpu.ReadByteRegister(Register1);
        var overflowBitNumber = _direction == Direction.Left ? 7 : 0;
        var overflowBit = value >> overflowBitNumber & 1;
        
        var carryBit = _useCarry ? (cpu.F.CarryFlag ? 1 : 0) : overflowBit;
        
        
        var newValue = _direction switch
        {
            Direction.Left => (byte)(value << 1 | carryBit),
            Direction.Right => (byte)(value >> 1 | carryBit << 7),
            _ => throw new ArgumentOutOfRangeException()
        };

        cpu.WriteByteRegister(Register1, newValue);
        cpu.F.ZeroFlag = false;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = overflowBit == 1;
    }
}

public enum Direction
{
    Left,
    Right
} 