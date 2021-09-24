using ExitGames.Client.Photon;
using MelonLoader;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CancerClient
{
	class Patches
	{
		private static HarmonyLib.HarmonyMethod GetPatch(String name)
		{
			return new HarmonyLib.HarmonyMethod(typeof(Patches).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
		}

		internal static void Init()
		{
			MelonLogger.Msg("Creating HarmonyInstance");

			var harmonyInstane = new HarmonyLib.Harmony("PhotonDebug");
			harmonyInstane.Patch(typeof(Photon.Realtime.LoadBalancingClient).GetMethod("OnEvent", BindingFlags.Public | BindingFlags.Instance), GetPatch("OnEvent"));
		}

		private static int xdi = 0;
		private static byte[] xdpacket = null;

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		private static void OnEvent(ExitGames.Client.Photon.EventData param_1)
		{
			switch (param_1.Code)
			{
				case 1:
					UInt32 serverTime = PhotonExtensions.GetServerTimeInMilliseconds();

					Il2CppSystem.Object sendData;

					if (CancerClient.VoiceCrashEnabled)
					{
						if (xdi++ == 20 || xdpacket == null)
						{
							xdi = 0;
							xdpacket = (byte[])Serialization.FromIL2CPPToManaged<object>(param_1.CustomData);
						}

						Array.Copy(BitConverter.GetBytes(serverTime), 0, xdpacket, 4, 4);

						sendData = Serialization.FromManagedToIL2CPP<Il2CppSystem.Object>(xdpacket);
					}
					else if (CancerClient.VoiceMusicEnabled)
					{
						byte[] musicData = VoiceHelpers.GetVoiceData(11, serverTime);

						if (musicData == null)
						{
							return;
						}

						sendData = Serialization.FromManagedToIL2CPP<Il2CppSystem.Object>(musicData);

					}
					else if (CancerClient.VoiceRepeatEnabled)
					{
						byte[] incomingPacketData = (byte[])Serialization.FromIL2CPPToManaged<object>(param_1.CustomData);

						Array.Copy(BitConverter.GetBytes(serverTime), 0, incomingPacketData, 4, 4);

						sendData = Serialization.FromManagedToIL2CPP<Il2CppSystem.Object>(incomingPacketData);

					}
					else if (CancerClient.VoiceRecodeEnabled)
					{
						byte[] incomingPacketData = (byte[])Serialization.FromIL2CPPToManaged<object>(param_1.CustomData);

						byte[] recoded =  VoiceHelpers.RecodeAudioFrame(incomingPacketData);

						if (recoded == null)
						{
							return;
						}

						sendData = Serialization.FromManagedToIL2CPP<Il2CppSystem.Object>(recoded);

						return;
					}
					else 
					{
						return;
					}

					PhotonExtensions.OpRaiseEvent(1, sendData, new Photon.Realtime.RaiseEventOptions()
					{
						field_Public_ReceiverGroup_0 = Photon.Realtime.ReceiverGroup.All,
						field_Public_Byte_0 = 1,
						field_Public_Byte_1 = 1,
					}, SendOptions.SendUnreliable);
					break;
				default:
					//Console.WriteLine(param_1.Code);
					break;
			}
		}
	}
}
