using GameboyEmu.Logic.IOController;

namespace GameboyEmu.Logic.Gpu;

public class Gpu
{
    private readonly VRam _vram;
    private readonly LcdControl _lcdControl;

    private byte[] FrameBuffer = new byte[160 * 144];
    
    public Gpu(VRam vram, LcdControl lcdControl)
    {
        _vram = vram;
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