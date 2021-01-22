using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rod.Utilities.Patterns;

namespace Rod.Utilities.InputController
{
    /// <summary>
    /// Sistema encargado de controlar el input a través de ui para interactuar con objetos con colliders
    /// por medio de raycast o sphercast
    /// </summary>
    public class InputController : MonoBehaviourSingleton<InputController>
    {
        /// <summary>
        /// Controlador de tap
        /// </summary>
        [SerializeField]
        private TapInput tapInput;
        /// <summary>
        /// Controlador de drag
        /// </summary>
        [SerializeField]
        private DragInput dragInput;

        /// <summary>
        /// Referencia a la main camara para evitar usar camera.main
        /// </summary>
        public Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }
        public override void InitializeSingleton()
        {

        }

        /// <summary>
        /// Habilitar sistema de tap
        /// </summary>
        /// <param name="isEnebled"></param>
        public void EnableTap(bool isEnebled)
        {
            tapInput.enabled = (isEnebled);
        }

        /// <summary>
        /// Habilitar sistema de drag
        /// </summary>
        /// <param name="isEnabled"></param>
        public void EnableDrag(bool isEnabled)
        {
            dragInput.enabled = (isEnabled);
        }

        /// <summary>
        /// Utility para no usar camera.main
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3 ScreenToWorldPoint(Vector3 position)
        {
            return mainCamera.ScreenToWorldPoint(position);
        }
    }
}