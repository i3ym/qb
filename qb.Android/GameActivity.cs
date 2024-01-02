using Android.Views;
using osu.Framework.Android;

namespace qb.Android;

[Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true)]
public class GameActivity : AndroidGameActivity
{
    GameAndroid? Game;

    protected override osu.Framework.Game CreateGame() => Game = new GameAndroid();

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        // temporary fix until https://github.com/ppy/osu-framework/pull/6100
        Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));

        base.OnCreate(savedInstanceState);

        Window?.AddFlags(WindowManagerFlags.Fullscreen);
        Window?.AddFlags(WindowManagerFlags.KeepScreenOn);
    }


    public override void OnBackPressed()
    {
        base.OnBackPressed();
        Game?.OnAndroidPause();
    }
    protected override void OnPause()
    {
        base.OnPause();
        Game?.OnAndroidPause();
    }
    protected override void OnResume()
    {
        base.OnResume();
        Game?.OnAndroidResume();
    }
}
