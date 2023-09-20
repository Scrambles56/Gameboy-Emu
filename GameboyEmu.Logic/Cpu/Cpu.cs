using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Instructions;
using static GameboyEmu.Logic.Cpu.Instructions.Instructions;

namespace GameboyEmu.Cpu;

public class Cpu
{
    public Register8 A { get; set; } = new(0x01);
    public Register8 B { get; set; } = new(0x00);
    public Register8 C { get; set; } = new(0x13);
    public Register8 D { get; set; } = new(0x00);
    public Register8 E { get; set; } = new(0xD8);
    public RegisterFlags F { get; set; } = new();
    public Register8 H { get; set; } = new(0x01);
    public Register8 L { get; set; } = new(0x4D);
    
    public Register16 AF => new((ushort)((A << 8) | F));
    public Register16 BC => new((ushort)((B << 8) | C));
    public Register16 DE => new((ushort)((D << 8) | E));
    public Register16 HL => new((ushort)((H << 8) | L));

    public Register16 SP { get; set; } = new(0xFFFE);
    public Register16 PC { get; set; } = new(0x100);
    
    private AddressBus _addressBus;

    private FetchedData? FetchedData { get; set; }
    public Instruction? LastInstruction { get; set; } = null;
    
    private Dictionary<string, int> MissingInstructions { get; set; } = new();

    public Cpu(AddressBus addressBus)
    {
        _addressBus = addressBus;
    }


    public byte ReadNextByte()
    {
        var value = _addressBus.ReadByte(PC);
        PC++;
        return value;
    }

    public void Step()
    {
        ushort currentPc = PC;
        var opCode = ReadNextByte();
        var instruction = GetInstruction(opCode);
        LogCurrentState(instruction, opCode, currentPc);

        if (instruction is not null)
        {
            FetchedData = instruction.FetchData(this);
            instruction.Execute(this, FetchedData);
        }

        LastInstruction = instruction;
    }
    
    private void LogCurrentState(Instruction? inst, byte opcode, ushort pc)
    {
        var mnemonic = inst?.Mnemonic ?? "UNKN";
        var byte1 = _addressBus.ReadByte(PC + 1);
        var byte2 = _addressBus.ReadByte(PC + 2);
        
        Console.WriteLine($"{pc:X4}: ({opcode:X2})  {mnemonic} ({byte1:X2}, {byte2:X2})");
    }

    public byte ReadByteRegister(RegisterType registerType)
    {
        return registerType switch
        {

            RegisterType.A => A,
            RegisterType.B => B,
            RegisterType.C => C,
            RegisterType.D => D,
            RegisterType.E => E,
            RegisterType.H => H,
            RegisterType.L => L,
            _ => throw new ArgumentOutOfRangeException(nameof(registerType), registerType, null)
        };
    }
    
    public ushort ReadUshortRegister(RegisterType registerType)
    {
        return registerType switch
        {
            RegisterType.AF => (ushort)((A << 8) | F),
            RegisterType.BC => (ushort)((B << 8) | C),
            RegisterType.DE => (ushort)((D << 8) | E),
            RegisterType.HL => (ushort)((H << 8) | L),
            RegisterType.SP => SP,
            RegisterType.PC => PC,
            _ => throw new ArgumentOutOfRangeException(nameof(registerType), registerType, null)
        };
    }

    public override string ToString()
    {
        return $$"""
CPU Registers
----
Stack Pointer: {{SP}}
Program Counter: {{PC}}

A: {{A}}
B: {{B}}
C: {{C}}
D: {{D}}
E: {{E}}
F: {{F}}
H: {{H}}
L: {{L}}
""";
    }

    public bool CheckCondition(Condition condition)
    {
        return condition switch
        {
            Condition.None => true,
            Condition.C => F.CarryFlag,
            Condition.NC => !F.CarryFlag,
            Condition.Z => F.ZeroFlag,
            Condition.NZ => !F.ZeroFlag,
            _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, null)
        };
    }
}