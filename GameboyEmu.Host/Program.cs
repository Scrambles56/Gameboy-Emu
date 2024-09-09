using GameboyEmu.Cpu;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Instructions;
using GameboyEmu.Logic.Gpu;
using GameboyEmu.Logic.IOController;
using GameboyEmu.Logic.Memory;
using Microsoft.Extensions.Configuration;


var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var rom = configuration.GetValue<string>("rom");
if (rom == null)
{
    Console.WriteLine("Please specify a rom file.");
    return 2;
}

var cartridge = await new Cartridge(rom).Load();
var vram = new VRam();

var gpu = new Gpu(vram, new LcdControl());


var lowerWorkram = new WorkRAM(0xC000);
var upperWorkram = new WorkRAM(0xD000);
var highRam = new HighRam();
var lcdControl = new LcdControl();
var ioBus = new IOBus(lcdControl);
var addressBus = new AddressBus(cartridge, lowerWorkram, upperWorkram, highRam, ioBus, vram);
var cpu = new Cpu(addressBus);

var instructionsExecutedCount = 0;

var shouldPrintTable = configuration.GetValue<bool>("printTable");
if (shouldPrintTable)
{
    Instructions.PrintInstructionTable(cpu);
    return 1;
}

var cpuTask = Task.Run(() =>
{
    while (true)
    {
        try
        {
            cpu.Step();

            if (cpu.LastInstruction == null)
            {
                break;
            }

            instructionsExecutedCount++;
        }
        catch
        {
            throw;
        }
    }
});

var gpuTask = Task.Run(() =>
{
    while (true)
    {
        try
        {
            gpu.Tick();
        }
        catch
        {
            throw;
        }
    }
});

var res = await Task.WhenAny(new List<Task>
{
    cpuTask,
    gpuTask
});

Console.WriteLine(res == cpuTask ? "CPU task finished first." : "GPU task finished first.");

return 1;