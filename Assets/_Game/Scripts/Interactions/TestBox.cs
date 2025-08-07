using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Interactions
{
    public class TestBox : MonoBehaviour, IInteractable
    {
        private SpriteRenderer _spriteRenderer;
        private Color cachedColor;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            cachedColor = _spriteRenderer.color;
        }
        
        public void Interact()
        {
            StartCoroutine(ChangeColour());
        }

        private IEnumerator ChangeColour()
        {
            _spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(1f);
            _spriteRenderer.color = cachedColor;
        }
    }
}