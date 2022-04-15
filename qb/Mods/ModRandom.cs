namespace qb.Mods
{
    public class ModRandom : Mod
    {
        public override TranslatedString Name => ("Random", "Рандом");
        public override int Priority => 20;

        readonly Random Random = new Random();

        public override GameMatrix<TileBool> OverrideMatrixRender(FieldController controller, ReadOnlyGameMatrix<TileBool> matrix)
        {
            var output = new GameMatrix<TileBool>(matrix.Width, matrix.Height);

            for (int y = 0; y < controller.MaxY; y++)
                for (int x = 0; x < controller.MaxX; x++)
                {
                    if (Random.Next(2) == 0)
                        output[x, y] = new TileBool(CubeTile.None);
                    else output[x, y] = matrix[x, y];
                }

            return output;
        }
    }
}