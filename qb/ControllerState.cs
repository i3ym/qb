namespace qb
{
    public readonly struct ControllerState
    {
        public bool Up { get; init; }
        public bool Down { get; init; }
        public bool Left { get; init; }
        public bool Right { get; init; }
        public bool A { get; init; }
        public bool B { get; init; }
        public bool Back { get; init; }

        public ControllerState(bool up, bool down, bool left, bool right, bool a, bool b, bool back)
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