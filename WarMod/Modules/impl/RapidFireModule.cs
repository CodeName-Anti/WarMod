using WarMod.Patches;

namespace WarMod.Modules;

[RegisterModule]
public class RapidFireModule : Module
{

	private bool Enabled;

	protected override void Initialize()
	{
		Name = "RapidFire";
		WindowId = WindowIDs.Player.ToInt();
	}

	public override void OnRender()
	{
		if (Toggle("Rapid Fire", ref Enabled))
		{
			vp_FPCameraPatch.RapidFire = Enabled;
		}
	}

}
