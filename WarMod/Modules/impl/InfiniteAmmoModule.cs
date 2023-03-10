using WarMod.Patches;

namespace WarMod.Modules;

[RegisterModule]
public class InfiniteAmmoModule : Module
{

	private bool Enabled;

	protected override void Initialize()
	{
		Name = "Infinite Ammo";
		WindowId = WindowIDs.Player.ToInt();
	}

	public override void OnRender()
	{
		if (Toggle(Name, ref Enabled))
		{
			FirstPersonPlayerPatch.InfiniteAmmo = Enabled;
		}
	}

}
