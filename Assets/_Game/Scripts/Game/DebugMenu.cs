using _Game.Scripts.Interactions;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Game
{
    public class DebugMenu : MonoBehaviour
    {
        [SerializeField] private Slider gameSpeed;
        [SerializeField] private GameObject debugRoot;
        [SerializeField] private UIPopup mainUI;
        
        private void Awake()
        {
            gameSpeed.onValueChanged.AddListener(UpdateSpeed);
        }

        private void UpdateSpeed(float speed)
        {
            Time.timeScale = speed;
        }

        public void OnClose()
        {
            debugRoot.SetActive(false);
        }

        public void OnOpen()
        {
            debugRoot.SetActive(true);
        }

        public void ShowUI()
        {
            mainUI.ShowPage();
        }

        public void HideUI()
        {
            mainUI.HidePage();
        }
    }
}