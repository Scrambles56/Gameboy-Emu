namespace GameboyEmu.Logic.Cpu.Instructions;

public static class SwapBitsInstructions
{
    public static List<Instruction> CbInstructions = new List<Instruction>
    {
        new SwapRegisterBitsInstruction(0x30, "SWAP B", 8, RegisterType.B),
        new SwapRegisterBitsInstruction(0x31, "SWAP C", 8, RegisterType.C),
        new SwapRegisterBitsInstruction(0x32, "SWAP D", 8, RegisterType.D),
        new SwapRegisterBitsInstruction(0x33, "SWAP E", 8, RegisterType.E),
        new SwapRegisterBitsInstruction(0x34, "SWAP H", 8, RegisterType.H),
        new SwapRegisterBitsInstruction(0x35, "SWAP L", 8, RegisterType.L),
        new SwapRegisterBitsInstruction(0x37, "SWAP A", 8, RegisterType.A),

        new SwapAddressBitsInstruction(0x36, "SWAP (HL)", 16, RegisterType.HL)
    };
}

public class SwapRegisterBitsInstruction : Instruction
{
    public SwapRegisterBitsInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regValue = cpu.ReadByteRegister(Register1);
        var lowerNibble = (byte)(regValue & 0x0F);
        var upperNibble = (byte)(regValue & 0xF0);
        
        regValue = (byte)((lowerNibble << 4) | (upperNibble >> 4));
        cpu.WriteByteRegister(Register1, regValue);
        
        cpu.F.ZeroFlag = regValue == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = false;
        
        return Cycles;
    }
}

public class SwapAddressBitsInstruction : Instruction
{
    public SwapAddressBitsInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        RegisterType register1) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadUshortRegister(Register1);
        var value = cpu.ReadByte(address);
        
        var lowerNibble = (byte)(value & 0x0F);
        var upperNibble = (byte)(value & 0xF0);
        
        value = (byte)((lowerNibble << 4) | (upperNibble >> 4));
        cpu.WriteByte(address, value);
        
        cpu.F.ZeroFlag = value == 0;
        cpu.F.SubtractFlag = false;
        cpu.F.HalfCarryFlag = false;
        cpu.F.CarryFlag = false;
        
        return Cycles;
    }
}