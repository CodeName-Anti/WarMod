using WarMod.Patches;

namespace WarMod.Modules;

[RegisterModule]
public class NoRecoilModule : Module
{

	private bool Enabled;


	protected override void Initialize()
	{
		Name = "No Recoil";
		WindowId = WindowIDs.Main.ToInt();
	}

	public override void OnRender()
	{
		if (Toggle(Name, ref Enabled))
		{
			vp_FPCameraPatch.NoRecoil = Enabled;
		}
	}

}
