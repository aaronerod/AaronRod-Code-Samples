using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rod.Utilities.Tweens
{
    /// <summary>
    /// Agrupa TweenUtilities
    /// </summary>
    public class TweenGroup : MonoBehaviour
    {
        /// <summary>
        /// Conjunto de tweens pertenecientes al grupo
        /// </summary>
        [SerializeField]
        TweenUtilities[] tweeners;
        /// <summary>
        /// Obtiene el conjunto a partir de los hijos
        /// </summary>
        [SerializeField]
        bool useChildren;

        private void Awake()
        {
            if (useChildren)
                tweeners = GetComponentsInChildren<TweenUtilities>();
        }
        /// <summary>
        /// Inicia el tween de todos los elementos del grupo
        /// </summary>
        [ContextMenu("Tween")]
        public void Tween()
        {
            for (int i = 0; i < tweeners.Length; i++)
            {
                if (!tweeners[i].ExcludeFromGroup)
                    tweeners[i].TweenAll();
            }
        }
        /// <summary>
        /// Inicia el tween en reversa de todos los elementos del grupo
        /// </summary>
        [ContextMenu("Tween Reversed")]
        public void TweenReversed()
        {
            for (int i = 0; i < tweeners.Length; i++)
            {
                if (!tweeners[i].ExcludeFromGroup)
                    tweeners[i].TweenAllReverse();
            }
        }
        /// <summary>
        /// Realiza un tween inmediato (en 0 segundos)
        /// </summary>
        public void TweenImmediately()
        {
            for (int i = 0; i < tweeners.Length; i++)
            {
                if (!tweeners[i].ExcludeFromGroup)
                    tweeners[i].TweenImmediately();
            }
        }
        /// <summary>
        /// Realiza un tween en reversa inmediato (en 0 segundos)
        /// </summary>
        public void TweenReveresImmediately()
        {
            for (int i = 0; i < tweeners.Length; i++)
            {
                if (!tweeners[i].ExcludeFromGroup)
                    tweeners[i].TweenImmediatelyReversed();
            }
        }
    }
}