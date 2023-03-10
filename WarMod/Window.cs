using System;
using UnityEngine;

namespace WarMod;

public class Window
{
	public int WindowId;
	public Rect WindowRect;
	public string Title;

	public bool Scrollbar;
	public Vector2 ScrollPosition = Vector2.zero;

	public Action<int> RenderAction = delegate { };

	public void OnGui()
	{
		WindowRect = GUI.Window(WindowId, WindowRect, (GUI.WindowFunction)RenderWindow, Title);
		WindowRect.x = Mathf.Clamp(WindowRect.x, 0, Screen.currentResolution.width - WindowRect.width);
		WindowRect.y = Mathf.Clamp(WindowRect.y, 0, Screen.currentResolution.height - WindowRect.height);
	}

	public void PositionWindowY(Rect otherWindowRect, float distance)
	{
		WindowRect.y = otherWindowRect.y + otherWindowRect.width + distance;
	}

	private void RenderWindow(int id)
	{
		if (Scrollbar)
			ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);

		RenderAction.Invoke(id);

		if (Scrollbar)
			GUILayout.EndScrollView();

		GUI.DragWindow(new Rect(0, 0, 10000, 20));
	}

}
