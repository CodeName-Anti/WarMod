using HarmonyLib;
using System;
using System.Runtime.InteropServices;

namespace WarMod.Patches;

[HarmonyPatch(typeof(vp_FPCamera))]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Game is retarded")]
public static class vp_FPCameraPatch
{

	public static bool NoRecoil;
	public static bool RapidFire;

	[DllImport("GameAssembly", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	public static extern void il2cpp_raise_exception(IntPtr exception);

	[HarmonyPatch(nameof(vp_FPCamera.AddRecoilForce))]
	[HarmonyPrefix]
	public static bool AddRecoilForce()
	{

		if (RapidFire)
		{
			// very hacky solution, but it works
			il2cpp_raise_exception(new Il2CppSystem.Exception().Pointer);
		}

		return !NoRecoil;
	}

}
