using GameboyEmu.Logic.IOController;
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
    
    private readonly Dictionary<GbButton, bool> _buttonStates = new()
    {
        { GbButton.Up, false },
        { GbButton.Down, false },
        { GbButton.Left, false },
        { GbButton.Right, false },
        { GbButton.A, false },
        { GbButton.B, false },
        { GbButton.Start, false },
        { GbButton.Select, false }
    };
    
    public void Update(InputControl inputControl)
    {
        foreach (var (button, key) in ButtonMappings)
        {
            var previousState = _buttonStates[button];
            var currentState = Raylib.IsKeyDown(key);
            
            if (currentState && !previousState)
            {
                inputControl.PressButton(button);
            }
            else if (!currentState && previousState)
            {
                inputControl.ReleaseButton(button);
            }
            
            _buttonStates[button] = currentState;
        }
    }
    
}