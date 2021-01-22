
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace Rod.Utilities.InputController
{
    /// <summary>
    /// Implementación de un receptor de tap
    /// </summary>
    public class TapReceiver : MonoBehaviour, ITapReceiver
    {
        [SerializeField]
        private UnityEvent pointerDown;
        [SerializeField]
        private UnityEvent pointerUp;
        public UnityEvent PonterDown
        {
            get
            {
                return pointerDown;
            }
        }

        public UnityEvent PointerUp
        {
            get
            {
                return pointerUp;
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void OnPointerDown(Vector2 pointerPosition)
        {
            pointerDown.Invoke();
        }

        public void OnPointerUp(Vector2 pointerPosition)
        {
            pointerUp.Invoke();
        }
    }
}