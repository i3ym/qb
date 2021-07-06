namespace qb.Mods
{
    public abstract class Mod
    {
        public abstract TranslatedString Name { get; }
        public abstract int Priority { get; }

        public virtual void OnAdd(FieldController controller) { }
        public virtual void OnRemove(FieldController controller) { }

        public virtual GameMatrix<TileBool> OverrideMatrixRender(FieldController controller, ReadOnlyGameMatrix<TileBool> matrix) => matrix;
        public virtual GameMatrix<TileBool> OverrideObjectRender(FieldController controller, ReadOnlyGameMatrix<TileBool> matrix) => matrix;
        public virtual void OverrideObjectRenderPositions(FieldController controller, ref int currentX, ref int currentY) { }
    }
}