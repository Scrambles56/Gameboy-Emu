using GameboyEmu.Logic.IOController;

namespace GameboyEmu.Logic.Gpu;

using Cpu;

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
    
    private const bool DrawGrid = false;

    public byte[] FrameBuffer { get; } = new byte[ScreenWidth * ScreenHeight];

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

    public class OamDataTile
    {
        public byte Y { get; private set; }
        public byte X { get; private set; }
        public byte TileId { get; private set; }
        public byte Flags { get; private set; }
        
        public void SetData(byte y, byte x, byte tileId, byte flags)
        {
            Y = y;
            X = x;
            TileId = tileId;
            Flags = flags;
        }
        
        public void SetDataFromVRam(ushort oamTileBase, VRam vram)
        {
            Y = vram.ReadByte(oamTileBase, AccessSource.Gpu);
            X = vram.ReadByte((ushort)(oamTileBase + 1), AccessSource.Gpu);
            TileId = vram.ReadByte((ushort)(oamTileBase + 2), AccessSource.Gpu);
            Flags = vram.ReadByte((ushort)(oamTileBase + 3), AccessSource.Gpu);
        }
    };

    private static readonly OamDataTile[] OamDataTiles = new OamDataTile[40];
    public IEnumerable<OamDataTile> GetOamData()
    {
        for (var i = 0; i < 40; i++)
        {
            var oamTileBase = (ushort)(0xFE00 + i * 4);
            
            var y = oam.ReadByte(oamTileBase, AccessSource.Gpu);
            var x = oam.ReadByte((ushort)(oamTileBase + 1), AccessSource.Gpu);
            var tileId = oam.ReadByte((ushort)(oamTileBase + 2), AccessSource.Gpu);
            var flags = oam.ReadByte((ushort)(oamTileBase + 3), AccessSource.Gpu);
            
            SetOamData(i, y, x, tileId, flags);
        }

        return OamDataTiles;
        
        void SetOamData(int idx, byte y, byte x, byte tileId, byte flags)
        {
            OamDataTiles[idx] ??= new OamDataTile();
            OamDataTiles[idx].SetData(y, x, tileId, flags);
        }
    }

    private static readonly Tile[] TileDataTiles = new Tile[384];
    private static readonly byte[] TileDataPxData = new byte[16];

    public IEnumerable<Tile> GetTileData()
    {
        for (var i = 0; i < 384; i++)
        {
            for (var j = 0; j < 16; j++)
            {
                TileDataPxData[j] = vram.ReadByte((ushort)(0x8000 + i * 16 + j), AccessSource.Gpu);
            }

            SetTileData(i, TileDataPxData);
        }

        return TileDataTiles;

        void SetTileData(int idx, byte[] data)
        {
            TileDataTiles[idx] ??= new Tile(data);
            TileDataTiles[idx].SetData(data);
        }
    }

    private readonly Tile _bgTile = new Tile();
    private readonly byte[] _bgTileData = new byte[16];

    public Tile GetBgTile(int x, int y)
    {
        // First we need to take the individual pixel coodinates, and convert them to 8x8 tile coordinates
        var tileX = x / 8;
        var tileY = y / 8;

        // Next we need to find the tile number
        var tileId = vram.ReadByte((ushort)(0x9800 + tileY * 32 + tileX), AccessSource.Gpu);

        return GetTile(tileId);
    }

    private Tile GetTile(byte tileId)
    {
        for (var i = 0; i < 16; i++)
        {
            _bgTileData[i] = vram.ReadByte((ushort)(0x8000 + tileId * 16 + i), AccessSource.Gpu);
        }

        _bgTile.SetData(_bgTileData);
        return _bgTile;
    }

    private static readonly OamDataTile OamData = new();
    public Tile? GetOamTile(int x, int y)
    {
        for (var oamIndex = 0; oamIndex < 40; oamIndex++)
        {   
            var oamTileBase = (ushort)(0xFE00 + oamIndex * 4);
            
            OamData.SetData(
                oam.ReadByte(oamTileBase, AccessSource.Gpu), 
                oam.ReadByte((ushort)(oamTileBase + 1), AccessSource.Gpu), 
                oam.ReadByte((ushort)(oamTileBase + 2), AccessSource.Gpu), 
                oam.ReadByte((ushort)(oamTileBase + 3), AccessSource.Gpu)
            );
            
            if (OamData.Y == 0 || OamData.X == 0)
            {
                continue;
            }
            
            var pxXToOam = x / 8 * 8;
            var pxYToOam = y / 8 * 8;

            const int oamXOffset = 8;
            const int oamYOffset = 16;
            
            
            if (pxXToOam == OamData.X - oamXOffset && pxYToOam == OamData.Y - oamYOffset)
            {
                return GetTile(OamData.TileId);
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
        
        var px = tile.GetPixel(nX % 8, nY % 8);
        
        if (DrawGrid && (nX % 8 == 0 || nY % 8 == 0))
        {
            px = 0xFF;
        }

        FrameBuffer[x + y * ScreenWidth] = px;
    }

    private void DrawOamPx(int x, int y)
    {
        var tile = GetOamTile(x, y);

        if (tile is not null)
        {
            var color = tile.GetPixel(x % 8, y % 8);
            if (color != 0)
            {
                FrameBuffer[x + y * ScreenWidth] = color;
            }
        }
    }
}