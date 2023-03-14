using System.Diagnostics.CodeAnalysis;
using osu.Framework.Graphics.Rendering;

namespace qb.Gui
{
    public partial class UvSprite : Sprite
    {
        static readonly Random Random = new Random();

        [Resolved, AllowNull] IRenderer Renderer { get; set; }

        public readonly BindableInt Tile = new BindableInt(0);

        public UvSprite() => Height = Width = 1;

        [BackgroundDependencyLoader]
        void Load(SkinManager skinmanager)
        {
            Tile.BindValueChanged(e => SetSkin(skinmanager.SelectedSkin.Value));
            skinmanager.SelectedSkin.BindValueChanged(e => Scheduler.AddDelayed(() => SetSkin(e.NewValue), Random.Next(250)));

            SetSkin(skinmanager.SelectedSkin.Value);
        }

        void SetSkin(Skin skin)
        {
            skin.Load(Renderer);
            Texture = skin.Textures[Tile.Value];
        }
    }
}