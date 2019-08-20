using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller controller;

    [SerializeField]
    public ControllerButton Up, Down, Left, Right, Rotate, RotateC;


    void Awake() => controller = this;
}