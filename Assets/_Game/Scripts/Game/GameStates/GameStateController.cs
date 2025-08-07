using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Game
{
    public class GameStateController : MonoBehaviour
    {
        //Very quick & dirty implementation of state machine
        public GameState CurrentGameState { get; private set; }

        [SerializeField] private GameOverScreen gameOverScreen;
        
        public static GameStateController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            if (gameOverScreen == null)
            {
                gameOverScreen = FindFirstObjectByType<GameOverScreen>();
            }
            TransitionToState(GameState.Playing);
        }

        public bool TransitionToState(GameState targetState)
        {
            if(IsValidTransition(targetState))
            {
                CurrentGameState = targetState;
                OnStateChanged();
                return true;
            }
            return false;
        }

        private void OnStateChanged()
        {
            switch (CurrentGameState)
            {
                case GameState.Playing:
                    OnPlayingStateEntered();
                    break;
                case GameState.Restart:
                    OnRestartStateEntered();
                    break;
                case GameState.GameOver:
                    OnGameOverStateEntered();
                    break;
                default:
                    Debug.LogError($"State {CurrentGameState} not implemented");
                    break;
            }
        }

        private bool IsValidTransition(GameState targetState)
        {
            switch(CurrentGameState)
            {
                case GameState.Playing:
                    return targetState == GameState.Restart || targetState == GameState.GameOver;
                case GameState.Restart:
                    return targetState == GameState.Playing;
                case GameState.GameOver:
                    return targetState == GameState.Restart;
                default:
                    return false;
            }
        }
        
        private void OnPlayingStateEntered()
        {
        }
        
        private void OnRestartStateEntered()
        {
            ScoreTracker.ResetScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if(gameOverScreen.gameObject.activeSelf)
            {
                gameOverScreen.IsActive = false;
            }
        }
        
        private void OnGameOverStateEntered()
        {
            if (!gameOverScreen.IsActive)
            {
                gameOverScreen.IsActive = true;
                gameOverScreen.UpdateText();
            }
        }
    }
}