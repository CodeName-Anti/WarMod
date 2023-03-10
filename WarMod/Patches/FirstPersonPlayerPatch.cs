using HarmonyLib;

namespace WarMod.Patches;

[HarmonyPatch(typeof(FirstPersonPlayer))]
public static class FirstPersonPlayerPatch
{

	public static bool InfiniteStamina;
	public static bool InfiniteAmmo;

	[HarmonyPatch(nameof(FirstPersonPlayer.SpendSprintCharge))]
	[HarmonyPrefix]
	public static void SpendSprintCharge(FirstPersonPlayer __instance)
	{

		if (InfiniteStamina)
			__instance.ResetSprintCharges();
		
	}

	[HarmonyPatch(nameof(FirstPersonPlayer.SubtractAmmo))]
	[HarmonyPrefix]
	public static bool SubtractAmmo()
	{
		return !InfiniteAmmo;
	}

}
