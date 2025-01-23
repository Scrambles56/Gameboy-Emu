using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.IOController;

using Microsoft.Extensions.Logging;

public class LcdControl(ILogger logger, bool docMode) : RAM(0x000B, 0xFF40)
{
    public override byte ReadByte(ushort address)
    {
        // Gameboy Doctor
        if (docMode && address == 0xFF44)
        {
            return 0x90;
        }

        return Data[address - LowerBound];
    }

    public override void WriteByte(ushort address, byte value)
    {
        if (!docMode && address == 0xFF40)
        {
            var oldState = new LcdFlags(
                LcdAndPpuEnabled,
                WindowTileMapDisplaySelect,
                WindowDisplayEnable,
                BackgroundAndWindowTileDataSelect,
                BackgroundTileMapDisplaySelect,
                ObjectSize,
                ObjectEnable,
                BackgroundAndWindowEnable
            );

            Data[address - LowerBound] = value;

            var newState = new LcdFlags(
                LcdAndPpuEnabled,
                WindowTileMapDisplaySelect,
                WindowDisplayEnable,
                BackgroundAndWindowTileDataSelect,
                BackgroundTileMapDisplaySelect,
                ObjectSize,
                ObjectEnable,
                BackgroundAndWindowEnable
            );


            newState.LogChangedFlags(oldState, logger);
        }

        Data[address - LowerBound] = value;
    }

    private bool IsBitSet(byte value, int bit) => (value & (1 << bit)) != 0;

    private byte LcdControlByte => ReadByte(0xFF40);

    public bool LcdAndPpuEnabled => IsBitSet(LcdControlByte, 7);
    public bool WindowTileMapDisplaySelect => IsBitSet(LcdControlByte, 6);
    public bool WindowDisplayEnable => IsBitSet(LcdControlByte, 5);
    public bool BackgroundAndWindowTileDataSelect => IsBitSet(LcdControlByte, 4);
    public bool BackgroundTileMapDisplaySelect => IsBitSet(LcdControlByte, 3);
    public bool ObjectSize => IsBitSet(LcdControlByte, 2);
    public bool ObjectEnable => IsBitSet(LcdControlByte, 1);
    public bool BackgroundAndWindowEnable => IsBitSet(LcdControlByte, 0);
}

file record LcdFlags(
    bool LcdAndPpuEnabled,
    bool WindowTileMapDisplaySelect,
    bool WindowDisplayEnable,
    bool BackgroundAndWindowTileDataSelect,
    bool BackgroundTileMapDisplaySelect,
    bool ObjectSize,
    bool ObjectEnable,
    bool BackgroundAndWindowEnable
)
{
    public void LogChangedFlags(LcdFlags oldState, ILogger logger)
    {
        if (LcdAndPpuEnabled != oldState.LcdAndPpuEnabled)
        {
            logger.LogInformation("LcdAndPpuEnabled changed from {OldStateLcdAndPpuEnabled} to {LcdAndPpuEnabled}",
                oldState.LcdAndPpuEnabled, LcdAndPpuEnabled);
        }

        if (WindowTileMapDisplaySelect != oldState.WindowTileMapDisplaySelect)
        {
            logger.LogInformation(
                "WindowTileMapDisplaySelect changed from {OldStateWindowTileMapDisplaySelect} to {WindowTileMapDisplaySelect}",
                oldState.WindowTileMapDisplaySelect, WindowTileMapDisplaySelect);
        }

        if (WindowDisplayEnable != oldState.WindowDisplayEnable)
        {
            logger.LogInformation(
                "WindowDisplayEnable changed from {OldStateWindowDisplayEnable} to {WindowDisplayEnable}",
                oldState.WindowDisplayEnable, WindowDisplayEnable);
        }

        if (BackgroundAndWindowTileDataSelect != oldState.BackgroundAndWindowTileDataSelect)
        {
            logger.LogInformation(
                "BackgroundAndWindowTileDataSelect changed from {OldStateBackgroundAndWindowTileDataSelect} to {BackgroundAndWindowTileDataSelect}",
                oldState.BackgroundAndWindowTileDataSelect, BackgroundAndWindowTileDataSelect);
        }

        if (BackgroundTileMapDisplaySelect != oldState.BackgroundTileMapDisplaySelect)
        {
            logger.LogInformation(
                "BackgroundTileMapDisplaySelect changed from {OldStateBackgroundTileMapDisplaySelect} to {BackgroundTileMapDisplaySelect}",
                oldState.BackgroundTileMapDisplaySelect, BackgroundTileMapDisplaySelect);
        }

        if (ObjectSize != oldState.ObjectSize)
        {
            logger.LogInformation("ObjectSize changed from {OldStateObjectSize} to {ObjectSize}", oldState.ObjectSize,
                ObjectSize);
        }

        if (ObjectEnable != oldState.ObjectEnable)
        {
            logger.LogInformation("ObjectEnable changed from {OldStateObjectEnable} to {ObjectEnable}",
                oldState.ObjectEnable, ObjectEnable);
        }

        if (BackgroundAndWindowEnable != oldState.BackgroundAndWindowEnable)
        {
            logger.LogInformation(
                "BackgroundAndWindowEnable changed from {OldStateBackgroundAndWindowEnable} to {BackgroundAndWindowEnable}",
                oldState.BackgroundAndWindowEnable, BackgroundAndWindowEnable);
        }
    }
};