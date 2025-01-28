using System.Text;
using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Cpu.Instructions;
using GameboyEmu.Logic.IOController;
using static GameboyEmu.Logic.Cpu.Instructions.Instructions;

namespace GameboyEmu.Cpu;

using System.Diagnostics;
using Microsoft.Extensions.Logging;

public partial class Cpu
{
    
    public Register8 A { get; set; } = new(0x00);
    public Register8 B { get; set; } = new(0x00);
    public Register8 C { get; set; } = new(0x00);
    public Register8 D { get; set; } = new(0x00);
    public Register8 E { get; set; } = new(0x00);
    public RegisterFlags F { get; set; } = new(0x00);
    public Register8 H { get; set; } = new(0x00);
    public Register8 L { get; set; } = new(0x0);
    public Register16 SP { get; set; } = new(0x0000);
    public Register16 PC { get; set; } = new(0x0000);

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

    public bool Halted { get; set; }

    private readonly AddressBus _addressBus;
    private readonly InterruptsController _interruptsController;
    private readonly ILogger _logger;

    private FetchedData? FetchedData { get; set; }

    public bool CbMode = false;
    private readonly Dictionary<string, int> _executedInstructions = new();

    public Cpu(
        AddressBus addressBus,
        InterruptsController interruptsController,
        ILogger logger
    )
    {
        _addressBus = addressBus;
        _interruptsController = interruptsController;
        _logger = logger;
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

    /// <summary>
    /// Returns executed instruction length
    /// </summary>
    /// <returns></returns>
    public int Step()
    {
        if (HandleInterrupts())
        {
            return 20;
        }

        if (_interruptsController.SetInterruptMasterEnabledFlag)
        {
            _interruptsController.InterruptMasterEnabledFlag = true;
            _interruptsController.SetInterruptMasterEnabledFlag = false;
        }
        
        if (Halted)
        {
            return 4;
        }

        var opCode = ReadNextByte();
        var instruction = GetInstruction(opCode, this);
        int cycles = -1;

        if (instruction is not null)
        {
            FetchedData = instruction.FetchData(this);
            cycles = instruction.Execute(this, FetchedData);

            if (DocMode && instruction.Opcode != 0xCB)
            {
                LogGbDocState();
            }

            _executedInstructions[instruction.Mnemonic] =
                _executedInstructions.TryGetValue(instruction.Mnemonic, out var counter) ? counter + 1 : 1;
        }
        else
        {
            _logger.LogWarning("Unknown instruction: {OpCode}", $"{(CbMode ? "CB" : "")}{opCode:X2}");
            Debugger.Break();
        }


        LastInstruction = instruction;
        FetchedData = null;
        
        if (instruction is null || cycles == -1)
        {
            throw new("Instruction not found");
        }
        
        return cycles;
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