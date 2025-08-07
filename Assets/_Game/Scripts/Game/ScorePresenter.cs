using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        
        private void Start()
        {
            ScoreTracker.OnScoreUpdate += UpdateScore;
            ScoreTracker.ResetScore();
        }

        private void UpdateScore()
        {
            scoreText.text = ScoreTracker.Score.ToString(); 
        }
    }
}