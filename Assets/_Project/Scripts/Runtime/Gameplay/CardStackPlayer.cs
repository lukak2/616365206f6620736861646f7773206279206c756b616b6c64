using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.Runtime.Gameplay
{
    public class CardStackPlayer : MonoBehaviour
    {
        [SerializeField] private CardStackController stackA;
        [SerializeField] private CardStackController stackB;

        [SerializeField] private float cardSwapDuration = 2f;
        
        private void Start()
        {
            Play().Forget();
        }

        private async UniTask Play()
        {
            while (stackA.TryPopCard(out var card))
            {
                stackB.PushCard(card, cardSwapDuration);
                
                await UniTask.Delay(TimeSpan.FromSeconds(1)); // Simulate some delay for the card movement
            }
        }
    }
}
