using Scripts.Runtime.Common;
using UnityEngine;

namespace Scripts.Runtime.Gameplay
{
    public class FlameController : MonoBehaviour
    {
        [SerializeField] private Animator flameAnimator;
        
        public void Toggle()
        {
            var isVisible = flameAnimator.GetBool(Constants.AnimatorKeys.IsVisibleBool);
            
            flameAnimator.SetBool(Constants.AnimatorKeys.IsVisibleBool, !isVisible);
        }
    }
}