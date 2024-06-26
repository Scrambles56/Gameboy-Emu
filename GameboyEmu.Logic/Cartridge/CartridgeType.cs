﻿namespace GameboyEmu.Logic.Cartridge;

public class CartridgeType
{
    private readonly byte _value;

    public CartridgeType(byte value)
    {
        _value = value;
    }

    public string ByteString() => _value.ToString("X2");
    
    public MbcType MbcType => _value switch
    {
        0x01 or 0x02 or 0x03 => MbcType.Mbc1,
        0x05 or 0x06 => MbcType.Mbc2,
        0x0F or 0x10 or 0x11 or 0x12 or 0x13 => MbcType.Mbc3,
        0x19 or 0x1A or 0x1B => MbcType.Mbc5,
        0x20 => MbcType.Mbc6,
        0x22 => MbcType.Mbc7,
        _ => MbcType.NoMbc
    };

    public override string ToString()
    {
        return _value switch
        {
            0x00 => "ROM ONLY",
            0x01 => "MBC1",
            0x02 => "MBC1+RAM",
            0x03 => "MBC1+RAM+BATTERY",
            0x05 => "MBC2",
            0x06 => "MBC2+BATTERY",
            0x08 => "ROM+RAM 1",
            0x09 => "ROM+RAM+BATTERY 1",
            0x0B => "MMM01",
            0x0C => "MMM01+RAM",
            0x0D => "MMM01+RAM+BATTERY",
            0x0F => "MBC3+TIMER+BATTERY",
            0x10 => "MBC3+TIMER+RAM+BATTERY 2",
            0x11 => "MBC3",
            0x12 => "MBC3+RAM 2",
            0x13 => "MBC3+RAM+BATTERY 2",
            0x19 => "MBC5",
            0x1A => "MBC5+RAM",
            0x1B => "MBC5+RAM+BATTERY",
            0x1C => "MBC5+RUMBLE",
            0x1D => "MBC5+RUMBLE+RAM",
            0x1E => "MBC5+RUMBLE+RAM+BATTERY",
            0x20 => "MBC6",
            0x22 => "MBC7+SENSOR+RUMBLE+RAM+BATTERY",
            0xFC => "POCKET CAMERA",
            0xFD => "BANDAI TAMA5",
            0xFE => "HuC3",
            0xFF => "HuC1+RAM+BATTERY",
            _ => "Unknown"
        };
    }
}

public enum MbcType {
    NoMbc,
    Mbc1,
    Mbc2,
    Mbc3,
    Mbc5,
    Mbc6,
    Mbc7
}