using UnityEngine;

public static class ExternalFuncs
{
    public static Vector3 MultiplyVec(this Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z);
    }

    public static Vector3 DevideVec(this Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
    }
}
