using System.Diagnostics;
using GameboyEmu.Logic.IOController;

namespace GameboyEmu.Logic.Gpu;

using Cpu;
using Cpu.Extensions;

public class Gpu
{
    private readonly VRam _vram;
    private readonly OAM _oam;
    private readonly LcdControl _lcdControl;
    private readonly AddressBus _addressBus;

    private const int ScreenWidth = 456;
    private const int ScreenHeight = 153;
    private byte[] _frameBuffer = new byte[ScreenWidth * ScreenHeight];
    
    public byte[] FrameBuffer => _frameBuffer.Clone() as byte[];

    private long TickCount { get; set; }
    
    public Gpu(
        VRam vram, 
        OAM oam, 
        LcdControl lcdControl,
        AddressBus addressBus
    )
    {
        _vram = vram;
        _oam = oam;
        _lcdControl = lcdControl ?? throw new ArgumentNullException(nameof(lcdControl));
        _addressBus = addressBus;
    }

    public void Tick()
    {
        if (!_lcdControl.LcdAndPpuEnabled)
        {
            return;
        }
        
        var framePointer = (int)(TickCount % (ScreenWidth * ScreenHeight));
        
        var x = framePointer % ScreenWidth;
        var y = framePointer / ScreenWidth;

        if (y >= 144)
        {
            if (y == 144 && x == 0)
            {
                _addressBus.RequestInterrupt(Interrupt.VBlank);
            }
        }
        else if (x < 80)
        {
            // OAM search time
        }
        else if (x < 252)
        {
            // Pixel transfer time
            UpdateFrameBuffer(x - 80, y);
        }
        else
        {
            // HBlank time
        }
        

        TickCount++;
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

    public Tile GetTile(int x, int y)
    {
        // First we need to take the individual pixel coodinates, and convert them to 8x8 tile coordinates
        var tileX = x / 8;
        var tileY = y / 8;
        
        // Next we need to find the tile number
        var tileId = _vram.ReadByte((ushort)(0x9800 + tileY * 32 + tileX));
        
        var tileData = new byte[16];
        for (var i = 0; i < 16; i++)
        {
            tileData[i] = _vram.ReadByte((ushort)(0x8000 + tileId * 16 + i));
        }
        
        return new Tile(tileData);
        
    }
    
    private void UpdateFrameBuffer(int x, int y)
    {
        var tile = GetTile(x, y);
        
        _frameBuffer[x + y * ScreenWidth] = tile.GetPixel(x % 8, y % 8);
        
    }
}