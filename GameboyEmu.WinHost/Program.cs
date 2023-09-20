using GameboyEmu.Cpu;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cpu;
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

Console.WriteLine($"Loading rom {rom}...");
var cartridge = await new Cartridge(rom).Load();
Console.WriteLine(cartridge);
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();



var addressBus = new AddressBus(cartridge);
var cpu = new Cpu(addressBus);

for (var i = 0; i < 100; i++)
{
    cpu.Step();

    if (cpu.LastInstruction == null)
    {
        break;
    }
}

Console.WriteLine(cpu);

return 1;