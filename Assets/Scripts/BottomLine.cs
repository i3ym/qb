using UnityEngine;
using UnityEngine.UI;

public class BottomLine : MonoBehaviour
{
    public static RawImage Instance;

    void Awake() => Instance = GetComponent<RawImage>();
}