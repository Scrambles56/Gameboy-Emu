using System.Numerics;
using GameboyEmu.Logic.Gpu;
using Raylib_cs;

namespace GameboyEmu.Windowing;

public static class GameboyWindow
{
    private const int ScreenWidth = 160;
    private const int ScreenHeight = 144;
    private const int Scaling = 5;
    
    private const int TileDataWidth = 16 * 8;
    private const int TileDataHeight = 24 * 8;
    // private const int TileDataWidth = 1 * 8;
    // private const int TileDataHeight = 1 * 8;
    
    public static unsafe void Open(Cpu.Cpu cpu, Gpu gpu)
    {
        var width = ScreenWidth * Scaling + TileDataWidth * Scaling;
        var height = Math.Max(ScreenHeight * Scaling, TileDataHeight * Scaling);
        Raylib.InitWindow(width, height, GetTitle(cpu));
        
        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose())
        {
            
            Raylib.SetWindowTitle(GetTitle(cpu));
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            DrawScreen(gpu, new Vector2(0,0));
            DrawTileData(gpu, new Vector2(-160 * Scaling, 0));

            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
    }
    
    private static string GetTitle(Cpu.Cpu cpu) => "Gameboy Emulator";

    private static void DrawTestTile(Vector2 position)
    {
        var tile = Tile.TestTile;
        var tileWidth = 8;
        var tileHeight = 8;
        
        var pixelBuffer = new byte[tileWidth * tileHeight];
        for (var i = 0; i < pixelBuffer.Length; i++)
        {
            var x = i % tileWidth;
            var y = i / tileWidth;
            
            var pixel = tile.GetPixel(x, y);
            pixelBuffer[i] = pixel;
        }
        
        var texture = MakeTextureForBuffer(pixelBuffer, tileWidth, tileHeight);
        
        var srcRec = new Rectangle(0, 0, texture.Width, texture.Height);
        var dstRec = new Rectangle(0, 0, tileWidth * Scaling, tileHeight * Scaling);
        Raylib.DrawTexturePro(texture, srcRec, dstRec, position, 0, Color.White);
    }
    
    private static void DrawScreen(Gpu gpu, Vector2 position)
    {
        var texture = MakeTextureForBuffer(gpu.FrameBuffer, ScreenWidth, ScreenHeight);

        var srcRec = new Rectangle(0, 0, texture.Width, texture.Height);
        var dstRec = new Rectangle(0, 0, ScreenWidth * Scaling, ScreenHeight * Scaling);
        Raylib.DrawTexturePro(texture, srcRec, dstRec, position, 0, Color.White);
    }

    private static void DrawTileData(Gpu gpu, Vector2 position)
    {
        var tiles = gpu.GetTileData().ToArray();
        var tileWidth = 8;
        var tileHeight = 8;
        var tileCount = 384;
        
        var pixelBuffer = new byte[tileWidth * tileHeight * tileCount];
        for (var i = 0; i < pixelBuffer.Length; i++)
        {
            var tileIndex = i / (tileWidth * tileHeight);
            var tile = tiles[tileIndex];
            var x = i % tileWidth;
            var y = i / tileWidth % tileHeight;
            
            var pixel = tile.GetPixel(x, y);
            pixelBuffer[i] = pixel;
        }
        
        
        var texture = MakeTextureForBuffer(pixelBuffer, TileDataWidth, TileDataHeight);
        
        var srcRec = new Rectangle(0, 0, texture.Width, texture.Height);
        var dstRec = new Rectangle(0, 0, TileDataWidth * Scaling, TileDataHeight * Scaling);
        Raylib.DrawTexturePro(texture, srcRec, dstRec, position, 0, Color.White);
    }
    
    private static unsafe Texture2D MakeTextureForBuffer(byte[] pixelBuffer, int width, int height)
    {
        var colors = (Color *)Raylib.MemAlloc((uint)(width * height * sizeof(Color)));
        
        for (var i = 0; i < pixelBuffer.Length; i++)
        {
            var b = pixelBuffer[i];
            colors[i] = new Color(b,b,b,(byte)255);
        }
                
        var image = new Image
        {
            Data = colors,
            Format = PixelFormat.UncompressedR8G8B8A8,
            Width = width,
            Height = height,
            Mipmaps = 1
        };
        var texture = Raylib.LoadTextureFromImage(image);
        Raylib.MemFree(colors);
        
        return texture;
    }
}