using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Extensions;
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
    public RegisterFlags F { get; set; } = new(0xB0);
    public Register8 H { get; set; } = new(0x01);
    public Register8 L { get; set; } = new(0x4D);
    
    public Register16 AF
    {
        get => new((ushort)((A << 8) | F));
        set
        {
            var (high, low) = value.GetValue().ToBytes();
            
            A.SetValue(high);
            F.SetValue(low);
        }
    }

    public Register16 BC
    {
        get => new((ushort)((B << 8) | C));
        set
        {
            var (high, low) = value.GetValue().ToBytes();
            
            B.SetValue(high);
            C.SetValue(low);
        }
    }

    public Register16 DE
    {
        get => new((ushort)((D << 8) | E));
        set
        {
            var (high, low) = value.GetValue().ToBytes();
            
            D.SetValue(high);
            E.SetValue(low);
        }
    }

    public Register16 HL
    {
        get => new((ushort)((H << 8) | L));
        set
        {
            var (high, low) = value.GetValue().ToBytes();
            
            H.SetValue(high);
            L.SetValue(low);
        }

    }

    public Register16 SP { get; set; } = new(0xFFFE);
    public Register16 PC { get; set; } = new(0x100);
    
    private AddressBus _addressBus;

    private FetchedData? FetchedData { get; set; }

    public bool cbMode = false;
    private Dictionary<string, int> _executedInstructions = new();

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
    
    public void WriteByte(ushort address, byte value)
    {
        _addressBus.WriteByte(address, value);
    }

    public byte ReadByte(ushort address) => _addressBus.ReadByte(address);

    public void Step()
    {
        ushort currentPc = PC;
        var opCode = ReadNextByte();
        var instruction = GetInstruction(opCode, this);

        if (instruction is not null)
        {
            FetchedData = instruction.FetchData(this);
            LogCurrentState(instruction, opCode, currentPc);
            instruction.Execute(this, FetchedData);
            
            _executedInstructions[instruction.Mnemonic] = _executedInstructions.TryGetValue(instruction.Mnemonic, out var counter) ? counter + 1 : 1;
        }
        else
        {
            LogCurrentState(instruction, opCode, currentPc);
        }
        

        LastInstruction = instruction;
        FetchedData = null;
    }
    
    private void LogCurrentState(Instruction? inst, byte opcode, ushort pc)
    {
        var mnemonic = inst?.Mnemonic ?? "UNKN";
        
        var a = A.GetValue();
        var b = B.GetValue();
        var c = C.GetValue();
        var d = D.GetValue();
        var e = E.GetValue();
        var f = F.GetValue();
        var h = H.GetValue();
        var l = L.GetValue();
        var sp = SP.GetValue(); 
        
        Console.WriteLine($"{pc:X4}: ({opcode:X2})  {mnemonic,-20} {FetchedData,-20} {inst?.InstructionSize.ToString(),-10} A: {a:X2} B: {b:X2} C: {c:X2} D: {d:X2} E: {e:X2} H: {h:X2} L: {l:X2} SP: {sp:X4}  \t Flags: Carry: {(F.CarryFlag?1:0)}, Zero: {(F.ZeroFlag?1:0)}, HalfCarry: {(F.HalfCarryFlag?1:0)}, Subtract: {(F.SubtractFlag?1:0)}, IME: {(_addressBus.InterruptsEnabled?1:0)}");
    }

    public void WriteByteRegister(RegisterType registerType, byte value)
    {
        switch (registerType)
        {
            case RegisterType.A:
                A.SetValue(value);
                break;
            case RegisterType.B:
                B.SetValue(value);
                break;
            case RegisterType.C:
                C.SetValue(value);
                break;
            case RegisterType.D:
                D.SetValue(value);
                break;
            case RegisterType.E:
                E.SetValue(value);
                break;
            case RegisterType.H:
                H.SetValue(value);
                break;
            case RegisterType.L:
                L.SetValue(value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(registerType), registerType, null);
        }
    }
    
    public void WriteUshortRegister(RegisterType registerType, ushort value)
    {
        switch (registerType)
        {
            case RegisterType.AF:
                AF = value;
                break;
            case RegisterType.BC:
                BC = value;
                break;
            case RegisterType.DE:
                DE = value;
                break;
            case RegisterType.HL:
                HL = value;
                break;
            case RegisterType.SP:
                SP.SetValue(value);
                break;
            case RegisterType.PC:
                PC.SetValue(value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(registerType), registerType, null);
        }
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
    
    
    public Instruction? LastInstruction { get; set; } = null;
    
    private Dictionary<string, int> MissingInstructions { get; set; } = new();
}