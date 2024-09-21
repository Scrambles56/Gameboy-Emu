namespace GameboyEmu.Logic.Cpu.Instructions;

public static class ReturnInstructions
{
    public static List<Instruction> Instructions => new()
    {
        new ReturnInstruction(
            0xC9,
            "RET",
            16
        ),
        new ReturnInstruction(
            0xC0,
            "RET NZ",
            20,
            Condition.NZ
        ),
        new ReturnInstruction(
            0xC8,
            "RET Z",
            20,
            Condition.Z
        ),
        new ReturnInstruction(
            0xD0,
            "RET NC",
            20,
            Condition.NC
        ),
        new ReturnInstruction(
            0xD8,
            "RET C",
            20,
            Condition.C
        )
    };
}

public class ReturnInstruction : Instruction 
{
    private readonly Condition _condition;

    public ReturnInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles,
        Condition condition = Condition.None
    ) 
    : base(opcode, mnemonic, cycles)
    {
        _condition = condition;
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        if (cpu.CheckCondition(_condition))
        {
            var low = cpu.ReadByte(cpu.SP);
            cpu.SP++;
            var high = cpu.ReadByte(cpu.SP);
            cpu.SP++;
            cpu.PC.SetValue((ushort)((high << 8) | low));
        }
    }
}