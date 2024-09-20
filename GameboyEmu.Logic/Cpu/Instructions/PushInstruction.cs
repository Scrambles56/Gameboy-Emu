namespace GameboyEmu.Logic.Cpu.Instructions;

using Extensions;
using GameboyEmu.Cpu;

public static class PushInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new PushInstruction(
            0xC5,
            "PUSH BC",
            16,
            RegisterType.BC
        ),
        new PushInstruction(
            0xD5,
            "PUSH DE",
            16,
            RegisterType.DE
        ),
        new PushInstruction(
            0xE5,
            "PUSH HL",
            16,
            RegisterType.HL
        ),
        new PushInstruction(
            0xF5,
            "PUSH AF",
            16,
            RegisterType.AF
        )
    };
}

public class PushInstruction : Instruction 
{
    public PushInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        RegisterType register1 = RegisterType.None) 
    : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override void Execute(Cpu cpu, FetchedData data)
    {
        var regVal = cpu.ReadUshortRegister(Register1).ToBytes();
        cpu.SP--;
        cpu.WriteByte(cpu.SP, regVal.high);
        cpu.SP--;
        cpu.WriteByte(cpu.SP, regVal.low);
    }
}