using WarMod.Patches;

namespace WarMod.Modules;

[RegisterModule]
public class InfiniteStaminaModule : Module
{

	private bool Enabled;

	protected override void Initialize()
	{
		Name = "Infinite Stamina";
		WindowId = WindowIDs.Main.ToInt();
	}

	public override void OnRender()
	{
		if (Toggle(Name, ref Enabled))
		{
			FirstPersonPlayerPatch.InfiniteStamina = Enabled;
		}
	}

}
