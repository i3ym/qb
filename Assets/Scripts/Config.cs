using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config Instance;

    [SerializeField]
    public Texture2D Atlas = null;

    void Awake() => Instance = this;
}