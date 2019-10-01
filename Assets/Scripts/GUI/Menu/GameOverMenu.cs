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

        if (Advertisement.isSupported) Advertisement.Initialize("3286669", false);
        else Debug.Log("Unity Ads is not supported");

        Buttons[0].OnClick += async() =>
        {
            if (!Advertisement.IsReady())
            {
                IsLocked = true;

                while (!Advertisement.IsReady())
                    await Task.Delay(100);

                IsLocked = false;
            }

            Advertisement.Show();
        };
        Buttons[1].OnClick += () =>
        {
            Buttons[0].gameObject.SetActive(true);
            Buttons[1].gameObject.SetActive(false);

            GameStarter.Game.ContinueGameWithAd();
            SetMenu(null as Menu);
        };
        Buttons[2].OnClick += () =>
        {
            GameStarter.Game.StartGame();
            SetMenu(null as Menu);
        };
        Buttons[3].OnClick += () =>
        {
            GameStarter.Game.SetDefaults();
            SetMenu<MainMenu>();
        };

        Buttons[1].gameObject.SetActive(false);
    }

    public void AdWatched()
    {
        Buttons[0].gameObject.SetActive(false);
        Buttons[1].gameObject.SetActive(true);
        
        ActiveButtonIndex = 1;
        UpdateButtonDesign();
    }

    protected override void OnShow()
    {
        Buttons[0].gameObject.SetActive(!GameStarter.Game.IsAdContinued);

        if (GameStarter.Game.IsAdContinued)
        {
            ActiveButtonIndex = 2;
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
            if (showResult == ShowResult.Finished)
                GetMenu<GameOverMenu>().AdWatched();
        }

        public void OnUnityAdsDidStart(string placementId)
        {

        }

        public void OnUnityAdsReady(string placementId)
        {

        }
    }
}