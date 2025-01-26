using Raylib_cs;

namespace GameboyEmu.Windowing;

public class Controller
{
    public Dictionary<GbButton, KeyboardKey> ButtonMappings { get; } = new()
    {
        { GbButton.Up, KeyboardKey.Up },
        { GbButton.Down, KeyboardKey.Down },
        { GbButton.Left, KeyboardKey.Left },
        { GbButton.Right, KeyboardKey.Right },
        { GbButton.A, KeyboardKey.Z },
        { GbButton.B, KeyboardKey.X },
        { GbButton.Start, KeyboardKey.Enter },
        { GbButton.Select, KeyboardKey.RightShift }
    };
    
    private List<GbButton> GetPressedButtons()
    {
        return ButtonMappings.Keys
            .Where(button => Raylib.IsKeyDown(ButtonMappings[button]))
            .ToList();
    }
    
    public bool IsKeyDown(GbButton button) => Raylib.IsKeyDown(ButtonMappings[button]);
}

public enum GbButton
{
    Up,
    Down,
    Left,
    Right,
    A,
    B,
    Start,
    Select
}