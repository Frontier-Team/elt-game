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
            var grids = Object.FindObjectsOfType<CameraBoundsGridDebug>();
            foreach (var grid in grids)
            {
                if (!grid.showGrid || grid.camera == null || !grid.camera.orthographic)
                    continue;

                var cam = grid.camera;
                var camHeight = cam.orthographicSize * 2f;
                var camWidth = camHeight * cam.aspect;
                var yMin = cam.transform.position.y - camHeight / 2f;
                var yMax = cam.transform.position.y + camHeight / 2f;

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

                    Handles.DrawLine(bottomLeft, topLeft);
                    Handles.DrawLine(bottomRight, topRight);
                    Handles.DrawLine(bottomLeft, bottomRight);
                    Handles.DrawLine(topLeft, topRight);
                }
            }
        }
    }

}