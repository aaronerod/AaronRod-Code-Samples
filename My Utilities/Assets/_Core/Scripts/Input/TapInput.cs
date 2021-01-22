using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rod.Utilities.InputController
{
    /// <summary>
    /// Sistema para escuchar tap e interactuar con objetos en worldspace por
    /// medio de sus colliders
    /// </summary>
    public class TapInput : BaseInput, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// Radio de interacción si usa círculo
        /// </summary>
        [SerializeField]
        private float circleCastRadius = 1;
        /// <summary>
        /// Usar interacción con círculo
        /// </summary>
        [SerializeField]
        private bool useCircleCast;

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            position = eventData.position;
            if (enableMultiTouch || activePointers.Count == 0)
            {
                activePointers.Add(eventData.pointerId);
                List<ITapReceiver> pointerDownReceivers;
                if (useCircleCast)
                {
                    pointerDownReceivers = InputToCircleCast<ITapReceiver>(eventData.position, circleCastRadius);
                }
                else
                {
                    pointerDownReceivers = InputToRaycast<ITapReceiver>(eventData.position);
                }
                /*
                for (int i = 0; i < pointerDownReceivers.Count; i++)
                    pointerDownReceivers[i].OnPointerDown(position);
                    */
                ITapReceiver tapReceiver = GetCloserObject(pointerDownReceivers);
                tapReceiver?.OnPointerDown(position);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (enableMultiTouch || activePointers.Count == 0)
            {
                List<ITapReceiver> pointerDownReceivers;
                if (useCircleCast)
                {
                    pointerDownReceivers = InputToCircleCast<ITapReceiver>(eventData.position, circleCastRadius);
                }
                else
                {
                    pointerDownReceivers = InputToRaycast<ITapReceiver>(eventData.position);
                }
                /*
                for (int i = 0; i < pointerDownReceivers.Count; i++)
                    pointerDownReceivers[i].OnPointerUp(position);
                    */

                ITapReceiver tapReceiver = GetCloserObject(pointerDownReceivers);
                tapReceiver?.OnPointerUp(position);
            }
            activePointers.Remove(eventData.pointerId);
        }
        Vector2 position;
        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(InputController.Instance.ScreenToWorldPoint(position), circleCastRadius);
            }
        }

        /// <summary>
        /// Obtener el receptor más cercano
        /// </summary>
        /// <param name="tapReceivers"></param>
        /// <returns></returns>
        public ITapReceiver GetCloserObject(List<ITapReceiver> tapReceivers)
        {
            if (tapReceivers.Count > 0)
            {
                ITapReceiver closer = tapReceivers[0];
                float closerDistance = closer.GetGameObject().transform.position.z-InputController.Instance.mainCamera.transform.position.z;
                for(int i = 1; i < tapReceivers.Count; i++)
                {
                    float distance =tapReceivers[i].GetGameObject().transform.position.z - InputController.Instance.mainCamera.transform.position.z;
                    if (distance < closerDistance)
                    {
                        closerDistance = distance;
                        closer = tapReceivers[i];
                    }
                }
                return closer;
            }
            return null;
        }
    }
}