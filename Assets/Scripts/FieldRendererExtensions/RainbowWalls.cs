using UnityEngine;

public class RainbowWalls : IFieldRendererExtension
{
    float WallBorderHue = .5f;

    [SerializeField]
    protected Material MaterialWallsBorder;

    public void Start()
    {
        
    }
    public void Update()
    {
        MaterialWallsBorder.color = Color.HSVToRGB((WallBorderHue += .001f) > 1f ? WallBorderHue = 0f : WallBorderHue, 1f, 1f);
    }
}