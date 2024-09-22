using System.Diagnostics;
using GameboyEmu.Logic.IOController;

namespace GameboyEmu.Logic.Gpu;

public class Gpu
{
    private readonly VRam _vram;
    private readonly LcdControl _lcdControl;

    private const int ScreenWidth = 160;
    private const int ScreenHeight = 144;
    private byte[] _frameBuffer = new byte[ScreenWidth * ScreenHeight];
    
    public byte[] FrameBuffer => _frameBuffer.Clone() as byte[];
    
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
        
        UpdateFrameBuffer();
    }
    
    public IEnumerable<Tile> GetTileData()
    {
        var tileData = new List<Tile>();
        for (var i = 0; i < 384; i++)
        {
            var tile = new byte[16];
            for (var j = 0; j < 16; j++)
            {
                tile[j] = _vram.ReadByte((ushort)(0x8000 + i * 16 + j));
            }
            tileData.Add(new Tile(tile));
        }

        return tileData;
    }
    
    private void UpdateFrameBuffer()
    {
        for (var x = 0; x < ScreenWidth; x++)
        {
            for (var y = 0; y < ScreenHeight; y++)
            {
                var address = (ushort)(0x8000 + x + y * ScreenWidth);
                var b = (byte)((x / (double)ScreenWidth + y / (double)ScreenHeight) / 2 * 255);
                _frameBuffer[x + y * ScreenWidth] = b;
            }
        }
    }
}