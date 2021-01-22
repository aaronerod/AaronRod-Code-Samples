using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Rod.Utilities.InputController {
    /// <summary>
    /// Sistema para escuchar drag e interactuar con objetos en worldspace por
    /// medio de sus colliders
    /// </summary>
    public class DragInput : BaseInput, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        /// <summary>
        /// Receptores de drag actuales
        /// </summary>
        private Dictionary<int, List<IDragReceiver>> dragReceiversByPointer = new Dictionary<int, List<IDragReceiver>>();
        /// <summary>
        /// Relación de los punteros activos
        /// </summary>
        private Dictionary<int, bool> pointersDragStarted = new Dictionary<int, bool>();
        public void OnPointerDown(PointerEventData eventData)
        {
            if (enableMultiTouch || activePointers.Count == 0)
            {
                activePointers.Add(eventData.pointerId);
                List<IDragReceiver> dragReceivers = new List<IDragReceiver>();// = InputToRaycast<IDragReceiver>(eventData.position);
                if (useCircleCast)
                {
                    dragReceivers = InputToCircleCast<IDragReceiver>(eventData.position, circleCastRadius);
                }
                else
                {
                    dragReceivers = InputToRaycast<IDragReceiver>(eventData.position);
                }

                dragReceiversByPointer.Add(eventData.pointerId, dragReceivers);
                pointersDragStarted.Add(eventData.pointerId, false);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            List<IDragReceiver> dragReceivers;
            bool recordExists = dragReceiversByPointer.TryGetValue(eventData.pointerId, out dragReceivers);
            
            if (recordExists)
            {
                pointersDragStarted[eventData.pointerId] = true;
                IDragReceiver dragReceiver = GetCloserObject(dragReceivers);
                if(dragReceiver!=null)
                    dragReceiver?.OnBeginDrag(InputController.Instance.ScreenToWorldPoint(eventData.position));
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            position = eventData.position;
            List<IDragReceiver> dragReceivers;
            bool recordExists = dragReceiversByPointer.TryGetValue(eventData.pointerId, out dragReceivers);
            if (recordExists)
            {
                IDragReceiver dragReceiver = GetCloserObject(dragReceivers);
                if(dragReceiver!=null)
                    dragReceiver?.OnDrag(InputController.Instance.ScreenToWorldPoint(eventData.position));
                
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            List<IDragReceiver> dragReceivers;
                activePointers.Remove(eventData.pointerId);
            bool recordExists = dragReceiversByPointer.TryGetValue(eventData.pointerId, out dragReceivers);
            if (recordExists)
            {
                IDragReceiver dragReceiver = GetCloserObject(dragReceivers);
                if(dragReceiver!=null)
                    dragReceiver?.OnEndDrag(InputController.Instance.ScreenToWorldPoint(eventData.position));
                
            }
            ClearDragReceiversKey(eventData.pointerId);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            bool dragStarted = false;
            pointersDragStarted.TryGetValue(eventData.pointerId,out dragStarted);
            if (!dragStarted)
            {
                activePointers.Remove(eventData.pointerId);
                if (!activePointers.Contains(eventData.pointerId))
                    ClearDragReceiversKey(eventData.pointerId);
            }
        }

        /// <summary>
        /// Limpiar los punteros y receptores activos 
        /// </summary>
        /// <param name="key"></param>
        void ClearDragReceiversKey(int key)
        {
            dragReceiversByPointer.Remove(key);
            pointersDragStarted.Remove(key);
        }

        /// <summary>
        /// Obtener el receptor más cercano
        /// </summary>
        /// <param name="dragReceivers"></param>
        /// <returns></returns>
        public IDragReceiver GetCloserObject(List<IDragReceiver> dragReceivers)
        {
            if (dragReceivers.Count > 0)
            {
                IDragReceiver closer = dragReceivers[0];
                float closerDistance = float.MaxValue;
                if (closer != null)
                {
                    GameObject gameObject = closer.GetGameObject();
                    if (gameObject)
                        closerDistance = gameObject.transform.position.z - InputController.Instance.mainCamera.transform.position.z;
                    else
                        closer = null;
                }
                for (int i = 1; i < dragReceivers.Count; i++)
                {
                    float distance = float.MaxValue;
                    if (dragReceivers[i] != null)
                    {
                        GameObject gameObject = dragReceivers[i].GetGameObject();
                        if (gameObject)
                            distance = gameObject.transform.position.z - InputController.Instance.mainCamera.transform.position.z;

                    }
                    if (distance < closerDistance)
                    {
                        closerDistance = distance;
                        closer = dragReceivers[i];
                    }
                }
                return closer;
            }
            return null;
        }
        Vector2 position;
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(position), .5f);
        }
    }
}