using ExitGames.Client.Photon;
using MelonLoader;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

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

			senderThread = new Thread(AudioTransmitFunc);
			senderThread.Start();
		}

		private static void AudioTransmitFunc()
		{
			while (true)
			{
				Thread.Sleep(20);

				if (!CancerClient.VoiceMusicEnabled || !PlayerExtensions.IsInWorld())
					continue;

				var localPlayer = PlayerExtensions.LocalPlayer;
				if (localPlayer == null)
					continue;

				var playerApi = localPlayer.GetVRCPlayerApi();
				if (playerApi == null)
					continue;

				byte[] voiceData = VoiceHelpers.GetVoiceData(playerApi.playerId, PhotonExtensions.GetServerTimeInMilliseconds());

				if (voiceData == null)
					continue;

				PhotonExtensions.OpRaiseEvent(1, voiceData, new Photon.Realtime.RaiseEventOptions()
				{
					field_Public_ReceiverGroup_0 = Photon.Realtime.ReceiverGroup.Others,
					field_Public_EventCaching_0 = Photon.Realtime.EventCaching.DoNotCache
				},
				SendOptions.SendUnreliable);
			}
		}

		private static int xdi = 0;
		private static byte[] xdpacket = null;
		private static Thread senderThread;

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		private static void OnEvent(ExitGames.Client.Photon.EventData param_1)
		{
			switch (param_1.Code)
			{
				case 1:
					Int32 serverTime = PhotonExtensions.GetServerTimeInMilliseconds();

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
					else if (CancerClient.VoiceRepeatEnabled)
					{
						byte[] incomingPacketData = (byte[])Serialization.FromIL2CPPToManaged<object>(param_1.CustomData);

						Array.Copy(BitConverter.GetBytes(serverTime), 0, incomingPacketData, 4, 4);

						sendData = Serialization.FromManagedToIL2CPP<Il2CppSystem.Object>(incomingPacketData);

					}
					else if (CancerClient.VoiceRecodeEnabled)
					{
						VoiceHelpers.RecodeAudioFrame((byte[])Serialization.FromIL2CPPToManaged<object>(param_1.CustomData));
						return;
					}
					else
					{
						return;
					}

					PhotonExtensions.OpRaiseEvent(1, sendData, new Photon.Realtime.RaiseEventOptions()
					{
						field_Public_ReceiverGroup_0 = Photon.Realtime.ReceiverGroup.Others,
						field_Public_EventCaching_0 = Photon.Realtime.EventCaching.DoNotCache
					},
					SendOptions.SendUnreliable);
					break;
				default:
					//Console.WriteLine(param_1.Code);
					break;
			}
		}
	}
}
