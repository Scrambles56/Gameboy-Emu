DEFAULTVALUE=1
TESTCASE=${1:-$DEFAULTVALUE}

if [[ "$TESTCASE" -eq 1 ]]; then
  echo "01-special"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/01-special.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 1
elif [[ "$TESTCASE" -eq 2 ]]; then
  echo "02-interrupts"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/02-interrupts.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 2
elif [[ "$TESTCASE" -eq 3 ]]; then
  echo "03-op sp,hl"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/03-op sp,hl.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 3
elif [[ "$TESTCASE" -eq 4 ]]; then
  echo "04-op r,imm"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/04-op r,imm.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 4
elif [[ "$TESTCASE" -eq 5 ]]; then
  echo "05-op rp"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/05-op rp.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 5
elif [[ "$TESTCASE" -eq 6 ]]; then
  echo "06-ld r,r"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/06-ld r,r.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 6
elif [[ "$TESTCASE" -eq 7 ]]; then
  echo "07-jr,jp,call,ret,rst"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/07-jr,jp,call,ret,rst.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 7
elif [[ "$TESTCASE" -eq 8 ]]; then
  echo "08-misc instrs"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/08-misc instrs.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 8
elif [[ "$TESTCASE" -eq 9 ]]; then
  echo "09-op r,r"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/09-op r,r.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 9
elif [[ "$TESTCASE" -eq 10 ]]; then
  echo "10-bit ops"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/10-bit ops.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 10
elif [[ "$TESTCASE" -eq 11 ]]; then
  echo "11-op a,(hl)"
  dotnet GameboyEmu.Host/bin/Debug/net8.0/GameboyEmu.Host.dll --rom "./roms/gb-test-roms-master/cpu_instrs/individual/11-op a,(hl).gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 11
fi