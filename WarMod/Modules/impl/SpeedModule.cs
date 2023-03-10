namespace WarMod.Modules;

[RegisterModule]
public class SpeedModule : Module
{
	private bool SpeedEnabled;
	private float SpeedValue = 0.2f;

	protected override void Initialize()
	{
		Name = "Speed";
		WindowId = WindowIDs.Player.ToInt();
	}

	public override void OnRender()
	{
		if (Toggle("Speed", ref SpeedEnabled))
		{
			if (!SpeedEnabled)
			{
				vp_FPController.cs.MotorAcceleration = 0.2f;
			}
		}

		HorizontalSlider("Speed Amount", 0.1f, 1, ref SpeedValue);
	}

	public override void Update()
	{
		if (SpeedEnabled)
			vp_FPController.cs.MotorAcceleration = SpeedValue;
	}

}
