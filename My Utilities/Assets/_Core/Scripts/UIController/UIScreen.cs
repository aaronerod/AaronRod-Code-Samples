using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using Rod.Utilities.Tweens;
using UnityEngine.Events;
namespace Rod.Utilities.UI
{
    public enum ScreenBehindBehaviour
    {
        none,
        hide,
        close
    }
    /// <summary>
    /// Base de pantalla para UIController
    /// </summary>
    [RequireComponent(typeof(UINavigator))]
    public abstract class UIScreen : MonoBehaviour
    {
        /// <summary>
        /// Notifica que ha sido abierta
        /// </summary>
        public UnityEvent ScreenOpened;
        /// <summary>
        /// Notifica que ha sido cerrada
        /// </summary>
        public UnityEvent ScreenClosed;
        /// <summary>
        /// Notifica que recibió el foco
        /// </summary>
        public UnityEvent ScreenFocused;
        /// <summary>
        /// Notifica que perdió el foco
        /// </summary>
        public UnityEvent ScreenOutFocused;
        /// <summary>
        /// Notifica que ha sido mostrada
        /// </summary>
        public UnityEvent ScreenShowed;
        /// <summary>
        /// Notifica que ha sido ocultada
        /// </summary>
        public UnityEvent ScreenHided;
        /// <summary>
        /// Componente encargado de realizar animaciones
        /// </summary>
        public UITweener tweener;
        [SerializeField]
        Transform reparentTarget;
        [SerializeField]
        UINavigator navigator;
        /// <summary>
        /// Componente encargado de la navegación
        /// </summary>
        public UINavigator Navigator => navigator;
        /// <summary>
        /// Referencia del padre principal de la pantalla
        /// </summary>
        public Transform ReparentTarget
        {
            get
            {
                Transform target = reparentTarget;
                if (target == null)
                    target = transform;
                return target;

            }
        }
        /// <summary>
        /// Indica si es un popup
        /// </summary>
        [SerializeField]
        bool _isPopUp;
        [SerializeField]
        ScreenBehindBehaviour behindBehavior;
        [SerializeField]
        bool _isPermanent;

        /// <summary>
        /// Tweeners anidados
        /// </summary>
        public List<UITweener> tweeners = new List<UITweener>();
        /// <summary>
        /// Tweener más largo
        /// </summary>
        public UITweener longestTweener;
        private bool _isShowing;
        /// <summary>
        /// Indica si es un pop up
        /// </summary>
        public bool IsPopUp
        {
            get { return _isPopUp; }
        }
        /// <summary>
        /// Indica el comportamiento al estar detrás de otra pantalla
        /// </summary>
        public ScreenBehindBehaviour BehindBehaviour => behindBehavior;
        /// <summary>
        /// Indica si se encuentra abierta y se está mostrando
        /// </summary>
        public bool IsShowing
        {
            get { return _isShowing; }
            set { _isShowing = value; }
        }
        /// <summary>
        /// Indica si es permanente. No puede cerrarse
        /// </summary>
        public bool IsPermanent
        {
            get { return _isPermanent; }
        }
        private void Awake()
        {
            CalculateLongestTweener();
        }
        /// <summary>
        /// Acciones al abrir pantalla
        /// </summary>
        /// <returns></returns>
        public abstract bool Open();
        /// <summary>
        /// Acciones al cerrar pantalla
        /// </summary>
        /// <returns></returns>
        public abstract bool Close();
        /// <summary>
        /// Acciones al esconder pantalla
        /// </summary>
        public virtual void Hide()
        {
            if (IsShowing)
            {
                ScreenHided.Invoke();
                if (IsTweenerAvailable())
                {
                    _isShowing = false;
                    if (longestTweener)
                        longestTweener.OnFadeOutFinished.AddListener(TurnOff);
                    if (tweener)
                        tweener.FadeOut();
                    foreach (UITweener tweener in tweeners)
                        tweener.FadeOutDefault();
                }
                else
                {
                    TurnOff();
                }
            }
        }
        /// <summary>
        /// Acciones al mostrar pantalla
        /// </summary>
        public virtual void Show()
        {
            ScreenShowed.Invoke();
            _isShowing = true;
            if (tweener)
                tweener.FadeIn();
            foreach (UITweener tweener in tweeners)
                tweener.FadeIn();

        }
        /// <summary>
        /// Verifica si cumple las condiciones para cerrarse
        /// </summary>
        /// <returns></returns>
        public abstract bool CanBeClosed();
        /// <summary>
        /// Verifica si cumple las condiciones para ser abierta
        /// </summary>
        /// <returns></returns>
        public abstract bool CanBeOpened();
        /// <summary>
        /// Método invocado al recibir el foco
        /// </summary>
        public abstract void ScreenOnFocus();
        /// <summary>
        /// Método invocado al perder el foco
        /// </summary>
        public abstract void ScreenOutFocus();
        /// <summary>
        /// Indica si el tweener más largo está disponible el tween o se encuntra en reproducción
        /// </summary>
        /// <returns></returns>
        public bool IsTweenerAvailable()
        {
            bool result = false;
            if (longestTweener)
            {
                result = !longestTweener.IsFading || longestTweener.forceFade;
            }
            return result;
        }
        /// <summary>
        /// Desactiva objeto
        /// </summary>
        public void TurnOff()
        {
            gameObject.SetActive(false);
            if (longestTweener)
                longestTweener.OnFadeOutFinished.RemoveListener(TurnOff);
        }

        public virtual void OnDestroy()
        {
            if (_isPermanent)
                _isPermanent = false;
            if (navigator != null)
                navigator.CloseScreen();
        }
        /// <summary>
        /// Calcula la duración del tween más largo
        /// </summary>
        void CalculateLongestTweener()
        {
            float longestTweenerDuration = 0;
            if (tweener)
            {
                longestTweenerDuration = tweener.fadeDuration + tweener.delayAtStart;
                longestTweener = tweener;
            }
            foreach (UITweener tweener in tweeners)
            {
                float tweenerDuration = tweener.TotalDuration;
                if (tweenerDuration > longestTweenerDuration && tweener.gameObject.activeInHierarchy)
                {
                    longestTweenerDuration = tweenerDuration;
                    longestTweener = tweener;
                }
            }
        }
        /// <summary>
        /// Agrega un nuevo tween
        /// </summary>
        /// <param name="tweener"></param>
        public void AddDependantTweener(UITweener tweener)
        {
            if (!tweeners.Contains(tweener))
            {
                tweeners.Add(tweener);
                CalculateLongestTweener();
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UIScreen), true)]
    public class UI_ElementEditor : Editor
    {
        UIScreen element;
        private void OnEnable()
        {
            element = (UIScreen)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!element.tweener && GUILayout.Button("Set tweener"))
            {
                element.tweener = element.gameObject.GetComponent<UITweener>();
                if (!element.tweener)
                    element.tweener = element.gameObject.AddComponent<UITweener>();
            }
            if (element.tweener && GUILayout.Button("Remove tweener"))
            {
                DestroyImmediate(element.tweener);
                element.tweener = null;
            }
            if (GUILayout.Button("Add dependant tweeners from children"))
            {
                element.tweeners.Clear();
                UITweener[] tweeners = element.gameObject.GetComponentsInChildren<UITweener>(true);
                foreach (UITweener tweener in tweeners)
                    element.AddDependantTweener(tweener);

                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}