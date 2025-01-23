using System.Diagnostics;
using GameboyEmu.Logic.Cartridge;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Gpu;
using GameboyEmu.Logic.IOController;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Cpu;

public class AddressBus
{
    public byte EnabledInterrupts { get; set; }
    public bool InterruptMasterEnabledFlag { get; set; } = false;

    private readonly LoadedCartridge _cartridge;
    private readonly WorkRAM _lowerWorkRam;
    private readonly WorkRAM _upperWorkRam;
    private readonly HighRam _highRam;
    private readonly IOBus _ioBus;
    private readonly VRam _vram;
    private readonly OAM _oam;

    public AddressBus(LoadedCartridge cartridge, WorkRAM lowerWorkRam, WorkRAM upperWorkRam, HighRam highRam,
        IOBus ioBus, VRam vram, OAM oam)
    {
        _cartridge = cartridge;
        _lowerWorkRam = lowerWorkRam;
        _upperWorkRam = upperWorkRam;
        _highRam = highRam;
        _ioBus = ioBus;
        _vram = vram;
        _oam = oam;
    }

    public byte ReadByte(ushort address)
    {
        Debug.Assert(!address.IsBetween(0xFEA0, 0xFEFF), "Reading from Unusable Memory");

        if (address.IsBetween(0x0000, 0x00FF))
        {
            return BootRoms.GameboyClassic[address];
        }

        if (address.IsBetween(0x0100, 0x7FFF))
        {
            return _cartridge.Read(address);
        }

        if (address.IsBetween(0x8000, 0x9FFF))
        {
            return _vram.ReadByte(address);
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
            return _oam.ReadByte(address);
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
            var mask = (byte)(InterruptMasterEnabledFlag ? 0xFF : 0x00);
            return (byte)(EnabledInterrupts & mask);
        }

        throw new NotImplementedException($"Not implemented reads for Address: {address:X4}");
    }

    public void WriteByte(ushort address, byte value)
    {
        Debug.Assert(!address.IsBetween(0xFEA0, 0xFEFF), "Writing to Unusable Memory");

        if (address.IsBetween(0x0000, 0x7FFF))
        {
            _cartridge.Write(address, value);
            return;
        }

        if (address.IsBetween(0x8000, 0x9FFF))
        {
            _vram.WriteByte(address, value);
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
            _oam.WriteByte(address, value);
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
            EnabledInterrupts = value;
            return;
        }

        throw new NotImplementedException($"Not implemented writes for Address: {address:X4}");
    }

    public Interrupt? GetRequestedInterrupt()
    {
        if (!InterruptMasterEnabledFlag)
        {
            return null;
        }

        var requests = _ioBus.ReadByte(0xFF0F);
        var enabled = EnabledInterrupts;

        var requested = requests & enabled;

        // Return first enabled interrupt, from smallest bit

        if (requested == 0)
        {
            return null;
        }

        if ((requested & (byte)Interrupt.VBlank) != 0)
        {
            return Interrupt.VBlank;
        }

        if ((requested & (byte)Interrupt.LcdStat) != 0)
        {
            return Interrupt.LcdStat;
        }

        if ((requested & (byte)Interrupt.Timer) != 0)
        {
            return Interrupt.Timer;
        }

        if ((requested & (byte)Interrupt.Serial) != 0)
        {
            return Interrupt.Serial;
        }

        if ((requested & (byte)Interrupt.Joypad) != 0)
        {
            return Interrupt.Joypad;
        }

        Debug.Assert(true, "Unexpected interrupt state");
        return null;
    }

    public void RequestInterrupt(Interrupt interrupt)
    {
        _ioBus.RequestInterrupt(interrupt);
    }

    public void ClearInterrupt(Interrupt interrupt)
    {
        _ioBus.ClearInterrupt(interrupt);
    }
}