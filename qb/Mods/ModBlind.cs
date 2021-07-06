namespace qb.Mods
{
    public class ModBlind : Mod
    {
        public override TranslatedString Name => ("Blind", "Слепой");
        public override int Priority => 0;

        public override GameMatrix<TileBool> OverrideMatrixRender(FieldController controller, ReadOnlyGameMatrix<TileBool> matrix)
        {
            var output = new GameMatrix<TileBool>(matrix.MaxX, matrix.MaxY);

            for (int x = 0; x < controller.MaxX; x++)
                for (int y = controller.MaxY - 1; y >= 0; y--)
                {
                    if (matrix[x, y])
                    {
                        output[x, y] = matrix[x, y];

                        for (int yy = y - 1; yy >= 0; yy--)
                            output[x, yy] = CubeTile.None;

                        break;
                    }
                    else output[x, y] = CubeTile.None;
                }

            return output;
        }
    }
}