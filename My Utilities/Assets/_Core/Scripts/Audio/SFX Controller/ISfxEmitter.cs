using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rod.Utilities.SfxController
{
    /// <summary>
    /// Emisor de SFXs
    /// </summary>
    public interface ISfxEmitter
    {
        /// <summary>
        /// Notifica cuando un sonido ha sido emitido
        /// </summary>
        event Action OnSoundEmmited;
        /// <summary>
        /// Notifica cuando un sonido ha sido silenciado
        /// </summary>
        event Action OnSoundMuted;
        /// <summary>
        /// Data del sfx a reproducir
        /// </summary>
        SfxData AudioData { get; }
        /// <summary>
        /// Emitir Sfx
        /// </summary>
        void EmmitSfx();
        /// <summary>
        /// Silenciar Sfx
        /// </summary>
        /// <param name="mute"></param>
        void MuteSfx(bool mute);
    }
}