using System;
using UnityEngine;

namespace _Game.Scripts.Interactions
{
    public class CharacterVote : MonoBehaviour
    {
        public event Action<int> OnVoteDescriptionViewed;

        [SerializeField] private CharacterData data;

        public Sprite CharacterImage => data.characterImage;
        public int VoteId => data.voteId;
        public string VoteSummary => data.voteSummary;
        public string CharacterName => data.characterName;
        public string VoteDescription => data.voteDescription;

        public bool HasViewed { get; private set; } = false;

        public void SetAsViewed()
        {
            HasViewed = true;
            OnVoteDescriptionViewed?.Invoke(VoteId);
        }

        public void ResetViewState()
        {
            HasViewed = false;
        }
    }
}