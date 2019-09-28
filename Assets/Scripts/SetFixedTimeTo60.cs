using UnityEngine;

public class SetFixedTimeTo60 : MonoBehaviour
{
    void Awake() => Time.fixedDeltaTime = 1f / 60f;
}