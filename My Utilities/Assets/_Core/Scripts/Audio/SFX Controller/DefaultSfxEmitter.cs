using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Rod.Utilities.SfxController
{
    /// <summary>
    /// Utility para reproducir sfx a través del SFX Controller
    /// </summary>
    public class DefaultSfxEmitter : SfxEmitter
    {
        /// <summary>
        /// Source generado
        /// </summary>
        private AudioSource playedSource;
        /// <summary>
        /// Duración de fadout
        /// </summary>
        [SerializeField]
        private float fadeOutDuration;

        public override void EmmitSfx()
        {
            if (AudioData)
                SFXController.Instance.PlaySound(AudioData, SourceGenerated);
            else
                Debug.LogError("Please asign Audio Data to " + gameObject.name);
        }

        /// <summary>
        /// Detener reproducción de Sfx
        /// </summary>
        public void StopSound()
        {
            if (playedSource != null)
            {
                if (fadeOutDuration > 0)
                {
                    playedSource.DOFade(0, fadeOutDuration).OnComplete(() => playedSource.Stop());
                }
                else
                {
                    playedSource.Stop();
                }
            }
        }

        /// <summary>
        /// Receptor de Callback al reproducir Sfx
        /// </summary>
        /// <param name="source"></param>
        public void SourceGenerated(AudioSource source)
        {
            this.playedSource = source;
        }

        public override void MuteSfx(bool mute)
        {
            throw new System.NotImplementedException();
        }
    }
}