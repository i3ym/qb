namespace qb.Gui.Menu
{
    public partial class StartGameMenu : ControllableMenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc, KeyboardController maincontroller, Storage storage)
        {
            var level = new PixelFlowContainer()
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                ChildAxes = Anchor.Centre,
                Children = new Drawable[]
                {
                    new PixelTextButton(("-"), leveldown) { Width = 2 },
                    PixelText.Create(gamesc.Controller.StartLevel, x => x.ToString("00")),
                    new PixelTextButton(("+"), levelup) { Width = 2 },
                },
            };

            var mods = new ToggleMenuButtonContainer<Mod>()
            {
                AutoSizeAxes = Axes.Both,
                ChildrenAnchor = Anchor.Centre,
                OnEnable = v => { v.OnAdd(gamesc); gamesc.Controller.Mods.Add(v); },
                OnDisable = v => { v.OnRemove(gamesc); gamesc.Controller.Mods.Remove(v); },
                Children = Mod.Mods.Select(x => new ValueMenuButton<Mod>(x, x.Name)).ToArray(),
            };

            AddButton(new PixelTextButton(("Start", "Начать"), start));
            AddButtonContainer(level);
            AddButtonContainer(mods);

            /*foreach (var mod in Mod.Mods)
            {
                var clickfunc = (bool enabled) =>
                {
                    if (enabled) { mod.OnAdd(gamesc); gamesc.Controller.Mods.Add(mod); }
                    else { mod.OnRemove(gamesc); gamesc.Controller.Mods.Remove(mod); }
                };

                AddButton(new PixelSwitchButton(mod.Name, () => gamesc.Controller.Mods.Contains(mod), clickfunc));
            }*/

            AddButton(new PixelTextButton(("Back", "Назад"), Back));



            void start()
            {
                PixelScreenStack.PopTo(null);
                gamesc.Controller.StartGame(maincontroller, storage);
            }
            void levelup() => gamesc.Controller.StartLevel.Value++;
            void leveldown() => gamesc.Controller.StartLevel.Value--;
        }
    }
}