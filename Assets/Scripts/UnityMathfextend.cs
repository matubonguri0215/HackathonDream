using UnityEngine;

public static class UnityMathfExtend
{
    

    // ======================
    // Šî–{‰‰ŽZ
    // ======================
    public static float Abs(this float value) => Mathf.Abs(value);
    public static int Abs(this int value) => Mathf.Abs(value);

    public static float Min(this float value, float max) => Mathf.Min(value, max);
    public static int Min(this int value, int max) => Mathf.Min(value, max);
    public static float Min(this float[] values) => Mathf.Min(values);
    public static int Min(this int[] values) => Mathf.Min(values);

    public static float Max(this float value, float min) => Mathf.Max(value, min);
    public static int Max(this int value, int min) => Mathf.Max(value, min);
    public static float Max(this float[] values) => Mathf.Max(values);
    public static int Max(this int[] values) => Mathf.Max(values);

    public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
    public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);
    public static float Clamp01(this float value) => Mathf.Clamp01(value);

    // ======================
    // •âŠÔ
    // ======================
    public static float Lerp(this float a, float b, float t) => Mathf.Lerp(a, b, t);
    public static float LerpAngle(this float a, float b, float t) => Mathf.LerpAngle(a, b, t);
    public static float SmoothStep(this float from, float to, float t) => Mathf.SmoothStep(from, to, t);
    public static float InverseLerp(this float a, float b, float value) => Mathf.InverseLerp(a, b, value);

    // ======================
    // Šp“x
    // ======================
    public static float DeltaAngle(this float current, float target) => Mathf.DeltaAngle(current, target);
    public static float MoveTowardsAngle(this float current, float target, float maxDelta) => Mathf.MoveTowardsAngle(current, target, maxDelta);

    // ======================
    // ŽOŠpŠÖ”
    // ======================
    public static float Sin(this float f) => Mathf.Sin(f);
    public static float Cos(this float f) => Mathf.Cos(f);
    public static float Tan(this float f) => Mathf.Tan(f);
    public static float Asin(this float f) => Mathf.Asin(f);
    public static float Acos(this float f) => Mathf.Acos(f);
    public static float Atan(this float f) => Mathf.Atan(f);
    public static float Atan2(this float y, float x) => Mathf.Atan2(y, x);

    // ======================
    // ”ŠwŠÖ”
    // ======================
    public static float Pow(this float f, float p) => Mathf.Pow(f, p);
    public static float Sqrt(this float f) => Mathf.Sqrt(f);
    public static float Sign(this float f) => Mathf.Sign(f);
    public static float Exp(this float f) => Mathf.Exp(f);
    public static float Log(this float f) => Mathf.Log(f);
    public static float Log(this float f, float p) => Mathf.Log(f, p);
    public static float Log10(this float f) => Mathf.Log10(f);

    // ======================
    // ŠÛ‚ßˆ—
    // ======================
    public static float Round(this float f) => Mathf.Round(f);
    public static int RoundToInt(this float f) => Mathf.RoundToInt(f);
    public static int FloorToInt(this float f) => Mathf.FloorToInt(f);
    public static int CeilToInt(this float f) => Mathf.CeilToInt(f);
    public static float Floor(this float f) => Mathf.Floor(f);
    public static float Ceil(this float f) => Mathf.Ceil(f);

    // ======================
    // ƒ‹[ƒvŒn
    // ======================
    public static float Repeat(this float t, float length) => Mathf.Repeat(t, length);
    public static float PingPong(this float t, float length) => Mathf.PingPong(t, length);

    // ======================
    // ˆÚ“®Œn
    // ======================
    public static float MoveTowards(this float current, float target, float maxDelta) => Mathf.MoveTowards(current, target, maxDelta);
    public static float SmoothDamp(this float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
        => Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);

    // ======================
    // ”äŠr
    // ======================
    public static bool Approximately(this float a, float b) => Mathf.Approximately(a, b);

    // ======================
    // ‚»‚Ì‘¼
    // ======================
    public static float Gamma(this float value, float absmax, float gamma) => Mathf.Gamma(value, absmax, gamma);
    public static float PerlinNoise(float x, float y) => Mathf.PerlinNoise(x, y);
    public static float PerlinNoise(this Vector2 pos) => Mathf.PerlinNoise(pos.x, pos.y);
}
