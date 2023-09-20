using GameboyEmu.Logic.Cartridge;

namespace GameboyEmu.Logic.Cpu;

public class AddressBus
{
    private readonly LoadedCartridge _cartridge;

    public AddressBus(LoadedCartridge cartridge)
    {
        _cartridge = cartridge;
    }
    
    public byte ReadByte(ushort address)
    {
        if (address < 0x8000)
        {
            return _cartridge.Read(address);
        }

        throw new NotImplementedException();
    }
}