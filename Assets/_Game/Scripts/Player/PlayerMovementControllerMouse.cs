using System;
using _Game.Scripts.Interactions;
using Core.Scripts.Audio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Serialization;
using AudioType = Core.Scripts.Audio.AudioType;

namespace _Game.Player
{
    public class PlayerMovementControllerMouse : MonoBehaviour
    {
        public event Action<Vector2> OnMoveClicked;
        public event Action<IInteractable> OnInteract;

        private InputAction clickAction;
        private PlayerControls controls;
        private Animator animator;

        public float Speed { get; private set; } = 5f;
        public bool CanMove { get; private set; } = true;

        [SerializeField] private float interactableRange = 1.5f;
        [SerializeField] private float playerShiftAmount = 1f;
        [SerializeField] private AudioClip clickSound;
        private Vector2 targetPosition;
        private bool isMovingToClick = false;
        private bool isMoving = false;
        private float direction;
        private bool mouseHeld = false;

        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int AnimationDirection = Animator.StringToHash("direction");
        private LayerMask clickableLayer;
        private LayerMask moveableLayer;

        private void Awake()
        {
            TouchSimulation.Enable();
            controls = new PlayerControls();
            clickAction = controls.Player.Press;
            clickAction.performed += HandleMouseClick;
            
            animator = GetComponent<Animator>();
            clickableLayer = LayerMask.GetMask("Clickable");
            moveableLayer = LayerMask.GetMask("Moveable");
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void FixedUpdate()
        {
            if (!Mouse.current.leftButton.isPressed)
            {
                mouseHeld = false;
            }
            
            if (isMoving)
            {
                MovePlayer(targetPosition);
                return;
            }

            if (isMovingToClick)
            {
                MovePlayer(targetPosition);
                isMovingToClick = Vector2.Distance(transform.position, targetPosition) >= 0.1f;
            }
            
            animator.SetFloat(AnimationDirection, direction);
        }

        public void MoveToShiftPosition(int direction)
        {
            isMoving = true;
            targetPosition = new Vector2(transform.position.x + (playerShiftAmount * direction), transform.position.y);
        }

        public void EnableMovement()
        {
            CanMove = true;
        }

        public void DisableMovement()
        {
            CanMove = false;
        }

        private void MovePlayer(Vector2 destination)
        {
            if (!CanMove)
            {
                return;
            }
            
            Vector2 currentPosition = transform.position;
            Vector2 movement = destination - currentPosition;

            if (movement.sqrMagnitude > 0)
            {
                if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
                    direction = movement.x > 0 ? 3f : 1f;
                else
                    direction = movement.y > 0 ? 2f : 0f;
                animator.SetBool(IsRunning, true);
            }

            transform.position = Vector2.MoveTowards(currentPosition, destination, Speed * Time.fixedDeltaTime);
    
            if (Vector2.Distance(currentPosition, destination) < 0.01f)
            {
                transform.position = destination;
                isMoving = false;
                animator.SetBool(IsRunning, false);
            }

            if (movement.sqrMagnitude <= 0.1f)
            {
                animator.SetBool(IsRunning, false);
            }
            //FlipSprite(movement.x);
        }

        private void HandleMouseClick(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (!Application.isFocused)
            {
                return;
            }
#endif
            
            if (mouseHeld)
            {
                return;
            }
    
            mouseHeld = true;
            var clickPosition = context.ReadValue<Vector2>();

            if (!IsValidScreenPosition(clickPosition))
            {
                return;
            }
            var clickToWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(clickPosition.x, clickPosition.y, 0f));

            var clickedObject = Physics2D.OverlapPoint(clickToWorldPosition, clickableLayer);
            if (clickedObject != null && 
                clickedObject.TryGetComponent(out IInteractable interactable) &&
                clickedObject.CompareTag("Interactable"))
            {
                var playerDistance = Vector2.Distance(transform.position, clickedObject.transform.position);
                var clickDistance = Vector2.Distance(clickToWorldPosition, clickedObject.transform.position);
                
                if (playerDistance <= interactableRange &&
                    clickDistance <= interactableRange)
                {
                    AudioManager.Instance.Play(clickSound, AudioType.SFX, false, 0.2f);

                    OnInteract?.Invoke(interactable);
                    return;
                }
            }

            if (!CanMove)
            {
                return;
            }
            
            var moveable = Physics2D.OverlapPoint(clickToWorldPosition, moveableLayer);
            if (moveable == null)
            {
                return;
            }
            AudioManager.Instance.Play(clickSound, AudioType.SFX, false, 0.2f);

            targetPosition = clickToWorldPosition;
            isMovingToClick = true;
            OnMoveClicked?.Invoke(clickPosition);
        }

        private void FlipSprite(float moveX)
        {
            if (Mathf.Abs(moveX) > 0.01f)
            {
                transform.localScale = new Vector3(moveX > 0 ? 1 : -1, 1, 1);
            }
        }

        private bool IsValidScreenPosition(Vector2 position)
        {
            return position is { x: >= 0, y: >= 0 } &&
                   position.x <= Screen.width && position.y <= Screen.height;
        }
    }
}
