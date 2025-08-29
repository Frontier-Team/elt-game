using System;
using _Game.Player;
using _Game.Scripts.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Interactions
{
    public class InteractiveItem : MonoBehaviour, IInteractable, ICollectable
    {
        private string popupText;
        private UIPopup popup;
        private PlayerMovementControllerMouse player;
        private CharacterVote characterVote;

        private VotePopup votePopup;
        private VotingController votingController;

        [SerializeField] private bool destroyOnCollect;
        [SerializeField] private Canvas textCanvas;
        [SerializeField] private Canvas highlightImageCanvas;
        [SerializeField] private bool disableTextOnClick = true;
        [SerializeField] private InteractionType interactionType = InteractionType.VoteCharacter;

        private Animator textAnim;

        private Action lastPageHandler;
        private Action hideHandler;

        private void Start()
        {
            if (interactionType == InteractionType.VoteScreen)
            {
                votePopup = FindFirstObjectByType<VotePopup>(FindObjectsInactive.Include);
                votingController = FindFirstObjectByType<VotingController>();

                if (votingController != null && highlightImageCanvas != null)
                {
                    votingController.OnVoteAllowed += () =>
                    {
                        highlightImageCanvas.gameObject.SetActive(true);
                    };
                    votingController.OnVoteMade += () =>
                    {
                        highlightImageCanvas.gameObject.SetActive(false);
                    };
                }

                return;
            }

            player = FindFirstObjectByType<PlayerMovementControllerMouse>();
            popup = FindFirstObjectByType<UIPopup>();
            textAnim = GetComponentInChildren<Animator>();

            if (interactionType == InteractionType.VoteCharacter)
            {
                characterVote = GetComponent<CharacterVote>();
            }

            if (textAnim != null)
            {
                textAnim.Play("Start", 0, Random.Range(0f, 1f));
            }

            if (characterVote != null)
            {
                popupText = $"{characterVote.CharacterName}: \n\n\n{characterVote.VoteSummary}\n\n\n{characterVote.VoteDescription}";
            }

            if (popup != null && player != null)
            {
                popup.OnShowPage += player.DisableMovement;
                popup.OnHidePage += player.EnableMovement;
            }
        }

        private void OnDestroy()
        {
            if (interactionType == InteractionType.VoteScreen)
            {
                if (votingController != null && highlightImageCanvas != null)
                {
                    votingController.OnVoteAllowed -= () =>
                    {
                        highlightImageCanvas.gameObject.SetActive(true);
                    };
                    votingController.OnVoteMade -= () =>
                    {
                        highlightImageCanvas.gameObject.SetActive(false);
                    };
                }

                return;
            }

            UnhookPopupHandlers();

            if (popup != null && player != null)
            {
                popup.OnShowPage -= player.DisableMovement;
                popup.OnHidePage -= player.EnableMovement;
            }
        }

        public void Interact()
        {
            if (interactionType == InteractionType.VoteScreen)
            {
                if (votingController != null && votingController.CanVote)
                {
                    if (votePopup != null)
                    {
                        votePopup.ShowPage();
                    }
                }

                return;
            }

            HookPopupHandlers();

            if (popup != null)
            {
                popup.UpdateTextElement(popupText);
                popup.ShowPage();
            }

            if (interactionType == InteractionType.Collectible && destroyOnCollect)
            {
                Collect();
                gameObject.SetActive(false);
            }

            if (disableTextOnClick)
            {
                if (textCanvas != null)
                {
                    textCanvas.gameObject.SetActive(false);
                }
            }
        }

        public void Collect()
        {
            CollectibleTracker.Instance.CollectItem(this);
        }

        private void HookPopupHandlers()
        {
            if (popup == null)
            {
                return;
            }

            UnhookPopupHandlers();

            if (interactionType == InteractionType.VoteCharacter && characterVote != null)
            {
                lastPageHandler = () =>
                {
                    characterVote.SetAsViewed();

                    if (textCanvas != null)
                    {
                        textCanvas.gameObject.SetActive(false);
                    }

                    UnhookPopupHandlers();
                };

                hideHandler = UnhookPopupHandlers;

                popup.OnLastPage += lastPageHandler;
                popup.OnHidePage += hideHandler;
            }
        }

        private void UnhookPopupHandlers()
        {
            if (popup == null)
            {
                return;
            }

            if (lastPageHandler != null)
            {
                popup.OnLastPage -= lastPageHandler;
            }

            if (hideHandler != null)
            {
                popup.OnHidePage -= hideHandler;
            }

            lastPageHandler = null;
            hideHandler = null;
        }
    }
}
