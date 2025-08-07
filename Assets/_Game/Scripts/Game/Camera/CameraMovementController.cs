using System;
using Core.Scripts.Audio;
using UnityEngine;
using AudioType = Core.Scripts.Audio.AudioType;

namespace _Game.Scripts.Game
{
    public class CameraMovementController : MonoBehaviour
    {
        [SerializeField] private AudioClip sfxMoveSound;
        public event Action OnCameraMoveStart;
        public event Action OnCameraMoveComplete;

        [SerializeField] private float moveSpeed = 5f;
        private float cameraTargetX;
        public bool IsMoving { get; private set; }
        private static bool isCameraShifting = false;

        private void Start()
        {
            cameraTargetX = transform.position.x;
        }

        private void FixedUpdate()
        {
            if (!IsMoving)
            {
                return;
            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(cameraTargetX, transform.position.y, transform.position.z), Time.fixedDeltaTime * moveSpeed);

            if (Mathf.Abs(transform.position.x - cameraTargetX) < 0.01f)
            {
                transform.position = new Vector3(cameraTargetX, transform.position.y, transform.position.z);
                IsMoving = false;
                isCameraShifting = false;
                OnCameraMoveComplete?.Invoke();
            }
        }

        public void ShiftCamera(int direction)
        {
            if (IsMoving || isCameraShifting)
            {
                return;
            }

            isCameraShifting = true;
            cameraTargetX += direction * (2f * Camera.main.orthographicSize * Camera.main.aspect);
            OnCameraMoveStart?.Invoke();
            if (sfxMoveSound != null)
            {
                AudioManager.Instance.Play(sfxMoveSound, AudioType.UI);
            }
            IsMoving = true;
        }
    }
}
