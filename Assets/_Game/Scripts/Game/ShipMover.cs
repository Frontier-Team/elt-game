using System;
using UnityEngine;

namespace _Game.Scripts.Game
{
    public class ShipMover : MonoBehaviour
    {
        [SerializeField] private float shipSpeed = 2f;

        private Vector2 initialLocation;
        
        private void Awake()
        {
            initialLocation = transform.position;
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector2.left * (shipSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("ShipReset"))
            {
                transform.position = initialLocation;
            }
        }
    }
}