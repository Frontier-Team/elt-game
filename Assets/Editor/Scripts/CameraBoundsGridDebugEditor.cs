using System;
using Core.Scripts.Debug;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    [CustomEditor(typeof(CameraBoundsGridDebug))]
    public class CameraBoundsGridDebugEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var grid = target as CameraBoundsGridDebug;

            if (grid.camera == null || !grid.camera.orthographic)
            {
                return;
            }

            var camera = grid.camera;
            var camHeight = camera.orthographicSize * 2f;
            var camWidth = camHeight * camera.aspect;
            var yMin = camera.transform.position.y - camHeight;
            var yMax = camera.transform.position.y + camHeight;

            Handles.color = grid.gridColor;

            for (var i = -grid.gridLeft; i <= grid.gridRight; i++)
            {
                var pageCenterX = i * camWidth;
                var xLeft = pageCenterX - camWidth / 2f;
                var xRight = pageCenterX + camWidth / 2f;
                
                Handles.DrawLine(new Vector3(xLeft, yMin, 0), new Vector3(xLeft, yMax, 0));
                Handles.DrawLine(new Vector3(xRight, yMin, 0), new Vector3(xRight, yMax, 0));
            }
        }
    }
}