using UnityEngine;
using UnityEngine.Events;
namespace Rod.Utilities.InputController
{
    /// <summary>
    /// Receptor de evento tap
    /// </summary>
    public interface ITapReceiver
    {
        /// <summary>
        /// Obtener el gameobject del objeto que implementa la interfaz
        /// </summary>
        /// <returns></returns>
        GameObject GetGameObject();
        /// <summary>
        /// Notifica que se ha recibido un pointer down
        /// </summary>
        UnityEvent PonterDown { get; }
        /// <summary>
        /// Notifica que se ha recibido un pointer up
        /// </summary>
        UnityEvent PointerUp { get; }
        /// <summary>
        /// Receptor de pointer down
        /// </summary>
        /// <param name="pointerPosition"></param>
        void OnPointerDown(Vector2 pointerPosition);
        /// <summary>
        /// Receptor de pointer up
        /// </summary>
        /// <param name="pointerPosition"></param>
        void OnPointerUp(Vector2 pointerPosition);
    }
    /// <summary>
    /// Receptor de evento drag
    /// </summary>
    public interface IDragReceiver
    {
        /// <summary>
        /// Obtener el gameobject del objeto que implementa la interfaz
        /// </summary>
        /// <returns></returns>
        GameObject GetGameObject();
        /// <summary>
        /// Receptor del inicio del drag
        /// </summary>
        /// <param name="worldPosition"></param>
        void OnBeginDrag(Vector3 worldPosition);
        /// <summary>
        /// Receptor del drag en proceso
        /// </summary>
        /// <param name="worldPosition"></param>
        void OnDrag(Vector3 worldPosition);
        /// <summary>
        /// Receptor del fin del drag
        /// </summary>
        /// <param name="worldPosition"></param>
        void OnEndDrag(Vector3 worldPosition);
    }
}