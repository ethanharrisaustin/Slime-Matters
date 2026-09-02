using UnityEngine;

public static class Quateniextras
{
    public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Vector3 refVel, float smoothTime)
    {
        return SmoothDamp(current, target, ref refVel, smoothTime, Mathf.Infinity, Time.deltaTime);
    }

    public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Vector3 refVel, float smoothTime, float maxSpeed)
    {
        return SmoothDamp(current, target, ref refVel, smoothTime, maxSpeed, Time.deltaTime);
    }

    public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Vector3 refVel, float smoothTime, float maxSpeed, float deltaTime)
    {
        Vector3 currentEuler = current.eulerAngles;
        Vector3 targetEuler = target.eulerAngles;

        Vector3 resultAngle = new Vector3(
            Mathf.SmoothDampAngle(currentEuler.x, targetEuler.x, ref refVel.x, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDampAngle(currentEuler.y, targetEuler.y, ref refVel.y, smoothTime, maxSpeed, deltaTime),
            Mathf.SmoothDampAngle(currentEuler.z, targetEuler.z, ref refVel.z, smoothTime, maxSpeed, deltaTime)
        );

        return Quaternion.Euler(resultAngle);
    }
}
