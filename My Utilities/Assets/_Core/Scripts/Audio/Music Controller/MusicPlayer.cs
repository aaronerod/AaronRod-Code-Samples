using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.MusicController
{
    /// <summary>
    /// Utility para iniciar la reproducción de una pista
    /// </summary>
    public class MusicPlayer : MonoBehaviour
    {
        /// <summary>
        /// Referencia del scriptable para conectar con MusicController
        /// </summary>
        [SerializeField]
        private MusicConnector musicConnector;
        /// <summary>
        /// Key id de la pista que se desea reproducir (debe estar incluido en el
        /// tracklist asignado en MusicController
        /// </summary>
        [SerializeField]
        private string musicKey;
        /// <summary>
        /// Realizar reproducción en loop
        /// </summary>
        [SerializeField]
        private bool isLoop;
        /// <summary>
        /// Iniciar reproducción en un tiempo aleatorio
        /// </summary>
        [SerializeField]
        private bool randomTime;
        /// <summary>
        /// Iniciar reproducción cuando se reciba la notificación de que el sistema fue asignado
        /// </summary>
        [SerializeField]
        private bool playImmediately;
        /// <summary>
        /// Iniciar reproducción al activarse el objeto
        /// </summary>
        [SerializeField]
        private bool playOnEnable;

        private bool isInitialized;

        private void Start()
        {
            //Suscribirse al connector para ser notificado cuando el sistema sea asignado
            musicConnector.ControllerAssigned += OnMusicControllerAssigned;
        }

        private void OnEnable()
        {
            if (isInitialized && playOnEnable)
                PlayMusic();
        }

        private void OnDestroy()
        {
            //Desuscribirse al ser eliminado
            musicConnector.ControllerAssigned -= OnMusicControllerAssigned;
        }

        /// <summary>
        /// Iniciar reproducción de la música seleccionada
        /// </summary>
        [ContextMenu("Playe Music")]
        public void PlayMusic()
        {
            musicConnector.Controller.PlayMusic(musicKey, isLoop, randomTime);
        }

        /// <summary>
        /// Receptor de la notificación del sistema asignado
        /// </summary>
        /// <param name="musicController"></param>
        void OnMusicControllerAssigned(MusicController musicController)
        {
            if (playImmediately)
                PlayMusic();
            isInitialized = true;
        }
    }
}