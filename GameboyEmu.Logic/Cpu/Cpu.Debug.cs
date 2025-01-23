namespace GameboyEmu.Cpu;

using System.Text;
using Logic.Cpu.Instructions;

public partial class Cpu
{

    public double AvgOpTimeMs { get; private set; } = 0;
    public int AvgCount { get; private set; } = 0;
    public Instruction? LastInstruction { get; set; } = null;

    private Dictionary<string, int> MissingInstructions { get; set; } = new();

    private void StepTimerCallback(TimeSpan ts)
    {
        AvgOpTimeMs = (AvgOpTimeMs * AvgCount + ts.TotalMilliseconds) / ++AvgCount;
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
    private StringBuilder _logBuilder = new();

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
            .Append(
                $"PCMEM:{ReadByte(pc):X2},{ReadByte((ushort)(pc + 1)):X2},{ReadByte((ushort)(pc + 2)):X2},{ReadByte((ushort)(pc + 3)):X2} ")
            // .Append($"SPMEM:{spInboundMemory[0]:X2},{spInboundMemory[1]:X2},{spInboundMemory[2]:X2},{spInboundMemory[3]:X2} ")
            .ToString();

        Console.WriteLine(logString);
    }
}