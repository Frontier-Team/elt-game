using System;

namespace _Game.Scripts.Game
{
    public static class ScoreTracker
    {
        public static int Score { get; private set; }
        public static event Action OnScoreUpdate; 
        
        public static void IncreaseScore(int value)
        {
            Score += value;
            OnScoreUpdate?.Invoke();
        }
        
        public static void ResetScore()
        {
            Score = 0;
            OnScoreUpdate?.Invoke();
        }
    }
}