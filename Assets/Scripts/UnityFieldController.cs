using System.Collections;
using qb.Shared;
using UnityEngine;

public class UnityFieldController : FieldController
{
    public UnityFieldController(IFieldRenderer renderer, ITetrisControlsProvider controls) : base(renderer, controls) { }

    protected override void StartUpdating() => Config.Instance.StartCoroutine(UpdateCoroutine());

    IEnumerator UpdateCoroutine()
    {
        var wfs = new WaitForFixedUpdate();

        while (true)
        {
            yield return wfs;
            Update();
        }
    }
}