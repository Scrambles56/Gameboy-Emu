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
        .ToDictionary(i => i.Opcode);

    public static Dictionary<byte, Instruction> Instrs { get; set; } = new List<Instruction>
        {
            new NoopInstruction(),
            new CompareInstruction(
                0xFE,
                "CP nn",
                8,
                InstructionSize.D8,
                RegisterType.A
            ),
            new GenericInstruction(
                0xAF,
                "XOR A",
                4,
                register1: RegisterType.A,
                action: (instr, cpu, data) =>
                {
                    var newA = cpu.ReadByteRegister(instr.Register1) ^ cpu.A;
                    cpu.A.SetValue((byte)newA);
                    
                    cpu.F.ZeroFlag = true;
                    cpu.F.SubtractFlag = false;
                    cpu.F.HalfCarryFlag = false;
                    cpu.F.CarryFlag = false;
                }),
            new GenericInstruction(
                0xEA,
                "LD (a16),A",
                16,
                InstructionSize.D16,
                RegisterType.A,
                action: (instruction, cpu, data) =>
                {
                    var regValue = cpu.ReadByteRegister(instruction.Register1);
                    cpu.WriteByte(data.ToUshort(), regValue);
                }
            ),
            new GenericInstruction(
                0xF3,
                "DI",
                4,
                action: (instruction, cpu, data) =>
                {
                    cpu.WriteByte(0xFFFF, 0);
                }
            ),
            new GenericInstruction(
                0xE0,
                "LDH (a8),A",
                12,
                InstructionSize.D8,
                RegisterType.A,
                action: (instruction, cpu, data) =>
                {
                    var regValue = cpu.ReadByteRegister(instruction.Register1);
                    cpu.WriteByte((ushort)(0xFF00 + data.ToByte()), regValue);
                }
            ),
            new GenericInstruction(
                0xF0,
                "LDH A,(a8)",
                12,
                InstructionSize.D8,
                RegisterType.A,
                action: (instruction, cpu, data) =>
                {
                    var regValue = cpu.ReadByte((ushort)(0xFF00 + data.ToByte()));
                    cpu.WriteByteRegister(instruction.Register1, regValue);
                }
            ),
            new GenericInstruction(
                0xCD,
                "CALL a16",
                24,
                InstructionSize.D16,
                action: (instruction, cpu, data) =>
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
                action: (instruction, cpu, data) =>
                {
                    cpu.cbMode = true;
                }
            )
        }
        .Concat(JumpInstructions.Instructions)
        .Concat(LoadInstructions.Instructions)
        .Concat(DecrementInstructions.Instructions)
        .Concat(IncrementInstructions.Instructions)
        .ToDictionary(i => i.Opcode);


    public static void PrintInstructionTable(GameboyEmu.Cpu.Cpu cpu)
    {
        var cbMode = cpu.cbMode;
        
        Console.WriteLine("  0 1 2 3 4 5 6 7 8 9 A B C D E F");
        for (var i = 0; i < 16; i++)
        {
            Console.Write("{0:X}", i);
            
            for (var j = 0; j < 16; j++)
            {
                var opcode = (byte)(i * 16 + j);
                var instr = GetInstruction(opcode, cpu);
                cpu.cbMode = cbMode;
                Console.Write(instr == null ? "  " : " .");
            }
            
            Console.WriteLine();
        }
    }
}