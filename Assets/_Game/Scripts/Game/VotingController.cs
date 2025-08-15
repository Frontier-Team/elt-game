using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Game.Scripts.Interactions;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace _Game.Scripts.Game
{
    public class VotingController : MonoBehaviour
    {
        public event Action OnVoteAllowed;
        public event Action OnVoteMade;

        public bool CanVote { get; private set; } = false;
        public bool HasVoted { get; private set; } = false;
        public int VoteChoiceIndex { get; private set; }

        [SerializeField] private string voteApiHost = "https://voting-system-lyart.vercel.app";
        [SerializeField] private string voteApiPath = "/api/vote/";
        [SerializeField] private int retryRequestAmount = 3;
        [SerializeField] private int retryTimeoutInMs = 200;

        [SerializeField] private List<CharacterVote> characterVotes = new();
        public List<CharacterVote> CharacterVotes => characterVotes;
        private readonly HashSet<int> viewedVoteItems = new();

        private void Awake()
        {
            if (characterVotes == null || characterVotes.Count == 0)
            {
                characterVotes = FindObjectsByType<CharacterVote>(FindObjectsSortMode.None).ToList();
            }

            foreach (var item in characterVotes)
            {
                item.OnVoteDescriptionViewed += HandleOnVoteDescriptionViewed;
            }
        }

        private void OnDestroy()
        {
            foreach (var item in characterVotes)
            {
                item.OnVoteDescriptionViewed -= HandleOnVoteDescriptionViewed;
            }
        }

        private void HandleOnVoteDescriptionViewed(int voteIndex)
        {
            viewedVoteItems.Add(voteIndex);

            if (viewedVoteItems.Count == characterVotes.Count)
            {
                EnableVoting();
                OnVoteAllowed?.Invoke();
            }
        }

        public void EnableVoting()
        {
            CanVote = true;
        }

        public void DisableVoting()
        {
            CanVote = false;
        }

        public void ResetVote()
        {
            HasVoted = false;
            VoteChoiceIndex = default;
            viewedVoteItems.Clear();
            DisableVoting();
        }

        public async Task<bool> PostVote(int voteId)
        {
            if (!CanVote)
            {
                return false;
            }

            var url = $"{voteApiHost}{voteApiPath}{voteId}";
            var delayMs = retryTimeoutInMs;

            for (var attempt = 1; attempt <= retryRequestAmount; attempt++)
            {
                using var req = UnityWebRequest.PostWwwForm(url, string.Empty);
                await req.SendWebRequest().AsTask();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    HasVoted = true;
                    VoteChoiceIndex = voteId;
                    DisableVoting();
                    OnVoteMade?.Invoke();
                    return true;
                }

                var retryable = req.result == UnityWebRequest.Result.ConnectionError
                                || req.result == UnityWebRequest.Result.DataProcessingError
                                || (req.result == UnityWebRequest.Result.ProtocolError && req.responseCode >= 500);

                if (!retryable || attempt == retryRequestAmount)
                {
                    return false;
                }

                await Task.Delay(delayMs);
                delayMs *= 2;
            }

            return false;
        }
    }
}
