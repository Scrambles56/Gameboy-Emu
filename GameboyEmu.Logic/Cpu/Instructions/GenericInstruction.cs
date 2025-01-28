namespace GameboyEmu.Logic.Cpu.Instructions;

public class GenericInstruction : Instruction
{
    private readonly Func<Instruction, GameboyEmu.Cpu.Cpu, FetchedData, int> _action;

    public GenericInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize, 
        RegisterType register1, 
        RegisterType register2,
        Func<Instruction, GameboyEmu.Cpu.Cpu, FetchedData, int> action
    ) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        _action = action;
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data) => _action.Invoke(this, cpu, data);
}