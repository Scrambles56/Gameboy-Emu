using GameboyEmu.Logic.Cpu.Extensions;

namespace GameboyEmu.Logic.Cpu.Instructions;

public static class LoadInstructions
{
    public static List<Instruction> Instructions = new()
    {
        new LoadFromRegisterToRegister(0x40, "LD B,B", 4, RegisterType.B, RegisterType.B),
        new LoadFromRegisterToRegister(0x41, "LD B,C", 4, RegisterType.B, RegisterType.C),
        new LoadFromRegisterToRegister(0x42, "LD B,D", 4, RegisterType.B, RegisterType.D),
        new LoadFromRegisterToRegister(0x43, "LD B,E", 4, RegisterType.B, RegisterType.E),
        new LoadFromRegisterToRegister(0x44, "LD B,H", 4, RegisterType.B, RegisterType.H),
        new LoadFromRegisterToRegister(0x45, "LD B,L", 4, RegisterType.B, RegisterType.L),
        new LoadFromRegisterToRegister(0x47, "LD B,A", 4, RegisterType.B, RegisterType.A),
        new LoadFromRegisterToRegister(0x48, "LD C,B", 4, RegisterType.C, RegisterType.B),
        new LoadFromRegisterToRegister(0x49, "LD C,C", 4, RegisterType.C, RegisterType.C),
        new LoadFromRegisterToRegister(0x4A, "LD C,D", 4, RegisterType.C, RegisterType.D),
        new LoadFromRegisterToRegister(0x4B, "LD C,E", 4, RegisterType.C, RegisterType.E),
        new LoadFromRegisterToRegister(0x4C, "LD C,H", 4, RegisterType.C, RegisterType.H),
        new LoadFromRegisterToRegister(0x4D, "LD C,L", 4, RegisterType.C, RegisterType.L),
        new LoadFromRegisterToRegister(0x4F, "LD C,A", 4, RegisterType.C, RegisterType.A),
        new LoadFromRegisterToRegister(0x50, "LD D,B", 4, RegisterType.D, RegisterType.B),
        new LoadFromRegisterToRegister(0x51, "LD D,C", 4, RegisterType.D, RegisterType.C),
        new LoadFromRegisterToRegister(0x52, "LD D,D", 4, RegisterType.D, RegisterType.D),
        new LoadFromRegisterToRegister(0x53, "LD D,E", 4, RegisterType.D, RegisterType.E),
        new LoadFromRegisterToRegister(0x54, "LD D,H", 4, RegisterType.D, RegisterType.H),
        new LoadFromRegisterToRegister(0x55, "LD D,L", 4, RegisterType.D, RegisterType.L),
        new LoadFromRegisterToRegister(0x57, "LD D,A", 4, RegisterType.D, RegisterType.A),
        new LoadFromRegisterToRegister(0x58, "LD E,B", 4, RegisterType.E, RegisterType.B),
        new LoadFromRegisterToRegister(0x59, "LD E,C", 4, RegisterType.E, RegisterType.C),
        new LoadFromRegisterToRegister(0x5A, "LD E,D", 4, RegisterType.E, RegisterType.D),
        new LoadFromRegisterToRegister(0x5B, "LD E,E", 4, RegisterType.E, RegisterType.E),
        new LoadFromRegisterToRegister(0x5C, "LD E,H", 4, RegisterType.E, RegisterType.H),
        new LoadFromRegisterToRegister(0x5D, "LD E,L", 4, RegisterType.E, RegisterType.L),
        new LoadFromRegisterToRegister(0x5F, "LD E,A", 4, RegisterType.E, RegisterType.A),
        new LoadFromRegisterToRegister(0x60, "LD H,B", 4, RegisterType.H, RegisterType.B),
        new LoadFromRegisterToRegister(0x61, "LD H,C", 4, RegisterType.H, RegisterType.C),
        new LoadFromRegisterToRegister(0x62, "LD H,D", 4, RegisterType.H, RegisterType.D),
        new LoadFromRegisterToRegister(0x63, "LD H,E", 4, RegisterType.H, RegisterType.E),
        new LoadFromRegisterToRegister(0x64, "LD H,H", 4, RegisterType.H, RegisterType.H),
        new LoadFromRegisterToRegister(0x65, "LD H,L", 4, RegisterType.H, RegisterType.L),
        new LoadFromRegisterToRegister(0x67, "LD H,A", 4, RegisterType.H, RegisterType.A),
        new LoadFromRegisterToRegister(0x68, "LD L,B", 4, RegisterType.L, RegisterType.B),
        new LoadFromRegisterToRegister(0x69, "LD L,C", 4, RegisterType.L, RegisterType.C),
        new LoadFromRegisterToRegister(0x6A, "LD L,D", 4, RegisterType.L, RegisterType.D),
        new LoadFromRegisterToRegister(0x6B, "LD L,E", 4, RegisterType.L, RegisterType.E),
        new LoadFromRegisterToRegister(0x6C, "LD L,H", 4, RegisterType.L, RegisterType.H),
        new LoadFromRegisterToRegister(0x6D, "LD L,L", 4, RegisterType.L, RegisterType.L),
        new LoadFromRegisterToRegister(0x6F, "LD L,A", 4, RegisterType.L, RegisterType.A),
        new LoadFromRegisterToRegister(0x78, "LD A,B", 4, RegisterType.A, RegisterType.B),
        new LoadFromRegisterToRegister(0x79, "LD A,C", 4, RegisterType.A, RegisterType.C),
        new LoadFromRegisterToRegister(0x7A, "LD A,D", 4, RegisterType.A, RegisterType.D),
        new LoadFromRegisterToRegister(0x7B, "LD A,E", 4, RegisterType.A, RegisterType.E),
        new LoadFromRegisterToRegister(0x7C, "LD A,H", 4, RegisterType.A, RegisterType.H),
        new LoadFromRegisterToRegister(0x7D, "LD A,L", 4, RegisterType.A, RegisterType.L),
        new LoadFromRegisterToRegister(0x7F, "LD A,A", 4, RegisterType.A, RegisterType.A),
        
        new LoadDirectToRegisterInstruction(0x06, "LD B,d8", 8, InstructionSize.D8, RegisterType.B),
        new LoadDirectToRegisterInstruction(0x0E, "LD C,d8", 8, InstructionSize.D8, RegisterType.C),
        new LoadDirectToRegisterInstruction(0x16, "LD D,d8", 8, InstructionSize.D8, RegisterType.D),
        new LoadDirectToRegisterInstruction(0x1E, "LD E,d8", 8, InstructionSize.D8, RegisterType.E),
        new LoadDirectToRegisterInstruction(0x26, "LD H,d8", 8, InstructionSize.D8, RegisterType.H),
        new LoadDirectToRegisterInstruction(0x2E, "LD L,d8", 8, InstructionSize.D8, RegisterType.L),
        new LoadDirectToRegisterInstruction(0x3E, "LD A,d8", 8, InstructionSize.D8, RegisterType.A),
        
        new LoadDirectToRegisterInstruction(0x01, "LD BC,d16", 12, InstructionSize.D16, RegisterType.BC),
        new LoadDirectToRegisterInstruction(0x11, "LD DE,d16", 12, InstructionSize.D16, RegisterType.DE),
        new LoadDirectToRegisterInstruction(0x21, "LD HL,d16", 12, InstructionSize.D16, RegisterType.HL),
        new LoadDirectToRegisterInstruction(0x31, "LD SP,d16", 12, InstructionSize.D16, RegisterType.SP),
        
        new GenericInstruction(
            0x08,
            "LD (a16),SP",
            20,
            InstructionSize.D16,
            action: (instruction, cpu, data) =>
            {
                var sp = cpu.SP;
                var (high, low) = sp.GetValue().ToBytes();
                
                cpu.WriteByte(data.ToUshort(), low);
                cpu.WriteByte((ushort)(data.ToUshort() + 1), high);
            }
        ),
        
        new LoadAddressToRegister(0x0A, "LD A,(BC)", 8, InstructionSize.None, RegisterType.A, RegisterType.BC),
        new LoadAddressToRegister(0x1A, "LD A,(DE)", 8, InstructionSize.None, RegisterType.A, RegisterType.DE),
        // new LoadAddressToRegister(0x2A, "LD A,(HL+)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL, IncDecOperation.Increment),
        // new LoadAddressToRegister(0x3A, "LD A,(HL-)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL, IncDecOperation.Decrement),
        new LoadAddressToRegister(0x46, "LD B,(HL)", 8, InstructionSize.None, RegisterType.B, RegisterType.HL),
        new LoadAddressToRegister(0x4E, "LD C,(HL)", 8, InstructionSize.None, RegisterType.C, RegisterType.HL),
        new LoadAddressToRegister(0x56, "LD D,(HL)", 8, InstructionSize.None, RegisterType.D, RegisterType.HL),
        new LoadAddressToRegister(0x5E, "LD E,(HL)", 8, InstructionSize.None, RegisterType.E, RegisterType.HL),
        new LoadAddressToRegister(0x66, "LD H,(HL)", 8, InstructionSize.None, RegisterType.H, RegisterType.HL),
        new LoadAddressToRegister(0x6E, "LD L,(HL)", 8, InstructionSize.None, RegisterType.L, RegisterType.HL),
        new LoadAddressToRegister(0x7E, "LD A,(HL)", 8, InstructionSize.None, RegisterType.A, RegisterType.HL),
        
        new LoadRegisterIntoAddress(0x02, "LD (BC),A", 8, InstructionSize.None, RegisterType.BC, RegisterType.A),
        new LoadRegisterIntoAddress(0x12, "LD (DE),A", 8, InstructionSize.None, RegisterType.DE, RegisterType.A),
        new LoadRegisterIntoAddress(0x70, "LD (HL),B", 8, InstructionSize.None, RegisterType.HL, RegisterType.B),
        new LoadRegisterIntoAddress(0x71, "LD (HL),C", 8, InstructionSize.None, RegisterType.HL, RegisterType.C),
        new LoadRegisterIntoAddress(0x72, "LD (HL),D", 8, InstructionSize.None, RegisterType.HL, RegisterType.D),
        new LoadRegisterIntoAddress(0x73, "LD (HL),E", 8, InstructionSize.None, RegisterType.HL, RegisterType.E),
        new LoadRegisterIntoAddress(0x74, "LD (HL),H", 8, InstructionSize.None, RegisterType.HL, RegisterType.H),
        new LoadRegisterIntoAddress(0x75, "LD (HL),L", 8, InstructionSize.None, RegisterType.HL, RegisterType.L),
        new LoadRegisterIntoAddress(0x77, "LD (HL),A", 8, InstructionSize.None, RegisterType.HL, RegisterType.A),
        
        
        
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
            0xFA,
            "LD A,(a16)",
            16,
            InstructionSize.D16,
            RegisterType.A,
            action: (instruction, cpu, data) =>
            {
                var regValue = cpu.ReadByte(data.ToUshort());
                cpu.WriteByteRegister(instruction.Register1, regValue);
            }
        ),
        
