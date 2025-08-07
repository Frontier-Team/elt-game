using UnityEngine;

namespace Core.UI
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float horizontalDeadzone = 0.5f;
        [SerializeField] private float verticalDeadzone = 0.5f;
        private float smoothSpeed = 0.125f;
        
        private Vector3 targetPosition;
        private Camera camera;
        
        private void Start()
        {
            camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var position = transform.position;
            
            var xOffset = target.position.x - position.x;
            var yOffset = target.position.y - position.y;

            var targetX = position.x;
            var targetY = position.y;

            if (Mathf.Abs(xOffset) > horizontalDeadzone)
            {
                targetX = target.position.x - Mathf.Sign(xOffset) * horizontalDeadzone;
            }

            if (Mathf.Abs(yOffset) > verticalDeadzone)
            {
                targetY = target.position.y - Mathf.Sign(yOffset) * verticalDeadzone;
            }

            targetPosition = new Vector3(targetX, targetY, transform.position.z);
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, smoothSpeed);
        }
    }
}