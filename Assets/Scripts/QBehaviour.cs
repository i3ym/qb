using UnityEngine;

namespace qb.Unity
{
    public class QBehaviour : MonoBehaviour
    {
        public new RectTransform transform { get; private set; }

        protected void Awake()
        {
            transform = base.transform as RectTransform;
            AwakeOverride();
        }

        protected virtual void AwakeOverride() { }
    }
}