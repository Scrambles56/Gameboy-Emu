namespace GameboyEmu.Logic.Cpu.Instructions;

public static class StopInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new StopInstruction(),
        new GenericInstruction(
            0x76,
            "HALT",
            4,
            InstructionSize.None,
            RegisterType.None,
            RegisterType.None,
            action: (instruction, cpu, _) =>
            {
                cpu.Halted = true;

                return instruction.Cycles;
            }
        )
    };
}

public class StopInstruction() : Instruction(
    0x10,
    "STOP",
    4
)
{
    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        // var joypadButtons = cpu.ReadByte(0xFF00);
        // if (joypadButtons == 0x3F)
        // {
        //     if (cpu.GetRequestedInterrupt() == null)
        //     {
        //         var stopByte = cpu.ReadNextByte();
        //     }
        // }

        return Opcode;
    }
}