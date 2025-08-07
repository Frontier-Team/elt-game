using UnityEngine;

namespace _Game.Scripts.Game
{
    public class KillObject : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameStateController.Instance.TransitionToState(GameState.GameOver);
            }
        }
    }
}