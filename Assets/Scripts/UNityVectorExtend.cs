using UnityEngine;

public static class UnityVector3Extend
{
    public static Vector3 ToXY(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
    public static Vector3 ToXZ(this Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
    public static Vector3 ToYZ(this Vector3 vector)
    {
        return new Vector3(0, vector.y, vector.z);
    }
}

public static class UnityVector2Extend
{
    public static Vector3 ToXY(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y,0);
    }
    public static Vector3 ToXZ(this Vector2 vector)
    {
        return new Vector3(vector.x, 0,vector.y);
    }
    public static Vector3 ToYZ(this Vector2 vector)
    {
        return new Vector3(0, vector.x,vector.y);
    }
}



