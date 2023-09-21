namespace GameboyEmu.Logic.Cpu.Instructions;

public class LoadRegisterIntoAddress : Instruction
{
    public LoadRegisterIntoAddress(
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
        var value = cpu.ReadByteRegister(Register2);
        var address = cpu.ReadUshortRegister(Register1);
        cpu.WriteByte(address, value);
    }
}