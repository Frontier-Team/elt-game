using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace _Game.Scripts.Interactions
{
    public class VotingController : MonoBehaviour
    {
        public bool CanVote { get; private set; } = true;
        public bool HasVoted { get; private set; } = false;
        public int VoteChoice { get; private set; }

        [SerializeField] private string voteApiHost = "https://voting-system-lyart.vercel.app";
        [SerializeField] private string voteApiPath = "/api/vote/";
        
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
            VoteChoice = default;
        }

        public async Task<bool> PostVote(int voteId)
        {
            if (!CanVote)
            {
                return false;
            }
            
            var url = $"{voteApiHost}{voteApiPath}{voteId}";
            var delayMs = 200;

            for (var attempt = 1; attempt <= 3; attempt++)
            {
                using var req = UnityWebRequest.PostWwwForm(url, string.Empty);
                await req.SendWebRequest().AsTask();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    HasVoted = true;
                    VoteChoice = voteId;
                    DisableVoting();
                    return true;
                }

                var retryable = req.result == UnityWebRequest.Result.ConnectionError
                                || req.result == UnityWebRequest.Result.DataProcessingError
                                || (req.result == UnityWebRequest.Result.ProtocolError && req.responseCode >= 500);

                if (!retryable || attempt == 3)
                    return false;

                await Task.Delay(delayMs);
                delayMs *= 2;
            }

            return false;
        }

    }
}