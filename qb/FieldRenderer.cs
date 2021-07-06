using System.Linq;

namespace qb
{
    public partial class FieldController
    {
        public readonly GameScreen Field;

        public void UpdateField()
        {
            var matrix = Tiles;
            var objectMatrix = CurrentObject;
            var currentX = CurrentX;
            var currentY = CurrentY;

            if (Mods != null)
                foreach (var mod in Mods.Where(x => x != null).ToArray())
                {
                    matrix = mod.OverrideMatrixRender(this, matrix);
                    objectMatrix = mod.OverrideObjectRender(this, objectMatrix);
                    mod.OverrideObjectRenderPositions(this, ref currentX, ref currentY);
                }

            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                    Field[x, y] = matrix[x, y];

            for (int y = 0; y < objectMatrix.MaxY; y++)
                for (int x = 0; x < objectMatrix.MaxX; x++)
                    if (objectMatrix[x, y])
                        Field[x + currentX, y + currentY] = objectMatrix[x, y];
        }

        public void ShowNextShape()
        {
            int addx = 0;
            int addy = 0;

            if (NextShape == Shapes.O) addx = 1;
            else addy = -1;

            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                    Field.SetNextTile(x, y, NextShape.Matrix[x - addx, y - addy] ? NextShape.Tile : CubeTile.None);
        }
    }
}