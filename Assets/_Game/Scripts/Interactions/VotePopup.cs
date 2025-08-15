using System;
using System.Collections.Generic;
using _Game.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace _Game.Scripts.Interactions
{
    public class VotePopup : UIPopup
    {
        [SerializeField] private Button voteButton;
        [SerializeField] private ToggleGroup voteOptionToggleGroup;
        [SerializeField] private GameObject voteContainer;
        [SerializeField] private GameObject ItemPrefab;
        [SerializeField] private VotingController controller;

        private VotePopupItemEntry selectedItem;
        private readonly List<VotePopupItemEntry> itemList = new();
        private bool firstTime = true;

        protected override void Start()
        {
            base.Start();
            voteButton.onClick.AddListener(HandleVoteButton);
            voteButton.interactable = false;
        }

        private void OnEnable()
        {
            if (firstTime)
            {
                BuildOnce();
                firstTime = false;
            }

            if (selectedItem != null && controller.CanVote)
            {
                voteButton.interactable = true;
            }
            else
            {
                voteButton.interactable = false;
            }
        }

        private void BuildOnce()
        {
            var votableItems = controller.CharacterVotes;
            foreach (var cv in votableItems)
            {
                var go = Instantiate(ItemPrefab, voteContainer.transform);
                var entry = go.GetComponent<VotePopupItemEntry>();
                entry.SetValues(cv);
                entry.SetToggleGroup(voteOptionToggleGroup);
                entry.OnSelected += HandleSelected;
                itemList.Add(entry);
            }
        }

        private void HandleSelected(VotePopupItemEntry entry)
        {
            selectedItem = entry;

            if (controller.CanVote)
            {
                voteButton.interactable = true;
            }
            else
            {
                voteButton.interactable = false;
            }
        }

        private async void HandleVoteButton()
        {
            if (selectedItem == null)
            {
                return;
            }

            voteButton.interactable = false;
            var ok = await controller.PostVote(selectedItem.VoteId);

            if (!ok && controller.CanVote)
            {
                voteButton.interactable = true;
            }
            else
            {
                voteButton.interactable = false;
            }

            if (ok)
            {
                HidePage();
            }
        }
    }
}
