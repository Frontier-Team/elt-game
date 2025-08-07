using System;
using UnityEngine;

namespace Core.Scripts.Debug
{
    public class CameraBoundsGridDebug : MonoBehaviour
    {
        public Camera camera;
        public int gridLeft = 100;
        public int gridRight = 100;
        public Color gridColor = Color.red;
    }
}