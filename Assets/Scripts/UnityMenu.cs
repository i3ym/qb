using System;
using System.Collections.Generic;
using System.Linq;
using qb.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace qb.Unity
{
    public class UnityMenu : QBehaviour, IMenu
    {
        public Action<bool> OnActiveStateChanged { get; set; } = delegate { };
        public Action OnGoBack { get; set; } = delegate { };
        public bool Active
        {
            get => _Active;
            set
            {
                _Active = value;
                gameObject.SetActive(value);
                OnActiveStateChanged(value);
            }
        }
        public int ActiveButtonIndex
        {
            get => _ActiveButtonIndex;
            set
            {
                _ActiveButtonIndex = value;
                if (ButtonDesign != null) ButtonDesign.SetParent((Buttons[value] as UnityMenuButton).transform, false);
                if (Buttons != null) Buttons[value].OnSelect();

                if (IsScrollable && Buttons != null)
                    for (int i = 0; i < Buttons.Count; i++)
                    {
                        (Buttons[i] as UnityMenuButton).transform.anchoredPosition = new Vector2(
                            (Buttons[i] as UnityMenuButton).transform.position.x, -i * 2 + value * 2);
                    }
            }
        }
        public virtual IReadOnlyList<IMenuButton> Buttons
        {
            get => _Buttons;
            set
            {
                if (Buttons != null)
                    foreach (var obj in Buttons.Cast<UnityMenuButton>())
                        obj.transform.SetParent(null);

                _Buttons = value;

                var buttons = Buttons.OfType<UnityMenuButton>().Cast<UnityMenuButton>().ToArray();

                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].transform.SetParent(transform, false);
                    buttons[i].Text = buttons[i].Text;
                    buttons[i].transform.anchoredPosition = new Vector2(0, -(i - (buttons.Length / 2)) * 2);
                }

                ActiveButtonIndex = 0;
            }
        }
        public IMenu Previous { get; set; }

        public bool IsScrollable
        {
            get => _IsScrollable;
            set
            {
                _IsScrollable = value;

                if (value)
                {
                    gameObject.AddComponent<RectMask2D>();
                    ActiveButtonIndex = ActiveButtonIndex;
                }
                else
                {
                    var mask = GetComponent<RectMask2D>();
                    if (mask != null) Destroy(mask);

                    if (Buttons != null)
                        for (int i = 0; i < Buttons.Count; i++)
                        {
                            (Buttons[i] as UnityMenuButton).transform.SetParent(transform, false);
                            Buttons[i].Text = Buttons[i].Text;
                            (Buttons[i] as UnityMenuButton).transform.anchoredPosition = new Vector2(0, -(i - (Buttons.Count / 2)) * 2);
                        }
                }
            }
        }
        bool _IsScrollable = false;

        public Transform ButtonDesign;

        IReadOnlyList<IMenuButton> _Buttons;
        bool _Active = true;
        int _ActiveButtonIndex;

        protected void Start()
        {
            transform.anchorMax = Vector2.one;
            transform.anchorMin = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }
    }
}