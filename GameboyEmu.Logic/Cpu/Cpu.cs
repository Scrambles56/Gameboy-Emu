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

    public Register16 SP { get; set; } = new(0xFFFE);
    public Register16 PC { get; set; } = new(0x100);
    
    private AddressBus _addressBus;

    private ushort FetchedData { get; set; }
    private Instruction? LastInstruction { get; set; } = null;
    
    private Dictionary<string, int> MissingInstructions { get; set; } = new();

    public Cpu(AddressBus addressBus)
    {
        _addressBus = addressBus;
    }

    public bool IsHalted { get; set; } = false;

    public byte ReadNextByte()
    {
        var value = _addressBus.ReadByte(PC);
        PC++;
        return value;
    }

    public bool Step()
    {
        if (!IsHalted)
        {
            ushort currentPc = PC;
            var opCode = ReadNextByte();
            var instruction = GetInstruction(opCode);
            LogCurrentState(instruction, opCode, currentPc);
            FetchedData = FetchData(instruction);
            instruction?.Execute(this, FetchedData);
            LastInstruction = instruction;

            if (LastInstruction == null)
            {
                var key = $"{opCode:X2}";
                if (MissingInstructions.ContainsKey(key))
                {
                    MissingInstructions[key]++;
                }
                else
                {
                    MissingInstructions[key] = 1;
                }
            }
        }

        return false;
    }
    
    private void LogCurrentState(Instruction? inst, byte opcode, ushort pc)
    {
        var mnemonic = inst?.Mnemonic ?? "UNKN";
        var byte1 = _addressBus.ReadByte(PC + 1);
        var byte2 = _addressBus.ReadByte(PC + 2);
        
        Console.WriteLine($"{pc:X4}: {mnemonic} ({opcode:X2}, {byte1:X2}, {byte2:X2})");
    }
    
    private ushort FetchData(Instruction? inst)
    {
        if (inst == null)
        {
            return 0;
        }
        
        switch (inst.AddressingMode)
        {
            case AddressingMode.Impl: return 0;
            
            case AddressingMode.D8:
                return ReadNextByte();
            
            case AddressingMode.R_D16:
            case AddressingMode.D16:
                var byte1 = ReadNextByte();
                var byte2 = ReadNextByte();
                
                return (ushort) (byte1 | (byte2 << 8));
            
            case AddressingMode.Register:
                return ReadRegister(inst.Register1);
                
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private ushort ReadRegister(RegisterType instRegister1)
    {
        return instRegister1 switch
        {
            RegisterType.A => A,
            RegisterType.B => B,
            RegisterType.C => C,
            RegisterType.D => D,
            RegisterType.E => E,
            RegisterType.H => H,
            RegisterType.L => L,
            RegisterType.AF => (ushort)((A << 8) | F),
            RegisterType.BC => (ushort)((B << 8) | C),
            RegisterType.DE => (ushort)((D << 8) | E),
            RegisterType.HL => (ushort)((H << 8) | L),
            RegisterType.SP => SP,
            RegisterType.PC => PC,
            _ => throw new ArgumentOutOfRangeException(nameof(instRegister1), instRegister1, null)
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
}