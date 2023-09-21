namespace GameboyEmu.Logic.Cpu.Instructions;

public class LoadAddressToRegister : Instruction
{
    public LoadAddressToRegister(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadUshortRegister(Register2);
        var value = cpu.ReadByte(address);
        cpu.WriteByteRegister(Register1, value);
    }
}