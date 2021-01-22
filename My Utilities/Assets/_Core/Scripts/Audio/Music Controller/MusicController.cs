using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Rod.Utilities.MusicController
{
    /// <summary>
    /// Sistema para controlar música
    /// </summary>
    public class MusicController : MonoBehaviour
    {
        /// <summary>
        /// Referencia del scriptable para conectar con el sistema
        /// </summary>
        [SerializeField]
        private MusicConnector musicConnector;

        /// <summary>
        /// Lista de pistas disponibles
        /// </summary>
        [SerializeField]
        private AudioTrackList tracklist;

        /// <summary>
        /// Audio sources que se usarán para reproducir la música
        /// </summary>
        [SerializeField]
        private AudioSource[] audioSources = new AudioSource[2];

        /// <summary>
        /// índice del último AudioSource usado para reproducir música
        /// </summary>
        private int activeSource;

        /// <summary>
        /// Información de la última pista reproducida
        /// </summary>
        private MusicTrackData currentTrack;

        private void Awake()
        {
            //Iniciar la referencia del connector para que otros sistemas puedan acceder externamente
            musicConnector.Controller = this;
        }

        private void Start()
        {
            for (int i = 0; i < audioSources.Length; i++)
                audioSources[i].loop = true;
        }

        /// <summary>
        /// Iniciar la reproducción de una pista por su key id
        /// </summary>
        /// <param name="key">Idendificador de la pista</param>
        /// <param name="loop">Reproducir en loop o solo una vez</param>
        /// <param name="randomTime">Iniciar reproducción en un tiempo aleatorio</param>
        public void PlayMusic(string key, bool loop, bool randomTime)
        {
            MusicTrackData audioTrack = tracklist.GetTrack(key);
            if (audioTrack != null)
            {
                if (currentTrack != audioTrack)
                {
                    SetMusic(audioTrack, loop, randomTime);
                    currentTrack = audioTrack;
                }
            }
        }

        /// <summary>
        /// Detener la reproducción de la música con las configuraciones de fade
        /// indicados en su Data
        /// </summary>
        public void StopMusic()
        {
            if (audioSources[activeSource].isPlaying)
                audioSources[activeSource].DOFade(0, currentTrack.FadeDuration).onComplete += () => audioSources[activeSource].Stop();

        }

        /// <summary>
        /// Realizar un fade de la pista actual
        /// </summary>
        /// <param name="volumeTarget">Volumen después de la transición</param>
        /// <param name="duration">Duración de la transición</param>
        public void FadeCurrentTo(float volumeTarget, float duration)
        {
            AudioSource audioSource = audioSources[activeSource];
            audioSource.DOFade(volumeTarget, duration);
        }

        /// <summary>
        /// Iniciar reproducción de track seleccionado
        /// </summary>
        /// <param name="audioTrack">Descripción de música a reproducir</param>
        /// <param name="loop">Reproducir en loop o no</param>
        /// <param name="randomTime">Iniciar reproducción en un tiempo aleatorio</param>
        void SetMusic(MusicTrackData audioTrack, bool loop, bool randomTime)
        {
            int nextSource = activeSource + 1 > 1 ? 0 : 1;
            AudioSource audioSource = audioSources[nextSource];
            audioSource.loop = loop;

            if (audioSources[activeSource].isPlaying)
            {
                AudioSource previusSource = audioSources[activeSource];
                previusSource.DOFade(0, audioTrack.FadeDuration).onComplete += () => previusSource.Stop();
            }
            audioSource.clip = audioTrack.AudioClip;
            audioSource.volume = 0;
            audioSource.Play();
            if (randomTime)
                audioSource.time = Random.Range(0, audioSource.clip.length * .9f);
            audioSource.DOFade(audioTrack.Volume, audioTrack.FadeDuration);
            activeSource = nextSource;
        }
    }
}
