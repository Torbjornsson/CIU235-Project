using UnityEngine;

public static class Utility
{
    public const float GRID_SIZE = 1f;

    // Overloaded methods for getting grid positions, based on a specific grid size
    public static Vector3 GetGridPos(Vector3 pos)
    {
        return GetGridPos(pos.x, pos.y, pos.z);
    }
    public static Vector3 GetGridPos(Vector3 pos, float grid_size)
    {
        return GetGridPos(pos.x, pos.y, pos.z, grid_size);
    }
    public static Vector3 GetGridPos(float x, float y, float z)
    {
        return GetGridPos(x, y, z, GRID_SIZE);
    }
    public static Vector3 GetGridPos(float x, float y, float z, float grid_size)
    {
        return new Vector3(Mathf.Round(x / grid_size) * grid_size, Mathf.Round(y / grid_size) * grid_size, Mathf.Round(z / grid_size) * grid_size);
    }

    // Method for applying camera rotation to controls
    public static Vector3 RotateInputVector(float dir_x, float dir_y, float dir_z, CameraControls.Facing camera_facing)
    {
        switch(camera_facing)
        {
            case CameraControls.Facing.NORTH:
                return new Vector3(dir_x, dir_y, dir_z);
            case CameraControls.Facing.EAST:
                return new Vector3(dir_z, dir_y, -dir_x);
            case CameraControls.Facing.SOUTH:
                return new Vector3(-dir_x, dir_y, -dir_z);
            case CameraControls.Facing.WEST:
                return new Vector3(-dir_z, dir_y, dir_x);
            default:
                return new Vector3(0, 0, 0);
        }
    }
}
