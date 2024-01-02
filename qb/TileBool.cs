namespace qb;

public readonly struct TileBool
{
    readonly CubeTile Value;

    public TileBool(CubeTile tileValue) => Value = tileValue;

    public override string ToString() => Value.ToString();

    public static implicit operator bool(TileBool tileb) => tileb.Value != CubeTile.None;
    public static implicit operator CubeTile(TileBool tileb) => tileb.Value;

    public static implicit operator TileBool(CubeTile value) => new TileBool(value);
}
