using System;
using System.Threading;
using System.Threading.Tasks;
// using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameOverMenu : Menu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {

        

    /*    Ad = new InterstitialAd("ca-app-pub-6291991022802883/4758993198");

        Ad.OnAdClosed += (obj, e) =>
        {
            Game.Instance.ContinueGameWithAd();
            SetMenu(null as Menu);
        };
        Ad.OnAdFailedToLoad += (obj, e) =>
        {
            // Buttons[0].gameObject.SetActive(false);

            Buttons[0].GetComponent<TextMeshProUGUI>().enableAutoSizing = true;
            Buttons[0].GetComponent<TextMeshProUGUI>().text = "f " + e.Message;
        };

        Ad.LoadAd(new AdRequest.Builder().AddTestDevice("B6994A4369B1E37C").Build()); 
        
     */

        Buttons[0].OnClick += async() =>
        {

            if (Advertisement.isSupported)
            {
                Advertisement.Initialize("3286329", false);
            }
            else
            {
                Debug.Log("not supported");
            }

            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }

            /*    try
                {
                    Buttons[0].GetComponent<TextMeshProUGUI>().text = "ad isnt lod";

                    while (!Ad.IsLoaded()) await Task.Delay(1000);

                    Buttons[0].GetComponent<TextMeshProUGUI>().text = "ad loded";

                    Ad.Show();

                    Buttons[0].GetComponent<TextMeshProUGUI>().text = "ad showd";
                }
                catch (Exception ex)
                {
                    Buttons[0].GetComponent<TextMeshProUGUI>().text = ex.Message;
                } */
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