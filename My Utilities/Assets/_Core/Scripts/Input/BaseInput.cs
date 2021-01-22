using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rod.Utilities.InputController 
{
    /// <summary>
    /// Base para implementar sistemas de input
    /// </summary>
    public abstract class BaseInput : MonoBehaviour
    {
        /// <summary>
        /// Máscara usada para detectar los receptores
        /// </summary>
        [SerializeField]
        protected LayerMask targetMask;

        /// <summary>
        /// Permitir interacción con multitouch
        /// </summary>
        [SerializeField]
        protected bool enableMultiTouch;

        
        protected List<int> activePointers = new List<int>();

        /// <summary>
        /// Usar Raycast para obtener lista de receptores con un tipo de componente
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position">Posición en screenspace</param>
        /// <returns></returns>
        protected List<T> InputToRaycast<T>(Vector3 position)
        {
            List<T> receivers = new List<T>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(InputController.Instance.ScreenToWorldPoint(position), Vector3.forward, 1000,targetMask);
            for (int i = 0; i < hits.Length; i++)
            {
                T receiverComponent = hits[i].collider.gameObject.GetComponent<T>();
                if (hits[i].collider.gameObject.activeInHierarchy && receiverComponent != null)
                {
                    receivers.Add(receiverComponent);
                }
            }
            return receivers;
        }
        /// <summary>
        /// Usar Circlecast para obtener lista de receptores con un tipo de componente
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position">Posición en screenspace</param>
        /// <param name="radius">Radio de interacción</param>
        /// <returns></returns>
        protected List<T> InputToCircleCast<T>(Vector3 position,float radius)
        {
            List<T> receivers = new List<T>();
            RaycastHit2D[] hits = Physics2D.CircleCastAll(InputController.Instance.ScreenToWorldPoint(position), radius, Vector3.forward, 1000, targetMask);
            for(int i=0; i < hits.Length; i++)
            {
                T receiverComponent = hits[i].collider.gameObject.GetComponent<T>();
                if (hits[i].collider.gameObject.activeInHierarchy && receiverComponent != null)
                {
                    receivers.Add(receiverComponent);
                }
            }
            return receivers;
        }
    }
}