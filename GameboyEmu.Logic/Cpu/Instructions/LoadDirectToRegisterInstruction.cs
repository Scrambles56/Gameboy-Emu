using System.Diagnostics;

namespace GameboyEmu.Logic.Cpu.Instructions;

public class LoadDirectToRegisterInstruction : Instruction
{
    public LoadDirectToRegisterInstruction(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        InstructionSize instructionSize = InstructionSize.None, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, instructionSize, register1, register2)
    {
        Debug.Assert(InstructionSize != InstructionSize.None);
    }

    public override int Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        if (InstructionSize == InstructionSize.D8)
        {
            var value = data.ToByte();
            cpu.WriteByteRegister(Register1, value);
        }
        else if (InstructionSize == InstructionSize.D16)
        {
            var value = data.ToUshort();
            cpu.WriteUshortRegister(Register1, value);
        }
        
        return Cycles;
    }
}