using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Threading.Tasks;

namespace CancerClient
{
	class VoiceHelpers
	{
		static private USpeak.USpeakNative uspeakInstance = new USpeak.USpeakNative();
		static public void LoadMp3File(string mp3file)
		{
			Task.Run(() =>
			{
				try
				{
					uspeakInstance.StreamMp3(mp3file);
				}
				catch (System.Exception ex)
				{
					MelonLoader.MelonLogger.Msg($"Failed: {ex}");
				}
			});
		}
		static public byte[] GetVoiceData()
		{
			return uspeakInstance.GetAudioFrame(11, PhotonExtensions.GetServerTimeInMilliseconds());
		}
		static public byte[] GetVoiceData(Int32 actorNr, Int32 serverTicks)
		{
			return uspeakInstance.GetAudioFrame(actorNr, serverTicks);
		}
		static public byte[] RecodeAudioFrame(byte[] dataIn)
		{
			return uspeakInstance.RecodeAudioFrame(dataIn);
		}
	}
}
