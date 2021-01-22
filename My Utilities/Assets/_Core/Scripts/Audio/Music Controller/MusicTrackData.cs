using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.MusicController
{
    [CreateAssetMenu(fileName = "Music Connector", menuName = "Rod/Music Controller/Track")]
    public class MusicTrackData : ScriptableObject
    {
        [SerializeField]
        string key;
        [SerializeField]
        AudioClip audioClip;
        [SerializeField]
        float volume;
        [SerializeField]
        float fadeDuration;

        public string Key => key;
        public AudioClip AudioClip => audioClip;
        public float Volume => volume;
        public float FadeDuration => fadeDuration;
    }
}