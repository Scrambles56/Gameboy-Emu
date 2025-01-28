using System.Diagnostics;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cartridge.Carts;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Gpu;
using GameboyEmu.Logic.IOController;
using GameboyEmu.Logic.Memory;
using Microsoft.Extensions.Logging;

namespace GameboyEmu.Logic.Cpu;

public class AddressBus
{
    private readonly LoadedCartridge _cartridge;
    private readonly WorkRAM _lowerWorkRam;
    private readonly WorkRAM _upperWorkRam;
    private readonly HighRam _highRam;
    private readonly IOBus _ioBus;
    private readonly InterruptsController _interruptsController;
    private readonly VRam _vram;
    private readonly OAM _oam;
    private readonly ILogger _logger;

    public AddressBus(
        LoadedCartridge cartridge, 
        WorkRAM lowerWorkRam,
        WorkRAM upperWorkRam, 
        HighRam highRam,
        IOBus ioBus,
        InterruptsController interruptsController,
        VRam vram, 
        OAM oam,
        ILogger logger
    )
    {
        _cartridge = cartridge;
        _lowerWorkRam = lowerWorkRam;
        _upperWorkRam = upperWorkRam;
        _highRam = highRam;
        _ioBus = ioBus;
        _interruptsController = interruptsController;
        _vram = vram;
        _oam = oam;
        _logger = logger;
    }

    public byte ReadByte(ushort address)
    {
        try
        {
            Debug.Assert(!address.IsBetween(0xFEA0, 0xFEFF), "Reading from Unusable Memory");


            if (address.IsBetween(0x0000, 0x7FFF))
            {
                if (address.IsBetween(0x0000, 0x00FF) && _ioBus.ReadByte(0xFF50) == 0x00)
                {
                    return BootRoms.GameboyClassic[address];
                }
                
                return _cartridge.Read(address);
            }

            if (address.IsBetween(0x8000, 0x9FFF))
            {
                return _vram.ReadByte(address, AccessSource.Cpu);
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

            if (address.IsBetween(0xE000, 0xFDFF))
            {
                return ReadByte((ushort)(address - 0x2000));
            }

            if (address.IsBetween(0xFE00, 0xFE9F))
            {
                return _oam.ReadByte(address, AccessSource.Cpu);
            }

            if (address.IsBetween(0xFF00, 0xFF7F))
            {
                return _ioBus.ReadByte(address);
            }
            
            if (address.IsBetween(0xFEA0, 0xFEFF))
            {
                throw new InvalidOperationException("Reading from Unusable Memory");
            }

            if (address.IsBetween(0xFF80, 0xFFFE))
            {
                return _highRam.ReadByte(address);
            }

            if (address == 0xFFFF)
            {
                return _interruptsController.ReadByte(address);
            }

            throw new NotImplementedException($"Not implemented reads for Address: {address:X4}");
        }
        catch (Exception)
        {
            Debugger.Break();
            throw;
        }
    }

    public void WriteByte(ushort address, byte value)
    {
        try
        {
            if (address.IsBetween(0x0000, 0x7FFF))
            {
                _cartridge.Write(address, value);
                return;
            }

            if (address.IsBetween(0x8000, 0x9FFF))
            {
                _vram.WriteByte(address, value, AccessSource.Cpu);
                return;
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

            if (address.IsBetween(0xE000, 0xFDFF))
            {
                WriteByte((ushort)(address - 0x2000), value);
                return;
            }

            if (address.IsBetween(0xFE00, 0xFE9F))
            {
                _oam.WriteByte(address, value, AccessSource.Cpu);
                return;
            }
            
            if (address.IsBetween(0xFEA0, 0xFEFF))
            {
                _logger.LogWarning("Writing to Unusable Memory, Address: {Address:X4}, Value: {Value:X2}", address, value);
                return;
            }

            if (address.IsBetween(0xFF00, 0xFF7F))
            {
                if (address == 0xFF46)
                {
                    PerformDmaTransfer(value);
                    return;
                }
                
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
                _interruptsController.WriteByte(address, value);
                return;
            }

            throw new NotImplementedException($"Not implemented writes for Address: {address:X4}");
        }
        catch (Exception)
        {
            Debugger.Break();
            throw;
        }
    }
    
    private void PerformDmaTransfer(byte value)
    {
        var startAddress = (ushort)(value << 8);
        for (var i = 0; i < 0xA0; i++)
        {
            var byteValue = ReadByte((ushort)(startAddress + i));
            var address = (ushort)(0xFE00 + i);
            _oam.WriteByte(address, byteValue, AccessSource.Cpu);
        }
    }
}