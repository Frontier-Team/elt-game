using UnityEngine;

namespace _Game.Scripts.Game
{
    public class RestartGameDebug : MonoBehaviour
    {
        public void RestartGame()
        {
            GameStateController.Instance.TransitionToState(GameState.Restart);
        }
    }
}