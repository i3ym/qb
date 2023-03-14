namespace qb.Gui
{
    public partial class ToggleMenuButtonContainer<T> : MenuButtonContainer<ValueMenuButton<T>>
    {
        public Action<T> OnEnable = delegate { };
        public Action<T> OnDisable = delegate { };

        public IReadOnlyList<T> Selected => _Selected;
        readonly List<T> _Selected = new List<T>();

        public override void Add(ValueMenuButton<T> drawable)
        {
            void fade(bool enabled, float duration = 250) => drawable.PixelText.FadeColour(enabled ? Colour4.White : Colour4.Gray, duration, Easing.OutQuad);

            drawable.Action += () =>
            {
                var enabled = _Selected.Contains(drawable.Value);

                if (enabled)
                {
                    _Selected.Remove(drawable.Value);
                    OnDisable(drawable.Value);
                }
                else
                {
                    _Selected.Add(drawable.Value);
                    OnEnable(drawable.Value);
                }


                fade(!enabled);
            };
            fade(false, 0);


            base.Add(drawable);
        }
    }
}