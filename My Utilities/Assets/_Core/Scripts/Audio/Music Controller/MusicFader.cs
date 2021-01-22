using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.MusicController
{
    /// <summary>
    /// Utility para iniciar un fade en la música actual
    /// </summary>
    public class MusicFader : MonoBehaviour
    {
        /// <summary>
        /// Referencia del scriptable para conectar con MusicController
        /// </summary>
        [SerializeField]
        private MusicConnector musicConnector;
        /// <summary>
        /// Realizar fade al recibir la notificación de que el sistema ha sido asignado
        /// </summary>
        [SerializeField]
        private bool fadeImmediatelly;
        /// <summary>
        /// Realizar fade al activarse
        /// </summary>
        [SerializeField]
        private bool fadeOnEnable;
        /// <summary>
        /// Duración de fade
        /// </summary>
        [SerializeField]
        private float fadeDuration;
        /// <summary>
        /// Volumen al terminar fade
        /// </summary>
        [SerializeField]
        private float volumeTarget;

        private bool isInitialized;

        private void Start()
        {
            //Suscribirse al connector para ser notificado cuando el sistema sea asignado
            musicConnector.ControllerAssigned += OnMusicControllerAssigned;
        }

        private void OnEnable()
        {
            if (isInitialized && fadeOnEnable)
                FadeVolume();
        }

        private void OnDestroy()
        {
            //Desuscribirse al ser eliminado
            musicConnector.ControllerAssigned -= OnMusicControllerAssigned;
        }

        /// <summary>
        /// Realizar fade de volumen
        /// </summary>
        [ContextMenu("Playe Music")]
        public void FadeVolume()
        {
            musicConnector.Controller.FadeCurrentTo(volumeTarget, fadeDuration);
        }

        /// <summary>
        /// Receptor de la notificación del sistema asignado
        /// </summary>
        /// <param name="musicController"></param>
        void OnMusicControllerAssigned(MusicController musicController)
        {
            if (fadeImmediatelly)
                FadeVolume();
            isInitialized = true;
        }
    }
}