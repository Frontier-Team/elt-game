using System;
using UnityEngine;

namespace Core.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource musicSource;
        private AudioSource sfxSource;
        private AudioSource uiSource;
        private AudioSource ambienceSource;

        private bool isMuted = false;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupSources();
        }

        private void SetupSources()
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            uiSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            ambienceSource = gameObject.AddComponent<AudioSource>();

            musicSource.loop = true;
            ambienceSource.loop = true;
        }

        public void Play(AudioClip clip, AudioType type = AudioType.SFX, bool loop = false, float volume = 1f)
        {
            
            var source = GetSource(type);
            if (!source) return;

            if (loop)
            {
                source.clip = clip;
                source.volume = volume;
                source.loop = true;
                if (!source.isPlaying) source.Play();
            }
            else
            {
                source.PlayOneShot(clip, volume);
            }
        }

        public void Stop(AudioType type)
        {
            var source = GetSource(type);
            if (source && source.isPlaying) source.Stop();
        }

        public void StopAll()
        {
            musicSource.Stop();
            sfxSource.Stop();
            uiSource.Stop();
            ambienceSource.Stop();
        }

        private AudioSource GetSource(AudioType type) => type switch
        {
            AudioType.Music => musicSource,
            AudioType.SFX => sfxSource,
            AudioType.UI => uiSource,
            AudioType.Ambience => ambienceSource,
            _ => null
        };
        
        private void SetVolume(float volume)
        {
            musicSource.volume = volume;
            sfxSource.volume = volume;
            uiSource.volume = volume;
            ambienceSource.volume = volume;
        }

        private void MuteAll(bool mute)
        {
            isMuted = mute;
            SetVolume(mute ? 0f : 1f);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            MuteAll(!hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            MuteAll(pauseStatus);
        }
    }
}
