using System;
using System.Linq;

namespace qb.Mods
{
    public class ModFlashlight : Mod
    {
        public override TranslatedString Name => ("Flashlight", "Фонарик");
        public override int Priority => 10;

        public override GameMatrix<TileBool> OverrideMatrixRender(FieldController controller, ReadOnlyGameMatrix<TileBool> matrix)
        {
            var output = new GameMatrix<TileBool>(matrix.Width, matrix.Height);

            var objPoints = new(int x, int y) [4];
            (int x, int y) point2;

            int objindex = 0;
            for (int x = 0; x < controller.CurrentObject.Width; x++)
                for (int y = controller.CurrentObject.Height - 1; y >= 0; y--)
                {
                    if (controller.CurrentObject[x, y])
                        objPoints[objindex++] = (controller.CurrentX + x, controller.CurrentY + y);

                    if (objindex == 4) goto Out;
                }

            Out: ;

            for (int x = 0; x < controller.MaxX; x++)
                for (int y = controller.MaxY - 1; y >= 0; y--)
                {
                    point2 = (x, y);

                    if ((objPoints.Any(point => Math.Sqrt(
                            Math.Abs((point.x - point2.x) * (point.x - point2.x)) +
                            Math.Abs((point.y - point2.y) * (point.y - point2.y))) > 5)))
                        output[x, y] = CubeTile.None;
                    else output[x, y] = matrix[x, y];
                }

            return output;
        }
    }
}