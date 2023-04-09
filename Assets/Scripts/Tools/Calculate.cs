using UnityEngine;

public class Calculate
{
    public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
        float Vxz = Sxz / time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

}
