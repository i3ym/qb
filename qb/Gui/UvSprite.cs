using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace qb.Gui
{
    public class UvSprite : Sprite
    {
        static readonly Random Random = new Random();

        public readonly BindableInt Tile = new BindableInt(0);
        RectangleF Uv = new RectangleF();

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
            Texture = skin.Atlas;

            Uv.X = Tile.Value * skin.Size - float.Epsilon * 2;
            Uv.Y = skin.Index * skin.Size - float.Epsilon * 2;
            Uv.Width = Uv.Height = skin.Size - float.Epsilon * 2;
        }

        protected override DrawNode CreateDrawNode() => new CubeSpriteDrawNode(this);


        protected class CubeSpriteDrawNode : SpriteDrawNode
        {
            protected new UvSprite Source;

            public CubeSpriteDrawNode(UvSprite source) : base(source) => Source = source;

            protected override void Blit(Action<TexturedVertex2D> vertexAction)
            {
                DrawQuad(Texture, ScreenSpaceDrawQuad, DrawColourInfo.Colour, null, vertexAction,
                    new Vector2(InflationAmount.X / DrawRectangle.Width, InflationAmount.Y / DrawRectangle.Height), null, Source.Uv);
            }
        }
    }
}