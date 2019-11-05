using System.Linq;
using System.Threading.Tasks;
using qb.Shared;
using qb.Shared.Input;
using qb.Shared.Mods;
using qb.Unity.Mods;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace qb.Unity
{
    public class UnityMenuManager : MenuManager
    {
        readonly Transform ButtonDesign;

        public UnityMenuManager(Transform buttonDesign, FieldController playerGame, FieldController autoGame, SkinManager skinManager,
            params IInputManager[] inputManagers) : base(playerGame, autoGame, skinManager, inputManagers)
        {
            ButtonDesign = buttonDesign;

            if (Advertisement.isSupported && !Advertisement.isInitialized) Advertisement.Initialize("3286669", false);

            Debug.Log("Unity Ads is " +
                (Advertisement.isSupported ? "" : "not ") + "supported, " +
                (Advertisement.isInitialized ? "" : "not ") + "initialized, " +
                (Advertisement.IsReady() ? "" : "not ") + "ready");

            Mods = Mods.Select(x => x.GetType() == typeof(ModUpsideDown) ? new UnityModUpsideDown() : x).ToArray();
        }

        protected override void SetMenu(IMenu menu)
        {
            base.SetMenu(menu);

            if (menu != null)
                menu.ActiveButtonIndex = menu.ActiveButtonIndex;
        }

        T GetNewButton<T>(TranslatedString text, bool isClickable = true) where T : UnityMenuButton
        {
            var btn = new GameObject(text, typeof(RectTransform), typeof(T)).GetComponent<T>();
            btn.IsClickable = isClickable;
            btn.Text = text;

            return btn;
        }
        T GetNewMenu<T>(TranslatedString name, MenuCreateParameters parameters = MenuCreateParameters.None) where T : UnityMenu
        {
            T menu = new GameObject(name, typeof(RectTransform), typeof(T)).GetComponent<T>();
            menu.IsScrollable = parameters.HasFlag(MenuCreateParameters.Scroll);
            menu.ButtonDesign = ButtonDesign;

            if (parameters.HasFlag(MenuCreateParameters.HalfTransparentBackground))
            {
                var bg = new GameObject(name + "_bg", typeof(RectTransform), typeof(RawImage));

                bg.GetComponent<RawImage>().color = new Color(0, 0, 0, .75f);
                (bg.transform as RectTransform).anchorMax = Vector2.one;
                (bg.transform as RectTransform).anchorMin = Vector2.zero;
                (bg.transform as RectTransform).sizeDelta = Vector2.zero;

                bg.transform.SetParent(menu.transform, false);
                bg.transform.SetSiblingIndex(0);
            }

            return menu;
        }

        protected override IMenuButton GetNewButton(TranslatedString text, bool isClickable = true) =>
            GetNewButton<UnityMenuButton>(text, isClickable);
        protected override IMenu GetNewMenu(TranslatedString name, MenuCreateParameters parameters = MenuCreateParameters.None) =>
            GetNewMenu<UnityMenu>(name, parameters);

        protected override ISelectableMenuButton<T> GetNewSelectableButton<T>(TranslatedString text, T item, bool isClickable = true)
        {
            if (typeof(T) == typeof(Mod))
            {
                var btn = (UnitySelectableMenuButton<T>) (object) GetNewButton<UnitySelectableMenuButtonMod>(text, isClickable);
                btn.SelectableValue = item;

                return btn;
            }

            return GetNewButton<UnitySelectableMenuButton<T>>(text, isClickable);
        }
        protected override ISelectableMenu<T> GetNewSelectableMenu<T>(TranslatedString name, MenuCreateParameters parameters = MenuCreateParameters.None)
        {
            if (typeof(T) == typeof(Mod))
                return (UnitySelectableMenu<T>) (object) GetNewMenu<UnitySelectableMenuMod>(name, parameters);

            return GetNewMenu<UnitySelectableMenu<T>>(name, parameters);
        }

        protected override void QuitGame() => Application.Quit();

        protected override async Task<bool> ShowAd()
        {
            var completion = new TaskCompletionSource<bool>();
            var listener = new UnityAdListener(completion);

            Advertisement.AddListener(listener);

            if (!Advertisement.IsReady())
            {
                int waitTime = 30;
                var states = Lock();

                while (!Advertisement.IsReady())
                {
                    await Task.Delay(100);
                    if (--waitTime < 0) return false;
                }

                Unlock(states);
            }

            Advertisement.Show();
            return await completion.Task;
        }
    }
}