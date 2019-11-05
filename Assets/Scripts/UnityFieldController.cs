using System.Collections;
using System.Linq;
using qb.Shared;
using qb.Shared.Animations;
using qb.Shared.Input;
using UnityEngine;

namespace qb.Unity
{
    public class UnityFieldController : FieldController
    {
        public UnityFieldController(IField field, IControlsProvider controls,
            RemoveLinesAnimation removelines, GameOverAnimation gameover) : base(field, controls, removelines, gameover)
        {
            OnStartGame += (_, __) => (Field as UnityGameScreen).transform.localScale = Vector3.one;
        }

        protected override void StartUpdating() => (Field as MonoBehaviour).StartCoroutine(UpdateCoroutine());

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
}