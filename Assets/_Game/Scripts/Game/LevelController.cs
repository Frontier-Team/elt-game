using System;
using Core.Scripts.Audio;
using UnityEngine;
using AudioType = Core.Scripts.Audio.AudioType;

namespace _Game.Scripts.Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private AudioClip bgSound;
        [SerializeField] private float ambientVolume = 0.5f;
        private void Start()
        {
            AudioManager.Instance.Play(bgSound, AudioType.Ambience, true, ambientVolume);
        }
    }
}