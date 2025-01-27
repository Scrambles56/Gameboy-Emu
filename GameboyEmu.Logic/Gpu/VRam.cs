using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Gpu;

public class VRam() : RestrictedRam(8192, 0x8000);