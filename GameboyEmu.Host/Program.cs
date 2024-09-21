using System.Diagnostics;
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
cpu.LogGbDocState();

var shouldPrintTable = configuration.GetValue<bool>("printTable");
if (shouldPrintTable)
{
    Instructions.PrintInstructionTable(cpu);
    return 1;
}


var cpuTask = Task.Run(async () =>
{
    var avgTime = 0.0;
    var avgCount = 0;
    
    while (true)
    {
        try
        {
            using (new Stopwatcher(ts => 
            {
                avgTime = (avgTime * avgCount + ts.TotalMilliseconds) / ++avgCount;
            }))
            {
                await cpu.Step();
            }

            if (cpu.LastInstruction == null)
            {
                break;
            }

            // if (instructionsExecutedCount % 2000 == 0)
            // {
            //     Console.WriteLine($"Instructions executed: {instructionsExecutedCount}");
            //     Console.WriteLine($"Average time per instruction: {avgTime}ms");
            // }
        }
        catch
        {
            throw;
        }
        finally
        {
            Console.Error.Flush();
            Console.Out.Flush();
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

// Console.WriteLine(res == cpuTask ? "CPU task finished first." : "GPU task finished first.");

return 1;


public class Stopwatcher : IDisposable
{
    private Stopwatch _sw;
    private Action<TimeSpan> _callback;

    public Stopwatcher(Action<TimeSpan> callback)
    {
        _callback = callback;
        _sw = Stopwatch.StartNew();
    }
    
    
    public void Dispose()
    {
        _sw.Stop();
        _callback(_sw.Elapsed);
    }
}