using System;
using Core.Scripts.Debug;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    [CustomEditor(typeof(CameraBoundsGridDebug))]
    public class CameraBoundsGridDebugEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var grid = (CameraBoundsGridDebug)target;

            EditorGUILayout.LabelField("Camera Grid Debug", EditorStyles.boldLabel);

            grid.showGrid = EditorGUILayout.Toggle("Show Grid", grid.showGrid);
            grid.camera = (Camera)EditorGUILayout.ObjectField("Target Camera", grid.camera, typeof(Camera), true);
            grid.gridLeft = EditorGUILayout.IntField("Pages Left", grid.gridLeft);
            grid.gridRight = EditorGUILayout.IntField("Pages Right", grid.gridRight);
            grid.gridColor = EditorGUILayout.ColorField("Grid Color", grid.gridColor);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(grid);
                SceneView.RepaintAll();
            }
        }
    }
}