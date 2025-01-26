using System.Diagnostics;
using GameboyEmu.Cpu;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Instructions;
using GameboyEmu.Logic.Gpu;
using GameboyEmu.Logic.IOController;
using GameboyEmu.Logic.Memory;
using GameboyEmu.Windowing;
using Microsoft.Extensions.Configuration;
using Nito.AsyncEx;
using Serilog;
using Serilog.Extensions.Logging;


var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var docMode = configuration.GetValue<bool>("doc");
var rom = configuration.GetValue<string>("rom");
var useLogFile = configuration.GetValue<bool>("use-log-file");


var logFile = $"logs/log-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

LoggerConfiguration logConfiguration;
if (docMode && !useLogFile)
{
    logConfiguration = new LoggerConfiguration()
        .WriteTo.Async(wt =>
            wt.Console(
                outputTemplate: "{Message:lj}{NewLine}{Exception}"
            )
        );
}
else
{
    logConfiguration = new LoggerConfiguration()
        .WriteTo.Async(wt =>
            wt.Console(
                outputTemplate: "{Message:lj}{NewLine}{Exception}"
            )
        );
}

var msLogger = new SerilogLoggerProvider(logConfiguration.CreateLogger()).CreateLogger("Default");
if (rom == null)
{
    Console.WriteLine("Please specify a rom file.");
    return 2;
}

var cartridge = await new Cartridge(rom).Load();

var controller = new Controller();
var interruptsController = new InterruptsController();
var vram = new VRam();
var oam = new OAM(msLogger);
var lcdControl = new LcdControl(msLogger, docMode);
var inputControl = new InputControl(interruptsController, msLogger);

var lowerWorkram = new WorkRAM(0xC000);
var upperWorkram = new WorkRAM(0xD000);
var highRam = new HighRam();
var ioBus = new IOBus(lcdControl, inputControl, interruptsController);
var addressBus = new AddressBus(cartridge, lowerWorkram, upperWorkram, highRam, ioBus, interruptsController, vram, oam);


var gpu = new Gpu(vram, oam, lcdControl, interruptsController);
var cpu = new Cpu(addressBus, interruptsController, msLogger);

if (docMode)
{
    cpu.DocMode = true;
    cpu.SetToPostBootRomState();
}

cpu.LogGbDocState();

var shouldPrintTable = configuration.GetValue<bool>("printTable");
if (shouldPrintTable)
{
    Instructions.PrintInstructionTable(cpu);
    return 1;
}

var ctSource = new CancellationTokenSource();
var token = ctSource.Token;

var cpuTask = Task.Run(() =>
{
    var instructionDelay = 0;
    while (!token.IsCancellationRequested)
    {
        try
        {
            controller.Update(inputControl);
            
            if (instructionDelay == 0)
            {
                instructionDelay = cpu.Step();
            }
            
            gpu.Tick();
            instructionDelay--;
        }
        catch
        {
            Debugger.Break();
            ctSource.Cancel();
            throw;
        }
        finally
        {
            Console.Error.Flush();
            Console.Out.Flush();
        }
    }
}, token);

GameboyWindow.Open(cpu, gpu, token);

ctSource.Cancel();
await cpuTask;
return 1;