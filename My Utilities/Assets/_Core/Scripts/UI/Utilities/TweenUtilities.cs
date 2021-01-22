using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Rod.Utilities.Tweens
{
    /// <summary>
    /// Utility para realizar tweens utilizando dotween
    /// </summary>
    [CanEditMultipleObjects]
    public class TweenUtilities : MonoBehaviour
    {
        [SerializeField]
        bool excludeFromGroup;
        /// <summary>
        /// Excluir del grupo (si es parte de uno)
        /// </summary>
        public bool ExcludeFromGroup => excludeFromGroup;
        /// <summary>
        /// Iniciar tween al activarse
        /// </summary>
        [SerializeField]
        bool startTweenOnEnable;
        /// <summary>
        /// Conjunto de tweens
        /// </summary>
        [SerializeField]
        List<TweenData> tweens = new List<TweenData>();
        void OnEnable()
        {
            if (startTweenOnEnable)
                TweenAll();
        }
        void OnDisable()
        {
            KillAll();
        }
        /// <summary>
        /// Inicia todos los tweens
        /// </summary>
        [ContextMenu("TweenAll")]
        public void TweenAll()
        {
            foreach (TweenData tween in tweens)
            {
                tween.PerformTween(transform);
            }
        }
        /// <summary>
        /// Inicia todos los tweens en reversa
        /// </summary>
        [ContextMenu("Tween Reversed All")]
        public void TweenAllReverse()
        {
            foreach (TweenData tween in tweens)
            {
                tween.PerformReversedTween(transform);
            }
        }
        /// <summary>
        /// Realiza todos los tweens en 0 segundos
        /// </summary>
        public void TweenImmediately()
        {
            foreach (TweenData tween in tweens)
            {
                tween.PerformImmediateTween(transform);
            }
        }
        /// <summary>
        /// Realiza todos los tweens en reversa en 0 segundos
        /// </summary>
        public void TweenImmediatelyReversed()
        {
            foreach (TweenData tween in tweens)
            {
                tween.PerformImmediateTweenReversed(transform);
            }
        }
        /// <summary>
        /// Detiene todos los tweens
        /// </summary>
        public void KillAll()
        {
            foreach (TweenData tween in tweens)
            {
                tween.StopTween();
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(TweenUtilities), true), CanEditMultipleObjects]
    public class TweenUtilitiesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying && GUILayout.Button("Test Tween"))
            {
                foreach (Object target in targets)
                {
                    ((TweenUtilities)target).KillAll();
                    ((TweenUtilities)target).TweenAll();
                }
            }
            if (Application.isPlaying && GUILayout.Button("Test Reverse Tween"))
            {
                foreach (Object target in targets)
                {
                    ((TweenUtilities)target).KillAll();
                    ((TweenUtilities)target).TweenAllReverse();
                }
            }
        }
    }
#endif
}