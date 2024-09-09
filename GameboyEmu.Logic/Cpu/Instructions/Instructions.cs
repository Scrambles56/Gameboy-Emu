using GameboyEmu.Logic.Cpu.Extensions;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class Instructions
{
    public static Instruction? GetInstruction(byte opcode, GameboyEmu.Cpu.Cpu cpu)
    {
        if (!cpu.cbMode)
        {
            return Instrs.TryGetValue(opcode, out var instr) ? instr : null;
        }
        else
        {
            var instruction = cbInstructions.TryGetValue(opcode, out var instr) ? instr : null;
            cpu.cbMode = false;
            return instruction;
        }
    }

    private static Dictionary<byte, Instruction> cbInstructions { get; set; } = new List<Instruction>
        {
        }
        .Concat(ResetInstructions.Instructions)
        .Concat(ShiftRightLogicallyInstructions.Instructions)
        .ToDictionary(i => i.Opcode);

    public static Dictionary<byte, Instruction> Instrs { get; set; } = new List<Instruction>
        {
            new NoopInstruction(),
            new GenericInstruction(
                0xF3,
                "DI",
                4,
                action: (_, cpu, _) => { cpu.SetInterruptMasterFlag(false); }
            ),
            new GenericInstruction(
                0xFB,
                "EI",
                4,
                action: (_, cpu, _) => { cpu.SetInterruptMasterFlag(true); }
            ),
            new GenericInstruction(
                0xCD,
                "CALL a16",
                24,
                InstructionSize.D16,
                action: (_, cpu, data) =>
                {
                    var value = data.ToUshort();
                    cpu.SP--;
                    cpu.WriteByte(cpu.SP--, (byte)(cpu.PC >> 8));
                    cpu.WriteByte(cpu.SP, (byte)(cpu.PC & 0xFF));
                    cpu.PC.SetValue(value);
                }
            ),
            new GenericInstruction(
                0xCB,
                "PREFIX CB",
                4,
                action: (_, cpu, _) => { cpu.cbMode = true; }
            ),
            new RotateInstruction(
                0x07,
                "RLCA",
                4,
                RegisterType.A
            ),
            new RotateInstruction(
                0x17,
                "RLA",
                4,
                RegisterType.A,
                Direction.Left,
                true
            ),
            new RotateInstruction(
                0x0F,
                "RRCA",
                4,
                RegisterType.A,
                Direction.Right
            ),
            new RotateInstruction(
                0x1F,
                "RRA",
                4,
                RegisterType.A,
                Direction.Right,
                true
            ),
            new GenericInstruction(0xC9,
                "RET",
                16,
                action: (_, cpu, _) =>
                {
                    var low = cpu.ReadByte(cpu.SP++);
                    var high = cpu.ReadByte(cpu.SP++);
                    cpu.PC.SetValue((ushort)((high << 8) | low));
                }   
            )
        }
        .Concat(AddInstructions.Instructions)
        .Concat(SubtractInstructions.Instructions)
        .Concat(AndInstructions.Instructions)
        .Concat(OrInstructions.Instructions)
        .Concat(XorInstructions.Instructions)
        .Concat(CompareInstructions.Instructions)
        .Concat(JumpInstructions.Instructions)
        .Concat(LoadInstructions.Instructions)
        .Concat(DecrementInstructions.Instructions)
        .Concat(IncrementInstructions.Instructions)
        .ToDictionary(i => i.Opcode);

    public static void PrintInstructionTable(GameboyEmu.Cpu.Cpu cpu)
    {
        byte[] excludedOpCodes =
        {
            0xD3, 0xDB, 0xDD, 0xE3, 0xE4, 0xEB, 0xEC, 0xED, 0xF4, 0xFC, 0xFD
        };
        var cbMode = cpu.cbMode;

        Console.WriteLine("   x0 x1 x2 x3 x4 x5 x6 x7 x8 x9 xA xB xC xD xE xF");
        for (var i = 0; i < 16; i++)
        {
            Console.Write("{0:X}x", i);

            for (var j = 0; j < 16; j++)
            {
                var opcode = (byte)(i * 16 + j);
                if (excludedOpCodes.Contains(opcode))
                {
                    Console.Write("  -");
                    continue;
                }
                
                var instr = GetInstruction(opcode, cpu);
                cpu.cbMode = cbMode;
                Console.Write(instr == null ? "  x" : "  .");
            }

            Console.WriteLine();
        }
        
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("---------------------------------------");
        
        Console.WriteLine("CB Instructions");

        cbMode = true;
        cpu.cbMode = cbMode;
        Console.WriteLine("   x0 x1 x2 x3 x4 x5 x6 x7 x8 x9 xA xB xC xD xE xF");
        for (var i = 0; i < 16; i++)
        {
            Console.Write("{0:X}x", i);

            for (var j = 0; j < 16; j++)
            {
                var opcode = (byte)(i * 16 + j);
                var instr = GetInstruction(opcode, cpu);
                cpu.cbMode = cbMode;
                Console.Write(instr == null ? "  x" : "  .");
            }

            Console.WriteLine();
        }
    }
}