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

bool toConsole = true;

var logFile = $"logs/log-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

LoggerConfiguration logConfiguration;
if (toConsole)
{
    logConfiguration = new LoggerConfiguration()
        .WriteTo.Console(
            outputTemplate: "{Message:lj}{NewLine}{Exception}"    
        );
}
else
{
    logConfiguration = new LoggerConfiguration()
        .WriteTo.File(logFile);
}

var msLogger = new SerilogLoggerProvider(logConfiguration.CreateLogger()).CreateLogger("Default");

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var docMode = configuration.GetValue<bool>("doc");

var rom = configuration.GetValue<string>("rom");
if (rom == null)
{
    Console.WriteLine("Please specify a rom file.");
    return 2;
}

var cartridge = AsyncContext.Run(async () => await new Cartridge(rom).Load());
var vram = new VRam();
var oam = new OAM(msLogger);
var lcdControl = new LcdControl(msLogger, docMode);

var lowerWorkram = new WorkRAM(0xC000);
var upperWorkram = new WorkRAM(0xD000);
var highRam = new HighRam();
var ioBus = new IOBus(lcdControl);
var addressBus = new AddressBus(cartridge, lowerWorkram, upperWorkram, highRam, ioBus, vram, oam);


var gpu = new Gpu(vram, oam, lcdControl, addressBus);


var cpu = new Cpu(addressBus, msLogger);
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


var cpuTask = Task.Run(() =>
{
    var instructionDelay = 0;
    while (true)
    {
        try
        {
            if (instructionDelay == 0)
            {
                instructionDelay = cpu.Step();
            }
            
            gpu.Tick();
            instructionDelay--;
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

GameboyWindow.Open(cpu, gpu);

await cpuTask;
return 1;