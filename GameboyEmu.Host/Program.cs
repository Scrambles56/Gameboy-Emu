using GameboyEmu.Cpu;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cpu;
using GameboyEmu.Logic.IOController;
using GameboyEmu.Logic.Memory;
using Microsoft.Extensions.Configuration;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using SkiaSharp;
using Window = Silk.NET.Windowing.Window;

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
Console.WriteLine($"""
                   {cartridge}
                   
                   
                   """);


var options = WindowOptions.Default;
options.Size = new(800, 600);
options.Title = "Silk.NET backed Skia rendering!";
options.PreferredStencilBufferBits = 8;
options.PreferredBitDepth = new Vector4D<int>(8, 8, 8, 8);

GlfwWindowing.Use();
using var window = Window.Create(options);
window.Initialize();
using var grGlInterface = GRGlInterface.Create((name => window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : (IntPtr) 0));
grGlInterface.Validate();
using var grContext = GRContext.CreateGl(grGlInterface);
var renderTarget = new GRBackendRenderTarget(800, 600, 0, 8, new GRGlFramebufferInfo(0, 0x8058)); // 0x8058 = GL_RGBA8`
using var surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
using var canvas = surface.Canvas;

window.Render += d =>
{
    grContext.ResetContext();
    canvas.Clear(SKColors.Cyan);
    using var red = new SKPaint();
    red.Color = new SKColor(255, 0, 0, 255);
    canvas.DrawCircle(150, 150, 100, red);
    canvas.Flush();
};

Console.WriteLine("Window Run");
window.Run();

Console.WriteLine("Window run complete");


var lowerWorkram = new WorkRAM(0xC000);
var upperWorkram = new WorkRAM(0xD000);
var highRam = new HighRam();
var lcdControl = new LcdControl();
var ioBus = new IOBus(lcdControl);
var addressBus = new AddressBus(cartridge, lowerWorkram, upperWorkram, highRam, ioBus);
var cpu = new Cpu(addressBus);
var instructionsExecutedCount = 0;

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
            Console.WriteLine("""
                              Instructions Executed: {0}
                              
                              
                              
                              """, instructionsExecutedCount);

            Console.WriteLine(cartridge);
            Console.WriteLine(cpu);
            throw;
        }
    }
});

await Task.WhenAll(new List<Task>
{
    cpuTask
});

return 1;