using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Action OnClick = delegate { };
    public Action OnSelect = delegate { };
    public Action OnDeselect = delegate { };
}