        new GenericInstruction(
            0xE2,
            "LD (C),A",
            8,
            action: (instruction, cpu, data) =>
            {
                var regValue = cpu.ReadByteRegister(RegisterType.A);
                cpu.WriteByte((ushort)(0xFF00 + cpu.ReadByteRegister(RegisterType.C)), regValue);
            }
        ),
        new GenericInstruction(
            0xF2,
            "LD A,(C)",
            8,
            action: (instruction, cpu, data) =>
            {
                var regValue = cpu.ReadByte((ushort)(0xFF00 + cpu.ReadByteRegister(RegisterType.C)));
                cpu.WriteByteRegister(RegisterType.A, regValue);
            }
        ),
        
        new GenericInstruction(
            0x32,
            "LD (HL-),A",
            8,
            action: (instr, cpu, data) =>
            {
                var hl = cpu.ReadUshortRegister(RegisterType.HL);
                cpu.WriteByte(hl, cpu.A);
                hl--;
                cpu.WriteUshortRegister(RegisterType.HL, hl);
            }
        ),
        new GenericInstruction(
            0x22,
            "LD (HL+),A",
            8,
            action: (instr, cpu, data) =>
            {
                var hl = cpu.ReadUshortRegister(RegisterType.HL);
                cpu.WriteByte(hl, cpu.A);
                hl++;
                cpu.WriteUshortRegister(RegisterType.HL, hl);
            }
        ),
        new GenericInstruction(
            0x2A,
            "LD A,(HL+)",
            8,
            action: (instr, cpu, data) =>
            {
                var hl = cpu.ReadUshortRegister(RegisterType.HL);
                var value = cpu.ReadByte(hl);
                cpu.WriteByteRegister(RegisterType.A, value);
                hl++;
                cpu.WriteUshortRegister(RegisterType.HL, hl);
            }
        ),
        new GenericInstruction(
            0x3A,
            "LD A,(HL-)",
            8,
            action: (instr, cpu, data) =>
            {
                var hl = cpu.ReadUshortRegister(RegisterType.HL);
                var value = cpu.ReadByte(hl);
                cpu.WriteByteRegister(RegisterType.A, value);
                hl--;
                cpu.WriteUshortRegister(RegisterType.HL, hl);
            }
        ),
        new GenericInstruction(
            0x36,
            "LD (HL),d8",
            12,
            action: (instruction, cpu, data) =>
            {
                var hl = cpu.ReadUshortRegister(RegisterType.HL);
                cpu.WriteByte(hl, data.ToByte());
            }
        )
    };
}

public class LoadFromRegisterToRegister : Instruction
{
    public LoadFromRegisterToRegister(
        byte opcode, 
        string mnemonic, 
        int cycles, 
        RegisterType register1 = RegisterType.None, 
        RegisterType register2 = RegisterType.None) 
        : base(opcode, mnemonic, cycles, InstructionSize.None, register1, register2)
    {
    }

    public override void Execute(GameboyEmu.Cpu.Cpu cpu, FetchedData data)
    {
        var regVal = cpu.ReadByteRegister(Register2);
        cpu.WriteByteRegister(Register1, regVal);
    }
}