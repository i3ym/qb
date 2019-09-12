using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config Instance;

    public static int SelectedSkin = 1;

    [SerializeField]
    public Texture2D Atlas = null;

    void Awake() => Instance = this;
}