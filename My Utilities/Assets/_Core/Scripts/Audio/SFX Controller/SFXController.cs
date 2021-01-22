using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Rod.Utilities.Patterns;

namespace Rod.Utilities.SfxController
{
    /// <summary>
    /// Controlador de SFX
    /// </summary>
    public class SFXController : MonoBehaviourSingleton<SFXController>
    {
        /// <summary>
        /// Lista de sources usados para reproducir sfx
        /// </summary>
        private List<AudioSource> sfxSources = new List<AudioSource>();
        /// <summary>
        /// Límite de sfx reproduciendo simultaneamente
        /// </summary>
        [SerializeField]
        private int maxAllowedSounds = 10;
        /// <summary>
        /// Padre donde se anidarán los sources
        /// </summary>
        [SerializeField]
        private Transform sfxParent;
        /// <summary>
        /// Mixer group de salida para los sfx
        /// </summary>
        [SerializeField]
        private AudioMixerGroup sfxOutput;

        private List<SfxData> playingDatas = new List<SfxData>();
        /// <summary>
        /// Lista de SFX datas que se están reproduciendo actualmente
        /// </summary>
        public List<SfxData> PlayingDatas { get => playingDatas; set => playingDatas = value; }

        /// <summary>
        /// Método de la clase base Singleton para realizar inicializaciones
        /// </summary>
        public override void InitializeSingleton()
        {
            SetAsPersistentSingleton();
            AudioSource[] sfxSources = sfxParent.GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < sfxSources.Length; i++)
            {
                this.sfxSources.Add(sfxSources[i]);
                sfxSources[i].outputAudioMixerGroup = sfxOutput;
                sfxSources[i].gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Inicia la reproducción del sfx solicitado
        /// </summary>
        /// <param name="audioData">Data del sfx a reproducir</param>
        /// <param name="generatedSourceCallback">Callback regresando el Audiosource en el que se reproduce el sfx</param>
        /// <returns></returns>
        public Coroutine PlaySound(SfxData audioData, System.Action<AudioSource> generatedSourceCallback)
        {
            if (audioData != null && audioData.Clip != null && (PlayingDatas.Count < maxAllowedSounds || audioData.ForceToPlay))
                return StartCoroutine(PlaySfxCoroutine(audioData, generatedSourceCallback));
            else
                generatedSourceCallback?.Invoke(null);
            return null;
        }


        /// <summary>
        /// Reproducción de sfx
        /// </summary>
        /// <param name="data">Data del sfx a reproducir</param>
        /// <param name="generatedSourceCallback">Callback regresando el Audiosource en el que se reproduce el sfx</param>
        /// <returns></returns>
        IEnumerator PlaySfxCoroutine(SfxData data, System.Action<AudioSource> generatedSourceCallback)
        {
            if (data.AllowMultiplePlay || !PlayingDatas.Contains(data))
            {
                //Obtener un source inactivo 
                AudioSource audioSource = sfxSources.Find(source => source.gameObject.activeInHierarchy == false);
                //Crear un source si no se encontró uno
                if (audioSource == null)
                    audioSource = CreateAudioSfx();

                generatedSourceCallback?.Invoke(audioSource);

                audioSource.gameObject.SetActive(true);
                audioSource.clip = data.Clip;
                PlayingDatas.Add(data);
                audioSource.volume = data.Volume;

                //Agregar una variación de pitch si la data así lo define
                float pitchVariance = data.PitchVariance * Random.Range(-3, 3);
                audioSource.pitch = 1 + pitchVariance;
                audioSource.Play();
                audioSource.loop = data.IsLoop;
                yield return new WaitWhile(() => audioSource.isPlaying == true);
                audioSource.Stop();
                audioSource.gameObject.SetActive(false);
                PlayingDatas.Remove(data);
            }
            generatedSourceCallback?.Invoke(null);
        }

        /// <summary>
        /// Crear un nuevo audio source
        /// </summary>
        /// <returns></returns>
        AudioSource CreateAudioSfx()
        {
            GameObject sourceObject = new GameObject("SfxSource");
            sourceObject.transform.SetParent(sfxParent);
            AudioSource sfxSource = sourceObject.AddComponent<AudioSource>();

            sfxSources.Add(sfxSource);
            sfxSource.outputAudioMixerGroup = sfxOutput;
            sfxSource.gameObject.SetActive(false);

            return sfxSource;
        }
    }
}