namespace GameboyEmu.Logic.Cpu;

[Flags]
public enum AccessSource
{
    Cpu = 1,
    Gpu = 2,
    GpuCpu = Cpu | Gpu
}