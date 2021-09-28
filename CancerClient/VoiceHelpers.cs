using MelonLoader;
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
				catch (Exception ex)
				{
					MelonLogger.Msg($"Failed: {ex}");
				}
			});
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
