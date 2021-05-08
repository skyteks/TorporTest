using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension
{
    public static Vector3 ToVector3XZ(this Vector2 vector2, float y = 0f)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }

    public static Vector3 ToVector3XZ(this Vector2Int vector2Int, float y = 0f)
    {
        return new Vector3(vector2Int.x, y, vector2Int.y);
    }
}
