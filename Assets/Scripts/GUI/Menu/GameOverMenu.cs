using System;
using GoogleMobileAds.Api;

public class GameOverMenu : Menu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        Buttons[0].OnClick += () =>
        {
            var ad = new InterstitialAd("ca-app-pub-6291991022802883/4758993198");
            var request = new AdRequest.Builder().Build();
            ad.LoadAd(request);

            if (ad.IsLoaded()) ad.Show();

            ad.OnAdClosed += (obj, e) =>
            {
                Game.Instance.ContinueGameWithAd();
                SetMenu(null as Menu);
            };
        };
        Buttons[1].OnClick += () =>
        {
            Game.Instance.RestartGame();
            SetMenu(null as Menu);
        };
        Buttons[2].OnClick += () =>
        {
            Game.Instance.SetDefaults();
            SetMenu<MainMenu>();
        };
    }
}