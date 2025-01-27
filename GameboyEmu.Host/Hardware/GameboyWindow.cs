﻿using System.Numerics;
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
    
    private const int OamDataWidth = 5 * 8;
    private const int OamDataHeight = 8 * 8;

    private const bool ShowTileData = false;
    private const bool ShowOamData = true;
    
    public static void Open(Cpu.Cpu cpu, Gpu gpu, CancellationToken token)
    {
        var width = ScreenWidth * Scaling 
                    + (ShowTileData ? TileDataWidth * Scaling : 0)
                    + (ShowOamData ? OamDataWidth * Scaling : 0);
        var height = MathHelpers.Maximum(
            ScreenHeight * Scaling, 
            ShowTileData ? TileDataHeight * Scaling : 0,
            ShowOamData ? OamDataHeight * Scaling : 0
        );
        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        Raylib.InitWindow(width, height, GetTitle(cpu));
        
        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose() || token.IsCancellationRequested)
        {
            
            Raylib.SetWindowTitle(GetTitle(cpu));
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            

            DrawScreen(gpu, new Vector2(0,0));
            if (ShowTileData)
            {
                DrawTileData(gpu, new Vector2(-ScreenWidth * Scaling, 0));
            }
            
            if (ShowOamData)
            {
                var tileDataWidth = ShowTileData ? TileDataWidth : 0;
                var position = new Vector2(-ScreenWidth * Scaling - tileDataWidth, 0);
                
                DrawOamData(gpu, position);
            }

            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
    }
    
    private static string GetTitle(Cpu.Cpu cpu) => "Gameboy Emulator";
    
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
    
    private static void DrawOamData(Gpu gpu, Vector2 position)
    {
        var oamData = gpu.GetOamData().ToArray();
        var oamWidth = 8;
        var oamHeight = 8;
        var oamCount = 40;
        
        var pixelBuffer = new byte[oamWidth * oamHeight * oamCount];
        for (var i = 0; i < pixelBuffer.Length; i++)
        {
            var oamIndex = i / (oamWidth * oamHeight);
            var oam = oamData[oamIndex];
            var x = i % oamWidth;
            var y = i / oamWidth % oamHeight;
            
            var pixel = oam.GetPixel(x, y);
            pixelBuffer[i] = pixel;
        }
        
        var texture = MakeTextureForBuffer(pixelBuffer, OamDataWidth, OamDataHeight);
        
        var srcRec = new Rectangle(0, 0, texture.Width, texture.Height);
        var dstRec = new Rectangle(0, 0, OamDataWidth * Scaling, OamDataHeight * Scaling);
        Raylib.DrawTexturePro(texture, srcRec, dstRec, position, 0, Color.White);
    }
    
    private static unsafe Texture2D MakeTextureForBuffer(byte[] pixelBuffer, int width, int height)
    {
        var colors = (Color *)Raylib.MemAlloc((uint)(width * height * sizeof(Color)));
        
        for (var i = 0; i < pixelBuffer.Length; i++)
        {
            var b = pixelBuffer[i];
            colors[i] = _colors[b];
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

    private static Dictionary<byte, Color> _colors = new(){
        [0] = new Color(155,188,15, 255),
        [85] = new Color(139,172,15, 255),
        [170] = new Color(48,98,48, 255),
        [255] = new Color(15,56,15, 255)
    };
}

public static class MathHelpers
{
    public static T Maximum<T>(params T[] args) where T : IComparable<T>
    {
        return args.Max()!;
    }
}