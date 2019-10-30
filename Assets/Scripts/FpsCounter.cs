using System.Collections;
using TMPro;
using UnityEngine;

namespace qb.Unity
{
    public class FpsCounter : MonoBehaviour
    {
        TextMeshProUGUI Text;

        void Start()
        {
            Text = GetComponent<TextMeshProUGUI>();
            StartCoroutine(ShowFpsCoroutine());
        }

        IEnumerator ShowFpsCoroutine()
        {
            var wfs = new WaitForSeconds(.5f);

            while (true)
            {
                yield return wfs;
                Text.text = ((int) (1f / Time.deltaTime)).ToString();
            }
        }
    }
}