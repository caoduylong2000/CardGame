using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        private AudioManager(){}
        public static AudioManager Instance { get; private set; }

        public AudioSource musicSource;
        public AudioSource sfxSource;
        public AudioSource buttonSource;

        [SerializeField] private List<SoundProfileData> soundProfileDataList;

        [Header("Theme Song")]
        public AudioClip mainThemeSong;
        public AudioClip introThemeSong;
        public AudioClip battleThemeSong;

        private Dictionary<AudioActionType, SoundProfileData> _audioDict = new Dictionary<AudioActionType, SoundProfileData>();
        
        #region Setup
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                for (int i = 0; i < Enum.GetValues(typeof(AudioActionType)).Length; i++)
                    _audioDict.Add((AudioActionType)i,soundProfileDataList.FirstOrDefault(x=>x.AudioType == (AudioActionType)i));
            }
        }

        #endregion

        #region Public Methods

        public void PlayMusic(AudioClip clip)
        {
            if (!clip) return;
            
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlayMusic(AudioActionType type)
        {
            var clip = _audioDict[type].GetRandomClip();
            if (clip)
                PlayMusic(clip);
        }

        public void PlayOneShot(AudioActionType type)
        {
            var clip = _audioDict[type].GetRandomClip();
            if (clip)
                PlayOneShot(clip);
        }
        
        public void PlayOneShotButton(AudioActionType type)
        {
            var clip = _audioDict[type].GetRandomClip();
            if (clip)
                PlayOneShotButton(clip);
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (clip)
                sfxSource.PlayOneShot(clip);
        }
        
        public void PlayOneShotButton(AudioClip clip)
        {
            if (clip)
                buttonSource.PlayOneShot(clip);
        }

        #endregion
    }
}
