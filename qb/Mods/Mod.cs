namespace qb.Mods
{
    public abstract class Mod
    {
        public static readonly ImmutableArray<Mod> Mods;

        static Mod() => Mods = new Mod[] { new ModBlind(), new ModFlashlight(), new ModRandom(), new ModUpsideDown() }.ToImmutableArray();

        public abstract TranslatedString Name { get; }
        public abstract int Priority { get; }

        public virtual void OnAdd(GameScreen controller) { }
        public virtual void OnRemove(GameScreen controller) { }

        public virtual GameMatrix<TileBool> OverrideMatrixRender(FieldController controller, ReadOnlyGameMatrix<TileBool> matrix) => matrix;
        public virtual GameMatrix<TileBool> OverrideObjectRender(FieldController controller, ReadOnlyGameMatrix<TileBool> matrix) => matrix;
        public virtual void OverrideObjectRenderPositions(FieldController controller, ref int currentX, ref int currentY) { }
    }
}