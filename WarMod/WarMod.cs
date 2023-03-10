using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace WarMod;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class WarMod : BasePlugin
{
	public static WarMod Instance { get; private set; }

	public static new ManualLogSource Log; 

	public Harmony HarmonyInstance { get; private set; }

	public override void Load()
	{
		Instance = this;
		Log = base.Log;

		// Plugin startup logic
		Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

		HarmonyInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);

		ClassInjector.RegisterTypeInIl2Cpp<Cheat>();

		GameObject obj = new(PluginInfo.PLUGIN_NAME);
		obj.hideFlags |= HideFlags.HideAndDontSave;

		obj.AddComponent<Cheat>();

		Object.DontDestroyOnLoad(obj);
	}
}
