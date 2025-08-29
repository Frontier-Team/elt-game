using Core.Scripts.Debug;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    [InitializeOnLoad]
    public static class CameraGridGizmoDrawer
    {
        static CameraGridGizmoDrawer()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var grids = Object.FindObjectsByType<CameraBoundsGridDebug>(FindObjectsSortMode.None);
            
            foreach (var grid in grids)
            {
                if (!grid.showGrid || grid.camera == null || !grid.camera.orthographic)
                    continue;

                var cam = grid.camera;
                var camHeight = cam.orthographicSize * 2f;
                var camWidth = camHeight * cam.aspect;

                var yMin = cam.transform.position.y - camHeight / 2f;
                var yMax = cam.transform.position.y + camHeight / 2f;
                var yCenter = (yMin + yMax) * 0.5f;

                var triggerWidth = PixelToWorldSize(cam, grid.triggerWidthPixels);
                var triggerHeight = camHeight;

                Handles.color = grid.gridColor;

                for (var i = -grid.gridLeft; i <= grid.gridRight; i++)
                {
                    var pageCenterX = i * camWidth;
                    var xLeft = pageCenterX - camWidth / 2f;
                    var xRight = pageCenterX + camWidth / 2f;

                    var bottomLeft = new Vector3(xLeft, yMin, 0f);
                    var bottomRight = new Vector3(xRight, yMin, 0f);
                    var topLeft = new Vector3(xLeft, yMax, 0f);
                    var topRight = new Vector3(xRight, yMax, 0f);

                    // Grid box
                    Handles.DrawLine(bottomLeft, topLeft);
                    Handles.DrawLine(bottomRight, topRight);
                    Handles.DrawLine(bottomLeft, bottomRight);
                    Handles.DrawLine(topLeft, topRight);

                    if (!grid.showEdgeTriggers) continue;

                    var fillColor = new Color(1f, 0.5f, 0f, 0.1f);
                    var lineColor = new Color(1f, 0.5f, 0f, 0.9f);
                    var size = new Vector3(triggerWidth, triggerHeight, 0.01f);

                    var leftCenter = new Vector3(xLeft + triggerWidth * 0.5f, yCenter, 0f);
                    var rightCenter = new Vector3(xRight - triggerWidth * 0.5f, yCenter, 0f);

                    var leftVerts = new[]
                    {
                        new Vector3(leftCenter.x - size.x * 0.5f, leftCenter.y - size.y * 0.5f, 0f),
                        new Vector3(leftCenter.x - size.x * 0.5f, leftCenter.y + size.y * 0.5f, 0f),
                        new Vector3(leftCenter.x + size.x * 0.5f, leftCenter.y + size.y * 0.5f, 0f),
                        new Vector3(leftCenter.x + size.x * 0.5f, leftCenter.y - size.y * 0.5f, 0f)
                    };

                    var rightVerts = new[]
                    {
                        new Vector3(rightCenter.x - size.x * 0.5f, rightCenter.y - size.y * 0.5f, 0f),
                        new Vector3(rightCenter.x - size.x * 0.5f, rightCenter.y + size.y * 0.5f, 0f),
                        new Vector3(rightCenter.x + size.x * 0.5f, rightCenter.y + size.y * 0.5f, 0f),
                        new Vector3(rightCenter.x + size.x * 0.5f, rightCenter.y - size.y * 0.5f, 0f)
                    };

                    Handles.DrawSolidRectangleWithOutline(leftVerts, fillColor, lineColor);
                    Handles.DrawSolidRectangleWithOutline(rightVerts, fillColor, lineColor);
                }
            }
        }

        private static float PixelToWorldSize(Camera cam, float pixels)
        {
            var from = cam.ScreenToWorldPoint(Vector3.zero);
            var to = cam.ScreenToWorldPoint(new Vector3(pixels, 0f, 0f));
            return Mathf.Abs(to.x - from.x);
        }
    }
}
