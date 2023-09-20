namespace GameboyEmu.Logic.Cpu.Instructions;

public static class Instructions
{
    public static Instruction? GetInstruction(byte opcode)
    {
        return Instrs.TryGetValue(opcode, out var instr) ? instr : null;
    }
    
    private static Dictionary<byte, Instruction> Instrs { get; set; } = new List<Instruction>
    {
        new(
            0x00,
            "NOP",
            4,
            Instr.Noop
        ),
        new (
            0x01,
            "LD BC, nn",
            12,
            Instr.Ld,
            AddressingMode.R_D16,
            RegisterType.BC,
            action: (cpu, data) =>
            {
                cpu.B.SetValue((byte)(data >> 8));
                cpu.C.SetValue((byte)(data & 0xFF));
            }
        ),
        new(
            0xC3,
            "JP nn",
            16,
            Instr.Jump,
            AddressingMode.D16,
            action: (cpu, data) =>
            {
                cpu.PC = data;
            }
        ),
        new(
            0xAF,
            "XOR",
            4,
            Instr.Xor,
            AddressingMode.Register,
            RegisterType.A,
            action: (cpu, data) =>
            {
                var value = (byte)(cpu.A ^ (byte)(data & 0xFF));
                cpu.A.SetValue(value);
            }
        ),
        new(
            0x28,
            "JR Z, n",
            16,
            Instr.Jump,
            AddressingMode.D8,
            condition: Condition.Z,
            action: (cpu, data) =>
            {
                if (cpu.F.ZeroFlag)
                {
                    cpu.PC += (ushort)(sbyte)data;
                }
            }
        )
        
    }.ToDictionary(i => i.Opcode);
}