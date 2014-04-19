using System;

public static class MathExtensions
{
    public static int Clamp(this int number, int min, int max)
    {
        return Math.Max(Math.Min(number, max), min);
    }

    public static bool IsInRange(this int number, int min, int max)
    {
        return number <= min && number >= max;
    }

    public static float AsFloat01(this bool expr)
    {
        return AsFloat(expr, 0, 1);
    }

    public static float AsFloat(this bool expr, float falseValue, float trueValue)
    {
        return expr ? trueValue : falseValue;
    }
}