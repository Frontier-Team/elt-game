using System;
using _Game.Player;
using _Game.Scripts.Game;
using UnityEngine;

public class CameraShiftTrigger : MonoBehaviour
{
    private ShiftDirection shiftDirection;
    private float triggerCooldown = 0.5f;

    private CameraMovementController cameraMoveController;
    private PlayerMovementControllerMouse playerMoveController;
    private static ShiftDirection? lastTriggeredDirection = null;
    private bool canTrigger = true;

    public void ConfigureTrigger(ShiftDirection trigDirection, float cooldown)
    {
        shiftDirection = trigDirection;
        triggerCooldown = cooldown;
    }
    
    private void Start()
    {
        cameraMoveController = FindFirstObjectByType<CameraMovementController>();
        playerMoveController = FindFirstObjectByType<PlayerMovementControllerMouse>();
        cameraMoveController.OnCameraMoveStart += OnCameraMoveStart;
    }

    private void OnDisable()
    {
        cameraMoveController.OnCameraMoveStart -= OnCameraMoveStart;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTrigger || !other.CompareTag("Player") || cameraMoveController.IsMoving)
        {
            return;
        }

        if (lastTriggeredDirection == shiftDirection)
        {
            return;
        }

        lastTriggeredDirection = shiftDirection;
        canTrigger = false;

        cameraMoveController.ShiftCamera((int)shiftDirection);
    }

    private void OnCameraMoveStart()
    {
        if (lastTriggeredDirection == null)
        {
            return;
        }
        playerMoveController.MoveToShiftPosition((int)lastTriggeredDirection);
        Invoke(nameof(ResetTrigger), triggerCooldown);
    }

    private void ResetTrigger()
    {
        canTrigger = true;
        lastTriggeredDirection = null;
    }
}