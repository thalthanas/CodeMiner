namespace SK.GeolocatorWebGL
{
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class SK_GeolocatorJsLib : MonoBehaviour
{
[DllImport("__Internal")]
private static extern int SK_Geolocator_init(string name, System.Action<byte[], int, byte[], int, byte[], int> onBytes);
[DllImport("__Internal")]
private static extern bool SK_Geolocator_isAvailable();
[DllImport("__Internal")]
private static extern int SK_Geolocator_watch(string options);
[DllImport("__Internal")]
private static extern void SK_Geolocator_clearWatch(int watchId);
[DllImport("__Internal")]
private static extern void SK_Geolocator_getCurrentPosition(string options);

public static void Init(string name)
{
SK_Geolocator_init(name, SK_Geolocator_OnDynamicCall);
}

public static bool IsAvailable()
{
return SK_Geolocator_isAvailable();
}

public static int Watch(string options)
{
return SK_Geolocator_watch(options);
}

public static void ClearWatch(int watchId)
{
SK_Geolocator_clearWatch(watchId);
}

public static void GetCurrentPosition(string options)
{
SK_Geolocator_getCurrentPosition(options);
}


public static UnityEvent<string, byte[]> OnWatchPositionEvent = new UnityEvent<string, byte[]>();
public static UnityEvent<string, byte[]> OnWatchErrorEvent = new UnityEvent<string, byte[]>();
public static UnityEvent<string, byte[]> OnCurrentLocationEvent = new UnityEvent<string, byte[]>();
public static UnityEvent<string, byte[]> OnCurrentLocationErrorEvent = new UnityEvent<string, byte[]>();

      [AOT.MonoPInvokeCallback(typeof(System.Action<byte[], int, byte[], int, byte[], int>))]
      public static void SK_Geolocator_OnDynamicCall(
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 1)] byte[] funcNameBuff, int funcNameLen,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)] byte[] payloadBuff, int payloadLen,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 5)] byte[] buffer, int len)
{
var funcName = System.Text.Encoding.UTF8.GetString(funcNameBuff, 0, funcNameLen - 1);
var payload = System.Text.Encoding.UTF8.GetString(payloadBuff, 0, payloadLen - 1);
if(funcName == "OnWatchPosition")
{
if (OnWatchPositionEvent != null) { OnWatchPositionEvent.Invoke(payload, buffer); }
return;
}

if(funcName == "OnWatchError")
{
if (OnWatchErrorEvent != null) { OnWatchErrorEvent.Invoke(payload, buffer); }
return;
}

if(funcName == "OnCurrentLocation")
{
if (OnCurrentLocationEvent != null) { OnCurrentLocationEvent.Invoke(payload, buffer); }
return;
}

if(funcName == "OnCurrentLocationError")
{
if (OnCurrentLocationErrorEvent != null) { OnCurrentLocationErrorEvent.Invoke(payload, buffer); }
return;
}

}


}

}