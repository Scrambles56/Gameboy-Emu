using System.Diagnostics;
using GameboyEmu.Logic.Cpu.Extensions;
using GameboyEmu.Logic.Memory;

namespace GameboyEmu.Logic.Gpu;

using Microsoft.Extensions.Logging;

public class OAM() : RestrictedRam(160, 0xFE00);