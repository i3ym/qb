using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameOverMenu : Menu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        Advertisement.AddListener(new UnityAdListener());

        if (Advertisement.isSupported) Advertisement.Initialize("3286329", false);
        else Debug.Log("Unity Ads is not supported");

        Buttons[0].OnClick += async() =>
        {
            while (!Advertisement.IsReady()) await Task.Delay(100);
            Advertisement.Show();
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

    protected override void OnShow()
    {
        Buttons[0].gameObject.SetActive(!Game.Instance.IsAdContinued);

        if (Game.Instance.IsAdContinued)
        {
            ActiveButtonIndex = 1;
            UpdateButtonDesign();
        }
    }

    class UnityAdListener : IUnityAdsListener
    {
        public void OnUnityAdsDidError(string message)
        {

        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            Game.Instance.ContinueGameWithAd();
            SetMenu(null as Menu);
        }

        public void OnUnityAdsDidStart(string placementId)
        {

        }

        public void OnUnityAdsReady(string placementId)
        {

        }
    }
}