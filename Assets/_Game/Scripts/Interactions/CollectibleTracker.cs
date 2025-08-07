using System;
using System.Collections.Generic;
using _Game.Scripts.Interactions;
using UnityEngine;

namespace _Game.Scripts.Game
{
    [DefaultExecutionOrder(-2000)]
    public class CollectibleTracker : MonoBehaviour
    {
        public event Action OnCollectedAll; 
        public static CollectibleTracker Instance;
        
        [SerializeField] private List<CollectibleItem> collectiblesToCollect;
        private HashSet<ICollectable> collectedItems = new();

        private void Start()
        {
            CreateSingleton();
            RegisterCollectibles();
        }
        
        private void CreateSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void RegisterCollectibles()
        {
            foreach (var collectible in collectiblesToCollect)
            {
                collectedItems.Add(collectible);
            }
        }

        public void CollectItem(ICollectable collectible)
        {
            if (collectedItems.Contains(collectible))
            {
                collectedItems.Remove(collectible);
                if (collectedItems.Count == 0)
                {
                    OnCollectedAll?.Invoke();
                }
            }
        }
    }
}