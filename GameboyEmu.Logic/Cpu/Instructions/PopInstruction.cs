namespace GameboyEmu.Logic.Cpu.Instructions;

using GameboyEmu.Cpu;

public static class PopInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new PopInstruction( 0xC1, "POP BC", 12, RegisterType.BC),
        new PopInstruction( 0xD1, "POP DE", 12, RegisterType.DE),
        new PopInstruction( 0xE1, "POP HL", 12, RegisterType.HL),
        new PopInstruction( 0xF1, "POP AF", 12, RegisterType.AF)
    };
}

public class PopInstruction : Instruction
{
    public PopInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        RegisterType register1 = RegisterType.None) : base(opcode, mnemonic, cycles, InstructionSize.None, register1)
    {
    }

    public override int Execute(Cpu cpu, FetchedData data)
    {
        var low = cpu.ReadByte(cpu.SP);
        cpu.SP++;
        var high = cpu.ReadByte(cpu.SP);
        cpu.SP++;
        
        var value = (ushort)((high << 8) | low);

        if (Register1 == RegisterType.AF)
        {
            value = (ushort)(value & 0xFFF0);
        }
        
        cpu.WriteUshortRegister(Register1, value);
        
        return Cycles;
    }
}