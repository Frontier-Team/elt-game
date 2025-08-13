using System;
using UnityEngine;

namespace _Game.Scripts.Interactions
{
    public class CharacterVote : MonoBehaviour
    {
        public event Action<int> OnVoteDescriptionViewed;
        
        [SerializeField] private int voteId = 0;
        [SerializeField] private string voteSummary = "5 word summary";
        [TextArea(3, 6)]
        [SerializeField] private string voteDescription = "Long form description";
        [SerializeField] private string characterName = "Character name";
        [SerializeField] private Sprite characterSprite;
        
        public int VoteId => voteId;
        public string VoteSummary => voteSummary;
        public string CharacterName => characterName;
        public string VoteDescription => voteDescription;

        public bool HasViewed { get; private set; } = false;

        public void SetAsViewed()
        {
            HasViewed = true;
            OnVoteDescriptionViewed?.Invoke(voteId);
        }

        public void ResetViewState()
        {
            HasViewed = false;
        }
    }
}