using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public bool IsActive
        {
            get => gameObject.activeSelf;
            set=> gameObject.SetActive(value);
        }
        
        public void UpdateText()
        {
            text.text = "Game Over\nScore: " + ScoreTracker.Score;
        }
    }
}