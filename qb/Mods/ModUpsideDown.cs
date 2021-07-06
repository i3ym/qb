using System;
using osu.Framework.Graphics;

namespace qb.Mods
{
    public class ModUpsideDown : Mod
    {
        public override TranslatedString Name => ("Upside Down", "Вверх ногами");
        public override int Priority => int.MaxValue;

        public override void OnAdd(FieldController controller)
        {
            controller.Field.Height = -Math.Abs(controller.Field.Height);
            controller.Field.Anchor = Anchor.BottomCentre;
        }
        public override void OnRemove(FieldController controller)
        {
            controller.Field.Height = Math.Abs(controller.Field.Height);
            controller.Field.Anchor = Anchor.TopLeft;
        }
    }
}