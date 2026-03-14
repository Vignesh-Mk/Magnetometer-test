using System.Runtime.InteropServices;
using UnityEngine;

public static class MagnetometerIOS
{
#if UNITY_IOS && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void StartMagnetometer();

    [DllImport("__Internal")]
    private static extern void GetMagnetometer(out float x, out float y, out float z);

    [DllImport("__Internal")]
    private static extern void StopMagnetometer();

#endif

    public static void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        StartMagnetometer();
#endif
    }

    public static Vector3 GetField()
    {
#if UNITY_IOS && !UNITY_EDITOR
        float x, y, z;
        GetMagnetometer(out x, out y, out z);
        return new Vector3(x, y, z);
#else
        return Vector3.zero;
#endif
    }

    public static void Stop()
    {
#if UNITY_IOS && !UNITY_EDITOR
        StopMagnetometer();
#endif
    }
}