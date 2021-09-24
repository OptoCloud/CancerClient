using MelonLoader;
using UnityEngine;

namespace CancerClient
{
	public class CancerClient : MelonMod
	{
		static public bool VoiceCrashEnabled { get; set; } = false;
		static public bool VoiceMusicEnabled { get; set; } = false;
		static public bool VoiceRepeatEnabled { get; set; } = false;
		static public bool VoiceRecodeEnabled { get; set; } = false;
		public override void OnApplicationStart()
		{
			MelonLogger.Msg("Loading patches...");
			Patches.Init();
			MelonLogger.Msg("Loaded patches!");
			VoiceHelpers.LoadMp3File("D:\\User\\Music\\Deorro - Five Hours (Static Video) [LE7ELS]-K_yBUfMGvzc.mp3");
		}
		public override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				VoiceCrashEnabled = !VoiceCrashEnabled;
				string enabled = VoiceCrashEnabled ? "enabled" : "disabled";
				MelonLogger.Msg($"Voice Crash: { enabled }");
			}
			if (Input.GetKeyDown(KeyCode.F2))
			{
				VoiceMusicEnabled = !VoiceMusicEnabled;
				string enabled = VoiceMusicEnabled ? "enabled" : "disabled";
				MelonLogger.Msg($"Voice Music: { enabled }");
			}
			if (Input.GetKeyDown(KeyCode.F3))
			{
				VoiceRepeatEnabled = !VoiceRepeatEnabled;
				string enabled = VoiceRepeatEnabled ? "enabled" : "disabled";
				MelonLogger.Msg($"Voice Repeat: { enabled }");
			}
			if (Input.GetKeyDown(KeyCode.F4))
			{
				VoiceRecodeEnabled = !VoiceRecodeEnabled;
				string enabled = VoiceRecodeEnabled ? "enabled" : "disabled";
				MelonLogger.Msg($"Voice Recode: { enabled }");
			}
		}
		/*
		public override void OnApplicationLateStart()
		{
			MelonLogger.Msg("ApplicationLateStart");
		}
		public override void OnApplicationQuit()
		{
			MelonLogger.Msg("ApplicationQuit");
		}
		public override void OnFixedUpdate()
		{
		}
		public override void OnGUI()
		{
		}
		public override void OnLateUpdate()
		{
		}
		public override void OnPreferencesLoaded()
		{
			MelonLogger.Msg("PreferencesLoaded");
		}
		public override void OnPreferencesSaved()
		{
			MelonLogger.Msg("PreferencesSaved");
		}
		public override void OnSceneWasInitialized(Int32 buildIndex, String sceneName)
		{
			MelonLogger.Msg("SceneWasInitialized");
		}
		public override void OnSceneWasLoaded(Int32 buildIndex, String sceneName)
		{
			MelonLogger.Msg("SceneWasLoaded");
		}
		public override void OnSceneWasUnloaded(Int32 buildIndex, String sceneName)
		{
			MelonLogger.Msg("SceneWasUnloaded");
		}
		*/
	}
}