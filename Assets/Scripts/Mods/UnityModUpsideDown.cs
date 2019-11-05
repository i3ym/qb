using qb.Shared;
using qb.Shared.Mods;
using UnityEngine;

namespace qb.Unity.Mods
{
    public class UnityModUpsideDown : ModUpsideDown
    {
        public override void OnAdd(FieldController controller)
        {
            (controller.Field as UnityGameScreen).FieldGrid.transform.localScale = new Vector3(1, -1, 1);
        }
        public override void OnRemove(FieldController controller)
        {
            (controller.Field as UnityGameScreen).FieldGrid.transform.localScale = Vector3.one;
        }
    }
}