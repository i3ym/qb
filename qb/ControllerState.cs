namespace qb
{
    public readonly struct ControllerState
    {
        public readonly bool Up, Down, Left, Right, A, B, Back;

        public ControllerState(bool up = false, bool down = false, bool left = false, bool right = false, bool a = false, bool b = false, bool back = false)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            A = a;
            B = b;
            Back = back;
        }


        public ControllerState With(QbKey key) =>
            new ControllerState(
                Up || key == QbKey.Up,
                Down || key == QbKey.Down,
                Left || key == QbKey.Left,
                Right || key == QbKey.Right,
                A || key == QbKey.A,
                B || key == QbKey.B,
                Back || key == QbKey.Back
            );
        public ControllerState Or(ControllerState other) =>
            new ControllerState(
                Up || other.Up,
                Down || other.Down,
                Left || other.Left,
                Right || other.Right,
                A || other.A,
                B || other.B,
                Back || other.Back
            );

        public static ControllerState FromKey(QbKey key) => new ControllerState().With(key);
    }
}