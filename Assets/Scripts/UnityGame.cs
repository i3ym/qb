using System.IO;
using System.Linq;
using qb.Shared;
using qb.Shared.Animations;
using qb.Shared.Input;
using UnityEngine;

namespace qb.Unity
{
    public class UnityGame : QBehaviour
    {
        [SerializeField] UnityGameScreen Field = null;
        [SerializeField] MonoBehaviour[] _InputManagers = null;
        [SerializeField] Transform MenuParent = null, MenuButtonDesign = null;

        IInputManager[] InputManagers;
        UnityFieldController PlayerGame, AutoGame;

        protected override void AwakeOverride()
        {
            Config.GetDefaultLanguageFunc = () => Application.systemLanguage == SystemLanguage.English ? Language.English : Language.Russian;
            Config.Load(Path.Combine(Application.persistentDataPath, "config.cfg"));
        }
        void Start()
        {
            InputManagers = _InputManagers.OfType<IInputManager>().ToArray();

            Config.OnSkinChange += (skin) => Field.SkinManager.SelectedSkinIndex = skin;
            Field.SkinManager.SelectedSkinIndex = Config.SelectedSkin;

            PlayerGame = new UnityFieldController(Field, new PlayerControlsProvider(InputManagers),
                new RemoveLinesAnimationSlideDown(), new GameOverFillingAnimation());

            AutoGame = new UnityFieldController(Field, new AutoControlsProvider(),
                new RemoveLinesAnimationSlideDown(), new GameOverFillingAnimation());

            AutoGame.OnGameOver += (_, e) =>
            {
                AutoGame.StartGame();
                e.Cancel = true;
            };
            
            var menuManager = new UnityMenuManager(MenuButtonDesign, PlayerGame, AutoGame, Field.SkinManager, InputManagers);
            menuManager.CreateMenus();

            foreach (var menu in menuManager.Menus.OfType<UnityMenu>())
                menu.transform.SetParent(MenuParent, false);

            Config.SelectedSkin = Config.SelectedSkin;
        }
    }
}