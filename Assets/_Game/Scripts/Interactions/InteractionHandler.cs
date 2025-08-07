using _Game.Player;
using _Game.Scripts.Game;
using Core.Scripts.Audio;
using UnityEngine;
using AudioType = Core.Scripts.Audio.AudioType;

namespace _Game.Scripts.Interactions
{
    public class InteractionHandler : MonoBehaviour
    {
        [TextArea(3, 5)]
        [SerializeField] private string onCollectedAllText;

        [SerializeField] private AudioClip celebrateSound;
        private PlayerMovementControllerMouse playerMovement;
        private UIPopup uiPopup;
        
        private void Start()
        {
            playerMovement = FindFirstObjectByType<PlayerMovementControllerMouse>();
            uiPopup = FindFirstObjectByType<UIPopup>();
            playerMovement.OnInteract += HandleInteraction;
            CollectibleTracker.Instance.OnCollectedAll += HandleCollectedAllItems;
        }

        private void OnDestroy()
        {
            playerMovement.OnInteract -= HandleInteraction;
            CollectibleTracker.Instance.OnCollectedAll -= HandleCollectedAllItems;
        }

        private void HandleInteraction(IInteractable interactable)
        {
            if (uiPopup.IsShown)
            {
                return;
            }
            interactable.Interact();
        }

        private void HandleCollectedAllItems()
        {
            AudioManager.Instance.Play(celebrateSound, AudioType.SFX, false, 0.5f);
            uiPopup.UpdateTextElement(onCollectedAllText);
            uiPopup.ShowPage();
        }
    }
}