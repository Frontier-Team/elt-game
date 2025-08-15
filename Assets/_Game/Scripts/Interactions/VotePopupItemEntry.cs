using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Interactions
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class VotePopupItemEntry : MonoBehaviour
    {
        public Image characterSpriteImage;
        public TMP_Text nameLabel;
        public TMP_Text descLabel;

        [SerializeField] private Toggle toggle;

        private CharacterVote voteRef;

        public int VoteId => voteRef.VoteId;
        public event Action<VotePopupItemEntry> OnSelected;

        public void SetValues(CharacterVote voteInfo)
        {
            voteRef = voteInfo;
            characterSpriteImage.sprite = voteInfo.CharacterImage;
            nameLabel.text = voteInfo.CharacterName;
            descLabel.text = voteInfo.VoteSummary;
            toggle.onValueChanged.AddListener(HandleToggle);
        }

        public void SetToggleGroup(ToggleGroup group)
        {
            toggle.group = group;
        }

        private void HandleToggle(bool on)
        {
            if (on)
            {
                OnSelected?.Invoke(this);
            }
        }
    }

}