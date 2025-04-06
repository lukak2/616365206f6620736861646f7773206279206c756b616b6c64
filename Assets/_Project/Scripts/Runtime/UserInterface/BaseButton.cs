using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Runtime.UserInterface
{
    public abstract class BaseButton : MonoBehaviour
    {
        [SerializeField] protected Button button;

        protected virtual void OnEnable()
        {
            button.onClick.AddListener(OnClicked);
        }

        protected virtual void OnDisable()
        {
            button.onClick.RemoveListener(OnClicked);
        }

        protected abstract void OnClicked();
    }
}