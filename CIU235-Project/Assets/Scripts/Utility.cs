using UnityEngine;

public static class Utility
{
    // Overloaded methods for getting grid positions, based on a specific grid size
    public static Vector3 GetGridPos(Vector3 pos, float grid_size)
    {
        return GetGridPos(pos.x, pos.y, pos.z, grid_size);
    }
    public static Vector3 GetGridPos(float x, float y, float z, float grid_size)
    {
        return new Vector3(Mathf.Round(x / grid_size) * grid_size, Mathf.Round(y / grid_size) * grid_size, Mathf.Round(z / grid_size) * grid_size);
    }
}
