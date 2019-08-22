using System;

public class RenderingType
{
    public string Name { get; private set; }
    public Action OnSelect { get; private set; } = delegate { };
    public Action OnClick { get; private set; } = delegate { };

    public RenderingType(string name, Action onSelect) : this(name, onSelect, onSelect) { }
    public RenderingType(string name, Action onSelect, Action onClick)
    {
        Name = name;
        OnSelect = onSelect;
        OnClick = onClick;
    }
}