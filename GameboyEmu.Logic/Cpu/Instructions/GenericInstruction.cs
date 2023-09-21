namespace GameboyEmu.Logic.Cpu.Instructions;

public class GenericInstruction : Instruction
{
    private readonly Action<Instruction, GameboyEmu.Cpu.Cpu, FetchedData>? _action;

    public GenericInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None,
        Action<Instruction, GameboyEmu.Cpu.Cpu, FetchedData>? action = null
    ) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        _action = action;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data) => _action?.Invoke(this, cpu, data);
}