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

        if (timingX == 0)
        {
            lcdControl.WriteByte(0xFF44, (byte)timingY);
            var lyc = lcdControl.ReadByte(0xFF45);
            lcdControl.WriteLcdStatusFlag(LcdStatusFlag.LycEq, timingY == lyc);
        }


        if (timingY >= 144)
        {
            if (timingY == 144 && timingX == 0)
            {
                interruptsController.RequestInterrupt(Interrupt.VBlank);
                
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeHighBit, false);
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeLowBit, true);
            }
        }
        else if (timingX < 80)
        {
            if (timingX == 0)
            {
                oam.AllowedSources = AccessSource.Gpu;
                
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeHighBit, true);
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeLowBit, false);
            }
            // OAM search time
        }
        else if (timingX < 252)
        {
            if (timingX == 80)
            {
                vram.AllowedSources = AccessSource.Gpu;
                
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeHighBit, true);
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeLowBit, true);
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
                
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeHighBit, false);
                lcdControl.WriteLcdStatusFlag(LcdStatusFlag.PPUModeLowBit, false);
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
        
        public bool MirrorX => (Flags & 0b00100000) != 0;
        public bool MirrorY => (Flags & 0b01000000) != 0;
        
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
            TileDataTiles[idx] ??= new Tile(data, false, false);
            TileDataTiles[idx].SetData(data, false, false);
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

        return GetTile(TileSource.Bg, tileId, false, false);
    }

    private Tile GetTile(TileSource source, byte tileId, bool mirrorX, bool mirrorY)
    {
        for (var i = 0; i < 16; i++)
        {
            ushort baseAddress = 0x8000;
            if (source == TileSource.Bg 
                && !lcdControl.BackgroundAndWindowTileDataSelect 
                && tileId < 128)
            {
                baseAddress = 0x9000;
            }
            
            _bgTileData[i] = vram.ReadByte((ushort)(baseAddress + tileId * 16 + i), AccessSource.Gpu); 
        }

        _bgTile.SetData(_bgTileData, mirrorX, mirrorY);
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

            const int oamXOffset = 8;
            const int oamYOffset = 16;
            
            
            var isInXBounds = (OamData.X - oamXOffset) <= x && x < OamData.X;
            var isInYBounds = (OamData.Y - oamYOffset) <= y && y < (OamData.Y - oamYOffset + 8);
            
            if (isInXBounds && isInYBounds)
            {
                return GetTile(TileSource.Oam, OamData.TileId, OamData.MirrorX, OamData.MirrorY);
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
            var oamX = OamData.X - 8;
            var oamY = OamData.Y - 16;
            
            var nX = x - oamX;
            var nY = y - oamY;
            
            var color = tile.GetPixel(nX, nY);
            if (color != 0)
            {
                FrameBuffer[x + y * ScreenWidth] = color;
            }
        }
    }
    
    public enum TileSource
    {
        Bg,
        Oam
    }
}