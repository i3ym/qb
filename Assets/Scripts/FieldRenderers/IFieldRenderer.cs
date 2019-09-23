using System.Collections.Generic;

public interface IFieldRenderer
{
    void Initialize(IFieldController controller);

    void CreateField();
    void UpdateField();
    void UpdateFieldFast();
    void UpdateColors();
    void ShowNextShape();

    /// <returns> True to skip frame, false to stop animation </returns>
    IEnumerator<bool> RemoveLinesAnimation(int[] indexs);

    /// <returns> True to skip frame, false to stop animation </returns>
    IEnumerator<bool> GameOverAnimation();
}