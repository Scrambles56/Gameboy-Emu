namespace GameboyEmu.Logic.Cpu.Instructions;

public static class Instructions
{
    public static Instruction? GetInstruction(byte opcode)
    {
        return Instrs.TryGetValue(opcode, out var instr) ? instr : null;
    }

    /*
     * 0x20
     * 0x30
     * 0xc2
     * 0xd2
     * 0xc3
     * 0x18
     * 0x28
     * 0x38
     * 0xE9
     * 0xCA
     * 0xDA
     */

    private static Dictionary<byte, Instruction> Instrs { get; set; } = new List<Instruction>
        {
            new NoopInstruction(),
            new CompareInstruction(
                0xFE,
                "CP nn",
                8,
                Instr.Cp,
                InstructionSize.D8,
                RegisterType.A
            )
        }
        .Concat(JumpInstructions.Instructions)
        .ToDictionary(i => i.Opcode);
}