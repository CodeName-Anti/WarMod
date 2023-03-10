using UnityEngine;

namespace WarMod.Modules;

[RegisterModule]
public class ESPModule : Module
{
	private bool Enabled;

	private static Camera MainCam;

	protected override void Initialize()
	{
		Name = "ESP";
		WindowId = WindowIDs.Player.ToInt();
	}

	public override void OnRender()
	{
		Toggle("ESP", ref Enabled);
	}

	public override void OnGUI()
	{
		if (!Enabled)
			return;

		if (MainCam == null)
			MainCam = Camera.main;

		foreach (CPlayerData player in PlayerControll.Player)
		{
			if (player == null)
				continue;

			if (player.Team == (int)ETeam.Spectator)
				continue;

			if (player.DeadFlag == 1)
				continue;

			if (player.PlayertScript == null)
				continue;

			if (player.PlayertScript._head == null)
				continue;

			// ESP glitches weirdly sometimes without this
			if (IsBehindCamera(player))
				continue;


			DrawESP(player);
		}
	}

	bool IsBehindCamera(CPlayerData player)
	{
		Vector3 cameraPosition = MainCam.transform.position;
		Vector3 objectPosition = player.currPos;
		Vector3 cameraForward = MainCam.transform.forward;

		Vector3 toObject = objectPosition - cameraPosition;

		if (Vector3.Dot(toObject, cameraForward) < 0f)
		{
			return true;
		}

		return false;
	}

	private void DrawESP(CPlayerData player)
	{
		Vector3 w2s_head = MainCam.WorldToScreenPoint(player.PlayertScript._head.transform.position);
		Vector3 w2s_foot = MainCam.WorldToScreenPoint(player.currPos - new Vector3(0, 1, 0));

		if (w2s_head.z < 0 || w2s_foot.z < 0)
			return;

		float height = w2s_foot.y - w2s_head.y;

		Vector2 topLeft = new(w2s_head.x - (height / 4), Screen.height - w2s_head.y);
		Vector2 bottomRight = new(w2s_foot.x + (height / 4), Screen.height - w2s_foot.y);

		CalculateRect(topLeft, bottomRight, out Vector2 topRight, out Vector2 bottomLeft);
		DrawRect(topLeft, topRight, bottomLeft, bottomRight, 1, FirstPersonPlayer.Team == player.Team ? Color.green : Color.red);

		Vector2 bottomMiddle = new(bottomLeft.x + (bottomRight.x - bottomLeft.x), bottomLeft.y);

		DrawLine(new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2), bottomMiddle, 1, Color.green);
	}

	private static Texture2D lineTex;
	private static void DrawLine(Vector2 pointA, Vector2 pointB, float width, Color color)
	{
		Matrix4x4 matrix = GUI.matrix;
		if (!lineTex)
			lineTex = new Texture2D(1, 1);

		Color color2 = GUI.color;
		GUI.color = color;
		float num = Vector3.Angle(pointB - pointA, Vector2.right);

		if (pointA.y > pointB.y)
			num = -num;

		GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
		GUIUtility.RotateAroundPivot(num, pointA);
		GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), lineTex);
		GUI.matrix = matrix;
		GUI.color = color2;
	}

	private static void CalculateRect(Vector2 topLeft, Vector2 bottomRight, out Vector2 topRight, out Vector2 bottomLeft)
	{
		topRight = new(bottomRight.x, topLeft.y);
		bottomLeft = new(topLeft.x, bottomRight.y);
	}

	private static void DrawRect(Vector2 topLeft, Vector2 bottomRight, float width, Color color)
	{
		CalculateRect(topLeft, bottomRight, out Vector2 topRight, out Vector2 bottomLeft);
		DrawRect(topLeft, topRight, bottomLeft, bottomRight, width, color);
	}

	private static void DrawRect(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight, float width, Color color)
	{
		DrawLine(topLeft, topRight, width, color);
		DrawLine(topRight, bottomRight, width, color);
		DrawLine(bottomRight, bottomLeft, width, color);
		DrawLine(bottomLeft, topLeft, width, color);
	}

}
