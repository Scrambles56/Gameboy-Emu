namespace GameboyEmu.Logic.Cpu.Instructions;

public class NoopInstruction : Instruction
{
    public NoopInstruction() 
        : base(
            0x00,
            "NOP",
            4
        )
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
    }
}