using UnityEngine;
using UnityEngine.UI;

public class TopImage : MonoBehaviour
{
    public static RawImage Instance;

    void Awake()
    {
        Instance = GetComponent<RawImage>();
        Instance.enabled = true;
    }
}