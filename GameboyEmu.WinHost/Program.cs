using GameboyEmu.Cpu;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.Cpu.Instructions;
using GameboyEmu.Logic.IOController;
using GameboyEmu.Logic.Memory;
using Microsoft.Extensions.Configuration;

var printMode = true;

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var rom = configuration.GetValue<string>("rom");
if (rom == null)
{
    Console.WriteLine("Please specify a rom file.");
    return 2;
}

Console.WriteLine($"Loading rom {rom}...");
var cartridge = await new Cartridge(rom).Load();
Console.WriteLine(cartridge);
Console.WriteLine();
Console.WriteLine();


var lowerWorkram = new WorkRAM(0xC000);
var upperWorkram = new WorkRAM(0xD000);
var highRam = new HighRam();
var ioBus = new IOBus();
var addressBus = new AddressBus(cartridge, lowerWorkram, upperWorkram, highRam, ioBus);
var cpu = new Cpu(addressBus);

if (printMode)
{
    Instructions.PrintInstructionTable(cpu);
    cpu.cbMode = true;
    Console.WriteLine("\n\n");
    Instructions.PrintInstructionTable(cpu);
    return 1;
}

var instructionsExecutedCount = 0;
while(true)
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
        Console.WriteLine("Instructions Executed: {0} \n\n\n", instructionsExecutedCount);
        
        Console.WriteLine(cartridge);
        Console.WriteLine(cpu);
        throw;
    }
}

Console.WriteLine(cpu);

return 1;