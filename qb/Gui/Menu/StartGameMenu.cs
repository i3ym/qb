namespace qb.Gui.Menu
{
    public partial class StartGameMenu : ControllableMenuScreen
    {
        [Resolved] Storage Storage { get; set; } = null!;

        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc, KeyboardController maincontroller)
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

                gamesc.Controller.Controller = maincontroller;
                gamesc.Controller.RestartOnDeath = false;

                gamesc.Controller.StartGame();
                gamesc.Controller.Recorder = new ReplayRecorder(gamesc.Controller.Seed);
                gamesc.Controller.OnGameOver += () =>
                {
                    if (gamesc.Controller.Recorder is null) return;

                    try
                    {
                        using var stream = Storage.GetStream(ReplayController.ReplayPath, FileAccess.Write, FileMode.Create);
                        stream.Write(gamesc.Controller.Recorder.AsReplay().Serialize(0));
                    }
                    catch { }

                    gamesc.Controller.Recorder = null;
                };
            }
            void levelup() => gamesc.Controller.StartLevel.Value++;
            void leveldown() => gamesc.Controller.StartLevel.Value--;
        }
    }
}