using System;
using Microsoft.Xna.Framework;
public class MathTool
{
    public static float Lerp(float start, float end, float gap)
    {
        return start + (start - end) * gap;
    }

    public static float RandomRange(float start, float end)
    {
        Random random = new Random();
        return start + (float)random.NextDouble() * (end - start);
    }
}