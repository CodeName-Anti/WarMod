using UnityEngine;

namespace WarMod.Modules;

[RegisterModule]
public class MassiveKillModule : Module
{

	protected override void Initialize()
	{
		Name = "Massive kill";
		WindowId = WindowIDs.Player.ToInt();
	}

	public override void OnRender()
	{
		if (GUILayout.Button(Name))
		{
			for (int i = 0; i < PlayerControll.Player.Count; i++)
			{
				CPlayerData player = PlayerControll.Player[i];

				if (player == null)
					continue;

				if (player.Team == FirstPersonPlayer.Team)
					continue;

				if (player.Team == (int)ETeam.Spectator)
					continue;

				if (player.DeadFlag == 1)
					continue;

				Vector3 currPos = FirstPersonPlayer.Transform.position;

				NetClient.send_takedamage((byte)i, (byte)1, (byte)1, currPos, player.currPos, 0);
			}
		}
	}

}
