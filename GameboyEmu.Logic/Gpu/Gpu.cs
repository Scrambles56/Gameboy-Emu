using GameboyEmu.Logic.IOController;

namespace GameboyEmu.Logic.Gpu;

public class Gpu
{
    private readonly LcdControl _lcdControl;
    public VRam Vram { get; set; } = new();

    private byte[] FrameBuffer = new byte[160 * 144];
    
    public Gpu(LcdControl lcdControl)
    {
        _lcdControl = lcdControl ?? throw new ArgumentNullException(nameof(lcdControl));
    }

    public void Tick()
    {
        if (!_lcdControl.LcdAndPpuEnabled)
        {
            return;
        }

    }
}