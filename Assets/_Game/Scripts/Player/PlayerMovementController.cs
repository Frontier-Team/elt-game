using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace _Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public event Action OnJump;
        public event Action<Vector2> OnMove;

        private InputAction jumpAction;
        private InputAction moveAction;

        private Rigidbody2D rb;
        private PlayerInput playerInput;
        private float Move;

        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float rayLength = 0.5f; 

        private Animator animator;
        private bool isRight;
        private bool isInAir;
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            jumpAction = playerInput.actions["Jump"];
            moveAction = playerInput.actions["Move"];

            jumpAction.performed += _ => HandleJump();
            moveAction.performed += ctx => HandleMove(ctx.ReadValue<Vector2>());
            moveAction.canceled += _ => HandleMove(Vector2.zero);
        }

        public void Start()
        {
            isRight = true;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void HandleMove(Vector2 moveVal)
        {
            Move = moveVal.x;
            OnMove?.Invoke(moveVal);
        }

        private void HandleJump()
        {
            if (IsGrounded()) 
            {
                rb.AddForce(new Vector2(0, jumpForce * 10));
                OnJump?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = new Vector2(Move * speed, rb.linearVelocityY);

            animator.SetBool(IsRunning, Move != 0);

            if (!isRight && Move > 0)
                Flip();
            else if (isRight && Move < 0)
                Flip();
        }

        private void Flip()
        {
            isRight = !isRight;
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        private bool IsGrounded()
        { 
             var layerMask = ~LayerMask.GetMask("Player");
             var hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, layerMask);
            
                 if(hit.collider != null && 
                    hit.collider.GetComponent<TilemapCollider2D>())
                {
                    Debug.Log("Hit: " + hit.collider);
                    return true;
                }

                return false;
        }
    }
}
