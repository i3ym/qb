using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Action OnClick = delegate { };

    void Start()
    {

    }
    void Update()
    {

    }

    //

    public void Select()
    {
        ButtonDesign.Design.transform.SetParent(transform, false);
    }
}