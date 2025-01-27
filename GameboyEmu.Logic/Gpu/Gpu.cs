using System.Diagnostics;
using GameboyEmu.Logic.IOController;

namespace GameboyEmu.Logic.Gpu;

using Cpu;
using Cpu.Extensions;

public class Gpu(
    VRam vram,
    OAM oam,
    LcdControl lcdControl,
    InterruptsController interruptsController)
{
    private const int TimingWidth = 456;
    private const int TimingHeight = 153;
    
    private const int ScreenWidth = 160;
    private const int ScreenHeight = 144;
    private byte[] _frameBuffer = new byte[ScreenWidth * ScreenHeight];
    
    public byte[] FrameBuffer => _frameBuffer.Clone() as byte[];

    private long TickCount { get; set; }
    private long PxCounter { get; set; }

    public void Tick()
    {
        if (!lcdControl.LcdAndPpuEnabled)
        {
            return;
        }
        
        var framePointer = (int)(TickCount % (TimingWidth * TimingHeight));
        
        var timingX = framePointer % TimingWidth;
        var timingY = framePointer / TimingWidth;
        
        lcdControl.WriteByte(0xFF44, (byte)timingY);

        if (timingY >= 144)
        {
            if (timingY == 144 && timingX == 0)
            {
                interruptsController.RequestInterrupt(Interrupt.VBlank);
            }
        }
        else if (timingX < 80)
        {
            if (timingX == 0)
            {
                oam.AllowedSources = AccessSource.Gpu;
            }
            // OAM search time
        }
        else if (timingX < 252)
        {
            if (timingX == 80)
            {
                vram.AllowedSources = AccessSource.Gpu;
            }
            
            var pxPointer = (int)(PxCounter % (ScreenWidth * ScreenHeight));
            var pxX = pxPointer % ScreenWidth;
            var pxY = pxPointer / ScreenWidth;
            
            // Pixel transfer time
            DrawBgPx(pxX, pxY);
            DrawOamPx(pxX, pxY); 
            
            
            PxCounter++;
        }
        else
        {
            if (timingX == 252)
            {
                vram.AllowedSources = AccessSource.GpuCpu;
                oam.AllowedSources = AccessSource.GpuCpu;
            }
            // HBlank time
        }
        

        TickCount++;
    }

    public IEnumerable<Tile> GetOamData()
    {
        var oamData = new List<Tile>();
        for (var i = 0; i < 40; i++)
        {
            var oamTileBase = (ushort)(0xFE00 + i * 4);
            
            var tileId = oam.ReadByte((ushort)(oamTileBase + 2), AccessSource.Gpu);
            
            var tileData = new byte[16];
            for (var j = 0; j < 16; j++)
            {
                tileData[j] = vram.ReadByte((ushort)(0x8000 + tileId * 16 + j), AccessSource.Gpu);
            }
            
            oamData.Add(new Tile(tileData));
        }

        return oamData;
    }
    
    public IEnumerable<Tile> GetTileData()
    {
        var tileData = new List<Tile>();
        for (var i = 0; i < 384; i++)
        {
            var tile = new byte[16];
            for (var j = 0; j < 16; j++)
            {
                tile[j] = vram.ReadByte((ushort)(0x8000 + i * 16 + j), AccessSource.Gpu);
            }
            tileData.Add(new Tile(tile));
        }

        return tileData;
    }

    private Tile _tile = new Tile(); 
    public Tile GetBgTile(int x, int y)
    {
        // First we need to take the individual pixel coodinates, and convert them to 8x8 tile coordinates
        var tileX = x / 8;
        var tileY = y / 8;
        
        // Next we need to find the tile number
        var tileId = vram.ReadByte((ushort)(0x9800 + tileY * 32 + tileX), AccessSource.Gpu);
        
        var tileData = new byte[16];
        for (var i = 0; i < 16; i++)
        {
            tileData[i] = vram.ReadByte((ushort)(0x8000 + tileId * 16 + i), AccessSource.Gpu);
        }
        
        _tile.SetData(tileData);
        return _tile;
    }
    public Tile? GetOamTile(int x, int y)
    {
        for (var i = 0; i < 40; i++)
        {
            var oamTileBase = (ushort)(0xFE00 + i * 4);
            
            var oamX = oam.ReadByte((ushort)(oamTileBase + 1), AccessSource.Gpu);
            var oamY = oam.ReadByte((ushort)(oamTileBase), AccessSource.Gpu);
            
            if (oamX == x && oamY == y)
            {
                var tileId = oam.ReadByte((ushort)(oamTileBase + 2), AccessSource.Gpu);
                
                var tileData = new byte[16];
                for (var t = 0; i < 16; i++)
                {
                    tileData[t] = vram.ReadByte((ushort)(0x8000 + tileId * 16 + i), AccessSource.Gpu);
                }
        
                _tile.SetData(tileData);

                return _tile;
            }
        }
        
        return null;
    }
    
    private void DrawBgPx(int x, int y)
    {
        var scy = lcdControl.ReadByte(0xFF42);
        var scx = lcdControl.ReadByte(0xFF43);
        
        var nX = (x + scx) % 256;
        var nY = (y + scy) % 256;
        
        var tile = GetBgTile(nX, nY);
        
        _frameBuffer[x + y * ScreenWidth] = tile.GetPixel(nX % 8, nY % 8);
    }
    
    private void DrawOamPx(int x, int y)
    {
        var tile = GetOamTile(x, y);
        
        if (tile is not null)
        {
            var color = tile.GetPixel(x % 8, y % 8);
            if (color != 0)
            {
                _frameBuffer[x + y * ScreenWidth] = color;
            }
        }
    }
}