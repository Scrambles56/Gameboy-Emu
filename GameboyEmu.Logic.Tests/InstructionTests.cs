
using FluentAssertions;
using GameboyEmu.Logic.Gpu;

namespace GameboyEmu.Logic.Tests;
using Cpu;
using Cpu.Instructions;
using IOController;
using Memory;
using Microsoft.Extensions.Logging.Abstractions;

public class InstructionTests
{
    public record TestCase(byte OpCode)
    {
        public override string ToString() => $"""
                                              {OpCode:X2}
                                              """;
    }

    public static IEnumerable<object[]> NonCbTestCases()
    {
        return Enumerable.Range(0, 255)
            .Select(i => (byte)i)
            .Where(i => !ExcludedOpCodes.Contains(i))
            .Select(i => new object[] { new TestCase(i) });
    }
    
    public static IEnumerable<object[]> CbTestCases()
    {
        return Enumerable.Range(0, 255)
            .Select(i => (byte)i)
            .Select(i => new object[] { new TestCase(i) });
    }
    
    private static readonly byte[] ExcludedOpCodes = {
        0xD3, 0xDB, 0xDD, 0xE3, 0xE4, 0xEB, 0xEC, 0xED, 0xF4, 0xFC, 0xFD
    };

    private GameboyEmu.Cpu.Cpu Cpu { get; set; }

    public InstructionTests()
    {
        var logger = NullLogger.Instance;
        
        var cartridgePath = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), "../../../..", "roms", "gb-test-roms-master", "cpu_instrs", "cpu_instrs.gb"));
        var cartridge = new Cartridge.Carts.Cartridge(logger, cartridgePath).Load().GetAwaiter().GetResult();
        var interruptsController = new InterruptsController();
        var timer = new TimerController(interruptsController);
        var vram = new VRam();
        var lowerWorkram = new WorkRAM(0xC000);
        var upperWorkram = new WorkRAM(0xD000);
        var oam = new OAM();
        var highRam = new HighRam();
        var lcdControl = new LcdControl(logger, false);
        var inputControl = new InputControl(interruptsController, logger);
        var ioBus = new IOBus(lcdControl, inputControl, timer, interruptsController);
        var addressBus = new AddressBus(cartridge, lowerWorkram, upperWorkram, highRam, ioBus, interruptsController, vram, oam, logger);
        Cpu = new(addressBus, interruptsController, logger);
    }

    [Theory]
    [MemberData(nameof(NonCbTestCases))]
    public void InstructExistsForOpcode(TestCase testCase)
    {
        Cpu.CbMode = false;
        var opcode = testCase.OpCode;
        
        var instruction = Instructions.GetInstruction(opcode, Cpu);
        instruction.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(CbTestCases))]
    public void CbInstructExistsForOpcode(TestCase testCase)
    {
        Cpu.CbMode = true;
        var opcode = testCase.OpCode;
        var instruction = Instructions.GetInstruction(opcode, Cpu);
        instruction.Should().NotBeNull();
    }
}