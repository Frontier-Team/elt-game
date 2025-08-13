using System;
using _Game.Player;
using _Game.Scripts.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Interactions
{
    public class CollectibleItem : MonoBehaviour, IInteractable, ICollectable
    {
        private string popupText;
        private UIPopup popup;
        private PlayerMovementControllerMouse player;
        private CharacterVote characterVote;
        
        [SerializeField] private bool destroyOnCollect;
        [SerializeField] private Canvas textCanvas;
        [SerializeField] private bool disableTextOnClick = true;
        
        private Animator textAnim;
        
        private void Start()
        {
            player = FindFirstObjectByType<PlayerMovementControllerMouse>();
            popup = FindFirstObjectByType<UIPopup>();
            textAnim = GetComponentInChildren<Animator>();
            characterVote = GetComponent<CharacterVote>();
            
            if (textAnim != null)
            {
                textAnim.Play("Start", 0, Random.Range(0f, 1f));
            }
            if (popup != null && player != null)
            {
                popup.OnShowPage += player.DisableMovement;
                popup.OnHidePage += player.EnableMovement;
            }
        }

        private void OnDestroy()
        {
            if (popup != null && player != null)
            {
                popup.OnShowPage -= player.DisableMovement;
                popup.OnHidePage -= player.EnableMovement;
            }
        }

        public void Interact()
        {
            popup.UpdateTextElement(popupText);
            popup.ShowPage();
            if (destroyOnCollect)
            {
                Collect();
                gameObject.SetActive(false);
            }

            if (disableTextOnClick)
            {
                textCanvas?.gameObject.SetActive(false);
            }
        }

        public void Collect()
        {
            CollectibleTracker.Instance.CollectItem(this);
        }
    }
}