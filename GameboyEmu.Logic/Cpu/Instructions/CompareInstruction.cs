namespace GameboyEmu.Logic.Cpu.Instructions;

public class CompareInstruction : Instruction
{
    public CompareInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None
        ) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var registerValue = cpu.ReadByteRegister(Register1);
        var compValue = data.ToByte();
        
        cpu.F.ZeroFlag = registerValue == compValue;
        cpu.F.SubtractFlag = true;
        cpu.F.HalfCarryFlag = (registerValue & 0x0F) < (compValue & 0x0F);
        cpu.F.CarryFlag = registerValue < compValue;
    }
}