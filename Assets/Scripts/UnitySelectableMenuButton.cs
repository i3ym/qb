using qb.Shared;
using qb.Shared.Mods;

namespace qb.Unity
{
    public class UnitySelectableMenuButtonMod : UnitySelectableMenuButton<Mod> { }

    public class UnitySelectableMenuButton<T> : UnityMenuButton, ISelectableMenuButton<T>
    {
        public T SelectableValue { get; set; }
    }
}