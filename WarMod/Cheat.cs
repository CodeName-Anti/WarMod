using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;
using WarMod.Modules;
using CodeStage.AntiCheat.Detectors;

namespace WarMod;

public class Cheat : MonoBehaviour
{
	private ModuleManager moduleManager;
	private List<Window> windows;

	private bool MenuShown = false;

	public KeyCode MenuBind = KeyCode.RightShift;

	public void Awake()
	{
		SpeedHackDetector.StopDetection();
		SpeedHackDetector.Dispose();

		InjectionDetector.StopDetection();
		InjectionDetector.Dispose();

		ObscuredCheatingDetector.StopDetection();
		ObscuredCheatingDetector.Dispose();

		WallHackDetector.StopDetection();
		WallHackDetector.Dispose();

		moduleManager = new();
		windows = new()
		{
			new Window()
			{
				WindowId = (int)WindowIDs.Player,
				Title = "Player",
				WindowRect = new Rect(70, 90, 320, 400),
				RenderAction = RenderWindow
			}
		};
	}

	public void Update()
	{
		HandleInput();
		moduleManager.Update();
	}

	public void FixedUpdate()
	{
		moduleManager.FixedUpdate();
	}

	private void HandleInput()
	{
		if (!Input.anyKey || !Input.anyKeyDown)
			return;

		if (Input.GetKeyDown(MenuBind))
			MenuShown = !MenuShown;
	}

	public void OnGUI()
	{
		if (MenuShown)
			windows.Do(window => window.OnGui());

		moduleManager.OnGUI();
	}

	public void RenderWindow(int windowId)
	{
		moduleManager.Modules[windowId].Do(mod => mod.OnRender());
	}
}
