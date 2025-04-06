using System;
using Scripts.Runtime.UserInterface;
using UnityEngine;

namespace Scripts.Runtime.Gameplay
{
    public class ToggleFlameButton : BaseButton
    {
        [SerializeField] private FlameController flameController;

        protected override void OnClicked()
        {
            flameController.Toggle();
        }
    }
}
