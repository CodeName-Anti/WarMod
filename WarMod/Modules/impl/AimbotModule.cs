using UnityEngine;

namespace WarMod.Modules;

[RegisterModule]
public class AimbotModule : Module
{
	private bool Enabled;

	private float HeadYOffset = 0f;

	private LayerMask VisibleLayerMask = LayerMask.GetMask("Default", "Player3rd 21", "Map", "Player 25");
	private Camera MainCam;

	public int targetId;

	protected override void Initialize()
	{
		Name = "Aimbot";

		WindowId = WindowIDs.Player.ToInt();
	}

	public override void OnRender()
	{
		Toggle("Aimbot", ref Enabled);

		GUILayout.BeginHorizontal();
		
		HorizontalSlider("Head Y Offset", -1, 1, ref HeadYOffset);
		if (GUILayout.Button("Reset"))
			HeadYOffset = 0;

		GUILayout.EndHorizontal();
	}

	public override void OnGUI()
	{
		if(!Enabled)
			return;
		
		if (MainCam == null)
			MainCam = Camera.main;

		CPlayerData closestEnemy = GetClosestEnemy();

		if (closestEnemy != null)
		{
			if (targetId != closestEnemy.PlayertScript.Index)
				targetId = closestEnemy.PlayertScript.Index;

			Vector3 headPosition = closestEnemy.PlayertScript._head.transform.position;

			headPosition.y += HeadYOffset;

			AimAt(headPosition);
		}
	}

	private static bool IsVisible(Vector3 from, Vector3 to, LayerMask layerMask)
	{
		if (!Physics.Linecast(from, to, out RaycastHit hit, layerMask))
			return false;

		string name = hit.transform.name.ToLower();
		
		return name.Contains("bip") || name.Contains("player");
	}

	private static Vector2 CalcAngle(Vector3 src, Vector3 dst)
	{
		Vector2 angle;
		Vector3 relative;
		relative = src - dst;
		float magnitude = relative.magnitude;
		float pitch = Mathf.Asin(relative.y / magnitude);
		float yaw = -Mathf.Atan2(relative.x, -relative.z);

		yaw *= Mathf.Rad2Deg;
		pitch *= Mathf.Rad2Deg;

		angle.x = pitch;
		angle.y = yaw;
		return angle;
	}

	private void AimAt(Vector3 target)
	{
		Vector2 aimAngle = CalcAngle(MainCam.transform.position, target);

		vp_FPCamera.cs.Angle = aimAngle;
	}

	private CPlayerData GetClosestEnemy()
	{
		float closestEnemyDistance  = float.MaxValue;
		int bestEnemyIndex = -1;
		
		for (int i = 0; i < PlayerControll.Player.Count; i++)
		{
			CPlayerData currentPlayer = PlayerControll.Player[i];

			if (currentPlayer == null)
				continue;

			if (currentPlayer.Team == FirstPersonPlayer.Team)
				continue;

			if (currentPlayer.Team == (int)ETeam.Spectator)
				continue;

			if (currentPlayer.DeadFlag == 1)
				continue;

			if (currentPlayer.PlayertScript._head == null)
				continue;

			if (!IsVisible(MainCam.transform.position, currentPlayer.PlayertScript._head.transform.position, VisibleLayerMask))
				continue;

			float distance = Vector3.Distance(FirstPersonPlayer.Transform.position, currentPlayer.currPos);

			if (distance < closestEnemyDistance)
			{
				closestEnemyDistance = distance;
				bestEnemyIndex = i;
			}

		}

		if (bestEnemyIndex == -1)
			return null;

		return PlayerControll.Player[bestEnemyIndex];
	}

}
