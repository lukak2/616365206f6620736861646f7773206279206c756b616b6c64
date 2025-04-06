using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Runtime.Gameplay
{
    public class CardStackController : MonoBehaviour
    {
        [SerializeField] private int instantiateCount;
        [SerializeField] private Vector3 offsetPerCard;
        
        [SerializeField] private Transform stackPosition;
        [SerializeField] private Card cardPrefab;

        private readonly List<Card> _cards = new();

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < instantiateCount; i++)
            {
                var cardInstance = Instantiate(cardPrefab);
                
                PushCard(cardInstance, 0f);
            }
        }

        public Vector3 GetNextCardPosition()
        {
            return stackPosition.position + (offsetPerCard * _cards.Count);
        }

        public void PushCard(Card card, float duration)
        {
            var nextPosition = GetNextCardPosition();
            
            card.MoveToPosition(nextPosition, duration, () =>
            {
                card.SetParent(stackPosition);
                card.SetSortOrder(_cards.Count);
            });
            
            _cards.Add(card);
        }

        public bool TryPopCard(out Card card)
        {
            if (_cards.Count == 0)
            {
                card = null;
                return false;
            }
            
            var lastIndex = _cards.Count - 1;
            var lastCard = _cards[lastIndex];

            _cards.RemoveAt(lastIndex);

            card = lastCard;

            return true;
        }
    }
}
