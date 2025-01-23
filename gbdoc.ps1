
$TestCase = $args[0] -as [int]
$UseRelease = $args[1] -as [int]

if ($UseRelease -eq 1)
{
    $ReleaseType = "Release"
}
else
{
    $ReleaseType = "Debug"
}

if ($TestCase -eq 1)
{
    Write-Output "01-special"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/01-special.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 1
}
elseif ($TestCase -eq 2)
{
    Write-Output "02-interrupts"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/02-interrupts.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 2
}
elseif ($TestCase -eq 3)
{
    Write-Output "03-op sp,hl"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/03-op sp,hl.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 3
}
elseif ($TestCase -eq 4)
{
    Write-Output "04-op r,imm"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/04-op r,imm.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 4
}
elseif ($TestCase -eq 5)
{
    Write-Output "05-op rp"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/05-op rp.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 5
}
elseif ($TestCase -eq 6)
{
    Write-Output "06-ld r,r"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/06-ld r,r.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 6
}
elseif ($TestCase -eq 7)
{
    Write-Output "07-jr,jp,call,ret,rst"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/07-jr,jp,call,ret,rst.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 7
}
elseif ($TestCase -eq 8)
{
    Write-Output "08-misc instrs"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/08-misc instrs.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 8
}
elseif ($TestCase -eq 9)
{
    Write-Output "09-op r,r"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/09-op r,r.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 9
}
elseif ($TestCase -eq 10)
{
    Write-Output "10-bit ops"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/10-bit ops.gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 10
}
elseif ($TestCase -eq 11)
{
    Write-Output "11-op a,(hl)"
    dotnet GameboyEmu.Host/bin/${ReleaseType}/net8.0/GameboyEmu.Host.dll --doc true --rom "./roms/gb-test-roms-master/cpu_instrs/individual/11-op a,(hl).gb" | ~/Source/gameboy-doctor/gameboy-doctor - cpu_instrs 11
}
