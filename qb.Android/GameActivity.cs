using Android.Content.PM;
using Android.Views;
using osu.Framework.Android;

namespace qb.Android
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar", MainLauncher = true, ScreenOrientation = ScreenOrientation.FullUser, SupportsPictureInPicture = false, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, HardwareAccelerated = true)]
    public class GameActivity : AndroidGameActivity
    {
        GameAndroid? Game;

        protected override osu.Framework.Game CreateGame() => Game = new GameAndroid();

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window?.AddFlags(WindowManagerFlags.Fullscreen);
            Window?.AddFlags(WindowManagerFlags.KeepScreenOn);
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
}