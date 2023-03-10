namespace WarMod;

public enum WindowIDs
{
	Main = 0
}

public static class WindowIDsExtension
{
	public static int ToInt(this WindowIDs windowId) { return (int)windowId; }
}