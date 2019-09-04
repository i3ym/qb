using System.Collections;
using TMPro;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    TextMeshProUGUI Text;

    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ShowFPSCoroutine());
    }

    IEnumerator ShowFPSCoroutine()
    {
        var wfs = new WaitForSeconds(.5f);

        while (true)
        {
            yield return wfs;
            Text.text = ((int) (1f / Time.deltaTime)).ToString();
        }
    }
}