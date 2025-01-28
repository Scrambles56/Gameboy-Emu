using GameboyEmu.Logic.Cpu.Extensions;

namespace GameboyEmu.Logic.Cpu.Instructions;

using GameboyEmu.Cpu;

public static class CallInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new CallInstruction(0xC4, "CALL NZ, a16", 24, Condition.NZ),
        new CallInstruction(0xCC, "CALL Z, a16", 24, Condition.Z),
        new CallInstruction(0xD4, "CALL NC, a16", 24, Condition.NC),
        new CallInstruction(0xDC, "CALL C, a16", 24, Condition.C),
        new CallInstruction(0xCD, "CALL a16", 24, Condition.None),
        
        new RestartInstruction(0xC7,"RST $00",16,0x00),
        new RestartInstruction(0xCF,"RST $08",16,0x08),
        new RestartInstruction(0xD7,"RST $10",16,0x10),
        new RestartInstruction(0xDF,"RST $18",16,0x18),
        new RestartInstruction(0xE7,"RST $20",16,0x20),
        new RestartInstruction(0xEF,"RST $28",16,0x28),
        new RestartInstruction(0xF7,"RST $30",16,0x30),
        new RestartInstruction(0xFF,"RST $38",16,0x38)
    };
}

public class RestartInstruction : Instruction
{
    private readonly ushort _address;

    public RestartInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        ushort address) 
    : base(opcode, mnemonic, cycles)
    {
        _address = address;
    }

    public override int Execute(Cpu cpu, FetchedData data)
    {
        var value = cpu.PC.GetValue().ToBytes();
        cpu.SP--;
        cpu.WriteByte(cpu.SP, value.high);
        cpu.SP--;
        cpu.WriteByte(cpu.SP, value.low);
        cpu.PC = _address;
        
        return Cycles;
    }
}

public class CallInstruction : Instruction
{
    private readonly Condition _condition;

    public CallInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        Condition condition) 
    : base(opcode, mnemonic, cycles, InstructionSize.D16)
    {
        _condition = condition;
    }

    public override int Execute(Cpu cpu, FetchedData data)
    {
        if (cpu.CheckCondition(_condition))
        {
            var value = cpu.PC.GetValue().ToBytes();
            cpu.SP--;
            cpu.WriteByte(cpu.SP, value.high);
            cpu.SP--;
            cpu.WriteByte(cpu.SP, value.low);
            cpu.PC = data.ToUshort();
            
            return Cycles;
        }

        return 12;
    }
}