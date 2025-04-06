using System;
using Scripts.Runtime.Common;
using UnityEngine;

namespace Scripts.Runtime.Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;


        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        
        public void SetSortOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }

        public void MoveToPosition(Vector3 position, float duration, Action onComplete = null)
        {
            if (duration > 0)
            {
                PrimeTweenExtensions.Jump(transform, position, duration, 1f)
                    .OnComplete(() =>
                    {
                        onComplete?.Invoke();
                    });
            }
            else
            {
                transform.position = position;
                onComplete?.Invoke();
            }
        }
    }
}