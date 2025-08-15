using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Interactions
{
    public abstract class PopupBase : MonoBehaviour
    {
        public event Action OnShowPage;
        public event Action OnHidePage;
        public event Action OnNextPage;
        public event Action OnPreviousPage;
        public event Action OnUpdateText;
        
        [SerializeField] protected TMP_Text uiTextElement;
        [SerializeField] protected Button nextPageButton;
        [SerializeField] protected Button previousPageButton;
        public bool IsEnabled = false;
        protected int CurrentPage = 1;

        protected virtual void Start()
        {
            ResetTextElement();
        }

        public virtual void ShowPage()
        {
            IsEnabled = true;
            OnShowPage?.Invoke();
        }

        public virtual void HidePage()
        {
            OnHidePage?.Invoke();
        }

        private void Update()
        {
            if (!IsEnabled)
            {
                return;
            }
            CurrentPage = uiTextElement.pageToDisplay;
            
            if (uiTextElement.textInfo.pageCount > 1 && 
                CurrentPage > 1)
            {
                nextPageButton.interactable = true;
                previousPageButton.interactable = true;
            }

            if (uiTextElement.textInfo.pageCount == 1 &&
                CurrentPage == 1)
            {
                nextPageButton.interactable = false;
                previousPageButton.interactable = false;
            }

            if (uiTextElement.textInfo.pageCount > 1 &&
                CurrentPage == 1)
            {
                previousPageButton.interactable = false;
                nextPageButton.interactable = true;
            }

            if (uiTextElement.textInfo.pageCount > 1 &&
                CurrentPage == uiTextElement.textInfo.pageCount)
            {
                previousPageButton.interactable = true;
                nextPageButton.interactable = false;
            }
        }
        
        public virtual void NextPage()
        {
            if (uiTextElement.textInfo.pageCount > CurrentPage)
            {
                uiTextElement.pageToDisplay++;
            }
            OnNextPage?.Invoke();
        }

        public virtual void PreviousPage()
        {
            if (uiTextElement.textInfo.pageCount > 0 && CurrentPage > 0)
            {
                uiTextElement.pageToDisplay--;
            }
            OnPreviousPage?.Invoke();
        }

        public virtual void UpdateTextElement(string text)
        {
            uiTextElement.text = text;
            OnUpdateText?.Invoke();
        }

        public virtual void ResetTextElement()
        {
            uiTextElement.text = string.Empty;
            CurrentPage = 1;
            uiTextElement.pageToDisplay = 1;
            OnUpdateText?.Invoke();
        }
    }
}