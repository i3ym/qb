using UnityEngine;

public abstract class ButtonController : MonoBehaviour
{
    public static ButtonController ActiveController { get; protected set; }
    public static Button ActiveButton => ActiveController.Buttons[ActiveController.ActiveButtonIndex];

    [SerializeField]
    protected Button[] Buttons;

    int ActiveButtonIndex = 0;
    bool Down = false, Up = false, Select = false;

    bool _Down, _Up, _Select;

    void Update()
    {
        if (ActiveController != this || !enabled || !gameObject.activeSelf) return;

        _Down = Controls.Down;
        _Up = Controls.Up;
        _Select = Controls.Rotate;

        if (!Select && _Select) ActiveButton.OnClick();

        if (!Down && _Down)
        {
            ActiveButtonIndex = (ActiveButtonIndex + 1) % Buttons.Length;
            UpdateButtonDesign();
        }

        if (!Up && _Up)
        {
            ActiveButtonIndex--;
            if (ActiveButtonIndex < 0) ActiveButtonIndex = Buttons.Length - 1;

            UpdateButtonDesign();
        }

        Down = _Down;
        Up = _Up;
        Select = _Select;
    }

    protected void Enable()
    {
        gameObject.SetActive(true);
        Down = Up = Select = true;
    }

    protected static void UpdateButtonDesign()
    {
        if (ActiveController != null)
        {
            ActiveButton.OnDeselect();
            ButtonDesign.Design.transform.SetParent(ActiveButton.transform, false);
            ActiveButton.OnSelect();
        }
    }
}