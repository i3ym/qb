using System;
using osu.Framework.Graphics;

namespace qb.Mods
{
    public class ModUpsideDown : Mod
    {
        public override TranslatedString Name => ("Upside Down", "Вверх ногами");
        public override int Priority => int.MaxValue;

        public override void OnAdd(GameScreen gamesc)
        {
            gamesc.Playfield.Y = -2;
            gamesc.Playfield.Anchor = Anchor.BottomLeft;
            gamesc.Playfield.Height = -Math.Abs(gamesc.Playfield.Height);
        }
        public override void OnRemove(GameScreen gamesc)
        {
            gamesc.Playfield.X = 0;
            gamesc.Playfield.Anchor = Anchor.TopLeft;
            gamesc.Playfield.Height = Math.Abs(gamesc.Playfield.Height);
        }
    }
}