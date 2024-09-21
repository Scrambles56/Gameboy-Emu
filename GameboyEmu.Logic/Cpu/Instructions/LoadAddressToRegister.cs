namespace GameboyEmu.Logic.Cpu.Instructions;

public class LoadAddressToRegister : Instruction
{
    private readonly IncDecOperation _operation;

    public LoadAddressToRegister(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None,
        IncDecOperation operation = IncDecOperation.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        _operation = operation;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var address = cpu.ReadUshortRegister(Register2);
        var value = cpu.ReadByte(address);
        cpu.WriteByteRegister(Register1, value);
        
        if (_operation == IncDecOperation.Increment)
        {
            cpu.WriteUshortRegister(Register2, (byte)(address + 1));
        }
        else if (_operation == IncDecOperation.Decrement)
        {
            cpu.WriteUshortRegister(Register2, (byte)(address - 1));
        }
    }
}

public enum IncDecOperation
{
    Increment,
    Decrement,
    None
}