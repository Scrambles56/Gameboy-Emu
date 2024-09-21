using GameboyEmu.Logic.Cpu.Extensions;

namespace GameboyEmu.Logic.Cpu.Instructions;

using GameboyEmu.Cpu;

public static class CallInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new CallInstruction(
            0xC4,
            "CALL NZ, a16",
            24,
            Condition.NZ
        ),
        new CallInstruction(
            0xCC,
            "CALL Z, a16",
            24,
            Condition.Z
        ),
        new CallInstruction(
            0xD4,
            "CALL NC, a16",
            24,
            Condition.NC
        ),
        new CallInstruction(
            0xDC,
            "CALL C, a16",
            24,
            Condition.C
        ),
        new CallInstruction(
            0xCD,
            "CALL a16",
            24,
           Condition.None 
        )
    };
}

public class CallInstruction : Instruction
{
    private readonly Condition _condition;
    private readonly Func<RegisterFlags, bool>? _condFunc;

    public CallInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        Condition condition) 
    : base(opcode, mnemonic, cycles, InstructionSize.D16)
    {
        _condition = condition;
    }

    public override void Execute(Cpu cpu, FetchedData data)
    {
        if (cpu.CheckCondition(_condition))
        {
            var value = cpu.PC.GetValue().ToBytes();
            cpu.SP--;
            cpu.WriteByte(cpu.SP, value.high);
            cpu.SP--;
            cpu.WriteByte(cpu.SP, value.low);
            cpu.PC = data.ToUshort();
        }
    }
}