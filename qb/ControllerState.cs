namespace qb;

public readonly struct ControllerState
{
    public readonly bool Up, Down, Left, Right, A, B;

    public ControllerState(bool up = false, bool down = false, bool left = false, bool right = false, bool a = false, bool b = false)
    {
        Up = up;
        Down = down;
        Left = left;
        Right = right;
        A = a;
        B = b;
    }


    public ControllerState With(QbKey key) =>
        new ControllerState(
            Up || key == QbKey.Up,
            Down || key == QbKey.Down,
            Left || key == QbKey.Left,
            Right || key == QbKey.Right,
            A || key == QbKey.A,
            B || key == QbKey.B
        );

    public static ControllerState FromKey(QbKey key) => new ControllerState().With(key);


    public static ControllerState FromBitMask(byte mask) =>
        new ControllerState
        (
            up: ((mask >> (int) QbKey.Up) & 1) == 1,
            down: ((mask >> (int) QbKey.Down) & 1) == 1,
            left: ((mask >> (int) QbKey.Left) & 1) == 1,
            right: ((mask >> (int) QbKey.Right) & 1) == 1,
            a: ((mask >> (int) QbKey.A) & 1) == 1,
            b: ((mask >> (int) QbKey.B) & 1) == 1
        );
    public byte ToBitMask() =>
        (byte)
        (
            0
            | ((Up ? 1 : 0) << (int) QbKey.Up)
            | ((Down ? 1 : 0) << (int) QbKey.Down)
            | ((Left ? 1 : 0) << (int) QbKey.Left)
            | ((Right ? 1 : 0) << (int) QbKey.Right)
            | ((A ? 1 : 0) << (int) QbKey.A)
            | ((B ? 1 : 0) << (int) QbKey.B)
        );
}
