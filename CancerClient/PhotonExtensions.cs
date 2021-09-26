using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Runtime.InteropServices;

namespace CancerClient
{
	class PhotonExtensions
	{
		public static Int32 GetServerTimeInMilliseconds()
		{
			return PhotonNetwork.field_Public_Static_LoadBalancingClient_0.prop_LoadBalancingPeer_0.ServerTimeInMilliSeconds;
		}
		public static void OpRaiseEvent(byte code, object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
		{
			Il2CppSystem.Object Object = Serialization.FromManagedToIL2CPP<Il2CppSystem.Object>(customObject);
			OpRaiseEvent(code, Object, RaiseEventOptions, sendOptions);
		}
		public static void OpRaiseEvent(byte code, Il2CppSystem.Object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
			=> PhotonNetwork.Method_Public_Static_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0
			(code,
				customObject,
				RaiseEventOptions,
				sendOptions);
	}
}
