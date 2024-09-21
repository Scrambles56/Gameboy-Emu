using System.Text;
using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Cpu.Instructions;
using static GameboyEmu.Logic.Cpu.Instructions.Instructions;

namespace GameboyEmu.Cpu;

using System.Diagnostics;

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
        var b = _addressBus.ReadByte(PC);
        PC++;
        return b;
    }

    public void WriteByte(ushort address, byte value) => _addressBus.WriteByte(address, value);
    public byte ReadByte(ushort address) => _addressBus.ReadByte(address);

    public const decimal ClockSpeed = 4194304.0m;

    public async Task Step()
    {
        var isCbMode = cbMode;
        var opCode = ReadNextByte();
        var instruction = GetInstruction(opCode, this);

        if (instruction is not null)
        {
            FetchedData = instruction.FetchData(this);
            instruction.Execute(this, FetchedData);

            if (instruction.Opcode != 0xCB)
            {
                LogGbDocState();
            }

            _executedInstructions[instruction.Mnemonic] = _executedInstructions.TryGetValue(instruction.Mnemonic, out var counter) ? counter + 1 : 1;
        }
        else
        {
            Console.WriteLine($"Unknown instruction: {(isCbMode ? "CB" : "")}" + opCode.ToString("X2"));
            Debugger.Break();
        }
        

        LastInstruction = instruction;
        FetchedData = null;
    }

    /// <summary>
    /// Format of logging is important for this tool:
    /// https://github.com/robert/gameboy-doctor
    /// </summary>
    public void LogGbDocState()
    {
        var a = A.GetValue();
        var b = B.GetValue();
        var c = C.GetValue();
        var d = D.GetValue();
        var e = E.GetValue();
        var f = F.GetValue();
        var h = H.GetValue();
        var l = L.GetValue();
        var sp = SP.GetValue();
        var pc = PC.GetValue();


        var spInboundMemory = new byte?[]
        {
            ReadInboundsByte(sp),
            ReadInboundsByte((ushort)(sp + 1)),
            ReadInboundsByte((ushort)(sp + 2)),
            ReadInboundsByte((ushort)(sp + 3))
        };

        byte? ReadInboundsByte(ushort address)
        {
            try
            {
                return address < 0xFFFE ? ReadByte(address) : null;
            }
            catch
            {
                return null;
            }
        }
        

        var logString = new StringBuilder()
            .Append($"A:{a:X2} ")
            .Append($"F:{f:X2} ")
            .Append($"B:{b:X2} ")
            .Append($"C:{c:X2} ")
            .Append($"D:{d:X2} ")
            .Append($"E:{e:X2} ")
            .Append($"H:{h:X2} ")
            .Append($"L:{l:X2} ")
            .Append($"SP:{sp:X4} ")
            .Append($"PC:{pc:X4} ")
            .Append($"PCMEM:{ReadByte(pc):X2},{ReadByte((ushort)(pc + 1)):X2},{ReadByte((ushort)(pc + 2)):X2},{ReadByte((ushort)(pc + 3)):X2} ")
            // .Append($"SPMEM:{spInboundMemory[0]:X2},{spInboundMemory[1]:X2},{spInboundMemory[2]:X2},{spInboundMemory[3]:X2} ")
            .ToString();
        
        Console.WriteLine(logString);
        
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
        var interrupts = _addressBus.EnabledInterrupts;
        
        var logString = new StringBuilder()
            .Append($"{pc.ToString("X4")}: ")
            .Append($"({opcode.ToString("X2")}) {mnemonic,-20} ")
            .Append($"{FetchedData,-20} ")
            .Append($"{inst?.InstructionSize.ToString(),-10} ")
            .Append($"A: {a:X2} ")
            .Append($"B: {b:X2} ")
            .Append($"C: {c:X2} ")
            .Append($"D: {d:X2} ")
            .Append($"E: {e:X2} ")
            .Append($"H: {h:X2} ")
            .Append($"L: {l:X2} ")
            .Append($"SP: {sp:X4} \t")
            .Append("Flags: ")
            .Append($"Carry: {(F.CarryFlag?1:0)}, ")
            .Append($"Zero: {(F.ZeroFlag?1:0)}, ")
            .Append($"HalfCarry: {(F.HalfCarryFlag?1:0)}, ")
            .Append($"Subtract: {(F.SubtractFlag?1:0)} \t")
            .Append("Interrupts: ")
            .Append($"IME: {(_addressBus.InterruptMasterEnabledFlag?1:0)}")
            .ToString();
            
            
            Console.WriteLine(logString);
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

    public void SetInterruptMasterFlag(bool state)  
    {
        _addressBus.InterruptMasterEnabledFlag = state;
    }
}