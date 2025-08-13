using UnityEngine;

namespace _Game.Scripts.Interactions
{
    public class VotePostBox : MonoBehaviour, IInteractable
    {
        private UIPopup popup;
        private string popupText;
        
        public void Interact()
        {
            popup.UpdateTextElement(popupText);
            popup.ShowPage();
        }
    }
}