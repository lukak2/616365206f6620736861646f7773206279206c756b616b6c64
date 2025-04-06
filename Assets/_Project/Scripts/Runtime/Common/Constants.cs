using UnityEngine;

namespace Scripts.Runtime.Common
{
    public static class Constants
    {
        public static class Scenes
        {
            public const string MainMenu = "MainMenu";
        }

        public static class AnimatorKeys
        {
            public static readonly int IsVisibleBool = Animator.StringToHash("IsVisible");
        }
    }
}