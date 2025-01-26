namespace GameboyEmu.Logic.Extensions;

public static class GenericExtenstions
{
    public static bool OneOf<T>(this T value, params T[] values)
    {
        return values.Contains(value);
    }
}