namespace qb.Animations
{
    public class GameOverFillingAnimation : IGameOverAnimation
    {
        public void Animation(GameScreen field, Action after)
        {
            var enumerator = animation();
            field.Controller.BeforeUpdate += update;

            void update()
            {
                if (enumerator.MoveNext()) return;

                field.Controller.BeforeUpdate -= update;
                after();
            }


            IEnumerator<bool> animation()
            {
                var controller = field.Controller;

                for (int y = controller.Tiles.Height - 1; y >= controller.Tiles.Height / 2; y--)
                    for (int x = 0; x < controller.Tiles.Width; x++)
                    {
                        field.Playfield[x, y] = CubeTile.Background;
                        field.Playfield[controller.Tiles.Width - 1 - x, controller.Tiles.Height - 1 - y] = CubeTile.Background;

                        yield return true;
                    }
            }
        }
    }
}