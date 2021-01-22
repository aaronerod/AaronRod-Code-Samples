using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rod.Utilities.SfxController
{
    /// <summary>
    /// Implementación abstracta de un emisor de Sfx
    /// </summary>
    public abstract class SfxEmitter : MonoBehaviour, ISfxEmitter
    {
        [SerializeField] SfxData audioData;
        public SfxData AudioData
        {
            get
            {
                return audioData;
            }
        }
        public event Action OnSoundEmmited;
        public event Action OnSoundMuted;
        public abstract void EmmitSfx();

        public abstract void MuteSfx(bool mute);

        public virtual void Reset()
        {
#if UNITY_EDITOR
            //Si el gameobject posee un componente botón, entonces agregar un listener al
            //evento OnClick para emitir Sfx
            UnityEngine.UI.Button button = gameObject.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, EmmitSfx);
            }
#endif
        }
    }
}