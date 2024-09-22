using FluentAssertions;
using GameboyEmu.Logic.Gpu;

namespace GameboyEmu.Logic.Tests;

public class TileTests
{
    [Fact]
    public void TestTileData()
    {
        byte[][] expected =
        {
            new byte[] { 3, 1, 1, 1, 1, 1, 1, 3 },
            new byte[] { 3, 0, 0, 0, 0, 0, 0, 3 },
            new byte[] { 3, 0, 0, 0, 0, 0, 0, 3 },
            new byte[] { 3, 2, 2, 2, 2, 2, 2, 3 },
            new byte[] { 3, 1, 1, 1, 1, 1, 1, 3 },
            new byte[] { 3, 0, 0, 0, 0, 0, 0, 3 },
            new byte[] { 3, 0, 0, 0, 0, 0, 0, 3 },
            new byte[] { 3, 2, 2, 2, 2, 2, 2, 3 }
        };

        Tile.TestTile.Data2D.Should().BeEquivalentTo(expected);
    }
}