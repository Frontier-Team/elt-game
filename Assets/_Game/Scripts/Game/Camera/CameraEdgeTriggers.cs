using _Game.Scripts.Game;
using UnityEngine;

public class CameraEdgeTriggers : MonoBehaviour
{
    [SerializeField] private GameObject triggerPrefab;
    [SerializeField] private float triggerCooldown = 0.5f;
    [SerializeField] private int triggerWidth = 20;
    
    private GameObject leftTrigger, rightTrigger;
    private Camera cam;
    private float lastScreenWidth, lastScreenHeight;

    void Start()
    {
        cam = Camera.main;
        CreateTriggers();
        UpdateTriggers();
    }

    private void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            UpdateTriggers();
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
        }
    }

    private void CreateTriggers()
    {
        leftTrigger = Instantiate(triggerPrefab, transform);
        leftTrigger.GetComponent<CameraShiftTrigger>()?.ConfigureTrigger(ShiftDirection.Left, triggerCooldown);
        rightTrigger = Instantiate(triggerPrefab, transform);
        rightTrigger.GetComponent<CameraShiftTrigger>()?.ConfigureTrigger(ShiftDirection.Right, triggerCooldown);
    }

    private void UpdateTriggers()
    {
        if (!cam)
        {
            return;
        }

        var edgeOffset = PixelToWorldSize(triggerWidth);
        var bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        var topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        var minX = bottomLeft.x + edgeOffset - transform.position.x;
        var maxX = topRight.x - edgeOffset - transform.position.x;
        var centerY = (bottomLeft.y + topRight.y) / 2 - transform.position.y;

        leftTrigger.transform.localPosition = new Vector3(minX, centerY, 0);
        rightTrigger.transform.localPosition = new Vector3(maxX, centerY, 0);

        ResizeTriggers();
    }

    private void ResizeTriggers()
    {
        var thickness = PixelToWorldSize(triggerWidth);
        var height = Mathf.Abs(cam.orthographicSize * 2);

        leftTrigger.transform.localScale = new Vector3(thickness, height, 1);
        rightTrigger.transform.localScale = new Vector3(thickness, height, 1);
    }

    private float PixelToWorldSize(float pixels)
    {
        return cam.ScreenToWorldPoint(new Vector3(pixels, 0, cam.nearClipPlane)).x - cam.ScreenToWorldPoint(Vector3.zero).x;
    }
}
