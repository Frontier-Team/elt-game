using Core.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Core.Scripts.Audio.AudioType;

namespace _Game.Scripts.Interactions
{
    public class UIPopup : PopupBase
    {
        [SerializeField] private GameObject rootPopup;
        [SerializeField] private AudioClip sfxUIOpen;
        [SerializeField] private AudioClip sfxUIClose;

        public bool IsShown { get; private set; } = false;
        
        private void Start()
        {
        }

        public override void ShowPage()
        {
            uiTextElement.pageToDisplay = 1;
            AudioManager.Instance.Play(sfxUIOpen, AudioType.UI, false, 0.6f);
            base.ShowPage();
            rootPopup.SetActive(true);
            IsShown = rootPopup.activeSelf;
        }

        public override void HidePage()
        {
            rootPopup.SetActive(false);
            AudioManager.Instance.Play(sfxUIClose, AudioType.UI, false, 0.6f);
            base.HidePage();
            ResetTextElement();
            IsShown = rootPopup.activeSelf;
        }
    }
}