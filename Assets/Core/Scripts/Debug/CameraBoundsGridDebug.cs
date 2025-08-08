using System;
using UnityEngine;

namespace Core.Scripts.Debug
{
    public class CameraBoundsGridDebug : MonoBehaviour
    {
        public Camera camera;
        public bool showGrid;
        public int gridLeft = 20;
        public int gridRight = 20;
        public Color gridColor = Color.red;
    }
}