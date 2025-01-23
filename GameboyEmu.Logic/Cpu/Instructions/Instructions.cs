using GameboyEmu.Logic.Cpu.Extensions;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class Instructions
{
    public static Instruction? GetInstruction(byte opcode, GameboyEmu.Cpu.Cpu cpu)
    {
        if (!cpu.cbMode)
        {
            return Instrs.GetValueOrDefault(opcode);
        }

        var instruction = CbInstructions.GetValueOrDefault(opcode);
        cpu.cbMode = false;
        return instruction;
    }

    private static Dictionary<byte, Instruction> CbInstructions { get; set; } = new List<Instruction>
        {
        }
        .Concat(CheckBitInstructions.CbInstructions)
        .Concat(ResetInstructions.CbInstructions)
        .Concat(SetBitInstructions.CbInstructions)
        .Concat(ShiftRightLogicallyInstructions.CbInstructions)
        .Concat(RotateInstructions.CbInstructions)
        .Concat(SwapBitsInstructions.CbInstructions)
        .Concat(ShiftInstructions.CbInstructions)
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
            // TODO: The effect of ei is delayed by one instruction. This means that ei followed immediately by di does not allow any interrupts between them
            new GenericInstruction(
                0xFB,
                "EI",
                4,
                action: (_, cpu, _) => { cpu.SetInterruptMasterFlag(true); }
            ),
            new GenericInstruction(
                0xCB,
                "PREFIX CB",
                4,
                action: (_, cpu, _) =>
                {
                    cpu.cbMode = true;
                }
            ),
            new GenericInstruction(
                0x27,
                "DAA",
                4,
                action: (_, cpu, _) =>
                {
                    var adjustment = 0;
                    int result;
                    if (cpu.F.SubtractFlag)
                    {
                        adjustment += cpu.F.HalfCarryFlag ? 0x6 : 0;
                        adjustment += cpu.F.CarryFlag ? 0x60 : 0;
                        
                        result = cpu.A - adjustment;
                        var carry = result < 0;
                        
                        cpu.A.SetValue((byte)result);
                        cpu.F.CarryFlag = carry;
                    }
                    else
                    {
                        adjustment += cpu.F.HalfCarryFlag || (cpu.A & 0xF) > 0x9 ? 0x6 : 0;
                        adjustment += cpu.F.CarryFlag || cpu.A > 0x9F ? 0x60 : 0;
                        
                        result = cpu.A + adjustment;
                        var carry = result > 0xFF;
                        
                        cpu.A.SetValue((byte)result);
                        cpu.F.CarryFlag = carry;
                    }
                    
                    cpu.F.ZeroFlag = result == 0;
                    cpu.F.HalfCarryFlag = false;
                }
            )
        }
        .Concat(ReturnInstructions.Instructions)
        .Concat(RotateInstructions.Instructions)
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
        .Concat(PushInstructions.Instructions)
        .Concat(PopInstructions.Instructions)
        .Concat(CallInstructions.Instructions)
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