using System.Diagnostics;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.IOController;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Cpu;

public class AddressBus
{
    public bool InterruptsEnabled { get; private set; } = false;
    private readonly LoadedCartridge _cartridge;
    private readonly WorkRAM _lowerWorkRam;
    private readonly WorkRAM _upperWorkRam;
    private readonly HighRam _highRam;
    private readonly IOBus _ioBus;

    public AddressBus(LoadedCartridge cartridge, WorkRAM lowerWorkRam, WorkRAM upperWorkRam, HighRam highRam, IOBus ioBus)
    {
        _cartridge = cartridge;
        _lowerWorkRam = lowerWorkRam;
        _upperWorkRam = upperWorkRam;
        _highRam = highRam;
        _ioBus = ioBus;
    }
    
    public byte ReadByte(ushort address)
    {
        Debug.Assert(!address.IsBetween(0xE000, 0xFDFF), "Reading from ECHO Ram");
        Debug.Assert(!address.IsBetween(0xFEA0, 0xFEFF), "Reading from Unusable Memory");
        
        if (address.IsBetween(0x0000, 0x7FFF))
        {
            return _cartridge.Read(address);
        }
        
        if (address.IsBetween(0x8000, 0x9FFF))
        {
            throw new NotImplementedException($"Not implemented reads for Video RAM, Address: {address:X4}");
        }
        
        if (address.IsBetween(0xA000, 0xBFFF))
        {
            throw new NotImplementedException($"Not implemented reads for External RAM, Address: {address:X4}");
        }
        
        if (address.IsBetween(0xC000, 0xCFFF))
        {
            return _lowerWorkRam.ReadByte(address);
        }
        
        if (address.IsBetween(0xD000, 0xDFFF))
        {
            return _upperWorkRam.ReadByte(address);
        }
        
        if (address.IsBetween(0xFE00, 0xFE9F))
        {
            throw new NotImplementedException($"Not implemented reads for OAM, Address: {address:X4}");
        }
        
        if (address.IsBetween(0xFF00, 0xFF7F))
        {
            return _ioBus.ReadByte(address);
        }
        
        if (address.IsBetween(0xFF80, 0xFFFE))
        {
            return _highRam.ReadByte(address);
        }
        
        if (address == 0xFFFF)
        {
            return InterruptsEnabled ? (byte) 0xFF : (byte) 0x00;
        }

        throw new NotImplementedException($"Not implemented reads for Address: {address:X4}");
    }
    
    public void WriteByte(ushort address, byte value)
    {
        Debug.Assert(address >= 0x8000, "Writing to Cartridge");
        Debug.Assert(!address.IsBetween(0xE000, 0xFDFF), "Writing to ECHO Ram");
        Debug.Assert(!address.IsBetween(0xFEA0, 0xFEFF), "Writing to Unusable Memory");

        if (address.IsBetween(0x8000, 0x9FFF))
        {
            throw new NotImplementedException($"Not implemented writes for Video RAM, Address: {address:X4}");
        }
        
        if (address.IsBetween(0xA000, 0xBFFF))
        {
            throw new NotImplementedException($"Not implemented writes for External RAM, Address: {address:X4}");
        }
        
        if (address.IsBetween(0xC000, 0xCFFF))
        {
            _lowerWorkRam.WriteByte(address, value);
            return;
        }
        
        if (address.IsBetween(0xD000, 0xDFFF))
        {
            _upperWorkRam.WriteByte(address, value);
            return;
        }

        if (address.IsBetween(0xFF00, 0xFF7F))
        {
            _ioBus.WriteByte(address, value);
            return;
        }
        
        if (address.IsBetween(0xFF80, 0xFFFE))
        {
            _highRam.WriteByte(address, value);
            return;
        }
        
        if (address == 0xFFFF)
        {
            InterruptsEnabled = value != 0x00;
            return;
        }

        throw new NotImplementedException($"Not implemented writes for Address: {address:X4}");
    }
   
    
    
}