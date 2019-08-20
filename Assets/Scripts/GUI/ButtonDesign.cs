using UnityEngine;

public class ButtonDesign : MonoBehaviour
{
    public static ButtonDesign Design;

    [HideInInspector]
    public new RectTransform transform;

    void Awake()
    {
        Design = this;
        transform = gameObject.transform as RectTransform;
    }
}