namespace GameboyEmu.Logic.Gpu;

public class Tile
{
    public byte[] Data { get; }
    
    public Tile()
    {
        Data = new byte[64];
    }

    public Tile(byte[] data)
    {
        Data = new byte[64];
        SetData(data);
    }
    
    public void SetData(byte[] data)
    {
        for (var y = 0; y < 8; y++)
        {
            var low = data[y * 2];
            var high = data[y * 2 + 1];
            
            for (var x = 0; x < 8; x++)
            {
                var bit = 7 - x;
                var lowBit = (low >> bit) & 1;
                var highBit = (high >> bit) & 1;
                var index = y * 8 + x;
                Data[index] = (byte)((highBit << 1) | lowBit);
            }
        }
    }
    
    public byte[][] Data2D
    {
        get
        {
            var data2D = new byte[8][];
            for (var y = 0; y < 8; y++)
            {
                data2D[y] = new byte[8];
                for (var x = 0; x < 8; x++)
                {
                    data2D[y][x] = Data[y * 8 + x];
                }
            }
    
            return data2D;
        }
    }
    
    public byte GetPixel(int x, int y)
    {
        var value = Data[y * 8 + x];

        return value switch
        {
            0 => 0x00,
            1 => 0x55,
            2 => 0xAA,
            3 => 0xFF,
            _ => throw new InvalidOperationException()
        };
    }
    
    public bool IsEmpty => Data.All(b => b == 0);


    public static Tile TestTile { get; } = new([
        0xFF,
        0x81,
        0x81,
        0x81,
        0x81,
        0x81,
        0x81,
        0xFF,
        0xFF,
        0x81,
        0x81,
        0x81,
        0x81,
        0x81,
        0x81,
        0xFF
    ]);
}