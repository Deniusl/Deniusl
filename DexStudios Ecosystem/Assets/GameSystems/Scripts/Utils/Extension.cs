using System;

public static partial class Extension
{
    public static bool IntToBool(this int number) => number == 1;
    public static int BoolToInt(this bool state) => state ? 1 : 0;
    public static string GetLastSymbol(this string str, int countLastWords) 
        => countLastWords >= str.Length ? str : str.Substring(Math.Max(0, str.Length - countLastWords));

    public static double GetFractionAsInt(this double value)
    {
        var fraction = value - Math.Truncate(value);
        var length = value.ToString().Length - 2; //int 0 and eol symbol
        var multiplier = MathF.Pow(10, length);
        return multiplier * fraction;
    }
}
