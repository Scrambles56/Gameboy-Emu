using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Gpu;

public class OAM() : RestrictedRam(160, 0xFE00);