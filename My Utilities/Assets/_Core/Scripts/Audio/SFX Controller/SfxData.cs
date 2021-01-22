using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.SfxController
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Rod/Sfx Controller/Sfx Data")]
    public class SfxData : ScriptableObject
    {
        [SerializeField]
        string audioKey;
        [SerializeField]
        AudioClip[] audioClips;
        [SerializeField]
        bool isLoop;
        [SerializeField]
        float volume = 1;
        [SerializeField, Range(0, 1)]
        float pitchVariance = 0;
        [SerializeField]
        bool allowMultiplePlay;
        [SerializeField]
        bool forceToPlay;
        /// <summary>
        /// Identificador
        /// </summary>
        public string AudioKey
        {
            get
            {
                return audioKey;
            }
        }
        /// <summary>
        /// Obtener un Clip de manera aleatoria de la lista de clips
        /// </summary>
        public AudioClip Clip
        {
            get
            {
                AudioClip selectedClip = null;
                if (audioClips.Length > 0)
                {
                    if (audioClips.Length > 1)
                    {
                        selectedClip = audioClips[Random.Range(0, audioClips.Length)];
                    }
                    else
                        selectedClip = audioClips[0];
                }
                return selectedClip;
            }
        }
        /// <summary>
        /// Volumen máximo
        /// </summary>
        public float Volume
        {
            get
            {
                return volume;
            }
        }
        /// <summary>
        /// Variación de pitch
        /// </summary>
        public float PitchVariance
        {
            get
            {
                return pitchVariance;
            }
        }
        /// <summary>
        /// Permitir reproducir muchos sonidos con este id a la vez
        /// </summary>
        public bool AllowMultiplePlay => allowMultiplePlay;
        /// <summary>
        /// Reproducir en loop
        /// </summary>
        public bool IsLoop => isLoop;
        /// <summary>
        /// Forzar a reproducir aunque ya se haya alcanzado el límite
        /// </summary>
        public bool ForceToPlay => forceToPlay;
    }
}